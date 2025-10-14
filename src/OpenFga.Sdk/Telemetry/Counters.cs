using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenFga.Sdk.Telemetry;

/// <summary>
///     Class for managing telemetry counters.
/// </summary>
public class TelemetryCounters {
    public Counter<int> TokenExchangeCounter { get; }

    public Counter<int> RequestCounter { get; private set; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelemetryCounters" /> class.
    /// </summary>
    /// <param name="meter">The meter used to create counters.</param>
    public TelemetryCounters(Meter meter) {
        TokenExchangeCounter = meter.CreateCounter<int>(TelemetryMeter.TokenExchangeCount,
            description: "The count of token exchange requests");
        RequestCounter =
            meter.CreateCounter<int>(TelemetryMeter.RequestCount, description: "The count of requests made");
    }

    /// <summary>
    ///     Increments the counter for a token exchange.
    /// </summary>
    /// <param name="attributes">The attributes associated with the telemetry data.</param>
    public void IncrementTokenExchangeCounter(TagList attributes) => TokenExchangeCounter.Add(1, attributes);

    /// <summary>
    ///     Increments the counter for an API request.
    /// </summary>
    /// <param name="attributes">The attributes associated with the telemetry data.</param>
    public void IncrementRequestCounter(TagList attributes) => TokenExchangeCounter.Add(1, attributes);
}