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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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
    /// <param name="perRequestHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Response Type</typeparam>
    /// <returns></returns>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task<TRes> SendRequestAsync<TReq, TRes>(RequestBuilder<TReq> requestBuilder, string apiName,
        IDictionary<string, string>? perRequestHeaders = null,
        CancellationToken cancellationToken = default) {
        var sw = Stopwatch.StartNew();

        string? oauthToken = null;
        if (_oauth2Client != null) {
            try {
                oauthToken = await _oauth2Client.GetAccessTokenAsync();
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        var additionalHeaders = BuildHeaders(oauthToken, perRequestHeaders);

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
    /// <param name="perRequestHeaders"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task SendRequestAsync<TReq>(RequestBuilder<TReq> requestBuilder, string apiName,
        IDictionary<string, string>? perRequestHeaders = null,
        CancellationToken cancellationToken = default) {
        var sw = Stopwatch.StartNew();

        string? oauthToken = null;
        if (_oauth2Client != null) {
            try {
                oauthToken = await _oauth2Client.GetAccessTokenAsync();
            }
            catch (ApiException e) {
                throw new FgaApiAuthenticationError("Invalid Client Credentials", apiName, e);
            }
        }

        var additionalHeaders = BuildHeaders(oauthToken, perRequestHeaders);

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

    /// <summary>
    ///     Builds the complete headers dictionary by merging OAuth token and per-request headers.
    ///     Validates per-request headers and performs case-insensitive merging.
    /// </summary>
    /// <param name="oauthToken">OAuth access token if available</param>
    /// <param name="perRequestHeaders">Per-request custom headers</param>
    /// <returns>Merged headers dictionary or null if no headers to add</returns>
    /// <exception cref="ArgumentException">Thrown when header key is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentNullException">Thrown when header value is null</exception>
    private static IDictionary<string, string>? BuildHeaders(string? oauthToken, IDictionary<string, string>? perRequestHeaders) {
        // Validate per-request headers at the client boundary
        if (perRequestHeaders != null) {
            foreach (var header in perRequestHeaders) {
                if (string.IsNullOrWhiteSpace(header.Key)) {
                    throw new ArgumentException(
                        "Header name cannot be null, empty, or whitespace.",
                        nameof(perRequestHeaders));
                }

                if (header.Value == null) {
                    throw new ArgumentNullException(
                        nameof(perRequestHeaders),
                        $"Header '{header.Key}' has a null value. Header values cannot be null.");
                }
            }
        }

        // Return null if no headers to add
        if (string.IsNullOrEmpty(oauthToken) && (perRequestHeaders == null || perRequestHeaders.Count == 0)) {
            return null;
        }

        // Use case-insensitive dictionary for proper header merging
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Add OAuth token header first
        if (!string.IsNullOrEmpty(oauthToken)) {
            headers["Authorization"] = $"Bearer {oauthToken}";
        }

        // Overlay per-request headers (these take precedence regardless of casing)
        if (perRequestHeaders != null) {
            foreach (var header in perRequestHeaders) {
                headers[header.Key] = header.Value;
            }
        }

        return headers;
    }

    public void Dispose() => _baseClient.Dispose();
}