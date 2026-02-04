using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace OpenFga.Sdk.Client.ApiExecutor;

/// <summary>
/// A fluent builder for constructing API requests to be executed via the ApiExecutor.
/// </summary>
public class ApiExecutorRequestBuilder {
    /// <summary>
    /// Gets the HTTP method for this request.
    /// </summary>
    public HttpMethod Method { get; private set; }

    /// <summary>
    /// Gets the path template for this request (e.g., "/stores/{store_id}/check").
    /// </summary>
    public string PathTemplate { get; private set; }

    private readonly Dictionary<string, string> _pathParams = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _queryParams = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
    private object? _body;

    /// <summary>
    /// Private constructor to enforce usage of the Of() factory method.
    /// </summary>
    private ApiExecutorRequestBuilder() { }

    /// <summary>
    /// Creates a new ApiExecutorRequestBuilder with the specified HTTP method and path.
    /// </summary>
    /// <param name="method">The HTTP method (e.g., GET, POST, PUT, DELETE)</param>
    /// <param name="path">The path template (e.g., "/stores/{store_id}/check")</param>
    /// <returns>A new ApiExecutorRequestBuilder instance</returns>
    /// <exception cref="ArgumentNullException">Thrown when method or path is null</exception>
    /// <exception cref="ArgumentException">Thrown when path is empty or whitespace</exception>
    public static ApiExecutorRequestBuilder Of(HttpMethod method, string path) {
        if (method == null) {
            throw new ArgumentNullException(nameof(method), "HTTP method cannot be null");
        }

        if (path == null) {
            throw new ArgumentNullException(nameof(path), "Path cannot be null");
        }

        if (string.IsNullOrWhiteSpace(path)) {
            throw new ArgumentException("Path cannot be empty or whitespace", nameof(path));
        }

        if (!path.StartsWith("/")) {
            throw new ArgumentException("Path must start with '/'", nameof(path));
        }

        return new ApiExecutorRequestBuilder {
            Method = method,
            PathTemplate = path
        };
    }

    /// <summary>
    /// Adds a path parameter to the request. The parameter will replace {key} in the path template.
    /// </summary>
    /// <param name="key">The parameter name (without braces)</param>
    /// <param name="value">The parameter value</param>
    /// <returns>This builder instance for method chaining</returns>
    /// <exception cref="ArgumentException">Thrown when key is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
    public ApiExecutorRequestBuilder PathParam(string key, string value) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentException("Path parameter key cannot be null, empty, or whitespace", nameof(key));
        }

        if (value == null) {
            throw new ArgumentNullException(nameof(value), $"Path parameter value for key '{key}' cannot be null");
        }

        _pathParams[key] = value;
        return this;
    }

    /// <summary>
    /// Adds a query parameter to the request. The parameter will be appended to the URL as ?key=value.
    /// </summary>
    /// <param name="key">The query parameter name</param>
    /// <param name="value">The query parameter value</param>
    /// <returns>This builder instance for method chaining</returns>
    /// <exception cref="ArgumentException">Thrown when key is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
    public ApiExecutorRequestBuilder QueryParam(string key, string value) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentException("Query parameter key cannot be null, empty, or whitespace", nameof(key));
        }

        if (value == null) {
            throw new ArgumentNullException(nameof(value), $"Query parameter value for key '{key}' cannot be null");
        }

        _queryParams[key] = value;
        return this;
    }

    /// <summary>
    /// Adds a custom header to the request.
    /// </summary>
    /// <param name="key">The header name</param>
    /// <param name="value">The header value</param>
    /// <returns>This builder instance for method chaining</returns>
    /// <exception cref="ArgumentException">Thrown when the header is reserved or invalid</exception>
    public ApiExecutorRequestBuilder Header(string key, string value) {
        // Validate using the existing Configuration.ValidateHeaders method
        var headers = new Dictionary<string, string> { { key, value } };
        Configuration.Configuration.ValidateHeaders(headers, "header");

        _headers[key] = value;
        return this;
    }

    /// <summary>
    /// Sets the request body. The body will be JSON-serialized when sent.
    /// </summary>
    /// <param name="body">The request body object (will be serialized to JSON)</param>
    /// <returns>This builder instance for method chaining</returns>
    public ApiExecutorRequestBuilder Body(object? body) {
        _body = body;
        return this;
    }

    /// <summary>
    /// Validates and finalizes the builder. Call this method before passing to ApiExecutor.
    /// </summary>
    /// <returns>This builder instance</returns>
    /// <exception cref="FgaValidationError">Thrown when the builder state is invalid</exception>
    public ApiExecutorRequestBuilder Build() {
        if (Method == null) {
            throw new FgaValidationError("HTTP method must be specified. Use ApiExecutorRequestBuilder.Of() to create a builder.");
        }

        if (string.IsNullOrWhiteSpace(PathTemplate)) {
            throw new FgaValidationError("Path template must be specified. Use ApiExecutorRequestBuilder.Of() to create a builder.");
        }

        return this;
    }

    /// <summary>
    /// Converts this ApiExecutorRequestBuilder to an internal RequestBuilder for API execution.
    /// </summary>
    /// <param name="basePath">The base path/URL for the API</param>
    /// <returns>A RequestBuilder configured for the API client</returns>
    internal RequestBuilder<object> ToRequestBuilder(string basePath) {
        return new RequestBuilder<object> {
            Method = this.Method,
            BasePath = basePath,
            PathTemplate = this.PathTemplate,
            PathParameters = new Dictionary<string, string>(_pathParams),
            QueryParameters = new Dictionary<string, string>(_queryParams),
            Body = _body
        };
    }

    /// <summary>
    /// Gets the custom headers for this request.
    /// </summary>
    /// <returns>A dictionary of custom headers</returns>
    internal IDictionary<string, string> GetHeaders() {
        return new Dictionary<string, string>(_headers);
    }
}