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
using System;
using System.Collections.Generic;

namespace OpenFga.Sdk.Configuration;

/// <summary>
///     Setup OpenFga.Sdk Configuration
/// </summary>
public class Configuration {
    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    public Configuration() {
        DefaultHeaders ??= new Dictionary<string, string>();

        if (!DefaultHeaders.ContainsKey("User-Agent")) {
            DefaultHeaders.Add("User-Agent", DefaultUserAgent);
        }
    }

    #endregion Constructors

    #region Methods

    private static bool IsWellFormedUriString(string uri) =>
        Uri.TryCreate(uri, UriKind.Absolute, out var uriResult) &&
        (uriResult.ToString().Equals(uri) || uriResult.ToString().Equals($"{uri}/")) &&
        (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

    /// <summary>
    ///     Reserved HTTP headers that should not be overridden via custom headers.
    ///     Note: User-Agent is intentionally excluded as the SDK sets a default value
    ///     but allows users to customize it via DefaultHeaders.
    /// </summary>
    private static readonly HashSet<string> ReservedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
        "Authorization",
        "Content-Type",
        "Content-Length",
        "Host",
        "Accept",
        "Accept-Encoding",
        "Transfer-Encoding",
        "Connection",
        "Cookie",
        "Set-Cookie",
        "Date"
    };

    /// <summary>
    ///     Validates that HTTP headers are safe to use with HTTP requests
    /// </summary>
    /// <param name="headers">The headers dictionary to validate</param>
    /// <param name="paramName">The parameter name for exception messages</param>
    /// <exception cref="ArgumentException">Thrown when headers contain invalid data</exception>
    internal static void ValidateHeaders(IDictionary<string, string>? headers, string paramName = "headers") {
        if (headers == null) {
            return;
        }

        foreach (var header in headers) {
            if (string.IsNullOrWhiteSpace(header.Key)) {
                throw new ArgumentException("Header name cannot be null, empty, or whitespace.", paramName);
            }

            if (header.Value == null) {
                throw new ArgumentException($"Header '{header.Key}' has a null value. Header values cannot be null.", paramName);
            }

            // Prevent HTTP header injection attacks by checking for newline characters
            if (header.Value.Contains("\r") || header.Value.Contains("\n")) {
                throw new ArgumentException(
                    $"Header '{header.Key}' contains invalid characters (CR/LF). Header values cannot contain newline characters as this may lead to header injection vulnerabilities.",
                    paramName);
            }

            // Reject reserved headers that may cause unexpected behavior
            if (ReservedHeaders.Contains(header.Key)) {
                throw new ArgumentException(
                    $"Header '{header.Key}' is a reserved HTTP header and should not be set via custom headers. " +
                    $"Setting this header may cause authentication failures, request corruption, or other unexpected behavior. " +
                    $"Reserved headers include: {string.Join(", ", ReservedHeaders)}.",
                    paramName);
            }
        }
    }

    /// <summary>
    ///     Ensures that the configuration is valid otherwise throws an error
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    /// <exception cref="FgaValidationError"></exception>
    /// <exception cref="ArgumentException">Thrown when DefaultHeaders contain reserved or invalid headers</exception>
    public void EnsureValid() {
        if (BasePath == null || BasePath == "") {
            throw new FgaRequiredParamError("Configuration", "ApiUrl");
        }

        if (!IsWellFormedUriString(BasePath)) {
            throw new FgaValidationError(
                $"Configuration.ApiUrl ({ApiUrl ?? BasePath}) does not form a valid URI ({BasePath})");
        }

        if (MaxRetry > 15) {
            throw new FgaValidationError("Configuration.MaxRetry exceeds maximum allowed limit of 15");
        }

        // Validate that DefaultHeaders don't contain reserved headers
        ValidateHeaders(DefaultHeaders, nameof(DefaultHeaders));

        Credentials?.EnsureValid();
        Telemetry?.EnsureValid();
    }

    #endregion Methods

    #region Constants

    /// <summary>
    ///     Version of the package.
    /// </summary>
    /// <value>Version of the package.</value>
    public const string Version = "0.8.0";

    private const string DefaultUserAgent = "openfga-sdk dotnet/0.8.0";

    #endregion Constants


    #region Properties

    /// <summary>
    ///     Gets or sets the default headers.
    /// </summary>
    public IDictionary<string, string> DefaultHeaders { get; set; }

    /// <summary>
    ///     Gets or sets the HTTP user agent.
    /// </summary>
    /// <value>Http user agent.</value>
    public string UserAgent { get; set; }

    /// <summary>
    ///     Gets the Base Path.
    /// </summary>
    /// <value>Base Path.</value>
    public string BasePath {
        get {
            if (ApiUrl != null && ApiUrl != "") {
                return ApiUrl;
            }

            if (ApiHost != null && ApiUrl != "") {
                return $"{ApiScheme ?? "https"}://{ApiHost}";
            }

            return "";
        }
    }


    /// <summary>
    ///     Gets or sets the API Scheme.
    /// </summary>
    /// <value>ApiScheme.</value>
    [Obsolete("ApiScheme is deprecated, please use ApiUrl instead.")]
    public string ApiScheme { get; set; } = "https";

    /// <summary>
    ///     Gets or sets the API Host.
    /// </summary>
    /// <value>ApiHost.</value>
    [Obsolete("ApiHost is deprecated, please use ApiUrl instead.")]
    public string ApiHost { get; set; }

    /// <summary>
    ///     Gets or sets the API URL.
    /// </summary>
    /// <value>ApiUrl.</value>
    public string ApiUrl { get; set; }

    /// <summary>
    ///     Gets or sets the Store ID.
    /// </summary>
    /// <value>Store ID.</value>
    public string? StoreId { get; set; }

    /// <summary>
    ///     Gets or sets the Credentials
    /// </summary>
    /// <value>Credentials.</value>
    public Credentials? Credentials { get; set; }

    /// <summary>
    ///     Max number of times to retry after a request is rate limited
    /// </summary>
    /// <value>MaxRetry</value>
    public int MaxRetry { get; set; } = 3;

    /// <summary>
    ///     Minimum time in ms to wait before retrying
    /// </summary>
    /// <value>MinWaitInMs</value>
    public int MinWaitInMs { get; set; } = 100;

    /// <summary>
    ///     Gets or sets the telemetry configuration.
    /// </summary>
    public TelemetryConfig? Telemetry { get; set; }

    #endregion Properties
}