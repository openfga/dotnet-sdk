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


using OpenFga.Sdk.Exceptions;
using System.Text;
using System.Text.Json;
using System.Web;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
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