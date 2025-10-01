//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OpenFga.Sdk.Telemetry;

/// <summary>
///     Common attribute (tag) names.
///     For why `static readonly` over `const`, see https://github.com/dotnet/aspnetcore/pull/12441/files
/// </summary>
public static class TelemetryAttribute {
    // Attributes (tags) associated with the request made //

    /// <summary>
    ///     The FGA method/action that was performed (e.g. `Check`, `ListObjects`, ...) in TitleCase.
    /// </summary>
    public static readonly string RequestMethod = "fga-client.request.method";

    /// <summary>
    ///     The store ID that was sent as part of the request.
    /// </summary>
    public static readonly string RequestStoreId = "fga-client.request.store_id";

    /// <summary>
    ///     The authorization model ID that was sent as part of the request, if any.
    /// </summary>
    public static readonly string RequestModelId = "fga-client.request.model_id";

    /// <summary>
    ///     The client ID associated with the request, if any.
    /// </summary>
    public static readonly string RequestClientId = "fga-client.request.client_id";

    // Attributes (tags) associated with the response //

    /// <summary>
    ///     The authorization model ID that the FGA server used.
    /// </summary>
    public static readonly string ResponseModelId = "fga-client.response.model_id";

    // Attributes (tags) associated with specific actions //

    /// <summary>
    ///     The user that is associated with the action of the request for check and list objects.
    /// </summary>
    public static readonly string FgaRequestUser = "fga-client.user";

    // OTEL Semantic Attributes (tags) //

    /// <summary>
    ///     The HTTP method for the request.
    /// </summary>
    public static readonly string HttpMethod = "http.request.method";

    /// <summary>
    ///     The status code of the response.
    /// </summary>
    public static readonly string HttpStatus = "http.response.status_code";

    /// <summary>
    ///     Host identifier of the origin the request was sent to.
    /// </summary>
    public static readonly string HttpHost = "http.host";

    /// <summary>
    ///     HTTP Scheme of the request (`http`/`https`).
    /// </summary>
    public static readonly string HttpScheme = "url.scheme";

    /// <summary>
    ///     Full URL of the request.
    /// </summary>
    public static readonly string HttpUrl = "url.full";

    /// <summary>
    ///     User Agent used in the query.
    /// </summary>
    public static readonly string HttpUserAgent = "user_agent.original";

    /// <summary>
    ///     The number of retries attempted (Only sent if the request was retried. Count of `1` means the request was retried
    ///     once in addition to the original request).
    /// </summary>
    public static readonly string RequestRetryCount = "http.request.resend_count";

    /// <summary>
    /// Return all supported attributes
    /// </summary>
    public static HashSet<string> GetAllAttributes() {
        return new() {
            RequestMethod,
            RequestStoreId,
            RequestModelId,
            RequestClientId,
            ResponseModelId,
            FgaRequestUser,
            HttpMethod,
            HttpStatus,
            HttpHost,
            HttpScheme,
            HttpUrl,
            HttpUserAgent,
            RequestRetryCount
        };
    }
}

/// <summary>
///     Class for building attributes for telemetry.
/// </summary>
public class Attributes {
    /// <summary>
    ///     Gets the header value if valid.
    /// </summary>
    /// <param name="headers">The HTTP response headers.</param>
    /// <param name="headerName">The name of the header.</param>
    /// <returns>The header value if valid, otherwise null.</returns>
    private static string? GetHeaderValueIfValid(HttpResponseHeaders headers, string headerName) {
        if (headers.Contains(headerName) && headers.GetValues(headerName).Any()) {
            return headers.GetValues(headerName).First();
        }

        return null;
    }

    /// <summary>
    ///     Filters the attributes based on the enabled attributes.
    /// </summary>
    /// <param name="attributes">The list of attributes to filter.</param>
    /// <param name="enabledAttributes">The dictionary of enabled attributes.</param>
    /// <returns>A filtered list of attributes.</returns>
    public static TagList FilterAttributes(TagList attributes, HashSet<string>? enabledAttributes) {
        var filteredAttributes = new TagList();

        if (enabledAttributes != null && enabledAttributes.Count != 0) {
            foreach (var attribute in attributes) {
                if (enabledAttributes.Contains(attribute.Key)) {
                    filteredAttributes.Add(attribute);
                }
            }
        }

        return filteredAttributes;
    }

    /// <summary>
    ///     Builds an object of attributes that can be used to report alongside an OpenTelemetry metric event.
    /// </summary>
    /// <typeparam name="T">The type of the request builder.</typeparam>
    /// <param name="enabledAttributes">The list of enabled attributes.</param>
    /// <param name="credentials">The credentials object.</param>
    /// <param name="apiName">The GRPC method name.</param>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="requestBuilder">The request builder.</param>
    /// <param name="requestDuration">The stopwatch measuring the request duration.</param>
    /// <param name="retryCount">The number of retries attempted.</param>
    /// <returns>A TagList of attributes.</returns>
    public static TagList BuildAttributesForResponse<T>(
        HashSet<string> enabledAttributes, Credentials? credentials,
        string apiName, HttpResponseMessage response, RequestBuilder<T> requestBuilder,
        Stopwatch requestDuration, int retryCount) {
        var attributes = new TagList();

        attributes = AddRequestAttributes(enabledAttributes, apiName, requestBuilder, attributes);
        attributes = AddResponseAttributes(enabledAttributes, response, attributes);
        attributes = AddCommonAttributes(enabledAttributes, response, requestBuilder, credentials, retryCount, attributes);

        return attributes;
    }

