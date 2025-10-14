using OpenFga.Sdk.Exceptions.Parsers;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Exceptions;

public class FgaApiError : ApiException {
    /// <summary>
    /// Whether this Error should be retried
    /// </summary>
    public readonly bool ShouldRetry;

    /// <summary>
    /// The name of the API endpoint.
    /// </summary>
    [JsonPropertyName("api_name")]
    public string? ApiName { get; internal set; }

    /// <summary>
    /// The request method
    /// </summary>
    [JsonPropertyName("method")]
    public HttpMethod? Method { get; internal set; }

    /// <summary>
    /// The request URL.
    /// </summary>
    [JsonPropertyName("request_url")]
    public string? RequestUrl { get; internal set; }

    /// <summary>
    /// The request URL.
    /// </summary>
    [JsonPropertyName("store_id")]
    public string? StoreId { get; internal set; }

    /// <summary>
    /// The request data sent to the API
    /// </summary>
    [JsonPropertyName("request_data")]
    public HttpContent? RequestData { get; internal set; }

    /// <summary>
    /// The response data received from the API
    /// </summary>
    [JsonPropertyName("response_data")]
    public HttpContent? ResponseData { get; internal set; }

    /// <summary>
    /// The request headers sent to the API
    /// </summary>
    [JsonPropertyName("request_headers")]
    public HttpRequestHeaders RequestHeaders { get; internal set; }

    /// <summary>
    /// The response headers received from the API
    /// </summary>
    [JsonPropertyName("response_headers")]
    public HttpResponseHeaders? ResponseHeaders { get; internal set; }

    /// <summary>
    /// Optional <see cref="ApiError"/> from the failing API call.
    /// </summary>
    [JsonPropertyName("api_error")]
    public ApiErrorParser ApiError { get; }

    /// <summary>
    /// <see cref="HttpStatusCode"/> code from the failing API call.
    /// </summary>
    [JsonPropertyName("status_code")]
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Parsed Retry-After header value in seconds, if present in the response.
    /// This indicates how long the client should wait before retrying the request.
    /// </summary>
    [JsonPropertyName("retry_after")]
    public int? RetryAfter { get; internal set; }

    /// <summary>
    /// Raw Retry-After header value from the response, if present.
    /// May be in integer (seconds) or HTTP-date format.
    /// </summary>
    [JsonPropertyName("retry_after_raw")]
    public string? RetryAfterRaw { get; internal set; }

    /// <summary>
    /// Legacy X-Rate-Limit-Reset header value (Unix timestamp), if present.
    /// This is deprecated for retry logic but exposed for logging and debugging.
    /// </summary>
    [JsonPropertyName("x_rate_limit_reset")]
    public long? XRateLimitReset { get; internal set; }

    /// <summary>
    /// The retry attempt number when this error occurred (0 = initial request, 1+ = retry attempts).
    /// </summary>
    [JsonPropertyName("retry_attempt")]
    public int RetryAttempt { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaApiError"/> class.
    /// </summary>
    public FgaApiError() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitApiException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public FgaApiError(string message) : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaApiError"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.</param>
    public FgaApiError(string message, Exception innerException)
        : base(message, innerException) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaApiError"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="statusCode"><see cref="HttpStatusCode"/>code of the failing API call.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.</param>
    /// <param name="shouldRetry"></param>
    public FgaApiError(HttpStatusCode statusCode, string message, Exception innerException, bool shouldRetry = false)
        : base(message, innerException) {
        StatusCode = statusCode;
        ShouldRetry = shouldRetry;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified
    /// </summary>
    /// <param name="statusCode"><see cref="HttpStatusCode"/>code of the failing API call.</param>
    /// <param name="apiError">Optional <see cref="ApiErrorParser"/> of the failing API call.</param>
    public FgaApiError(HttpStatusCode statusCode, ApiErrorParser? apiError = null)
        : this((apiError == null ? statusCode.ToString() : apiError.Message) ?? string.Empty) {
        StatusCode = statusCode;
        ApiError = apiError ?? new ApiErrorParser();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified
    /// </summary>
    /// <param name="response"></param>
    /// <param name="request"></param>
    /// <param name="apiName"></param>
    /// <param name="apiError">Optional <see cref="ApiErrorParser"/> of the failing API call.</param>
    /// <param name="shouldRetry"></param>
    public FgaApiError(HttpResponseMessage? response, HttpRequestMessage request, string? apiName, ApiErrorParser? apiError = null, bool shouldRetry = false)
        : this((apiError == null ? (response?.StatusCode.ToString()) : apiError.Message) ?? string.Empty) {
        var requestPaths = request.RequestUri?.LocalPath.Split('/') ?? Array.Empty<string>();
        var storeId = requestPaths.Length > 2 ? requestPaths[2] : null;

        StatusCode = response?.StatusCode ?? HttpStatusCode.InternalServerError;
        ApiError = apiError ?? new ApiErrorParser();
        StoreId = storeId;
        ApiName = apiName;
        Method = request.Method;
        RequestUrl = request.RequestUri?.ToString();
        RequestData = request.Content;
        RequestHeaders = request.Headers;
        ResponseData = response?.Content;
        ResponseHeaders = response?.Headers;
        ShouldRetry = shouldRetry;
    }

    internal static async Task<FgaApiError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new FgaApiError(response, request, apiName, await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}