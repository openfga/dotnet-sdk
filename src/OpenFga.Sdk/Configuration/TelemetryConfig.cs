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


using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Telemetry;

namespace OpenFga.Sdk.Configuration;

/// <summary>
///     Configuration for a specific metric, including its enabled attributes.
/// </summary>
public class MetricConfig {
    /// <summary>
    ///     List of enabled attributes associated with the metric.
    /// </summary>
    public HashSet<string> Attributes { get; set; } = new();
}

/// <summary>
///     Configuration for telemetry, including metrics.
/// </summary>
public class TelemetryConfig {
    /// <summary>
    ///     Dictionary of metric configurations, keyed by metric name.
    /// </summary>
    public IDictionary<string, MetricConfig>? Metrics { get; set; } = new Dictionary<string, MetricConfig>();

    /// <summary>
    ///    Initializes a new instance of the <see cref="TelemetryConfig" /> class.
    /// </summary>
    /// <param name="metrics"></param>
    public TelemetryConfig(IDictionary<string, MetricConfig>? metrics) {
        Metrics = metrics;
    }

    /// <summary>
    ///    Initializes a new instance of the <see cref="TelemetryConfig" /> class.
    /// </summary>
    public TelemetryConfig() {
    }

    /// <summary>
    /// Sets the configuration to use the default metrics.
    /// </summary>
    public TelemetryConfig UseDefaultConfig() {
        Metrics = GetDefaultMetricsConfiguration();
        return this;
    }

    /// <summary>
    ///     Returns the default metrics configuration.
    /// </summary>
    /// <returns></returns>
    private static IDictionary<string, MetricConfig> GetDefaultMetricsConfiguration() {
        var defaultAttributes = new HashSet<string> {
            TelemetryAttribute.HttpHost,
            TelemetryAttribute.HttpStatus,
            TelemetryAttribute.HttpUserAgent,
            TelemetryAttribute.RequestMethod,
            TelemetryAttribute.RequestClientId,
            TelemetryAttribute.RequestStoreId,
            TelemetryAttribute.RequestModelId,
            TelemetryAttribute.RequestRetryCount,
            TelemetryAttribute.ResponseModelId

            // These metrics are not included by default because they are usually less useful
            // TelemetryAttribute.HttpScheme,
            // TelemetryAttribute.HttpMethod,
            // TelemetryAttribute.HttpUrl,

            // This not included by default as it has a very high cardinality which could increase costs for users
            // TelemetryAttribute.FgaRequestUser
        };

        return new Dictionary<string, MetricConfig> {
            { TelemetryMeter.TokenExchangeCount, new MetricConfig { Attributes = defaultAttributes } },
            { TelemetryMeter.RequestDuration, new MetricConfig { Attributes = defaultAttributes } },
            { TelemetryMeter.QueryDuration, new MetricConfig { Attributes = defaultAttributes } },
            // { TelemetryMeters.RequestCount, new MetricConfig { Attributes = defaultAttributes } }
        };
    }

    /// <summary>
    ///    Validates the telemetry configuration.
    /// </summary>
    /// <exception cref="FgaValidationError"></exception>
    public void EnsureValid() {
        if (Metrics == null) {
            return;
        }

        var supportedMeters = TelemetryMeter.GetAllMeters();
        var supportedAttributes = TelemetryAttribute.GetAllAttributes();
        foreach (var metricName in Metrics.Keys) {
            if (!supportedMeters.Contains(metricName)) {
                throw new FgaValidationError($"Telemetry.Metrics[{metricName}] is not a supported metric");
            }

            foreach (var attribute in Metrics[metricName].Attributes) {
                if (!supportedAttributes.Contains(attribute)) {
                    throw new FgaValidationError($"Telemetry.Metrics[{metricName}].Attributes[{attribute}] is not a supported attribute");
                }
            }
        }
    }
}