//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//


using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Telemetry;
using System.Collections.Generic;
using Xunit;

namespace OpenFga.Sdk.Test.Configuration {
    /// <summary>
    /// Unit tests for TelemetryConfig default metrics and attributes.
    /// </summary>
    public class TelemetryConfigTests {
        /// <summary>
        /// Verifies that the default telemetry configuration includes the expected metrics
        /// and that each metric has the correct default attributes enabled (or excluded).
        /// </summary>
        [Fact]
        public void DefaultMetrics_ShouldContainExpectedMetricsAndAttributes() {
            var telemetryConfig = new TelemetryConfig().UseDefaultConfig();

            Assert.NotNull(telemetryConfig.Metrics);

            // Verify all expected default metrics are present
            Assert.True(telemetryConfig.Metrics!.ContainsKey(TelemetryMeter.TokenExchangeCount),
                $"Default metrics should contain {TelemetryMeter.TokenExchangeCount}.");
            Assert.True(telemetryConfig.Metrics.ContainsKey(TelemetryMeter.RequestDuration),
                $"Default metrics should contain {TelemetryMeter.RequestDuration}.");
            Assert.True(telemetryConfig.Metrics.ContainsKey(TelemetryMeter.QueryDuration),
                $"Default metrics should contain {TelemetryMeter.QueryDuration}.");

            foreach (var metricName in new[] { TelemetryMeter.TokenExchangeCount, TelemetryMeter.RequestDuration, TelemetryMeter.QueryDuration }) {
                var metricAttributes = telemetryConfig.Metrics[metricName].Attributes;

                Assert.NotNull(metricAttributes);

                // Attributes that should be enabled by default
                Assert.True(metricAttributes.Contains(TelemetryAttribute.RequestMethod),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.RequestMethod}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.RequestStoreId),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.RequestStoreId}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.RequestModelId),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.RequestModelId}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.RequestClientId),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.RequestClientId}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.ResponseModelId),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.ResponseModelId}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.HttpHost),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.HttpHost}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.HttpStatus),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.HttpStatus}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.HttpUserAgent),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.HttpUserAgent}.");
                Assert.True(metricAttributes.Contains(TelemetryAttribute.RequestRetryCount),
                    $"{metricName}: default attributes should contain {TelemetryAttribute.RequestRetryCount}.");

                // Attributes that should NOT be enabled by default
                Assert.False(metricAttributes.Contains(TelemetryAttribute.FgaRequestUser),
                    $"{metricName}: default attributes should not contain {TelemetryAttribute.FgaRequestUser}.");
                Assert.False(metricAttributes.Contains(TelemetryAttribute.RequestBatchCheckSize),
                    $"{metricName}: default attributes should not contain {TelemetryAttribute.RequestBatchCheckSize}.");
                Assert.False(metricAttributes.Contains(TelemetryAttribute.HttpScheme),
                    $"{metricName}: default attributes should not contain {TelemetryAttribute.HttpScheme}.");
                Assert.False(metricAttributes.Contains(TelemetryAttribute.HttpMethod),
                    $"{metricName}: default attributes should not contain {TelemetryAttribute.HttpMethod}.");
                Assert.False(metricAttributes.Contains(TelemetryAttribute.HttpUrl),
                    $"{metricName}: default attributes should not contain {TelemetryAttribute.HttpUrl}.");
            }
        }

        /// <summary>
        /// Verifies that RequestBatchCheckSize can be explicitly enabled in a custom TelemetryConfig.
        /// </summary>
        [Fact]
        public void RequestBatchCheckSize_CanBeExplicitlyEnabled() {
            var customConfig = new TelemetryConfig(new Dictionary<string, MetricConfig> {
                {
                    TelemetryMeter.RequestDuration, new MetricConfig {
                        Attributes = new System.Collections.Generic.HashSet<string> {
                            TelemetryAttribute.RequestBatchCheckSize
                        }
                    }
                }
            });

            Assert.True(
                customConfig.Metrics![TelemetryMeter.RequestDuration].Attributes.Contains(TelemetryAttribute.RequestBatchCheckSize),
                $"Custom config should contain {TelemetryAttribute.RequestBatchCheckSize} when explicitly set.");
        }

        /// <summary>
        /// Verifies that TelemetryAttribute.GetAllAttributes() includes RequestBatchCheckSize.
        /// </summary>
        [Fact]
        public void GetAllAttributes_ShouldContainRequestBatchCheckSize() {
            var allAttributes = TelemetryAttribute.GetAllAttributes();

            Assert.True(allAttributes.Contains(TelemetryAttribute.RequestBatchCheckSize),
                $"GetAllAttributes() should contain {TelemetryAttribute.RequestBatchCheckSize}.");
        }
    }
}

