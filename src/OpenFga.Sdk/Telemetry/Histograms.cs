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


using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;

namespace OpenFga.Sdk.Telemetry;

/// <summary>
///     Class for managing telemetry histograms.
/// </summary>
public class TelemetryHistograms {
    /// <summary>
    /// Histogram for query duration.
    /// </summary>
    public Histogram<float> QueryDurationHistogram { get; }

    /// <summary>
    /// Histogram for request duration.
    /// </summary>
    public Histogram<float> RequestDurationHistogram { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TelemetryHistograms" /> class.
    /// </summary>
    /// <param name="meter">The meter used to create histograms.</param>
    public TelemetryHistograms(Meter meter) {
        RequestDurationHistogram =
            meter.CreateHistogram<float>(TelemetryMeter.RequestDuration, "The duration of requests", "milliseconds");
        QueryDurationHistogram = meter.CreateHistogram<float>(TelemetryMeter.QueryDuration,
            "The duration of queries on the FGA server", "milliseconds");
    }

    /// <summary>
    ///     Records the server query duration if the header is present.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="attributes">The attributes associated with the telemetry data.</param>
    public void RecordQueryDuration(HttpResponseMessage response, TagList attributes) {
        if (response.Headers.Contains("fga-query-duration-ms") &&
            response.Headers.GetValues("fga-query-duration-ms").Any()) {
            var durationHeader = response.Headers.GetValues("fga-query-duration-ms").First();
            if (!string.IsNullOrEmpty(durationHeader) && float.TryParse(durationHeader, out var durationFloat)) {
                QueryDurationHistogram?.Record(durationFloat, attributes);
            }
        }
    }

    /// <summary>
    ///     Records the total request duration (includes all requests needed till success such as credential exchange and
    ///     retries).
    /// </summary>
    /// <param name="requestDuration">The stopwatch measuring the request duration.</param>
    /// <param name="attributes">The attributes associated with the telemetry data.</param>
    public void RecordRequestDuration(Stopwatch requestDuration, TagList attributes) =>
        RequestDurationHistogram.Record(requestDuration.ElapsedMilliseconds, attributes);
}