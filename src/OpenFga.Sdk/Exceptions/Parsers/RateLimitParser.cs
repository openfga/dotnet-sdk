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


using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace OpenFga.Sdk.Exceptions.Parsers;

/// <summary>
/// Represents information about the rate limit for API calls.
/// </summary>
public class RateLimitParser {
    /// <summary>
    /// Defines the string headers to be parsed
    /// </summary>
    public static class RateLimitHeader {
        public const string LimitResetIn = "x-ratelimit-reset";
        public const string LimitTotalInPeriod = "x-ratelimit-limit";
        public const string LimitRemaining = "x-ratelimit-remaining";
    }

    /// <summary>
    /// The time period window for the rate limit
    /// </summary>
    public enum PeriodUnit {
        /// <summary>
        /// Seconds
        /// </summary>
        [EnumMember(Value = "second")] Second,

        /// <summary>
        /// Minutes
        /// </summary>
        [EnumMember(Value = "minute")] Minute,
    }

    /// <summary>
    /// The maximum number of requests the consumer is allowed to make.
    /// </summary>
    public long Limit { get; internal set; }

    /// <summary>
    /// The number of requests remaining in the current rate limit window.
    /// </summary>
    public long Remaining { get; internal set; }

    /// <summary>
    /// The period unit of the current rate limit window.
    /// </summary>
    public PeriodUnit LimitUnit { get; internal set; }

    /// <summary>
    /// Number of milliseconds after which rate limit is reset
    /// </summary>
    public long? ResetInMs { get; internal set; }

    /// <summary>
    /// The date and time offset at which the current rate limit window is reset.
    /// </summary>
    public DateTimeOffset? Reset { get; internal set; }

    /// <summary>
    /// The request method
    /// </summary>
    public HttpMethod? Method { get; internal set; }

    /// <summary>
    /// The name of the api.
    /// </summary>
    public string? ApiName { get; internal set; }

    /// <summary>
    /// Parse the rate limit headers into a <see cref="RateLimit"/> object.
    /// </summary>
    /// <param name="headers"><see cref="HttpHeaders"/> to parse.</param>
    /// <param name="method"></param>
    /// <param name="apiName"></param>
    /// <returns>Instance of <see cref="RateLimitParser"/> containing parsed rate limit headers.</returns>
    public static RateLimitParser Parse(HttpHeaders headers, HttpMethod method, string? apiName = null) {
        var reset = GetHeaderValue(headers, RateLimitHeader.LimitResetIn);
        var limitUnit = GetRateLimitPeriodUnitValue(apiName);
        return new RateLimitParser {
            Limit = GetHeaderValue(headers, RateLimitHeader.LimitTotalInPeriod),
            Remaining = GetHeaderValue(headers, RateLimitHeader.LimitRemaining),
            LimitUnit = limitUnit,
            ResetInMs = reset,
            Reset = reset == 0 ? null : AddResetToCurrentTime(limitUnit, reset),
            Method = method,
            ApiName = apiName ?? "",
        };
    }

    private static long GetHeaderValue(HttpHeaders headers, string name) {
        if (headers.TryGetValues(name, out var v) && long.TryParse(v?.FirstOrDefault(), out var value))
            return value;

        return 0;
    }

    private static PeriodUnit GetRateLimitPeriodUnitValue(string? apiName) {
        return apiName switch {
            "Check" => PeriodUnit.Second,
            "Read" => PeriodUnit.Second,
            "Write" => PeriodUnit.Second,
            _ => PeriodUnit.Minute,
        };
    }

    private static DateTimeOffset? AddResetToCurrentTime(PeriodUnit limitUnit, long reset) {
        return limitUnit switch {
            PeriodUnit.Second => DateTimeOffset.UtcNow.AddSeconds(reset),
            PeriodUnit.Minute => DateTimeOffset.UtcNow.AddMinutes(reset),
            _ => null
        };
    }
}