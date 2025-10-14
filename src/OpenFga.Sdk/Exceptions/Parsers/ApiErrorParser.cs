using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Exceptions.Parsers;

public class ApiErrorParser {
    /// <summary>
    /// Description of the failing HTTP Status Code.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// Error code returned by the API.
    /// </summary>
    [JsonPropertyName("code")]
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Description of the error.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Parse a <see cref="HttpResponseMessage"/> into an <see cref="ApiErrorParser"/> asynchronously.
    /// </summary>
    /// <param name="response"><see cref="HttpResponseMessage"/> to parse.</param>
    /// <returns><see cref="Task"/> representing the operation and associated <see cref="ApiErrorParser"/> on
    /// successful completion.</returns>
    public static async Task<ApiErrorParser?> Parse(HttpResponseMessage? response) {
        if (response == null || response?.Content == null)
            return null;

        try {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(content))
                return null;

            return Parse(content);
        }
        catch (ObjectDisposedException) {
            // .NET Framework 4.8 is having an error while testing where the content is disposed before it can be parsed.
            return new ApiErrorParser {
                Error = "Error Parsing Response Content",
                Message = "HTTP response content was disposed before it could be read"
            };
        }
    }

    internal static ApiErrorParser Parse(string content) {
        try {
            return JsonSerializer.Deserialize<ApiErrorParser>(content) ?? throw new InvalidOperationException();
        }
        catch (JsonException) {
            return new ApiErrorParser { Error = content, Message = content };
        }
    }
}