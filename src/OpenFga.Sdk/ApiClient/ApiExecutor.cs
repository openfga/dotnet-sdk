using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// Provides methods to execute custom API requests against the OpenFGA API.
/// Use this when you need to call endpoints not yet available in the SDK's typed methods,
/// or when you need access to full response details (status code, headers, raw response).
/// </summary>
public class ApiExecutor {
    private readonly ApiClient _apiClient;

    internal ApiExecutor(ApiClient apiClient) {
        _apiClient = apiClient;
    }

    /// <summary>
    /// Executes an API request using RequestBuilder and returns an ApiResponse with full response details.
    /// This provides a lower-level API for custom requests while leveraging authentication, retry logic, and error handling.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request body</typeparam>
    /// <typeparam name="TResponse">The type of the response body. Must be a type that can be deserialized from JSON.</typeparam>
    /// <param name="requestBuilder">The request builder containing request details (path, parameters, body, etc.)</param>
    /// <param name="apiName">The API name for telemetry and error reporting</param>
    /// <param name="options">Optional request options including custom headers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse containing status code, headers, raw response, and typed data</returns>
    /// <exception cref="Exceptions.FgaApiAuthenticationError">Thrown when authentication fails</exception>
    /// <exception cref="Exceptions.FgaApiError">Thrown when the API returns an error response</exception>
    /// <example>
    /// <code>
    /// var executor = client.ApiExecutor;
    /// var request = RequestBuilder&lt;object&gt;
    ///     .Create(HttpMethod.Get, config.ApiUrl, "/stores/{store_id}")
    ///     .WithPathParameter("store_id", storeId);
    /// var response = await executor.ExecuteAsync&lt;object, GetStoreResponse&gt;(request, "GetStore");
    /// if (response.IsSuccessful) {
    ///     Console.WriteLine($"Store: {response.Data.Name}");
    /// }
    /// </code>
    /// </example>
    public async Task<ApiResponse<TResponse>> ExecuteAsync<TRequest, TResponse>(
        RequestBuilder<TRequest> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        CancellationToken cancellationToken = default) {
        return await _apiClient.ExecuteAsync<TRequest, TResponse>(
            requestBuilder,
            apiName,
            options,
            cancellationToken);
    }

    /// <summary>
    /// Executes an API request using RequestBuilder and returns an ApiResponse with raw JSON string.
    /// This variant is useful when you want to process the JSON response manually without deserialization.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request body</typeparam>
    /// <param name="requestBuilder">The request builder containing request details</param>
    /// <param name="apiName">The API name for telemetry and error reporting</param>
    /// <param name="options">Optional request options including custom headers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An ApiResponse with the raw JSON response as the Data property</returns>
    /// <exception cref="Exceptions.FgaApiAuthenticationError">Thrown when authentication fails</exception>
    /// <exception cref="Exceptions.FgaApiError">Thrown when the API returns an error response</exception>
    /// <example>
    /// <code>
    /// var executor = client.ApiExecutor;
    /// var request = new RequestBuilder&lt;object&gt; { /* ... */ };
    /// var response = await executor.ExecuteAsync(request, "CustomEndpoint");
    /// string rawJson = response.Data; // Raw JSON string
    /// </code>
    /// </example>
    public async Task<ApiResponse<string>> ExecuteAsync<TRequest>(
        RequestBuilder<TRequest> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        CancellationToken cancellationToken = default) {
        return await _apiClient.ExecuteAsync(
            requestBuilder,
            apiName,
            options,
            cancellationToken);
    }

    /// <summary>
    /// Executes a streaming API request using RequestBuilder and returns an IAsyncEnumerable of response items.
    /// Use this for endpoints that stream results (e.g., /stores/{store_id}/streamed-list-objects).
    /// Note: Streaming responses cannot be retried once the stream has started.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request body</typeparam>
    /// <typeparam name="TResponse">The type of each streamed response item</typeparam>
    /// <param name="requestBuilder">The request builder containing request details (path, body, etc.)</param>
    /// <param name="apiName">The API name for telemetry and error reporting</param>
    /// <param name="options">Optional request options including custom headers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An async enumerable that yields response items as they are received from the server</returns>
    /// <exception cref="Exceptions.FgaApiAuthenticationError">Thrown when authentication fails</exception>
    /// <exception cref="Exceptions.FgaApiError">Thrown when the API returns an error response</exception>
    /// <example>
    /// <code>
    /// var executor = client.ApiExecutor;
    /// var request = RequestBuilder&lt;object&gt;
    ///     .Create(HttpMethod.Post, config.ApiUrl, "/stores/{store_id}/streamed-list-objects")
    ///     .WithPathParameter("store_id", storeId)
    ///     .WithBody(new { user = "user:anne", relation = "viewer", type = "document" });
    /// await foreach (var item in executor.ExecuteStreamingAsync&lt;object, StreamedListObjectsResponse&gt;(request, "StreamedListObjects")) {
    ///     Console.WriteLine(item.Object);
    /// }
    /// </code>
    /// </example>
    public IAsyncEnumerable<TResponse> ExecuteStreamingAsync<TRequest, TResponse>(
        RequestBuilder<TRequest> requestBuilder,
        string apiName,
        IRequestOptions? options = null,
        CancellationToken cancellationToken = default) {
        return _apiClient.SendStreamingRequestAsync<TRequest, TResponse>(
            requestBuilder, apiName, options, cancellationToken);
    }
}