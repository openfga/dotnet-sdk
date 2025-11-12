using OpenFga.Sdk.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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

    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseClient" /> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpClient">Optional <see cref="HttpClient" /> to use when sending requests.</param>
    /// <remarks>
    ///     If you supply a <see cref="HttpClient" /> it is your responsibility to manage its lifecycle and
    ///     dispose it when appropriate.
    ///     If you do not supply a <see cref="HttpClient" /> one will be created automatically and disposed
    ///     of when this object is disposed.
    /// </remarks>
    public BaseClient(Configuration.Configuration configuration, HttpClient? httpClient = null) {
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
    }

    /// <summary>
    ///     Handles calling the API
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TReq"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    /// <returns></returns>
    public async Task<ResponseWrapper<TRes>> SendRequestAsync<TReq, TRes>(RequestBuilder<TReq> requestBuilder,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        var request = requestBuilder.BuildRequest();

        return await SendRequestAsync<TRes>(request, additionalHeaders, apiName, cancellationToken);
    }

    // /// <summary>
    // /// Handles calling the API for requests that are expected to return no content
    // /// </summary>
    // /// <param name="requestBuilder"></param>
    // /// <param name="additionalHeaders"></param>
    // /// <param name="apiName"></param>
    // /// <param name="cancellationToken"></param>
    // /// <returns></returns>
    // public async Task SendRequestAsync(RequestBuilder requestBuilder,
    //     IDictionary<string, string>? additionalHeaders = null,
    //     string? apiName = null, CancellationToken cancellationToken = default) {
    //     var request = requestBuilder.BuildRequest();
    //
    //     await this.SendRequestAsync(request, additionalHeaders, apiName, cancellationToken);
    // }

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
        {
            try {
                response.EnsureSuccessStatusCode();
            }
            catch {
                throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
            }

            T responseContent = default;
            if (response.Content != null && response.StatusCode != HttpStatusCode.NoContent) {
                using var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                if (contentStream.Length > 0) {
                    responseContent = await JsonSerializer.DeserializeAsync<T>(contentStream, cancellationToken: cancellationToken).ConfigureAwait(false) ??
                                      throw new FgaError();
                }
            }

            return new ResponseWrapper<T> { rawResponse = response, responseContent = responseContent };
        }
    }

    /// <summary>
    ///     Handles calling the API for streaming responses (e.g., NDJSON)
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

        if (response.Content == null) {
            yield break;
        }

        // Register cancellation token to dispose response and unblock stalled reads
        using var disposeResponseRegistration = cancellationToken.Register(static state => ((HttpResponseMessage)state!).Dispose(), response);
        
        // Stream and parse NDJSON response
#if NET6_0_OR_GREATER
        await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#else
        using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
#endif
        using var reader = new StreamReader(stream, Encoding.UTF8);

        while (true) {
            string? line;
            try {
                line = await reader.ReadLineAsync().ConfigureAwait(false);
            }
            catch (ObjectDisposedException) when (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException("Streaming request was cancelled.", cancellationToken);
            }
            catch (IOException ex) when (cancellationToken.IsCancellationRequested) {
                throw new OperationCanceledException("Streaming request was cancelled.", ex, cancellationToken);
            }

            if (line == null) {
                break;
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(line)) {
                continue; // Skip empty lines
            }

            // Parse the NDJSON line - format is: {"result": {"object": "..."}}
            // Note: Cannot use yield inside try-catch, so we parse first then yield
            T? parsedResult = default;

            try {
                using var jsonDoc = JsonDocument.Parse(line);
                var root = jsonDoc.RootElement;

                if (root.TryGetProperty("result", out var resultElement)) {
                    parsedResult = JsonSerializer.Deserialize<T>(resultElement.GetRawText());
                }
            }
            catch (JsonException) {
                // Skip invalid JSON lines - similar to JS SDK behavior
                // In production, malformed lines from the server should be rare
            }

            // Yield outside of try-catch block (C# language requirement)
            if (parsedResult != null) {
                yield return parsedResult;
            }
        }
    }

    /// <summary>
    ///     Handles calling the API for streaming responses (e.g., NDJSON) from a RequestBuilder
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