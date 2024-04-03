//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// OAuth2 Client to exchange the credentials for an access token using the client credentials flow
/// </summary>
public class OAuth2Client {
    private const int TOKEN_EXPIRY_BUFFER_THRESHOLD_IN_SEC = 300;

    private const int
        TOKEN_EXPIRY_JITTER_IN_SEC = 300; // We add some jitter so that token refreshes are less likely to collide

    private static readonly Random _random = new();

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
    private IDictionary<string, string> _authRequest { get; set; }
    private string _apiTokenIssuer { get; set; }
    private RetryParams _retryParams;

    #endregion

    #region Methods

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuth2Client" /> class
    /// </summary>
    /// <param name="credentialsConfig"></param>
    /// <param name="httpClient"></param>
    /// <exception cref="NullReferenceException"></exception>
    public OAuth2Client(Credentials credentialsConfig, BaseClient httpClient, RetryParams retryParams) {
        if (string.IsNullOrWhiteSpace(credentialsConfig.Config!.ClientId)) {
            throw new FgaRequiredParamError("OAuth2Client", "config.ClientId");
        }

        if (string.IsNullOrWhiteSpace(credentialsConfig.Config.ClientSecret)) {
            throw new FgaRequiredParamError("OAuth2Client", "config.ClientSecret");
        }

        this._httpClient = httpClient;
        this._apiTokenIssuer = credentialsConfig.Config.ApiTokenIssuer;
        this._authRequest = new Dictionary<string, string>() {
            { "client_id", credentialsConfig.Config.ClientId },
            { "client_secret", credentialsConfig.Config.ClientSecret },
            { "audience", credentialsConfig.Config.ApiAudience },
            { "grant_type", "client_credentials" }
        };

        this._retryParams = retryParams;
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
            Body = Utils.CreateFormEncodedConent(this._authRequest),
        };

        var accessTokenResponse = await Retry(async () => await _httpClient.SendRequestAsync<AccessTokenResponse>(
            requestBuilder,
            null,
            "ExchangeTokenAsync",
            cancellationToken));

        _authToken = new AuthToken() {
            AccessToken = accessTokenResponse.AccessToken,
            ExpiresAt = DateTime.Now + TimeSpan.FromSeconds(accessTokenResponse.ExpiresIn)
        };
    }

    private async Task<TResult> Retry<TResult>(Func<Task<TResult>> retryable) {
        var numRetries = 0;
        while (true) {
            try {
                numRetries++;

                return await retryable();
            }
            catch (FgaApiRateLimitExceededError err) {
                if (numRetries > _retryParams.MaxRetry) {
                    throw;
                }
                var waitInMs = (int)((err.ResetInMs == null || err.ResetInMs < _retryParams.MinWaitInMs)
                    ? _retryParams.MinWaitInMs
                    : err.ResetInMs);

                await Task.Delay(waitInMs);
            }
            catch (FgaApiError err) {
                if (!err.ShouldRetry || numRetries > _retryParams.MaxRetry) {
                    throw;
                }
                var waitInMs = _retryParams.MinWaitInMs;

                await Task.Delay(waitInMs);
            }
        }
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