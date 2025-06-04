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
using System.Diagnostics;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
///     API Client - used by all the API related methods to call the API. Handles token exchange and retries.
/// </summary>
public class ApiClient : IDisposable {
    private readonly BaseClient _baseClient;
    private readonly Configuration.Configuration _configuration;
    private readonly OAuth2Client? _oauth2Client;
    private readonly Metrics metrics;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiClient" /> class.
    /// </summary>
    /// <param name="configuration">Client Configuration</param>
    /// <param name="userHttpClient">User Http Client - Allows Http Client reuse</param>
    public ApiClient(Configuration.Configuration configuration, HttpClient? userHttpClient = null) {
        configuration.EnsureValid();
        _configuration = configuration;
        _baseClient = new BaseClient(configuration, userHttpClient);

        metrics = new Metrics(_configuration);

        if (_configuration.Credentials == null) {
            return;
        }

        switch (_configuration.Credentials.Method) {
            case CredentialsMethod.ApiToken:
                _configuration.DefaultHeaders["Authorization"] =
                    $"Bearer {_configuration.Credentials.Config!.ApiToken}";
                _baseClient = new BaseClient(_configuration, userHttpClient);
                break;
            case CredentialsMethod.ClientCredentials:
                _oauth2Client = new OAuth2Client(_configuration.Credentials, _baseClient,
                    new RetryParams { MaxRetry = _configuration.MaxRetry, MinWaitInMs = _configuration.MinWaitInMs },
                    metrics);
                break;
            case CredentialsMethod.None:
            default:
                break;
        }
    }

    /// <summary>
    ///     Handles getting the access token, calling the API and potentially retrying
    ///     Based on:
    ///     https://github.com/auth0/auth0.net/blob/595ae80ccad8aa7764b80d26d2ef12f8b35bbeff/src/Auth0.ManagementApi/HttpClientManagementConnection.cs#L67
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TReq">Request Type</typeparam>
    /// <typeparam name="TRes">Response Type</typeparam>
    /// <returns></returns>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task<TRes> SendRequestAsync<TReq, TRes>(RequestBuilder<TReq> requestBuilder, string apiName,
        CancellationToken cancellationToken = default) {
        IDictionary<string, string> additionalHeaders = new Dictionary<string, string>();

        var sw = Stopwatch.StartNew();
        if (_oauth2Client != null) {
            try {
                var token = await _oauth2Client.GetAccessTokenAsync();

                if (!string.IsNullOrEmpty(token)) {
                    additionalHeaders["Authorization"] = $"Bearer {token}";
                }
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        var response = await Retry(async () =>
            await _baseClient.SendRequestAsync<TReq, TRes>(requestBuilder, additionalHeaders, apiName,
                cancellationToken));

        sw.Stop();
        metrics.BuildForResponse(apiName, response.rawResponse, requestBuilder, sw,
            response.retryCount);

        return response.responseContent;
    }

    /// <summary>
    ///     Handles getting the access token, calling the API and potentially retrying (use for requests that return no
    ///     content)
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TReq">Request Type</typeparam>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task SendRequestAsync<TReq>(RequestBuilder<TReq> requestBuilder, string apiName,
        CancellationToken cancellationToken = default) {
        IDictionary<string, string> additionalHeaders = new Dictionary<string, string>();

        var sw = Stopwatch.StartNew();
        if (_oauth2Client != null) {
            try {
                var token = await _oauth2Client.GetAccessTokenAsync();

                if (!string.IsNullOrEmpty(token)) {
                    additionalHeaders["Authorization"] = $"Bearer {token}";
                }
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        var response = await Retry(async () =>
            await _baseClient.SendRequestAsync<TReq, object>(requestBuilder, additionalHeaders, apiName,
                cancellationToken));

        sw.Stop();
        metrics.BuildForResponse(apiName, response.rawResponse, requestBuilder, sw,
            response.retryCount);
    }

    private async Task<ResponseWrapper<TResult>> Retry<TResult>(Func<Task<ResponseWrapper<TResult>>> retryable) {
        var requestCount = 0;
        while (true) {
            try {
                requestCount++;

                var response = await retryable();

                response.retryCount =
                    requestCount - 1; // OTEL spec specifies that the original request is not included in the count

                return response;
            }
            catch (FgaApiRateLimitExceededError err) {
                if (requestCount > _configuration.MaxRetry) {
                    throw;
                }

                var waitInMs = (int)(err.ResetInMs == null || err.ResetInMs < _configuration.MinWaitInMs
                    ? _configuration.MinWaitInMs
                    : err.ResetInMs);

                await Task.Delay(waitInMs);
            }
            catch (FgaApiError err) {
                if (!err.ShouldRetry || requestCount > _configuration.MaxRetry) {
                    throw;
                }

                var waitInMs = _configuration.MinWaitInMs;

                await Task.Delay(waitInMs);
            }
        }
    }

    public void Dispose() => _baseClient.Dispose();
}