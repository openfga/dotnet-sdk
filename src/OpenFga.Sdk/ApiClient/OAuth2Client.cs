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


using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Telemetry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
///     OAuth2 Client to exchange the credentials for an access token using the client credentials flow
/// </summary>
public class OAuth2Client {
    private const int TOKEN_EXPIRY_BUFFER_THRESHOLD_IN_SEC = 300;

    private const int
        TOKEN_EXPIRY_JITTER_IN_SEC = 300; // We add some jitter so that token refreshes are less likely to collide

    private static readonly Random _random = new();
    private readonly Metrics metrics;

    /// <summary>
    ///     Credentials Flow Response
    ///     https://auth0.com/docs/get-started/authentication-and-authorization-flow/client-credentials-flow
    /// </summary>
    public class AccessTokenResponse {
        /// <summary>
        ///     Time period after which the token will expire (in ms)
        /// </summary>
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        ///     Token Type
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        /// <summary>
        ///     Access token to use
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
    }

    private class AuthToken {
        public DateTime? ExpiresAt { get; set; }

        public string? AccessToken { get; set; }

        public bool IsValid() =>
            !string.IsNullOrWhiteSpace(AccessToken) && (ExpiresAt == null ||
                                                        ExpiresAt - DateTime.Now >
                                                        TimeSpan.FromSeconds(
                                                            TOKEN_EXPIRY_BUFFER_THRESHOLD_IN_SEC +
                                                            _random.Next(0, TOKEN_EXPIRY_JITTER_IN_SEC)));
    }

    #region Fields

    private readonly BaseClient _httpClient;
    private AuthToken _authToken = new();
    private IDictionary<string, string> _authRequest { get; }
    private string _apiTokenIssuer { get; }
    private readonly RetryHandler _retryHandler;

    #endregion

    #region Methods

    /// <summary>
    ///     Initializes a new instance of the <see cref="OAuth2Client" /> class
    /// </summary>
    /// <param name="credentialsConfig"></param>
    /// <param name="httpClient"></param>
    /// <param name="retryParams"></param>
    /// <param name="metrics"></param>
    /// <exception cref="NullReferenceException"></exception>
    public OAuth2Client(Credentials credentialsConfig, BaseClient httpClient, RetryParams retryParams,
        Metrics metrics) {
        if (credentialsConfig == null) {
            throw new Exception("Credentials are required for OAuth2Client");
        }

        if (string.IsNullOrWhiteSpace(credentialsConfig.Config!.ClientId)) {
            throw new FgaRequiredParamError("OAuth2Client", "config.ClientId");
        }

        if (string.IsNullOrWhiteSpace(credentialsConfig.Config.ClientSecret)) {
            throw new FgaRequiredParamError("OAuth2Client", "config.ClientSecret");
        }

        if (string.IsNullOrWhiteSpace(credentialsConfig.Config.ApiTokenIssuer)) {
            throw new FgaRequiredParamError("OAuth2Client", "config.ApiTokenIssuer");
        }

        _httpClient = httpClient;
        _apiTokenIssuer = credentialsConfig.Config.ApiTokenIssuer;
        _authRequest = new Dictionary<string, string> {
            { "client_id", credentialsConfig.Config.ClientId },
            { "client_secret", credentialsConfig.Config.ClientSecret },
            { "grant_type", "client_credentials" }
        };

        if (credentialsConfig.Config.ApiAudience != null) {
            _authRequest["audience"] = credentialsConfig.Config.ApiAudience;
        }

        _retryHandler = new RetryHandler(retryParams);
        this.metrics = metrics;
    }

    /// <summary>
    ///     Exchange client id and client secret for an access token, and handles token refresh
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    private async Task ExchangeTokenAsync(CancellationToken cancellationToken = default) {
        var requestBuilder = new RequestBuilder<IDictionary<string, string>> {
            Method = HttpMethod.Post,
            BasePath = $"https://{_apiTokenIssuer}",
            PathTemplate = "/oauth/token",
            Body = _authRequest,
            ContentType = "application/x-www-form-urlencode"
        };

        var sw = Stopwatch.StartNew();
        var accessTokenResponse = await Retry(async () =>
            await _httpClient.SendRequestAsync<IDictionary<string, string>, AccessTokenResponse>(
                requestBuilder,
                null,
                "ExchangeTokenAsync",
                cancellationToken));

        sw.Stop();

        metrics.BuildForClientCredentialsResponse(accessTokenResponse.rawResponse, requestBuilder,
            sw, accessTokenResponse.retryCount);

        _authToken = new AuthToken { AccessToken = accessTokenResponse.responseContent?.AccessToken };

        if (accessTokenResponse.responseContent?.ExpiresIn != null) {
            _authToken.ExpiresAt = DateTime.Now + TimeSpan.FromSeconds(accessTokenResponse.responseContent.ExpiresIn);
        }
    }

    private async Task<TResult> Retry<TResult>(Func<Task<TResult>> retryable) {
        var requestCount = 0;
        var attemptCount = 0; // 0 = initial request, 1+ = retry attempts

        while (true) {
            try {
                requestCount++;

                return await retryable();
            }
            catch (FgaApiError err) when (err is FgaApiRateLimitExceededError || err.ShouldRetry) {
                // Check if we should retry based on status code and attempt count
                if (!_retryHandler.ShouldRetry(err.StatusCode, attemptCount)) {
                    throw;
                }

                // Calculate delay using Retry-After header or exponential backoff
                var delay = _retryHandler.CalculateDelayFromHeaders(err.ResponseHeaders, attemptCount);

                await Task.Delay(delay);
                attemptCount++;
            }
            catch (Exception ex) when (_retryHandler.IsTransientError(ex, attemptCount)) {
                // Network error - retry with exponential backoff (no headers available)
                var delay = _retryHandler.CalculateDelayFromHeaders(null, attemptCount);
                await Task.Delay(delay);
                attemptCount++;
            }
        }
    }

    /// <summary>
    ///     Gets the access token, and handles exchanging, rudimentary in memory caching and refreshing it when expired
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