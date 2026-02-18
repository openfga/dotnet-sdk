using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// Represents the response from an API request executed via ApiClient.ExecuteAsync,
/// containing both raw and typed response data.
/// </summary>
/// <typeparam name="T">The type of the deserialized response data</typeparam>
public class ApiResponse<T> {
    /// <summary>
    /// Gets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the response headers as a read-only dictionary.
    /// Keys are case-insensitive. Includes both response and content headers.
    /// </summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    /// <summary>
    /// Gets the raw response body as a string.
    /// </summary>
    public string RawResponse { get; }

    /// <summary>
    /// Gets the deserialized response data.
    /// Will be null if deserialization fails or if the response has no content.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Gets whether the response indicates success (2xx status code).
    /// </summary>
    public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode < 300;

    /// <summary>
    /// Initializes a new instance of the ApiResponse class.
    /// </summary>
    /// <param name="statusCode">The HTTP status code</param>
    /// <param name="headers">The response headers</param>
    /// <param name="rawResponse">The raw response body</param>
    /// <param name="data">The deserialized response data</param>
    internal ApiResponse(
        HttpStatusCode statusCode,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        string rawResponse,
        T? data) {
        StatusCode = statusCode;
        Headers = headers;
        RawResponse = rawResponse;
        Data = data;
    }

    /// <summary>
    /// Creates an ApiResponse from an HttpResponseMessage.
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    /// <param name="rawResponse">The raw response body as a string</param>
    /// <param name="data">The deserialized response data</param>
    /// <returns>A new ApiResponse instance</returns>
    internal static ApiResponse<T> FromHttpResponse(
        HttpResponseMessage response,
        string rawResponse,
        T? data) {
        var headers = ConvertHeaders(response);
        return new ApiResponse<T>(
            response.StatusCode,
            headers,
            rawResponse,
            data);
    }

    /// <summary>
    /// Converts HttpResponseMessage headers to a case-insensitive dictionary.
    /// Includes both response headers and content headers.
    /// </summary>
    /// <param name="response">The HTTP response message</param>
    /// <returns>A read-only dictionary of headers</returns>
    private static IReadOnlyDictionary<string, IEnumerable<string>> ConvertHeaders(
        HttpResponseMessage response) {
        var headers = new Dictionary<string, IEnumerable<string>>(
            StringComparer.OrdinalIgnoreCase);

        // Add response headers
        foreach (var header in response.Headers) {
            headers[header.Key] = header.Value;
        }

        // Add content headers if present
        if (response.Content?.Headers != null) {
            foreach (var header in response.Content.Headers) {
                headers[header.Key] = header.Value;
            }
        }

        return headers;
    }
}

