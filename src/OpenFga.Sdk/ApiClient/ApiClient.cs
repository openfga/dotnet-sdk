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


using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
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
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T">Response Type</typeparam>
    /// <returns></returns>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task<TRes> SendRequestAsync<TReq, TRes>(RequestBuilder<TReq> requestBuilder, string apiName,
        IRequestOptions? options = null,
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

        var additionalHeaders = BuildHeaders(_configuration, oauthToken, options);

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
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="FgaApiAuthenticationError"></exception>
    public async Task SendRequestAsync<TReq>(RequestBuilder<TReq> requestBuilder, string apiName,
        IRequestOptions? options = null,
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

        var additionalHeaders = BuildHeaders(_configuration, oauthToken, options);

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
    ///     Builds the complete headers dictionary by merging default headers, OAuth token, and per-request headers.
    ///     Validates per-request headers and performs case-insensitive merging.
    ///     Header precedence (lowest to highest): DefaultHeaders → OAuth token → Per-request headers
    /// </summary>
    /// <param name="configuration">Configuration containing default headers</param>
    /// <param name="oauthToken">OAuth access token if available</param>
    /// <param name="options">Request options containing custom headers</param>
    /// <returns>Merged headers dictionary or null if no headers to add</returns>
    /// <exception cref="ArgumentException">Thrown when header key is null, empty, or whitespace</exception>
    /// <exception cref="ArgumentNullException">Thrown when header value is null</exception>
    private static IDictionary<string, string>? BuildHeaders(Configuration.Configuration configuration, string? oauthToken, IRequestOptions? options) {
        var defaultHeaders = configuration.DefaultHeaders;
        var perRequestHeaders = options?.Headers;

        // Validate per-request headers
        Configuration.Configuration.ValidateHeaders(perRequestHeaders, "options.Headers");

        // Return null if no headers to add
        if (string.IsNullOrEmpty(oauthToken) &&
            (defaultHeaders == null || defaultHeaders.Count == 0) &&
            (perRequestHeaders == null || perRequestHeaders.Count == 0)) {
            return null;
        }

        // Use case-insensitive dictionary for proper header merging
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Default headers from configuration, if set
        if (defaultHeaders != null) {
            foreach (var header in defaultHeaders) {
                headers[header.Key] = header.Value;
            }
        }

        // Authorization token header, if set
        if (!string.IsNullOrEmpty(oauthToken)) {
            headers["Authorization"] = $"Bearer {oauthToken}";
        }

        // Per-request headers, if set
        if (perRequestHeaders != null) {
            foreach (var header in perRequestHeaders) {
                headers[header.Key] = header.Value;
            }
        }

        return headers;
    }

    public void Dispose() => _baseClient.Dispose();
}