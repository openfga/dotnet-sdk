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


using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Configuration;
using System.Diagnostics;

namespace OpenFga.Sdk.Telemetry;

/// <summary>
///     Class for managing metrics.
/// </summary>
public class Metrics {
    public const string Name = "OpenFga.Sdk";

    // This is to store all the enabled attributes across all metrics so that they can be computed only once
    private readonly HashSet<string> _allEnabledAttributes = new HashSet<string>();
    private readonly Credentials? _credentialsConfig;

    private readonly TelemetryConfig _telemetryConfig;

    private TelemetryCounters Counters { get; }
    private TelemetryHistograms Histograms { get; }
    public Meter Meter { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Metrics" /> class.
    /// </summary>
    public Metrics(Configuration.Configuration config) {
        Meter = new Meter(Name, Configuration.Configuration.Version);
        Histograms = new TelemetryHistograms(Meter);
        Counters = new TelemetryCounters(Meter);
        _telemetryConfig = config.Telemetry ?? new TelemetryConfig().UseDefaultConfig();
        _credentialsConfig = config.Credentials;

        if (_telemetryConfig.Metrics?.Keys == null) {
            return;
        }

        foreach (var metricName in _telemetryConfig.Metrics.Keys) {
            var attributesHashSet = _telemetryConfig.Metrics[metricName]?.Attributes ?? new HashSet<string>();
            foreach (var attribute in attributesHashSet) {
                _allEnabledAttributes.Add(attribute);
            }
        }
    }

    /// <summary>
    ///     Builds metrics for a given HTTP response.
    /// </summary>
    /// <typeparam name="T">The type of the request builder response.</typeparam>
    /// <param name="apiName">The API name.</param>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="requestBuilder">The request builder.</param>
    /// <param name="requestDuration">The stopwatch measuring the request duration.</param>
    /// <param name="retryCount">The number of retries attempted.</param>
    public void BuildForResponse<T>(string apiName,
        HttpResponseMessage response, RequestBuilder<T> requestBuilder, Stopwatch requestDuration, int retryCount) {
        if (_telemetryConfig?.Metrics == null || _telemetryConfig?.Metrics.Count == 0) {
            // No point processing if all metrics are disabled
            return;
        }

        // Compute all enabled attributes once and then attached them to the metrics configured after
        var attributes = Attributes.BuildAttributesForResponse(
            _allEnabledAttributes, _credentialsConfig, apiName, response, requestBuilder, requestDuration, retryCount);

        if (apiName == "ClientCredentialsExchange" &&
            _telemetryConfig!.Metrics.TryGetValue(TelemetryMeter.TokenExchangeCount, out var exchangeCountConfig)) {
            Counters.IncrementTokenExchangeCounter(Attributes.FilterAttributes(attributes,
                exchangeCountConfig?.Attributes));
        }
        else {
            // We only want to increment the request counter for non-token exchange requests
            if (_telemetryConfig!.Metrics.TryGetValue(TelemetryMeter.RequestCount, out var requestCountConfig)) {
                Counters.IncrementRequestCounter(Attributes.FilterAttributes(attributes,
                    requestCountConfig?.Attributes));
            }

            // The query duration is only provided by OpenFGA servers
            if (_telemetryConfig.Metrics.TryGetValue(TelemetryMeter.QueryDuration, out var queryDurationConfig)) {
                Histograms.RecordQueryDuration(response,
                    Attributes.FilterAttributes(attributes, queryDurationConfig?.Attributes));
            }
        }

        if (_telemetryConfig.Metrics.TryGetValue(TelemetryMeter.RequestDuration, out var requestDurationConfig)) {
            Histograms.RecordRequestDuration(requestDuration,
                Attributes.FilterAttributes(attributes, requestDurationConfig?.Attributes));
        }
    }

    /// <summary>
    ///     Builds metrics for a client credentials response.
    /// </summary>
    /// <typeparam name="T">The type of the request builder.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="requestBuilder">The request builder.</param>
    /// <param name="requestDuration">The stopwatch measuring the request duration.</param>
    /// <param name="retryCount">The number of retries attempted.</param>
    public void BuildForClientCredentialsResponse<T>(
        HttpResponseMessage response, RequestBuilder<T> requestBuilder, Stopwatch requestDuration, int retryCount) =>
        BuildForResponse("ClientCredentialsExchange", response, requestBuilder, requestDuration, retryCount);
}