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
    ///     Ensures that the configuration is valid otherwise throws an error
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    /// <exception cref="FgaValidationError"></exception>
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

        Credentials?.EnsureValid();
        Telemetry?.EnsureValid();
    }

    #endregion Methods

    #region Constants

    /// <summary>
    ///     Version of the package.
    /// </summary>
    /// <value>Version of the package.</value>
    public const string Version = "0.7.0-alpha.1";

    private const string DefaultUserAgent = "openfga-sdk dotnet/0.7.0-alpha.1";

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