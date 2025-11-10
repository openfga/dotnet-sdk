using OpenFga.Sdk.Exceptions;
using System;
using System.Runtime.Serialization;

namespace OpenFga.Sdk.Configuration;

/// <summary>
/// Available credential methods
/// </summary>
public enum CredentialsMethod {
    /// <summary>
    /// None - no auth required
    /// </summary>
    [EnumMember(Value = "none")] None,

    /// <summary>
    /// API Bearer Token header required
    /// </summary>
    [EnumMember(Value = "api_token")] ApiToken,

    /// <summary>
    /// Client ID, Secret, API Token Issuer and Audience for client credential flow required
    /// </summary>
    [EnumMember(Value = "client_credentials")] ClientCredentials,
}

/// <summary>
///
/// </summary>
public interface IApiTokenConfig {
    public string? ApiToken { get; set; }
}

/// <summary>
///
/// </summary>
public interface IClientCredentialsConfig {

    /// <summary>
    ///     Gets or sets the Client ID.
    /// </summary>
    /// <value>Client ID.</value>
    string? ClientId { get; set; }

    /// <summary>
    ///     Gets or sets the Client Secret.
    /// </summary>
    /// <value>Client Secret.</value>
    string? ClientSecret { get; set; }

    /// <summary>
    ///     Gets or sets the API Token Issuer.
    ///     <para>
    ///     The SDK automatically normalizes URLs:
    ///     <list type="bullet">
    ///         <item><description>URLs without a scheme are prefixed with "https://"</description></item>
    ///         <item><description>If no path is provided (or only "/"), defaults to "/oauth/token"</description></item>
    ///         <item><description>Otherwise, the path is preserved</description></item>
    ///     </list>
    ///     </para>
    ///     <para>Examples:
    ///     <list type="bullet">
    ///         <item><description>"issuer.fga.example" → "https://issuer.fga.example/oauth/token"</description></item>
    ///         <item><description>"https://issuer.fga.example" → "https://issuer.fga.example/oauth/token"</description></item>
    ///         <item><description>"https://issuer.fga.example/custom/token" → "https://issuer.fga.example/custom/token"</description></item>
    ///     </list>
    ///     </para>
    /// </summary>
    /// <value>API Token Issuer.</value>
    string? ApiTokenIssuer { get; set; }

    /// <summary>
    ///     Gets or sets the API Audience.
    /// </summary>
    /// <value>API Audience.</value>
    string? ApiAudience { get; set; }
}

public interface ICredentialsConfig : IClientCredentialsConfig, IApiTokenConfig { }

public struct CredentialsConfig : ICredentialsConfig {
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? ApiTokenIssuer { get; set; }
    public string? ApiAudience { get; set; }
    public string? ApiToken { get; set; }
}

public interface IAuthCredentialsConfig {
    public CredentialsMethod Method { get; }
    public ICredentialsConfig? Config { get; }
}

/// <summary>
///
/// </summary>
public class Credentials : IAuthCredentialsConfig {

    /// <summary>
    ///     Initializes a new instance of the <see cref="Credentials" /> class
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    /// <exception cref="FgaValidationError"></exception>
    public Credentials() {
        EnsureValid();
    }

    /// <summary>
    /// credential methods
    /// </summary>
    public CredentialsMethod Method { get; set; } = CredentialsMethod.None;

    /// <summary>
    /// Credential config options
    /// </summary>
    public ICredentialsConfig? Config { get; set; }

    /// <summary>
    ///     Ensures the credentials configuration is valid otherwise throws an error
    /// </summary>
    /// <exception cref="FgaRequiredParamError"></exception>
    /// <exception cref="FgaValidationError"></exception>
    public void EnsureValid() {
        switch (Method) {
            case CredentialsMethod.ApiToken:
                if (string.IsNullOrWhiteSpace(Config?.ApiToken)) {
                    throw new FgaRequiredParamError("Configuration", nameof(Config.ApiToken));
                }
                break;
            case CredentialsMethod.ClientCredentials:
                if (string.IsNullOrWhiteSpace(Config?.ClientId)) {
                    throw new FgaRequiredParamError("Configuration", nameof(Config.ClientId));
                }

                if (string.IsNullOrWhiteSpace(Config?.ClientSecret)) {
                    throw new FgaRequiredParamError("Configuration", nameof(Config.ClientSecret));
                }

                if (string.IsNullOrWhiteSpace(Config?.ApiTokenIssuer)) {
                    throw new FgaRequiredParamError("Configuration", nameof(Config.ApiTokenIssuer));
                }

                if (string.IsNullOrWhiteSpace(Config?.ApiAudience)) {
                    throw new FgaRequiredParamError("Configuration", nameof(Config.ApiAudience));
                }

                // Validate that the normalized URL is well-formed
                var normalizedApiTokenIssuer = ApiTokenIssuerNormalizer.Normalize(Config?.ApiTokenIssuer);
                if (!string.IsNullOrWhiteSpace(normalizedApiTokenIssuer) && !IsWellFormedUriString(normalizedApiTokenIssuer)) {
                    throw new FgaValidationError($"Configuration.ApiTokenIssuer does not form a valid URI ({normalizedApiTokenIssuer})");
                }

                break;
            case CredentialsMethod.None:
            default:
                break;
        }
    }

    private static bool IsWellFormedUriString(string? uri) {
        return Uri.TryCreate(uri, UriKind.Absolute, out var uriResult) &&
               ((uriResult.ToString().Equals(uri) || uriResult.ToString().Equals($"{uri}/")) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));
    }

    static Credentials Init(IAuthCredentialsConfig config) {
        return new Credentials() {
            Method = config.Method,
            Config = config.Config,
        };
    }
}