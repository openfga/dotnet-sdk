using OpenFga.Sdk.Exceptions.Parsers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// FGA API Rate Limit Exceeded Error - Corresponding to HTTP Error Code 429
/// </summary>
public class FgaApiRateLimitExceededError : FgaApiError {
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
    public RateLimitParser.PeriodUnit LimitUnit { get; internal set; }

    /// <summary>
    /// Number of milliseconds after which rate limit is reset
    /// </summary>
    public long? ResetInMs { get; internal set; }

    /// <summary>
    /// The date and time offset at which the current rate limit window is reset.
    /// </summary>
    public DateTimeOffset? Reset { get; internal set; }

    /// <summary>
    /// Error Message
    /// </summary>
    public new string? Message { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaApiRateLimitExceededError"/> class with a specified
    /// </summary>
    /// <param name="response"></param>
    /// <param name="request"></param>
    /// <param name="apiName"></param>
    /// <param name="apiError"></param>
    public FgaApiRateLimitExceededError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError, true) {
        var rateLimit = RateLimitParser.Parse(response.Headers,
            request.Method, apiName);
        Limit = rateLimit.Limit;
        Remaining = rateLimit.Remaining;
        LimitUnit = rateLimit.LimitUnit;
        ResetInMs = rateLimit.ResetInMs;
        Reset = rateLimit.Reset;
        Message =
            $"Rate Limit Error for {rateLimit.Method} {rateLimit.ApiName} with API limit of {rateLimit.Limit} requests per {rateLimit.LimitUnit}.";

        // Populate X-Rate-Limit-Reset for logging (legacy header, deprecated for retry logic)
        XRateLimitReset = rateLimit.ResetInMs;
    }

    internal new static async Task<FgaApiError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new FgaApiRateLimitExceededError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}