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


using System.Diagnostics.Metrics;

namespace OpenFga.Sdk.Telemetry;

public class TelemetryCounters {
    // Meters
    // The total number of times a new token was requested when using ClientCredentials
    private const string TokenExchangeCountKey = "fga-client.credentials.request";

    protected Meter meter;
    public Counter<int> tokenExchangeCounter;

    public TelemetryCounters(Meter meter) {
        this.meter = meter;
        tokenExchangeCounter = this.meter.CreateCounter<int>(TokenExchangeCountKey,
            description: "The count of token exchange requests");
    }

    public void buildForResponse(KeyValuePair<string, object>[] attributes) => tokenExchangeCounter.Add(1, attributes!);
}