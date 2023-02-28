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
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// Base Client, used by the API and OAuth Clients
/// </summary>
public class BaseClient : IDisposable {
    private readonly HttpClient _httpClient;
    private bool _shouldDisposeWhenDone;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseClient"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpClient">Optional <see cref="HttpClient"/> to use when sending requests.</param>
    /// <remarks>
    /// If you supply a <see cref="HttpClient"/> it is your responsibility to manage its lifecycle and
    /// dispose it when appropriate.
    /// If you do not supply a <see cref="HttpClient"/> one will be created automatically and disposed
    /// of when this object is disposed.
    /// </remarks>
    public BaseClient(Configuration.Configuration configuration, HttpClient? httpClient = null) {
        _shouldDisposeWhenDone = httpClient == null;
        this._httpClient = httpClient ?? new HttpClient();
        this._httpClient.DefaultRequestHeaders.Accept.Clear();
        this._httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        foreach (var header in configuration.DefaultHeaders) {
            if (header.Value != null) {
                this._httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }

    /// <summary>
    /// Handles calling the API
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> SendRequestAsync<T>(RequestBuilder requestBuilder,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        var request = requestBuilder.BuildRequest();

        return await SendRequestAsync<T>(request, additionalHeaders, apiName, cancellationToken);
    }

    /// <summary>
    /// Handles calling the API for requests that are expected to return no content
    /// </summary>
    /// <param name="requestBuilder"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SendRequestAsync(RequestBuilder requestBuilder,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        var request = requestBuilder.BuildRequest();

        await this.SendRequestAsync(request, additionalHeaders, apiName, cancellationToken);
    }

    /// <summary>
    /// Handles calling the API
    /// </summary>
    /// <param name="request"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<T> SendRequestAsync<T>(HttpRequestMessage request,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders) {
                if (header.Value != null) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        {
            if (!response.IsSuccessStatusCode) {
                throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
            }

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false) ??
                throw new FgaError();
        }
    }

    /// <summary>
    /// Handles calling the API for requests that are expected to return no content
    /// </summary>
    /// <param name="request"></param>
    /// <param name="additionalHeaders"></param>
    /// <param name="apiName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="ApiException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task SendRequestAsync(HttpRequestMessage request,
        IDictionary<string, string>? additionalHeaders = null,
        string? apiName = null, CancellationToken cancellationToken = default) {
        if (additionalHeaders != null) {
            foreach (var header in additionalHeaders) {
                if (header.Value != null) {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
        }

        var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        {
            if (!response.IsSuccessStatusCode) {
                throw await ApiException.CreateSpecificExceptionAsync(response, request, apiName).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Disposes of any owned disposable resources such as the underlying <see cref="HttpClient"/> if owned.
    /// </summary>
    /// <param name="disposing">Whether we are actually disposing (<see langword="true"/>) or not (<see langword="false"/>).</param>
    protected virtual void Dispose(bool disposing) {
        if (disposing && _shouldDisposeWhenDone) {
            _httpClient.Dispose();
            _shouldDisposeWhenDone = false;
        }
    }

    /// <summary>
    /// Disposes of any owned disposable resources such as the underlying <see cref="HttpClient"/> if owned.
    /// </summary>
    public void Dispose() {
        Dispose(true);
    }
}