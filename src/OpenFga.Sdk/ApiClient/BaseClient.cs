using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Telemetry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.ApiClient;

public class ResponseWrapper<T> {
    public HttpResponseMessage rawResponse;
    public T? responseContent;

    public int retryCount;
}

/// <summary>
///     Base Client, used by the API and OAuth Clients
/// </summary>
public class BaseClient : IDisposable {
    private readonly HttpClient _httpClient;
    private bool _shouldDisposeWhenDone;
    private readonly Metrics? _metrics;

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseClient" /> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpClient">Optional <see cref="HttpClient" /> to use when sending requests.</param>
    /// <param name="metrics">Optional <see cref="Metrics" /> instance for telemetry.</param>
    /// <remarks>
    ///     If you supply a <see cref="HttpClient" /> it is your responsibility to manage its lifecycle and
    ///     dispose it when appropriate.
    ///     If you do not supply a <see cref="HttpClient" /> one will be created automatically and disposed
    ///     of when this object is disposed.
    /// </remarks>
    public BaseClient(Configuration.Configuration configuration, HttpClient? httpClient = null, Metrics? metrics = null) {
        _shouldDisposeWhenDone = httpClient == null;
        _httpClient = httpClient ?? new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        foreach (var header in configuration.DefaultHeaders) {
            if (header.Value != null) {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        _metrics = metrics;
    }

    /// <summary>
    ///     Handles calling the API
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="retryCount">The number of retry attempts (0 for the initial request)</param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    /// <returns></returns>
    public async Task<ResponseWrapper<TRes>> SendRequestAsync<TReq, TRes>(RequestBuilder<TReq> requestBuilder,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, int retryCount = 0, CancellationToken cancellationToken = default) {
        var request = requestBuilder.BuildRequest();

        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders.Where(h => h.Value != null)) {
                request.Headers.Add(header.Key, header.Value);
            }
        }
        var httpRequestStopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        httpRequestStopwatch.Stop();

        // Record HTTP request duration metric
        if (_metrics != null && apiName != null) {
            _metrics.BuildForHttpRequest(apiName, response, requestBuilder, httpRequestStopwatch, retryCount);
        }

        try {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) {
            throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
        }

        TRes responseContent = default;
        if (response.Content != null && response.StatusCode != HttpStatusCode.NoContent) {
            using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            // Guard against empty responses, including for non-seekable streams.
            // For seekable streams, use Length; for non-seekable streams, peek a byte and buffer if present.
            Stream deserializationStream = contentStream;
            var hasContent = false;
            if (deserializationStream.CanSeek) {
                hasContent = deserializationStream.Length > 0;
                if (hasContent && deserializationStream.Position != 0) {
                    deserializationStream.Position = 0;
                }
            }
            else {
                int firstByte = deserializationStream.ReadByte();
                if (firstByte != -1) {
                    hasContent = true;
                    var bufferedStream = new MemoryStream();
                    bufferedStream.WriteByte((byte)firstByte);
                    await deserializationStream.CopyToAsync(bufferedStream).ConfigureAwait(false);
                    bufferedStream.Position = 0;
                    deserializationStream = bufferedStream;
                }
            }
            if (hasContent) {
                responseContent = await JsonSerializer.DeserializeAsync<TRes>(deserializationStream, cancellationToken: cancellationToken).ConfigureAwait(false) ??
                                  throw new FgaError();
            }
        }

        return new ResponseWrapper<TRes> { rawResponse = response, responseContent = responseContent };
    }

    /// <summary>
    ///     Handles calling the API
    /// </summary>
    /// <param name="request"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<ResponseWrapper<T>> SendRequestAsync<T>(HttpRequestMessage request,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders) {
                if (header.Value != null) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

        try {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) {
            throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
        }

        T responseContent = default;
        if (response.Content != null && response.StatusCode != HttpStatusCode.NoContent) {
            var contentString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            // Guard against empty or whitespace-only responses before attempting JSON deserialization
            if (!string.IsNullOrWhiteSpace(contentString)) {
                responseContent = JsonSerializer.Deserialize<T>(contentString) ??
                                  throw new FgaError();
            }
        }

