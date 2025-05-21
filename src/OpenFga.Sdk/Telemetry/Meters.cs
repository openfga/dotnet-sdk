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

namespace OpenFga.Sdk.Telemetry;

/// <summary>
///     Meter names for telemetry.
///     For why `static readonly` over `const`, see https://github.com/dotnet/aspnetcore/pull/12441/files
/// </summary>
public static class TelemetryMeter {
    // Histograms //

    /// <summary>
    ///     The total request time for FGA requests
    /// </summary>
    public static readonly string RequestDuration = "fga-client.request.duration";

    /// <summary>
    ///     The amount of time the FGA server took to internally process and evaluate the request
    /// </summary>
    public static readonly string QueryDuration = "fga-client.query.duration";

    // Counters //

    /// <summary>
    ///     The total number of times a new token was requested when using ClientCredentials.
    /// </summary>
    public static readonly string TokenExchangeCount = "fga-client.credentials.request";

    /// <summary>
    ///     The total number of requests made to the FGA server.
    /// </summary>
    public static readonly string RequestCount = "fga-client.request.count";

    /// <summary>
    /// Return all supported meter names
    /// </summary>
    public static HashSet<string> GetAllMeters() {
        var meters = new HashSet<string>();
        meters.Add(RequestDuration);
        meters.Add(QueryDuration);
        meters.Add(TokenExchangeCount);
        meters.Add(RequestCount);
        return meters;
    }
}