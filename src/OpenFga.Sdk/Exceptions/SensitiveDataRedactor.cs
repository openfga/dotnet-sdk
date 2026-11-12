using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// Produces copies of outbound request data that are safe to retain on exceptions - and therefore safe for
/// callers to log, serialize or forward to error-monitoring services - by redacting credentials.
/// </summary>
internal static class SensitiveDataRedactor {
    /// <summary>
    /// Placeholder substituted for redacted values.
    /// </summary>
    internal const string RedactedPlaceholder = "[REDACTED]";

    private const string FormUrlEncodedMediaType = "application/x-www-form-urlencoded";

    /// <summary>
    /// Form fields that are safe to retain verbatim. The SDK only sends form-encoded bodies for the OAuth2 client
    /// credentials exchange, so this is an allow-list: any field that is not listed here (client_secret,
    /// client_assertion, refresh_token, ...) is redacted. Failing closed means a credential can never leak by omission.
    /// </summary>
    private static readonly HashSet<string> AllowedFormFields = new(StringComparer.OrdinalIgnoreCase) {
        "grant_type",
        "client_id",
        "audience",
        "scope",
    };

    /// <summary>
    /// Request headers whose value carries a credential.
    /// </summary>
    private static readonly HashSet<string> SensitiveHeaders = new(StringComparer.OrdinalIgnoreCase) {
        "Authorization",
        "Proxy-Authorization",
        "Cookie",
    };

    /// <summary>
    /// Headers whose value has the shape <c>&lt;scheme&gt; &lt;credentials&gt;</c>; the scheme is kept for debuggability.
    /// </summary>
    private static readonly HashSet<string> SchemePrefixedHeaders = new(StringComparer.OrdinalIgnoreCase) {
        "Authorization",
        "Proxy-Authorization",
    };

    /// <summary>
    /// Returns request content that is safe to retain on an exception.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>Non form-encoded content (e.g. the JSON bodies of the OpenFGA API) never carries credentials and is returned as-is.</item>
    /// <item>Form-encoded content is re-created with every field outside of <see cref="AllowedFormFields"/> redacted.</item>
    /// <item>If the form-encoded content cannot be safely re-read (e.g. it is a stream, or it has already been disposed by
    /// the <see cref="HttpClient"/> as happens on .NET Framework), <see langword="null"/> is returned rather than the raw content.</item>
    /// </list>
    /// </remarks>
    internal static HttpContent? RedactRequestContent(HttpContent? content) {
        if (content == null) {
            return null;
        }

        if (!IsFormUrlEncoded(content)) {
            return content;
        }

        // Only in-memory content is re-read: it has already been fully materialized to be sent, so reading it again
        // completes synchronously. Anything else (e.g. StreamContent) is dropped rather than risk blocking or leaking.
        if (content is not ByteArrayContent) {
            return null;
        }

        try {
            // CopyToAsync is used rather than ReadAsStringAsync: on .NET Framework the latter buffers through a
            // continuation that is not inlined, so it never reports completion synchronously even for in-memory content.
            using var buffer = new MemoryStream();
            var copyTask = content.CopyToAsync(buffer);
            if (!copyTask.IsCompleted) {
                // Never block inside exception construction; drop the body instead.
                return null;
            }

            copyTask.GetAwaiter().GetResult();
            var body = GetEncoding(content).GetString(buffer.ToArray());
            return new StringContent(RedactFormUrlEncodedBody(body), Encoding.UTF8, FormUrlEncodedMediaType);
        }
        catch {
            // Fail closed: never fall back to the raw content.
            return null;
        }
    }

    /// <summary>
    /// Returns a detached copy of the request headers in which credential-bearing headers are redacted.
    /// </summary>
    internal static HttpRequestHeaders RedactRequestHeaders(HttpRequestHeaders? headers) {
        // HttpRequestHeaders has no public constructor; a throwaway HttpRequestMessage (no content, nothing to dispose)
        // is the only supported way to obtain a fresh instance.
        var redacted = new HttpRequestMessage().Headers;

        if (headers == null) {
            return redacted;
        }

        foreach (var header in headers) {
            if (!SensitiveHeaders.Contains(header.Key)) {
                redacted.TryAddWithoutValidation(header.Key, header.Value);
                continue;
            }

            foreach (var value in header.Value) {
                redacted.TryAddWithoutValidation(header.Key, RedactHeaderValue(header.Key, value));
            }
        }

        return redacted;
    }

    /// <summary>
    /// Redacts the value of every field of a form-encoded body that is not explicitly allow-listed. Allowed fields
    /// are preserved byte-for-byte (ordering and encoding included). A field without a value is treated as
    /// potentially sensitive and becomes <c>field=[REDACTED]</c>.
    /// </summary>
    internal static string RedactFormUrlEncodedBody(string body) {
        if (string.IsNullOrEmpty(body)) {
            return string.Empty;
        }

        var pairs = body.Split('&');
        for (var i = 0; i < pairs.Length; i++) {
            var pair = pairs[i];
            if (pair.Length == 0) {
                continue;
            }

            var separatorIndex = pair.IndexOf('=');
            var encodedKey = separatorIndex < 0 ? pair : pair.Substring(0, separatorIndex);

            if (!AllowedFormFields.Contains(DecodeFormComponent(encodedKey))) {
                pairs[i] = encodedKey + "=" + RedactedPlaceholder;
            }
        }

        return string.Join("&", pairs);
    }

    private static bool IsFormUrlEncoded(HttpContent content) =>
        string.Equals(content.Headers.ContentType?.MediaType, FormUrlEncodedMediaType, StringComparison.OrdinalIgnoreCase);

    private static Encoding GetEncoding(HttpContent content) {
        var charSet = content.Headers.ContentType?.CharSet;
        if (string.IsNullOrEmpty(charSet)) {
            return Encoding.UTF8;
        }

        try {
            return Encoding.GetEncoding(charSet!.Trim('"'));
        }
        catch (ArgumentException) {
            return Encoding.UTF8;
        }
    }

    private static string DecodeFormComponent(string value) => Uri.UnescapeDataString(value.Replace('+', ' '));

    private static string RedactHeaderValue(string headerName, string? value) {
        if (!SchemePrefixedHeaders.Contains(headerName) || string.IsNullOrEmpty(value)) {
            return RedactedPlaceholder;
        }

        var schemeLength = value!.IndexOf(' ');
        return schemeLength > 0
            ? value.Substring(0, schemeLength) + " " + RedactedPlaceholder
            : RedactedPlaceholder;
    }
}