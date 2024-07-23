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
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Telemetry;

internal interface IPartialApiRequest {
    [DataMember(Name = "authorization_model_id", IsRequired = false, EmitDefaultValue = false)]
    [JsonPropertyName("authorization_model_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string AuthorizationModelId { get; set; }

    [DataMember(Name = "user", IsRequired = false, EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string User { get; set; }

    [DataMember(Name = "tuple_key.user", IsRequired = false, EmitDefaultValue = false)]
    [JsonPropertyName("tuple_key.user")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string TupleKeyUser { get; set; }
}

public class Attributes {
    /**
     * Common attribute (tag) names
     */

    // Attributes (tags) associated with the request made //

    // The FGA method/action that was performed (e.g. `check`, `listObjects`, ...) in camelCase
    private const string AttributeRequestMethod = "fga-client.request.method";

    // The store ID that was sent as part of the request
    private const string AttributeRequestStoreId = "fga-client.request.store_id";

    // The authorization model ID that was sent as part of the request, if any
    private const string AttributeRequestModelId = "fga-client.request.model_id";

    // The client ID associated with the request, if any
    private const string AttributeRequestClientId = "fga-client.request.client_id";

    // Attributes (tags) associated with the response //

    // The authorization model ID that the FGA server used
    private const string AttributeResponseModelId = "fga-client.response.model_id";

    // Attributes (tags) associated with specific actions //

    // The user that is associated with the action of the request for check and list objects
    private const string AttributeFgaRequestUser = "fga-client.user";

    // OTEL Semantic Attributes (tags) //

    // The total request time for FGA requests
    private const string AttributeHttpClientRequestDuration = "http.client.request.duration";

    // The amount of time the FGA server took to internally process nd evaluate the request
    private const string AttributeHttpServerRequestDuration = "http.server.request.duration";

    // The HTTP method for the request
    private const string AttributeHttpMethod = "http.request.method";

    // The status code of the response
    private const string AttributeHttpStatus = "http.response.status_code";

    // Host identifier of the origin the request was sent to
    private const string AttributeHttpHost = "http.host";

    // HTTP Scheme of the request (`http`/`https`)
    private const string AttributeHttpScheme = "url.scheme";

    // Full URL of the request
    private const string AttributeHttpUrl = "url.full";

    // User Agent used in the query
    private const string AttributeHttpUserAgent = "user_agent.original";

    // The number of retries attempted (starting from 1 for the original request)
    private const string AttributeRequestRetryCount = "http.request.resend_count";

    private static string? GetHeaderValueIfValid(HttpResponseHeaders headers, string headerName) {
        if (headers.Contains(headerName) && headers.GetValues(headerName).Any()) {
            return headers.GetValues(headerName).First();
        }

        return null;
    }

    /**
     * Builds an object of attributes that can be used to report alongside an OpenTelemetry metric event.
     *
     * @param response - The Axios response object, used to add data like HTTP status, host, method, and headers.
     * @param credentials - The credentials object, used to add data like the ClientID when using ClientCredentials.
     * @param methodAttributes - Extra attributes that the method (i.e. check, listObjects) wishes to have included. Any custom attributes should use the common names.
     * @returns {Attributes}
     */
    public static KeyValuePair<string, object>[] buildAttributesForResponse(string apiName,
        HttpResponseMessage response, RequestBuilder requestBuilder, Credentials? credentials,
        Stopwatch requestDuration, int retryCount) {
        var attributes = new List<KeyValuePair<string, object>>();

        // To match the JS SDK, we are converting the method name to camelCase
        attributes.Add(new KeyValuePair<string, object>(AttributeRequestMethod, apiName));

        if (requestBuilder.PathParameters.ContainsKey("store_id")) {
            var storeId = requestBuilder.PathParameters.GetValueOrDefault("store_id");
            if (!string.IsNullOrEmpty(storeId)) {
                attributes.Add(new KeyValuePair<string, object>(AttributeRequestStoreId, storeId));
            }
        }

        string? modelId = null;
        // if the model id is in the path params try to get it from there
        if (requestBuilder.PathParameters.ContainsKey("authorization_model_id")) {
            modelId = requestBuilder.PathParameters.GetValueOrDefault("authorization_model_id");
            if (!string.IsNullOrEmpty(modelId)) {
                attributes.Add(new KeyValuePair<string, object>(AttributeRequestModelId, modelId));
            }
        }
        // In the case of ReadAuthorizationModel, the path param is called ID
        else if (requestBuilder.PathTemplate == "/stores/{store_id}/authorization-models/{id}" &&
                 requestBuilder.PathParameters.ContainsKey("id")) {
            modelId = requestBuilder.PathParameters.GetValueOrDefault("id");
            if (!string.IsNullOrEmpty(modelId)) {
                attributes.Add(new KeyValuePair<string, object>(AttributeRequestModelId, modelId));
            }
        }
        // In many endpoints authorization_model_id is sent as a field in the body
        // if the apiName is Check or ListObjects, we always want to parse the request body to get the model ID and the user (subject)
        // if the apiName is Write, Expand or ListUsers we want to parse it to get the model ID
        else if (apiName is "Check" or "ListObjects" or "Write" or "Expand" or "ListUsers") {
            try {
                var partialApiResquest =
                    JsonSerializer.Deserialize<IPartialApiRequest>(requestBuilder.Body?.ToString()!);

                if (!string.IsNullOrEmpty(partialApiResquest?.AuthorizationModelId)) {
                    attributes.Add(new KeyValuePair<string, object>(AttributeRequestModelId,
                        partialApiResquest.AuthorizationModelId));
                }

                switch (apiName) {
                    case "Check": {
                            if (!string.IsNullOrEmpty(partialApiResquest?.TupleKeyUser)) {
                                attributes.Add(new KeyValuePair<string, object>(AttributeFgaRequestUser,
                                    partialApiResquest.TupleKeyUser));
                            }

                            break;
                        }
                    case "ListObjects": {
                            if (!string.IsNullOrEmpty(partialApiResquest?.User)) {
                                attributes.Add(new KeyValuePair<string, object>(AttributeFgaRequestUser,
                                    partialApiResquest.User));
                            }

                            break;
                        }
                }
            }
            catch {
            }
        }


        if (response.StatusCode != null) {
            attributes.Add(new KeyValuePair<string, object>(AttributeHttpStatus, (int)response.StatusCode));
        }

        if (response.RequestMessage != null) {
            if (response.RequestMessage.Method != null) {
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpMethod, response.RequestMessage.Method));
            }

            if (response.RequestMessage.RequestUri != null) {
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpScheme,
                    response.RequestMessage.RequestUri.Scheme));
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpHost,
                    response.RequestMessage.RequestUri.Host));
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpUrl,
                    response.RequestMessage.RequestUri.AbsoluteUri));
            }

            if (response.RequestMessage.Headers.UserAgent != null &&
                !string.IsNullOrEmpty(response.RequestMessage.Headers.UserAgent.ToString())) {
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpUserAgent,
                    response.RequestMessage.Headers.UserAgent.ToString()));
            }
        }

        var responseModelId = GetHeaderValueIfValid(response.Headers, "openfga-authorization-model-id");
        if (!string.IsNullOrEmpty(responseModelId)) {
            attributes.Add(new KeyValuePair<string, object>(AttributeResponseModelId, responseModelId));
        }
        else {
            responseModelId = GetHeaderValueIfValid(response.Headers, "fga-authorization-model-id");
            if (!string.IsNullOrEmpty(responseModelId)) {
                attributes.Add(new KeyValuePair<string, object>(AttributeResponseModelId, responseModelId));
            }
        }

        if (credentials is { Method: CredentialsMethod.ClientCredentials, Config.ClientId: not null }) {
            attributes.Add(new KeyValuePair<string, object>(AttributeRequestClientId, credentials.Config.ClientId));
        }

        var durationHeader = GetHeaderValueIfValid(response.Headers, "fga-query-duration-ms");
        if (!string.IsNullOrEmpty(durationHeader)) {
            var success = float.TryParse(durationHeader, out var durationFloat);
            if (success) {
                attributes.Add(new KeyValuePair<string, object>(AttributeHttpServerRequestDuration, durationFloat));
            }
        }

        attributes.Add(new KeyValuePair<string, object>(AttributeHttpClientRequestDuration,
            requestDuration.ElapsedMilliseconds));

        attributes.Add(new KeyValuePair<string, object>(AttributeRequestRetryCount, retryCount));

        return attributes.ToArray();
    }
}