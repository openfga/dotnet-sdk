using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Configuration;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Client.ApiExecutor;

/// <summary>
/// Provides the ability to execute arbitrary API requests against the OpenFGA API.
/// Automatically leverages SDK authentication, retry logic, and error handling.
/// </summary>
public class ApiExecutor : IDisposable {
    private readonly ApiClient.ApiClient _apiClient;
    private readonly Configuration.Configuration _configuration;

    /// <summary>
    /// Initializes a new instance of the ApiExecutor class.
    /// </summary>
    /// <param name="apiClient">The API client for handling requests</param>
    /// <param name="configuration">The SDK configuration</param>
    internal ApiExecutor(
        ApiClient.ApiClient apiClient,
        Configuration.Configuration configuration) {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Sends an API request and returns a typed response.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to</typeparam>
    /// <param name="request">The request builder containing the request details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse containing status, headers, raw response, and typed data</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="Exceptions.FgaValidationError">Thrown when the request is invalid</exception>
    /// <exception cref="Exceptions.FgaApiError">Thrown when the API returns an error response</exception>
    public async Task<ApiResponse<T>> SendAsync<T>(
        ApiExecutorRequestBuilder request,
        CancellationToken cancellationToken = default) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        // Validate the request
        request.Build();

        // Convert to internal RequestBuilder
        var requestBuilder = request.ToRequestBuilder(_configuration.BasePath);

        // Get custom headers from the request
        var customHeaders = request.GetHeaders();

        // Send the request and get the full ResponseWrapper
        var responseWrapper = await _apiClient.SendRequestWithWrapperAsync<object, T>(
            requestBuilder,
            "ApiExecutor",
            customHeaders,
            cancellationToken);

        // Read the raw response body
        var rawResponse = await responseWrapper.rawResponse.Content.ReadAsStringAsync();

        // Create and return ApiResponse
        return ApiResponse<T>.FromHttpResponse(
            responseWrapper.rawResponse,
            rawResponse,
            responseWrapper.responseContent);
    }

    /// <summary>
    /// Sends an API request and returns a response with raw JSON string as data.
    /// Useful when you want to process the JSON response manually.
    /// </summary>
    /// <param name="request">The request builder containing the request details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse with the raw JSON response as the Data property</returns>
    /// <exception cref="ArgumentNullException">Thrown when request is null</exception>
    /// <exception cref="Exceptions.FgaValidationError">Thrown when the request is invalid</exception>
    /// <exception cref="Exceptions.FgaApiError">Thrown when the API returns an error response</exception>
    public async Task<ApiResponse<string>> SendAsync(
        ApiExecutorRequestBuilder request,
        CancellationToken cancellationToken = default) {
        if (request == null) {
            throw new ArgumentNullException(nameof(request));
        }

        // Validate the request
        request.Build();

        // Convert to internal RequestBuilder
        var requestBuilder = request.ToRequestBuilder(_configuration.BasePath);

        // Get custom headers from the request
        var customHeaders = request.GetHeaders();

        // Send the request and get the full ResponseWrapper
        // Use JsonElement as intermediate type to avoid double deserialization
        var responseWrapper = await _apiClient.SendRequestWithWrapperAsync<object, JsonElement>(
            requestBuilder,
            "ApiExecutor",
            customHeaders,
            cancellationToken);

        // Read the raw response body
        var rawResponse = await responseWrapper.rawResponse.Content.ReadAsStringAsync();

        // Create and return ApiResponse with raw JSON as both RawResponse and Data
        return ApiResponse<string>.FromHttpResponse(
            responseWrapper.rawResponse,
            rawResponse,
            rawResponse);
    }

    /// <summary>
    /// Disposes of resources used by the ApiExecutor.
    /// </summary>
    public void Dispose() {
        // ApiClient is owned by OpenFgaApi, so we don't dispose it here
        GC.SuppressFinalize(this);
    }
}
