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

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (string.IsNullOrEmpty(content))
            return null;

        return Parse(content);
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