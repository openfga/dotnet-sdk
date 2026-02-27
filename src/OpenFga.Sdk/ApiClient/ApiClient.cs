using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using OpenFga.Sdk.Telemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
///     API Client - used by all the API related methods to call the API. Handles token exchange and retries.
/// </summary>
public class ApiClient : IDisposable {
    private readonly BaseClient _baseClient;
    private readonly Configuration.Configuration _configuration;
    private readonly OAuth2Client? _oauth2Client;
    private readonly Metrics metrics;
    private readonly RetryHandler _retryHandler;
    private readonly Lazy<ApiExecutor> _apiExecutor;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiClient" /> class.
    /// </summary>
    /// <param name="configuration">Client Configuration</param>
    /// <param name="userHttpClient">User Http Client - Allows Http Client reuse</param>
    public ApiClient(Configuration.Configuration configuration, HttpClient? userHttpClient = null) {
        configuration.EnsureValid();
        _configuration = configuration;
        metrics = new Metrics(_configuration);
        _baseClient = new BaseClient(configuration, userHttpClient, metrics);
        _retryHandler = new RetryHandler(new RetryParams { MaxRetry = _configuration.MaxRetry, MinWaitInMs = _configuration.MinWaitInMs });
        _apiExecutor = new Lazy<ApiExecutor>(() => new ApiExecutor(this));

        if (_configuration.Credentials == null) {
            return;
        }

        switch (_configuration.Credentials.Method) {
            case CredentialsMethod.ApiToken:
                // ApiToken authorization is handled in BuildHeaders method to avoid
                // modifying DefaultHeaders with reserved headers
                break;
            case CredentialsMethod.ClientCredentials:
                _oauth2Client = new OAuth2Client(_configuration.Credentials, _baseClient,
                    new RetryParams { MaxRetry = _configuration.MaxRetry, MinWaitInMs = _configuration.MinWaitInMs },
                    metrics);
                break;
            case CredentialsMethod.None:
            default:
                break;
        }
    }

    /// <summary>
    /// Gets the ApiExecutor for making custom API requests.
    /// Use this when you need to call OpenFGA API endpoints not yet available in the SDK's typed methods,
    /// or when you need access to full response details (status code, headers, raw response).
    /// </summary>
    /// <example>
    /// <code>
    /// var executor = apiClient.ApiExecutor;
    /// var request = RequestBuilder&lt;object&gt;
    ///     .Create(HttpMethod.Get, config.ApiUrl, "/stores/{store_id}")
    ///     .WithPathParameter("store_id", storeId);
    /// var response = await executor.ExecuteAsync&lt;object, GetStoreResponse&gt;(request, "GetStore");
    /// </code>
    /// </example>
    public ApiExecutor ApiExecutor => _apiExecutor.Value;