        return new ResponseWrapper<T> { rawResponse = response, responseContent = responseContent };
    }

    /// <summary>
    ///     Establishes the HTTP connection for a streaming request by sending the request and validating the
    ///     response status code. The response body stream is not read yet, making this phase safe to retry.
    /// </summary>
    /// <param name="requestBuilder">The request builder (a fresh HttpRequestMessage is created per attempt)</param>
    /// <param name="additionalHeaders">Additional headers to include</param>
    /// <param name="apiName">The API name for metrics and error reporting</param>
    /// <param name="retryCount">The current retry attempt count (0 for the initial request)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TReq">The request body type</typeparam>
    /// <returns>A ResponseWrapper containing the established HttpResponseMessage (caller takes ownership)</returns>
    internal async Task<ResponseWrapper<HttpResponseMessage>> EstablishStreamingConnectionAsync<TReq>(
        RequestBuilder<TReq> requestBuilder,
        IDictionary<string, string>? additionalHeaders,
        string? apiName,
        int retryCount,
        CancellationToken cancellationToken) {

        var request = requestBuilder.BuildRequest();

        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders.Where(h => h.Value != null)) {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        var httpRequestStopwatch = System.Diagnostics.Stopwatch.StartNew();
        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);
        httpRequestStopwatch.Stop();

        if (_metrics != null && apiName != null) {
            _metrics.BuildForHttpRequest(apiName, response, requestBuilder, httpRequestStopwatch, retryCount);
        }

        try {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) {
            using (response) {
                throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
            }
        }

        return new ResponseWrapper<HttpResponseMessage> { rawResponse = response, responseContent = response };
    }

    /// <summary>
    ///     Parses a streaming response from an already-established HTTP response, yielding each item as it arrives.
    ///     This phase should not be retried once started, as partial results may already have been delivered.
    ///     Uses <see cref="StreamReader.ReadLineAsync()"/> which handles internal buffering, partial chunks,
    ///     and last-line-without-newline naturally.
    /// </summary>
    /// <param name="response">The established HTTP response with an open body stream</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">The type of each streamed response object</typeparam>
    /// <returns>An async enumerable of parsed response objects</returns>
    internal async IAsyncEnumerable<T> StreamFromResponseAsync<T>(
        HttpResponseMessage response,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) {

        if (response.Content == null) {
            yield break;
        }

        // Register cancellation token to dispose response and unblock stalled reads on older runtimes
        using var disposeResponseRegistration = cancellationToken.Register(static state => ((HttpResponseMessage)state!).Dispose(), response);

        // Stream and parse the response
#if NET6_0_OR_GREATER
        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#endif
        using var reader = new StreamReader(stream, Encoding.UTF8);

        while (true) {
            string? line;
            try {
#if NET7_0_OR_GREATER
                // ReadLineAsync(CancellationToken) available from .NET 7+
                line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
#else
                line = await reader.ReadLineAsync().ConfigureAwait(false);
#endif
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException("Streaming request was cancelled.", cancellationToken);
            }
            catch (IOException ex) when (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException("Streaming request was cancelled.", ex, cancellationToken);
            }

            if (line == null) {
                break; // End of stream (ReadLineAsync returns null at EOF)
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(line)) {
                continue;
            }

            T? parsedResult = default;
            try {
                using var jsonDoc = JsonDocument.Parse(line);
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("result", out var resultElement)) {
                    parsedResult = resultElement.Deserialize<T>();
                }
            }
            catch (JsonException) {
                // Skip malformed lines — the server may send error envelopes or keep-alive pings
            }

            if (parsedResult != null) {
                yield return parsedResult;
            }
        }
    }

    /// <summary>
    ///     Handles calling the API for streaming responses (e.g., streaming)
    /// </summary>
    /// <param name="request">The HTTP request message</param>
    /// <param name="additionalHeaders">Additional headers to include</param>
    /// <param name="apiName">The API name for error reporting</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">The type of each streamed response object</typeparam>
    /// <returns>An async enumerable of parsed response objects</returns>
    /// <exception cref="ApiException"></exception>
    public async IAsyncEnumerable<T> SendStreamingRequestAsync<T>(
        HttpRequestMessage request,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) {

        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders.Where(header => header.Value != null)) {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        // Use ResponseHeadersRead to start streaming before full response is received
        using var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        try {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException) {
            throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
        }

        await foreach (var item in StreamFromResponseAsync<T>(response, cancellationToken)) {
            yield return item;
        }
    }

    /// <summary>
    ///     Handles calling the API for streaming responses (e.g., streaming) from a RequestBuilder
    /// </summary>
    /// <param name="requestBuilder">The request builder</param>
    /// <param name="additionalHeaders">Additional headers to include</param>
    /// <param name="apiName">The API name for error reporting</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TReq">The request type</typeparam>
    /// <typeparam name="TRes">The response type for each streamed object</typeparam>
    /// <returns>An async enumerable of parsed response objects</returns>
    public IAsyncEnumerable<TRes> SendStreamingRequestAsync<TReq, TRes>(
        RequestBuilder<TReq> requestBuilder,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null,
        CancellationToken cancellationToken = default) {

        var request = requestBuilder.BuildRequest();
        return SendStreamingRequestAsync<TRes>(request, additionalHeaders, apiName, cancellationToken);
    }

    /// <summary>
    ///     Disposes of any owned disposable resources such as the underlying <see cref="HttpClient" /> if owned.
    /// </summary>
    /// <param name="disposing">Whether we are actually disposing (<see langword="true" />) or not (<see langword="false" />).</param>
    protected virtual void Dispose(bool disposing) {
        if (disposing && _shouldDisposeWhenDone) {
            _httpClient.Dispose();
            _shouldDisposeWhenDone = false;
        }
    }

    /// <summary>
    ///     Disposes of any owned disposable resources such as the underlying <see cref="HttpClient" /> if owned.
    /// </summary>
    public void Dispose() => Dispose(true);
}