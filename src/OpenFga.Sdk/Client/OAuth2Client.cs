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
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client;

/// <summary>
/// OAuth2 Client to exchange the credentials for an access token using the client credentials flow
/// </summary>
public class OAuth2Client {
    private const int TOKEN_EXPIRY_BUFFER_THRESHOLD_IN_SEC = 300;

    private const int
        TOKEN_EXPIRY_JITTER_IN_SEC = 300; // We add some jitter so that token refreshes are less likely to collide

    private static readonly Random _random = new();

    private class AuthRequestBody {
        [JsonPropertyName("audience")] public string? Audience { get; set; }
        [JsonPropertyName("client_id")] public string? ClientId { get; set; }
        [JsonPropertyName("client_secret")] public string? ClientSecret { get; set; }
        [JsonPropertyName("grant_type")] public string? GrantType { get; set; }
    }

    /// <summary>
    /// Credentials Flow Response
    ///
    /// https://auth0.com/docs/get-started/authentication-and-authorization-flow/client-credentials-flow
    /// </summary>
    public class AccessTokenResponse {
        /// <summary>
        /// Time period after which the token will expire (in ms)
        /// </summary>
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// Token Type
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        /// <summary>
        /// Access token to use
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }

    private class AuthToken {
        public DateTime? ExpiresAt { get; set; }

        public string? AccessToken { get; set; }

        public bool IsValid() {
            return !string.IsNullOrWhiteSpace(AccessToken) && (ExpiresAt == null ||
                                                               ExpiresAt - DateTime.Now >
                                                               TimeSpan.FromSeconds(
                                                                   TOKEN_EXPIRY_BUFFER_THRESHOLD_IN_SEC +
                                                                   (_random.Next(0, TOKEN_EXPIRY_JITTER_IN_SEC))));
        }
    }

    #region Fields

    private readonly BaseClient _httpClient;
    private AuthToken _authToken = new();
    private AuthRequestBody _authRequest { get; set; }
    private string _apiTokenIssuer { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2Client" /> class
    /// </summary>
    /// <param name="config"></param>
    /// <param name="httpClient"></param>
    /// <exception cref="NullReferenceException"></exception>
    public OAuth2Client(Configuration.Configuration config, BaseClient httpClient) {
        if (string.IsNullOrWhiteSpace(config.ClientId)) {
            throw new OpenFgaRequiredParamError("OAuth2Client", "config.ClientId");
        }

        if (string.IsNullOrWhiteSpace(config.ClientSecret)) {
            throw new OpenFgaRequiredParamError("OAuth2Client", "config.ClientSecret");
        }

        this._httpClient = httpClient;
        this._apiTokenIssuer = config.ApiTokenIssuer;
        this._authRequest = new AuthRequestBody() {
            ClientId = config.ClientId,
            ClientSecret = config.ClientSecret,
            Audience = config.ApiAudience,
            GrantType = "client_credentials"
        };
    }

    /// <summary>
    /// Exchange client id and client secret for an access token, and handles token refresh
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    private async Task ExchangeTokenAsync(CancellationToken cancellationToken = default) {
        var requestBuilder = new RequestBuilder {
            Method = HttpMethod.Post,
            BasePath = $"https://{this._apiTokenIssuer}",
            PathTemplate = "/oauth/token",
            Body = Utils.CreateJsonStringContent(this._authRequest)
        };

        var accessTokenResponse = await _httpClient.SendRequestAsync<AccessTokenResponse>(
            requestBuilder,
            null,
            "ExchangeTokenAsync",
            cancellationToken);

        _authToken = new AuthToken() {
            AccessToken = accessTokenResponse.AccessToken,
            ExpiresAt = DateTime.Now + TimeSpan.FromMilliseconds(accessTokenResponse.ExpiresIn)
        };
    }

    /// <summary>
    /// Gets the access token, and handles exchanging, rudimentary in memory caching and refreshing it when expired
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<string> GetAccessTokenAsync() {
        // If we already have an access token in memory
        if (_authToken.IsValid()) {
            return _authToken.AccessToken!;
        }

        await ExchangeTokenAsync();

        return _authToken.AccessToken ?? throw new InvalidOperationException();
    }

    #endregion
}