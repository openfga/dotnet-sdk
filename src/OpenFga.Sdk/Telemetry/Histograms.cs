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

namespace OpenFga.Sdk.Telemetry;

public class TelemetryHistograms {
    // Meters

    // The total request time for FGA requests
    private const string RequestDurationKey = "fga-client.request.duration";

    // The amount of time the FGA server took to internally process nd evaluate the request
    private const string QueryDurationKey = "fga-client.query.duration";

    protected Meter meter;
    public Histogram<float> queryDuration;
    public Histogram<float> requestDurationHistogram;

    public TelemetryHistograms(Meter meter) {
        this.meter = meter;
        requestDurationHistogram = this.meter.CreateHistogram<float>(RequestDurationKey,
            description: "The duration of requests", unit: "milliseconds");
        queryDuration = this.meter.CreateHistogram<float>(QueryDurationKey,
            description: "The duration of queries on the FGA server", unit: "milliseconds");
    }

    public void buildForResponse(HttpResponseMessage response, TagList attributes,
        Stopwatch requestDuration) {
        if (response.Headers.Contains("fga-query-duration-ms") &&
            response.Headers.GetValues("fga-query-duration-ms").Any()) {
            var durationHeader = response.Headers.GetValues("fga-query-duration-ms").First();
            if (!string.IsNullOrEmpty(durationHeader)) {
                var success = float.TryParse(durationHeader, out var durationFloat);
                if (success) {
                    queryDuration?.Record(durationFloat, attributes);
                }
            }
        }

        requestDurationHistogram.Record(requestDuration.ElapsedMilliseconds, attributes);
    }
}