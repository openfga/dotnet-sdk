using OpenFga.Sdk.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// Builder for constructing HTTP requests with support for path parameters, query parameters, and request bodies.
/// Can be used with object initializer syntax or fluent builder methods.
/// </summary>
/// <typeparam name="TReq">Type of the Request Body</typeparam>
public class RequestBuilder<TReq> {
    public RequestBuilder() {
        PathParameters = new Dictionary<string, string>();
        QueryParameters = new Dictionary<string, string>();
    }

    public HttpMethod Method { get; set; }
    public string BasePath { get; set; }
    public string PathTemplate { get; set; }

    public Dictionary<string, string> PathParameters { get; set; }

    public Dictionary<string, string> QueryParameters { get; set; }

    public TReq? Body { get; set; }

    /// <summary>
    /// Creates a new RequestBuilder with the specified HTTP method, base path, and path template.
    /// This is an alternative to object initializer syntax that provides better validation.
    /// </summary>
    /// <param name="method">The HTTP method (e.g., GET, POST, PUT, DELETE)</param>
    /// <param name="basePath">The base URL for the API (e.g., "https://api.fga.example")</param>
    /// <param name="pathTemplate">The path template (e.g., "/stores/{store_id}/check")</param>
    /// <returns>A new RequestBuilder instance</returns>
    /// <exception cref="ArgumentNullException">Thrown when method, basePath, or pathTemplate is null</exception>
    /// <exception cref="ArgumentException">Thrown when basePath or pathTemplate is empty or whitespace</exception>
    public static RequestBuilder<TReq> Create(HttpMethod method, string basePath, string pathTemplate) {
        if (method == null) {
            throw new ArgumentNullException(nameof(method), "HTTP method cannot be null");
        }

        if (basePath == null) {
            throw new ArgumentNullException(nameof(basePath), "Base path cannot be null");
        }

        if (string.IsNullOrWhiteSpace(basePath)) {
            throw new ArgumentException("Base path cannot be empty or whitespace", nameof(basePath));
        }

        if (pathTemplate == null) {
            throw new ArgumentNullException(nameof(pathTemplate), "Path template cannot be null");
        }

        if (string.IsNullOrWhiteSpace(pathTemplate)) {
            throw new ArgumentException("Path template cannot be empty or whitespace", nameof(pathTemplate));
        }

        return new RequestBuilder<TReq> {
            Method = method,
            BasePath = basePath,
            PathTemplate = pathTemplate
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
    public RequestBuilder<TReq> WithPathParameter(string key, string value) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentException("Path parameter key cannot be null, empty, or whitespace", nameof(key));
        }

        if (value == null) {
            throw new ArgumentNullException(nameof(value), $"Path parameter value for key '{key}' cannot be null");
        }

        PathParameters[key] = value;
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
    public RequestBuilder<TReq> WithQueryParameter(string key, string value) {
        if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentException("Query parameter key cannot be null, empty, or whitespace", nameof(key));
        }

        if (value == null) {
            throw new ArgumentNullException(nameof(value), $"Query parameter value for key '{key}' cannot be null");
        }

        QueryParameters[key] = value;
        return this;
    }

    /// <summary>
    /// Sets the request body. The body will be JSON-serialized when sent.
    /// </summary>
    /// <param name="body">The request body object (will be serialized to JSON)</param>
    /// <returns>This builder instance for method chaining</returns>
    public RequestBuilder<TReq> WithBody(TReq? body) {
        Body = body;
        return this;
    }

    public string? JsonBody => Body == null ? null : JsonSerializer.Serialize(Body);

    public HttpContent? FormEncodedBody {
        get {
            if (Body == null) {
                return null;
            }

            if (ContentType != "application/x-www-form-urlencode") {
                throw new Exception(
                    "Content type must be \"application/x-www-form-urlencode\" in order to get the FormEncoded representation");
            }

            var body = (IDictionary<string, string>)Body;

            return new FormUrlEncodedContent(body.Select(p =>
                new KeyValuePair<string, string>(p.Key, p.Value ?? "")));
        }
    }

    private HttpContent? HttpContentBody =>
        Body == null ? null :
        ContentType == "application/json" ? new StringContent(JsonBody, Encoding.UTF8, ContentType) : FormEncodedBody;

    public string ContentType { get; set; } = "application/json";

    public string BuildPathString() {
        if (PathTemplate == null) {
            throw new FgaRequiredParamError("RequestBuilder.BuildUri", nameof(PathTemplate));
        }

        var path = PathTemplate;
        if (PathParameters == null || PathParameters.Count < 1) {
            return path;
        }

        foreach (var parameter in PathParameters) {
            path = path.Replace("{" + parameter.Key + "}", HttpUtility.UrlEncode(parameter.Value));
        }

        return path;
    }

    public string BuildQueryParamsString() {
        if (QueryParameters == null || QueryParameters.Count < 1) {
            return "";
        }

        var query = "?";
        foreach (var parameter in QueryParameters) {
            query = query + parameter.Key + "=" + HttpUtility.UrlEncode(parameter.Value) + "&";
        }

        return query;
    }

    public Uri BuildUri() {
        if (BasePath == null) {
            throw new FgaRequiredParamError("RequestBuilder.BuildUri", nameof(BasePath));
        }

        var uriString = $"{BasePath}";

        uriString += BuildPathString();
        uriString += BuildQueryParamsString();

        return new Uri(uriString);
    }

    public HttpRequestMessage BuildRequest() {
        if (Method == null) {
            throw new FgaRequiredParamError("RequestBuilder.BuildRequest", nameof(Method));
        }

        return new HttpRequestMessage { RequestUri = BuildUri(), Method = Method, Content = HttpContentBody };
    }
}