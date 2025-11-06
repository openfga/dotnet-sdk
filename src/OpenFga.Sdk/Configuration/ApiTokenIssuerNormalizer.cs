using System;

namespace OpenFga.Sdk.Configuration;

internal static class ApiTokenIssuerNormalizer {

    /// <summary>
    ///     Normalizes the API Token Issuer to a full URL.
    ///     If the input doesn't start with http:// or https://, prepends https://.
    /// </summary>
    /// <param name="apiTokenIssuer">The API token issuer (domain, domain with path, or full URL)</param>
    /// <returns>The normalized full URL</returns>
    internal static string? Normalize(string? apiTokenIssuer) {
        if (string.IsNullOrWhiteSpace(apiTokenIssuer)) {
            return null;
        }

        var normalizedUrl = apiTokenIssuer;

        // If no scheme is provided, prepend https://
        if (!normalizedUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !normalizedUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
            normalizedUrl = $"https://{normalizedUrl}";
        }

        return normalizedUrl;
    }
}
