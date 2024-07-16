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
using System.Diagnostics.Metrics;

namespace OpenFga.Sdk.Telemetry;

public class Metrics {
    public const string name = "OpenFga.Sdk";
    public TelemetryCounters counters;
    public TelemetryHistograms histograms;
    public Meter meter = new(name, Configuration.Configuration.Version);

    public Metrics() {
        histograms = new TelemetryHistograms(meter);
        counters = new TelemetryCounters(meter);
    }

    public void buildForResponse(string apiName, HttpResponseMessage response, RequestBuilder requestBuilder,
        Credentials? credentials, Stopwatch requestDuration, int retryCount) => histograms.buildForResponse(response,
        Attributes.buildAttributesForResponse(apiName, response, requestBuilder, credentials, requestDuration,
            retryCount),
        requestDuration);

    public void buildForClientCredentialsResponse(HttpResponseMessage response, RequestBuilder requestBuilder,
        Credentials? credentials, Stopwatch requestDuration, int retryCount) => counters.buildForResponse(
        Attributes.buildAttributesForResponse("ClientCredentialsExchange", response, requestBuilder, credentials,
            requestDuration, retryCount));
}