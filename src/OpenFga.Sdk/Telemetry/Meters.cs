using System.Collections.Generic;

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
    ///     The total number of times a new token was requested when using ClientCredentials.
    /// </summary>
    public static readonly string RequestCount = "fga-client.request.count";

    /// <summary>
    /// Return all supported meter names
    /// </summary>
    public static HashSet<string> GetAllMeters() {
        return new() {
            RequestDuration,
            QueryDuration,
            TokenExchangeCount,
            RequestCount
        };
    }
}