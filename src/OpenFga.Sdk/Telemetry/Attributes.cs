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
using OpenFga.Sdk.Client; // For extensions
using OpenFga.Sdk.Configuration;
using System.Diagnostics;

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

    /// <summary>
    ///     The internal retry count for an API request.
    /// </summary>
    public static readonly string RequestRetryCount = "fga-client.request.retry-count";

    /// <summary>
    ///     The HTTP host for an API request
    /// </summary>
    public static readonly string HttpHost = "http.host";

    /// <summary>
    ///     The HTTP status for an API request
    /// </summary>
    public static readonly string HttpStatus = "http.status";

    /// <summary>
    ///     The HTTP user agent for an API request
    /// </summary>
    public static readonly string HttpUserAgent = "http.user_agent";

    /// <summary>
    ///     The HTTP scheme for an API request
    /// </summary>
    public static readonly string HttpScheme = "http.scheme";

    /// <summary>
    ///     The HTTP method for an API request
    /// </summary>
    public static readonly string HttpMethod = "http.method";

    /// <summary>
    ///     The HTTP URL for an API request
    /// </summary>
    public static readonly string HttpUrl = "http.url";

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

    /// <summary>
    ///     The context/condition that is associated with the action of the request for check and list objects.
    /// </summary>
    public static readonly string FgaRequestContext = "fga-client.context";

    /// <summary>
    ///     The object that is associated with the action of the request for check and list relations.
    /// </summary>
    public static readonly string FgaRequestObject = "fga-client.object";

    /// <summary>
    ///     The relation that is associated with the action of the request for check and list objects.
    /// </summary>
    public static readonly string FgaRequestRelation = "fga-client.relation";

    /// <summary>
    ///     The type that is associated with the action of the request for list objects.
    /// </summary>
    public static readonly string FgaRequestType = "fga-client.type";

    /// <summary>
    ///     The internal retry count for an API request.
    /// </summary>
    public static readonly string RetryCount = "fga-client.retry-count";

    /// <summary>
    ///     The HTTP status returned for an API request
    /// </summary>
    public static readonly string ResponseStatus = "fga-client.response.status";

    /// <summary>
    ///     Indicates if an API request was successful
    /// </summary>
    public static readonly string ResponseSuccess = "fga-client.response.success";

    /// <summary>
    ///     Return all supported attribute names for the client
    /// </summary>
    public static HashSet<string> GetAllAttributes() {
        var attributes = new HashSet<string>();
        attributes.Add(RequestMethod);
        attributes.Add(RequestStoreId);
        attributes.Add(RequestModelId);
        attributes.Add(RequestClientId);
        attributes.Add(RequestRetryCount);
        attributes.Add(HttpHost);
        attributes.Add(HttpStatus);
        attributes.Add(HttpUserAgent);
        attributes.Add(HttpScheme);
        attributes.Add(HttpMethod);
        attributes.Add(HttpUrl);
        attributes.Add(ResponseModelId);
        attributes.Add(FgaRequestUser);
        attributes.Add(FgaRequestContext);
        attributes.Add(FgaRequestObject);
        attributes.Add(FgaRequestRelation);
        attributes.Add(FgaRequestType);
        attributes.Add(RetryCount);
        attributes.Add(ResponseStatus);
        attributes.Add(ResponseSuccess);
        return attributes;
    }
}

/// <summary>
///     Helper class for building attribute tags for telemetry
/// </summary>
public static class Attributes {
    /// <summary>
    ///     Filter all attributes against the allowed attributes hash set
    /// </summary>
    /// <param name="fullAttributes">The full attributes to be filtered</param>
    /// <param name="allowedAttributes">The hashset of allowed attribute names</param>
    /// <returns>A tag list containing only the allowed attributes</returns>
    public static TagList FilterAttributes(TagList fullAttributes, HashSet<string>? allowedAttributes) {
        if (allowedAttributes == null || allowedAttributes.Count == 0) {
            return new TagList();
        }

        var filteredAttributes = new TagList();
        foreach (var attribute in fullAttributes) {
            if (allowedAttributes.Contains(attribute.Key)) {
                filteredAttributes.Add(attribute.Key, attribute.Value);
            }
        }

        return filteredAttributes;
    }