    /// <summary>
    ///     Gets the authentication token based on the configured credentials method.
    ///     For OAuth (ClientCredentials), fetches token from OAuth2Client.
    ///     For ApiToken, returns the configured token directly.
    /// </summary>
    /// <param name="apiName">The API name for error reporting</param>
    /// <returns>The authentication token, or null if no credentials are configured</returns>
    /// <exception cref="FgaApiAuthenticationError">Thrown when OAuth token exchange fails</exception>
    private async Task<string?> GetAuthenticationTokenAsync(string apiName) {
        if (_oauth2Client != null) {
            try {
                return await _oauth2Client.GetAccessTokenAsync();
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        if (_configuration.Credentials?.Method == CredentialsMethod.ApiToken) {
            return _configuration.Credentials.Config?.ApiToken;
        }

        return null;
    }


    /// <summary>
    ///     Handles streaming requests that return IAsyncEnumerable.
    ///     Note: Streaming responses cannot be retried once the stream has started.
    /// </summary>
    /// <param name="requestBuilder">The request builder</param>
    /// <param name="apiName">The API name for error reporting and telemetry</param>
    /// <param name="options">Request options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="TReq">Request type</typeparam>
    /// <typeparam name="TRes">Response type for each streamed object</typeparam>
    /// <returns>An async enumerable of response objects</returns>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async IAsyncEnumerable<TRes> SendStreamingRequestAsync<TReq, TRes>(
        RequestBuilder<TReq> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) {

        var authToken = await GetAuthenticationTokenAsync(apiName);
        var additionalHeaders = BuildHeaders(_configuration, authToken, options);
        var streamIter = _baseClient.SendStreamingRequestAsync<TReq, TRes>(
            requestBuilder, additionalHeaders, apiName, cancellationToken);

        await foreach (var item in streamIter) {
            yield return item;
        }
    }

    private async Task<ResponseWrapper<TResult>> Retry<TResult>(Func<int, Task<ResponseWrapper<TResult>>> retryable) {
        var requestCount = 0;
        var attemptCount = 0; // 0 = initial request, 1+ = retry attempts

        while (true) {
            try {
                requestCount++;

                var response = await retryable(attemptCount);

                response.retryCount =
                    requestCount - 1; // OTEL spec specifies that the original request is not included in the count

                return response;
            }
            catch (FgaApiError err) when (err is FgaApiRateLimitExceededError || err.ShouldRetry) {
                // Check if we should retry based on status code and attempt count
                if (!_retryHandler.ShouldRetry(err.StatusCode, attemptCount)) {
                    // Populate retry metadata before throwing
                    PopulateRetryMetadata(err, attemptCount);
                    throw;
                }

                // Calculate delay using Retry-After header or exponential backoff
                var delay = _retryHandler.CalculateDelayFromHeaders(err.ResponseHeaders, attemptCount);

                await Task.Delay(delay);
                attemptCount++;
            }
            catch (Exception ex) when (_retryHandler.IsTransientError(ex, attemptCount)) {
                // Network error - retry with exponential backoff (no headers available)
                var delay = _retryHandler.CalculateDelayFromHeaders(null, attemptCount);
                await Task.Delay(delay);
                attemptCount++;
            }
        }
    }

    /// <summary>
    ///     Builds the complete headers dictionary by merging default headers, auth token, and per-request headers.
    ///     Validates per-request headers and performs case-insensitive merging.
    ///     Header precedence (lowest to highest): DefaultHeaders → Auth token (OAuth or ApiToken) → Per-request headers
    /// </summary>
    /// <param name="configuration">Configuration containing default headers</param>
    /// <param name="authToken">Authorization token (OAuth or ApiToken) if available</param>
    /// <param name="options">Request options containing custom headers</param>
    /// <returns>Merged headers dictionary or null if no headers to add</returns>
    /// <exception cref="ArgumentException">Thrown when header key is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentNullException">Thrown when header value is null</exception>
    private static IDictionary<string, string>? BuildHeaders(Configuration.Configuration configuration, string? authToken, IRequestOptions? options) {
        var defaultHeaders = configuration.DefaultHeaders;
        var perRequestHeaders = options?.Headers;

        // Validate per-request headers
        Configuration.Configuration.ValidateHeaders(perRequestHeaders, "options.Headers");

        // Return empty dictionary if no headers to add
        if (string.IsNullOrEmpty(authToken) &&
            (defaultHeaders == null || defaultHeaders.Count == 0) &&
            (perRequestHeaders == null || perRequestHeaders.Count == 0)) {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        // Use case-insensitive dictionary for proper header merging
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Default headers from configuration, if set
        if (defaultHeaders != null) {
            foreach (var header in defaultHeaders) {
                headers[header.Key] = header.Value;
            }
        }

        // Authorization token header, if set
        if (!string.IsNullOrEmpty(authToken)) {
            headers["Authorization"] = $"Bearer {authToken}";
        }

        // Per-request headers, if set
        if (perRequestHeaders != null) {
            foreach (var header in perRequestHeaders) {
                headers[header.Key] = header.Value;
            }
        }

        return headers;
    }

    /// <summary>
    /// Populates retry-related metadata in an exception before it's thrown.
    /// </summary>
    private void PopulateRetryMetadata(FgaApiError error, int attemptCount) {
        error.RetryAttempt = attemptCount;

        if (error.ResponseHeaders != null) {
            var retryInfo = _retryHandler.ExtractRetryAfterInfoFromHeaders(error.ResponseHeaders);
            error.RetryAfter = retryInfo.retryAfterSeconds;
            error.RetryAfterRaw = retryInfo.retryAfterRaw;
        }
    }

    /// <summary>
    ///     Executes an API request using RequestBuilder and returns an ApiResponse with full response details.
    /// </summary>
    /// <typeparam name="TReq">The type of the request body</typeparam>
    /// <typeparam name="TRes">The type of the response body</typeparam>
    /// <param name="requestBuilder">The request builder containing request details</param>
    /// <param name="apiName">The API name for telemetry and error reporting</param>
    /// <param name="options">Request options including custom headers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse containing status code, headers, raw response, and typed data</returns>
    /// <exception cref="FgaApiAuthenticationError">Thrown when authentication fails</exception>
    /// <exception cref="FgaApiError">Thrown when the API returns an error response</exception>
    public async Task<ApiResponse<TRes>> ExecuteAsync<TReq, TRes>(
        RequestBuilder<TReq> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        CancellationToken cancellationToken = default) {

        var responseWrapper = await SendRequestInternalAsync<TReq, TRes>(
            requestBuilder, apiName, options, cancellationToken);

        var rawResponse = responseWrapper.rawResponse.Content != null
            ? await responseWrapper.rawResponse.Content.ReadAsStringAsync().ConfigureAwait(false)
            : string.Empty;

        return ApiResponse<TRes>.FromHttpResponse(
            responseWrapper.rawResponse,
            rawResponse,
            responseWrapper.responseContent);
    }

    /// <summary>
    ///     Executes an API request using RequestBuilder and returns an ApiResponse with raw JSON string.
    ///     This variant is useful when you want to process the JSON response manually.
    /// </summary>
    /// <typeparam name="TReq">The type of the request body</typeparam>
    /// <param name="requestBuilder">The request builder containing request details</param>
    /// <param name="apiName">The API name for telemetry and error reporting</param>
    /// <param name="options">Request options including custom headers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse with the raw JSON response as the Data property</returns>
    /// <exception cref="FgaApiAuthenticationError">Thrown when authentication fails</exception>
    /// <exception cref="FgaApiError">Thrown when the API returns an error response</exception>
    public async Task<ApiResponse<string>> ExecuteAsync<TReq>(
        RequestBuilder<TReq> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        CancellationToken cancellationToken = default) {

        // Use object as intermediate type to avoid strong typing
        var responseWrapper = await SendRequestInternalAsync<TReq, object>(
            requestBuilder, apiName, options, cancellationToken);

        var rawResponse = responseWrapper.rawResponse.Content != null
            ? await responseWrapper.rawResponse.Content.ReadAsStringAsync().ConfigureAwait(false)
            : string.Empty;

        return ApiResponse<string>.FromHttpResponse(
            responseWrapper.rawResponse,
            rawResponse,
            rawResponse);
    }

    /// <summary>
    ///     Core private method that handles authentication, retry logic, and metrics.
    ///     ExecuteAsync builds on top of this shared implementation.
    /// </summary>
    private async Task<ResponseWrapper<TRes>> SendRequestInternalAsync<TReq, TRes>(
        RequestBuilder<TReq> requestBuilder,
        string apiName,
        IRequestOptions? options,
        CancellationToken cancellationToken) {

        var sw = Stopwatch.StartNew();

        var authToken = await GetAuthenticationTokenAsync(apiName);
        var additionalHeaders = BuildHeaders(_configuration, authToken, options);

        var response = await Retry(async (attemptCount) =>
            await _baseClient.SendRequestAsync<TReq, TRes>(requestBuilder, additionalHeaders, apiName,
                attemptCount, cancellationToken));

        sw.Stop();
        metrics.BuildForResponse(apiName, response.rawResponse, requestBuilder, sw,
            response.retryCount);

        return response;
    }

    public void Dispose() => _baseClient.Dispose();
}