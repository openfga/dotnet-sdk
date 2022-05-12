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

namespace OpenFga.Sdk.Client;

/// <summary>
/// API Client - used by all the API related methods to call the API. Handles token exchange and retries.
/// </summary>
public class ApiClient : IDisposable {
    private readonly BaseClient _baseClient;
    private readonly OAuth2Client? _oauth2Client;
    private readonly Configuration.Configuration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// </summary>
    /// <param name="configuration">Client Configuration</param>
    /// <param name="userHttpClient">User Http Client - Allows Http Client reuse</param>
    public ApiClient(Configuration.Configuration configuration, HttpClient? userHttpClient = null) {
        configuration.IsValid();
        _configuration = configuration;
        _baseClient = new BaseClient(configuration, userHttpClient);

        if (!string.IsNullOrEmpty(_configuration.ClientId)) {
            _oauth2Client = new OAuth2Client(configuration, _baseClient);
        }
    }

    /// <summary>
    /// Handles getting the access token, calling the API and potentially retrying
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Response Type</typeparam>
    /// <returns></returns>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task<T> SendRequestAsync<T>(RequestBuilder requestBuilder, string apiName,
        CancellationToken cancellationToken = default) {
        IDictionary<string, string> additionalHeaders = new Dictionary<string, string>();

        if (_oauth2Client != null) {
            try {
                var token = await _oauth2Client.GetAccessTokenAsync();

                if (!string.IsNullOrEmpty(token)) {
                    additionalHeaders.Add("Authorization", $"Bearer {token}");
                }
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        return await Retry(async () => await _baseClient.SendRequestAsync<T>(requestBuilder, additionalHeaders, apiName, cancellationToken));
    }

    /// <summary>
    /// Handles getting the access token, calling the API and potentially retrying (use for requests that return no content)
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task SendRequestAsync(RequestBuilder requestBuilder, string apiName,
        CancellationToken cancellationToken = default) {
        IDictionary<string, string> additionalHeaders = new Dictionary<string, string>();

        if (_oauth2Client != null) {
            try {
                var token = await _oauth2Client.GetAccessTokenAsync();

                if (!string.IsNullOrEmpty(token)) {
                    additionalHeaders.Add("Authorization", $"Bearer {token}");
                }
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        await Retry(async () => await _baseClient.SendRequestAsync(requestBuilder, additionalHeaders, apiName, cancellationToken));
    }

    private async Task<TResult> Retry<TResult>(Func<Task<TResult>> retryable) {
        var numRetries = 0;
        while (true) {
            try {
                numRetries++;

                return await retryable();
            }
            catch (FgaApiRateLimitExceededError err) {
                if (numRetries > _configuration.MaxRetry) {
                    throw;
                }
                var waitInMs = (int)((err.ResetInMs == null || err.ResetInMs < _configuration.MinWaitInMs)
                    ? _configuration.MinWaitInMs
                    : err.ResetInMs);

                await Task.Delay(waitInMs);
            }
        }
    }

    private async Task Retry(Func<Task> retryable) {
        var numRetries = 0;
        while (true) {
            try {
                numRetries++;

                await retryable();

                return;
            }
            catch (FgaApiRateLimitExceededError err) {
                if (numRetries > _configuration.MaxRetry) {
                    throw;
                }
                var waitInMs = (int)((err.ResetInMs == null || err.ResetInMs < _configuration.MinWaitInMs)
                    ? _configuration.MinWaitInMs
                    : err.ResetInMs);

                await Task.Delay(waitInMs);
            }
        }
    }

    public void Dispose() {
        _baseClient.Dispose();
    }
}