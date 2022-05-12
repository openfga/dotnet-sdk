//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.Exceptions;

namespace OpenFga.Sdk.Configuration;

/// <summary>
/// Setup OpenFGA Configuration
/// </summary>
public class Configuration {
    #region Methods

    private static bool IsWellFormedUriString(string uri) {
        return Uri.TryCreate(uri, UriKind.Absolute, out var uriResult) &&
               ((uriResult.ToString().Equals(uri) || uriResult.ToString().Equals($"{uri}/")) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));
    }

    /// <summary>
    ///     Checks if the configuration is valid
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    public void IsValid() {
        if (string.IsNullOrWhiteSpace(StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(StoreId));
        }

        if (string.IsNullOrWhiteSpace(ApiScheme)) {
            throw new FgaRequiredParamError("Configuration", nameof(ApiScheme));
        }

        if (string.IsNullOrWhiteSpace(ApiHost)) {
            throw new FgaRequiredParamError("Configuration", nameof(ApiHost));
        }

        if (!IsWellFormedUriString(BasePath)) {
            throw new FgaValidationError(
                $"Configuration.ApiScheme ({ApiScheme}) and Configuration.ApiHost ({ApiHost}) do not form a valid URI ({BasePath})");
        }

        if (!string.IsNullOrWhiteSpace(ClientId) || !string.IsNullOrWhiteSpace(ClientSecret)) {
            if (string.IsNullOrWhiteSpace(ClientId)) {
                throw new FgaRequiredParamError("Configuration", nameof(ClientId));
            }

            if (string.IsNullOrWhiteSpace(ClientSecret)) {
                throw new FgaRequiredParamError("Configuration", nameof(ClientSecret));
            }

            if (string.IsNullOrWhiteSpace(ApiTokenIssuer)) {
                throw new FgaRequiredParamError("Configuration", nameof(ApiTokenIssuer));
            }

            if (string.IsNullOrWhiteSpace(ApiAudience)) {
                throw new FgaRequiredParamError("Configuration", nameof(ApiAudience));
            }
        }

        if (!string.IsNullOrWhiteSpace(ApiTokenIssuer) && !IsWellFormedUriString($"https://{ApiTokenIssuer}")) {
            throw new FgaValidationError(
                $"Configuration.ApiTokenIssuer does not form a valid URI (https://{ApiTokenIssuer})");
        }

        if (MaxRetry > 5) {
            throw new FgaValidationError("Configuration.MaxRetry exceeds maximum allowed limit of 5");
        }
    }

    #endregion

    #region Constants

    /// <summary>
    ///     Version of the package.
    /// </summary>
    /// <value>Version of the package.</value>
    public const string Version = "0.0.1";

    #endregion Constants

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    public Configuration(string storeId) : this() {
        StoreId = storeId;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    public Configuration() {
        UserAgent = "openfga-sdk {sdkId}/{packageVersion}".Replace("{sdkId}", "dotnet").Replace("{packageVersion}", "0.0.1");
        DefaultHeaders ??= new Dictionary<string, string>();

        if (!DefaultHeaders.ContainsKey("User-Agent")) {
            DefaultHeaders.Add("User-Agent", UserAgent);
        }
    }

    #endregion Constructors


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
    public string BasePath => $"{ApiScheme}://{ApiHost}";

    /// <summary>
    ///     Gets or sets the API Scheme.
    /// </summary>
    /// <value>ApiScheme.</value>
    public string ApiScheme { get; set; } = "https";

    /// <summary>
    ///     Gets or sets the API Host.
    /// </summary>
    /// <value>ApiHost.</value>
    public string ApiHost { get; set; }

    /// <summary>
    ///     Gets or sets the Store ID.
    /// </summary>
    /// <value>Store ID.</value>
    public string StoreId { get; set; }

    /// <summary>
    ///     Gets or sets the Client ID.
    /// </summary>
    /// <value>Client ID.</value>
    public string? ClientId { get; set; }

    /// <summary>
    ///     Gets or sets the Client Secret.
    /// </summary>
    /// <value>Client Secret.</value>
    public string? ClientSecret { get; set; }

    /// <summary>
    ///     Gets or sets the API Token Issuer.
    /// </summary>
    /// <value>API Token Issuer.</value>
    public string ApiTokenIssuer { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the API Audience.
    /// </summary>
    /// <value>API Audience.</value>
    public string ApiAudience { get; set; } = null!;

    /// <summary>
    ///     Max number of times to retry after a request is rate limited
    /// </summary>
    /// <value>MaxRetry</value>
    public int MaxRetry { get; set; } = 0;

    /// <summary>
    ///     Minimum time in ms to wait before retrying
    /// </summary>
    /// <value>MinWaitInMs</value>
    public int MinWaitInMs { get; set; } = 100;

    #endregion Properties
}