    private static TagList AddRequestAttributes<T>(
        HashSet<string> enabledAttributes, string apiName, RequestBuilder<T> requestBuilder, TagList attributes) {
        if (enabledAttributes.Contains(TelemetryAttribute.RequestMethod)) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestMethod, apiName));
        }

        if (enabledAttributes.Contains(TelemetryAttribute.RequestStoreId) &&
            requestBuilder.PathParameters.TryGetValue("store_id", out var storeId) && !string.IsNullOrEmpty(storeId)) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestStoreId, storeId));
        }

        if (enabledAttributes.Contains(TelemetryAttribute.RequestModelId)) {
            attributes = AddRequestModelIdAttributes(requestBuilder, apiName, attributes);
        }

        return attributes;
    }

    private static TagList AddRequestModelIdAttributes<T>(
        RequestBuilder<T> requestBuilder, string apiName, TagList attributes) {
        string? modelId = null;

        if (requestBuilder.PathParameters.TryGetValue("authorization_model_id", out var authModelIdValue)) {
            modelId = authModelIdValue;
        }
        else if (requestBuilder.PathTemplate == "/stores/{store_id}/authorization-models/{id}" &&
            requestBuilder.PathParameters.TryGetValue("id", out var idValue)) {
            modelId = idValue;
        }

        if (!string.IsNullOrEmpty(modelId)) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestModelId, modelId));
        }

        if (apiName is "Check" or "ListObjects" or "Write" or "Expand" or "ListUsers") {
            AddRequestBodyAttributes(requestBuilder, apiName, attributes);
        }

        return attributes;
    }

    private static TagList AddRequestBodyAttributes<T>(
        RequestBuilder<T> requestBuilder, string apiName, TagList attributes) {
        try {
            if (requestBuilder.JsonBody != null) {
                using (var document = JsonDocument.Parse(requestBuilder.JsonBody)) {
                    var root = document.RootElement;

                    if (root.TryGetProperty("authorization_model_id", out var authModelId) &&
                        !string.IsNullOrEmpty(authModelId.GetString())) {
                        attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestModelId,
                            authModelId.GetString()));
                    }

                    if (apiName is "Check" or "ListObjects" && root.TryGetProperty("user", out var fgaUser) &&
                        !string.IsNullOrEmpty(fgaUser.GetString())) {
                        attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.FgaRequestUser,
                            fgaUser.GetString()));
                    }
                }
            }
        }
        catch {
            // Handle parsing errors if necessary
        }

        return attributes;
    }

    private static TagList AddResponseAttributes(
        HashSet<string> enabledAttributes, HttpResponseMessage response, TagList attributes) {
        if (enabledAttributes.Contains(TelemetryAttribute.HttpStatus) && response.StatusCode != null) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpStatus, (int)response.StatusCode));
        }

        if (enabledAttributes.Contains(TelemetryAttribute.ResponseModelId)) {
            var responseModelId = GetHeaderValueIfValid(response.Headers, "openfga-authorization-model-id") ??
                                  GetHeaderValueIfValid(response.Headers, "fga-authorization-model-id");
            if (!string.IsNullOrEmpty(responseModelId)) {
                attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.ResponseModelId, responseModelId));
            }
        }

        return attributes;
    }

    private static TagList AddCommonAttributes<T>(
        HashSet<string> enabledAttributes, HttpResponseMessage response, RequestBuilder<T> requestBuilder,
        Credentials? credentials, int retryCount, TagList attributes) {
        if (response.RequestMessage != null) {
            if (enabledAttributes.Contains(TelemetryAttribute.HttpMethod) && response.RequestMessage.Method != null) {
                attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpMethod,
                    response.RequestMessage.Method));
            }

            if (response.RequestMessage.RequestUri != null) {
                if (enabledAttributes.Contains(TelemetryAttribute.HttpScheme)) {
                    attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpScheme,
                        response.RequestMessage.RequestUri.Scheme));
                }

                if (enabledAttributes.Contains(TelemetryAttribute.HttpHost)) {
                    attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpHost,
                        response.RequestMessage.RequestUri.Host));
                }

                if (enabledAttributes.Contains(TelemetryAttribute.HttpUrl)) {
                    attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpUrl,
                        response.RequestMessage.RequestUri.AbsoluteUri));
                }
            }

            if (enabledAttributes.Contains(TelemetryAttribute.HttpUserAgent) &&
                response.RequestMessage.Headers.UserAgent != null &&
                !string.IsNullOrEmpty(response.RequestMessage.Headers.UserAgent.ToString())) {
                attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.HttpUserAgent,
                    response.RequestMessage.Headers.UserAgent.ToString()));
            }
        }

        if (enabledAttributes.Contains(TelemetryAttribute.RequestClientId) && credentials is { Method: CredentialsMethod.ClientCredentials, Config.ClientId: not null }) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestClientId,
                credentials.Config.ClientId));
        }

        if (enabledAttributes.Contains(TelemetryAttribute.RequestRetryCount) && retryCount > 0) {
            attributes.Add(new KeyValuePair<string, object?>(TelemetryAttribute.RequestRetryCount, retryCount));
        }

        return attributes;
    }
}