    /// <summary>
    ///     Builds the attribute tags for metrics for client credentials exchange requests
    /// </summary>
    /// <param name="enabledAttributeNames">The enabled attribute names (must be a subset of <see cref="TelemetryAttribute.GetAllAttributes"/>)</param>
    /// <param name="credentials">The client credentials used make the API call</param>
    /// <param name="requestDuration">Measures the total request time</param>
    /// <param name="retryCount">How many times the token request was retried</param>
    /// <returns>Attributes for telemetry</returns>
    public static TagList BuildAttributesForClientCredentials(HashSet<string> enabledAttributeNames, Credentials? credentials,
        Stopwatch requestDuration, int retryCount = 0) {
        if (enabledAttributeNames.Count == 0) {
            return new TagList();
        }

        var tagList = new TagList();
        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestMethod)) {
            tagList.Add(TelemetryAttribute.RequestMethod, "ClientCredentialsExchange");
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RetryCount)) {
            tagList.Add(TelemetryAttribute.RetryCount, retryCount);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestClientId)) {
            var clientId = credentials?.Config?.ClientId ?? null;
            if (clientId != null) {
                tagList.Add(TelemetryAttribute.RequestClientId, clientId);
            }
        }

        return tagList;
    }

    /// <summary>
    ///     Builds the attribute tags for metrics for a response
    /// </summary>
    /// <param name="enabledAttributeNames">The enabled attribute names (must be a subset of <see cref="TelemetryAttribute.GetAllAttributes"/>)</param>
    /// <param name="credentials">The client credentials used make the API call</param>
    /// <param name="apiName">The API method which is being called.</param>
    /// <param name="response">The response of the call which is being made</param>
    /// <param name="requestBuilder">The request builder used to make the call</param>
    /// <param name="requestDuration">Measures the total request time</param>
    /// <param name="retryCount">How many times the API request was retried</param>
    /// <typeparam name="T">The type of the request, required for API calls</typeparam>
    /// <returns>Attributes for telemetry</returns>
    public static TagList BuildAttributesForResponse<T>(HashSet<string> enabledAttributeNames, Credentials? credentials,
        string? apiName, HttpResponseMessage response, RequestBuilder<T> requestBuilder, Stopwatch requestDuration, int retryCount = 0) {
        if (enabledAttributeNames.Count == 0) {
            return new TagList();
        }

        var tagList = new TagList();

        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestMethod) && apiName != null) {
            tagList.Add(TelemetryAttribute.RequestMethod, apiName);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RetryCount)) {
            tagList.Add(TelemetryAttribute.RetryCount, retryCount);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.ResponseStatus)) {
            tagList.Add(TelemetryAttribute.ResponseStatus, (int)response.StatusCode);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.ResponseSuccess)) {
            tagList.Add(TelemetryAttribute.ResponseSuccess, response.IsSuccessStatusCode);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestClientId)) {
            var clientId = credentials?.Config?.ClientId ?? null;
            if (clientId != null) {
                tagList.Add(TelemetryAttribute.RequestClientId, clientId);
            }
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestStoreId) &&
            requestBuilder.PathParameters.TryGetValue("store_id", out var storeId) &&
            !string.IsNullOrEmpty(storeId)) {
            tagList.Add(TelemetryAttribute.RequestStoreId, storeId);
        }

        if (enabledAttributeNames.Contains(TelemetryAttribute.RequestModelId) && enabledAttributeNames.Contains(TelemetryAttribute.ResponseModelId)) {
            // Try to read the model ID from the path params
            var modelIdParam = requestBuilder.PathParameters.GetValueOrDefault("authorization_model_id");
            if (!string.IsNullOrEmpty(modelIdParam)) {
                tagList.Add(TelemetryAttribute.RequestModelId, modelIdParam);
                tagList.Add(TelemetryAttribute.ResponseModelId, modelIdParam);
            }
            else {
                // Try to read the model ID from the query params
                var modelIdQuery = requestBuilder.QueryParameters.GetValueOrDefault("authorization_model_id");
                if (!string.IsNullOrEmpty(modelIdQuery)) {
                    tagList.Add(TelemetryAttribute.RequestModelId, modelIdQuery);
                    tagList.Add(TelemetryAttribute.ResponseModelId, modelIdQuery);
                }
                else if (requestBuilder.Body != null) {
                    // Try to read the model ID from the request body
                    var requestBodyJson = requestBuilder.JsonBody;
                    if (!string.IsNullOrEmpty(requestBodyJson) && requestBodyJson.Contains("authorization_model_id")) {
                        try {
                            var bodyDoc = JsonDocument.Parse(requestBodyJson);
                            if (bodyDoc.RootElement.TryGetProperty("authorization_model_id", out var modelIdJsonElement)) {
                                var modelIdValue = modelIdJsonElement.GetString();
                                if (!string.IsNullOrEmpty(modelIdValue)) {
                                    tagList.Add(TelemetryAttribute.RequestModelId, modelIdValue);
                                    tagList.Add(TelemetryAttribute.ResponseModelId, modelIdValue);
                                }
                            }
                        }
                        catch (JsonException) {
                            // Safe to ignore parsing failures for telemetry
                        }
                        catch (InvalidOperationException) {
                            // Safe to ignore for telemetry
                        }
                    }
                }
            }
        }

        if (requestBuilder.Body != null) {
            var bodyWithTupleKey = requestBuilder.JsonBody;
            if (!string.IsNullOrEmpty(bodyWithTupleKey) && bodyWithTupleKey.Contains("tuple_key")) {
                try {
                    var bodyDoc = JsonDocument.Parse(bodyWithTupleKey);
                    if (bodyDoc.RootElement.TryGetProperty("tuple_key", out var tupleKeyElement)) {
                        if (enabledAttributeNames.Contains(TelemetryAttribute.FgaRequestUser) && tupleKeyElement.TryGetProperty("user", out var userElement)) {
                            var userValue = userElement.GetString();
                            if (!string.IsNullOrEmpty(userValue)) {
                                tagList.Add(TelemetryAttribute.FgaRequestUser, userValue);
                            }
                        }

                        if (enabledAttributeNames.Contains(TelemetryAttribute.FgaRequestRelation) && tupleKeyElement.TryGetProperty("relation", out var relationElement)) {
                            var relationValue = relationElement.GetString();
                            if (!string.IsNullOrEmpty(relationValue)) {
                                tagList.Add(TelemetryAttribute.FgaRequestRelation, relationValue);
                            }
                        }

                        if (enabledAttributeNames.Contains(TelemetryAttribute.FgaRequestObject) && tupleKeyElement.TryGetProperty("object", out var objectElement)) {
                            var objectValue = objectElement.GetString();
                            if (!string.IsNullOrEmpty(objectValue)) {
                                tagList.Add(TelemetryAttribute.FgaRequestObject, objectValue);
                            }
                        }
                    }

                    if (bodyDoc.RootElement.TryGetProperty("context", out var contextElement) &&
                        enabledAttributeNames.Contains(TelemetryAttribute.FgaRequestContext) &&
                        !contextElement.ValueKind.Equals(JsonValueKind.Null)) {
                        tagList.Add(TelemetryAttribute.FgaRequestContext, true);
                    }

                    if (bodyDoc.RootElement.TryGetProperty("type", out var typeElement)) {
                        var typeValue = typeElement.GetString();
                        if (!string.IsNullOrEmpty(typeValue) && enabledAttributeNames.Contains(TelemetryAttribute.FgaRequestType)) {
                            tagList.Add(TelemetryAttribute.FgaRequestType, typeValue);
                        }
                    }
                }
                catch (JsonException) {
                    // Safe to ignore parsing failures for telemetry
                }
                catch (InvalidOperationException) {
                    // Safe to ignore for telemetry
                }
            }
        }

        return tagList;
    }
}