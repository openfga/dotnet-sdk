using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
///
/// </summary>
public static class Utils {
    public static HttpContent CreateJsonStringContent<T>(T body) {
        var json = JsonSerializer.Serialize(body);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public static HttpContent CreateFormEncodedConent(IDictionary<string, string> parameters) {
        return new FormUrlEncodedContent(parameters.Select(p =>
            new KeyValuePair<string, string>(p.Key, p.Value ?? "")));
    }

    public static string BuildQueryParams(IDictionary<string, string> parameters) {
        var query = "";
        foreach (var parameter in parameters) {
            query = query + parameter.Key + "=" + HttpUtility.UrlEncode(parameter.Value) + "&";
        }

        return query;
    }

    public static Uri BuildUri(string basePath, string resource, IDictionary<string, string>? parameters = null) {
        var uriString = $"{basePath}/{resource}";

        if (parameters != null) {
            uriString += BuildQueryParams(parameters);
        }

        return new Uri(uriString);
    }

    public static HttpRequestMessage BuildRequest(HttpMethod method, string basePath, string resource, IDictionary<string, string>? parameters = null) {
        return new HttpRequestMessage() {
            RequestUri = BuildUri(basePath, resource, parameters),
            Method = method,
        };
    }
}