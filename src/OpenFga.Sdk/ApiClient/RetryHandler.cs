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


using OpenFga.Sdk.Client.Model;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.ApiClient;

/// <summary>
/// Handles retry logic including Retry-After header parsing and exponential backoff with jitter.
/// Implements standards-compliant retry behavior per RFC 7231.
/// </summary>
public sealed class RetryHandler {
    /// <summary>
    /// Maximum number of retry attempts allowed (hard limit).
    /// </summary>
    private const int MAX_RETRIES_HARD_LIMIT = 15;

    /// <summary>
    /// Maximum retry delay in milliseconds (cap for exponential backoff).
    /// Default: 120000ms (120 seconds / 2 minutes)
    /// </summary>
    private const int MAX_RETRY_DELAY_MS = 120000;

    /// <summary>
    /// Minimum valid Retry-After value in seconds.
    /// Values less than this will be considered invalid and fall back to exponential backoff.
    /// </summary>
    private const int MIN_VALID_RETRY_AFTER_SECONDS = 1;

    /// <summary>
    /// Maximum valid Retry-After value in seconds.
    /// Values greater than this will be considered invalid and fall back to exponential backoff.
    /// Default: 1800 seconds (30 minutes)
    /// </summary>
    private const int MAX_VALID_RETRY_AFTER_SECONDS = 1800;

    private readonly IRetryParams _retryParams;
    private readonly int _maxRetries;
    private static readonly ThreadLocal<Random> _random = new(() => new Random());

    /// <summary>
    /// Initializes a new instance of the RetryHandler class.
    /// </summary>
    /// <param name="retryParams">Retry parameters</param>
    public RetryHandler(IRetryParams retryParams) {
        _retryParams = retryParams ?? throw new ArgumentNullException(nameof(retryParams));

        // Enforce hard limit of 15 max retries
        if (_retryParams.MaxRetry < 0) {
            throw new ArgumentOutOfRangeException(nameof(retryParams.MaxRetry), "MaxRetry must be non-negative");
        }

        if (_retryParams.MinWaitInMs <= 0) {
            throw new ArgumentOutOfRangeException(nameof(retryParams.MinWaitInMs), "MinWaitInMs must be greater than 0");
        }

        // Cap at 15 as per spec
        _maxRetries = Math.Min(_retryParams.MaxRetry, MAX_RETRIES_HARD_LIMIT);
    }

    /// <summary>
    /// Determines whether a request should be retried based on the HTTP status code and attempt count.
    /// </summary>
    /// <param name="statusCode">HTTP status code from the response</param>
    /// <param name="attemptCount">Current retry attempt count (0 = initial request, 1+ = retries)</param>
    /// <returns>True if the request should be retried, false otherwise</returns>
    public bool ShouldRetry(HttpStatusCode statusCode, int attemptCount) {
        // Check if we've exhausted our retry attempts
        if (attemptCount >= _maxRetries) {
            return false;
        }

        // Retry on 429 Too Many Requests
        if ((int)statusCode == 429) {
            return true;
        }

        // Retry on 5xx server errors except 501 Not Implemented
        if ((int)statusCode >= 500 && (int)statusCode < 600) {
            return statusCode != HttpStatusCode.NotImplemented;
        }

        return false;
    }

    /// <summary>
    /// Determines whether an exception represents a transient error that should be retried.
    /// </summary>
    /// <param name="exception">The exception to check</param>
    /// <param name="attemptCount">Current retry attempt count</param>
    /// <returns>True if the exception is transient and should be retried</returns>
    public bool IsTransientError(Exception exception, int attemptCount) {
        // Check if we've exhausted our retry attempts
        if (attemptCount >= _maxRetries) {
            return false;
        }

        // Retry on network-related exceptions
        return exception switch {
            HttpRequestException => true,
            SocketException => true,
            TaskCanceledException tcEx when !tcEx.CancellationToken.IsCancellationRequested => true, // Timeout, not user cancellation
            TimeoutException => true,
            _ => false
        };
    }

    /// <summary>
    /// Calculates the delay before the next retry attempt.
    /// Prioritizes Retry-After header if present and valid, otherwise uses exponential backoff with jitter.
    /// </summary>
    /// <param name="response">HTTP response message (may contain Retry-After header)</param>
    /// <param name="attemptCount">Current retry attempt count (0-based: 0 = first retry)</param>
    /// <returns>TimeSpan to wait before retrying</returns>
    public TimeSpan CalculateDelay(HttpResponseMessage? response, int attemptCount) {
        return CalculateDelayFromHeaders(response?.Headers, attemptCount);
    }

    /// <summary>
    /// Calculates the delay before the next retry attempt.
    /// Prioritizes Retry-After header if present and valid, otherwise uses exponential backoff with jitter.
    /// </summary>
    /// <param name="headers">HTTP response headers (may contain Retry-After header)</param>
    /// <param name="attemptCount">Current retry attempt count (0-based: 0 = first retry)</param>
    /// <returns>TimeSpan to wait before retrying</returns>
    public TimeSpan CalculateDelayFromHeaders(HttpHeaders? headers, int attemptCount) {
        // Priority 1: Try to parse Retry-After header
        if (headers != null) {
            var retryAfterSeconds = ParseRetryAfterFromHeaders(headers);
            if (retryAfterSeconds.HasValue) {
                return TimeSpan.FromSeconds(retryAfterSeconds.Value);
            }
        }

        // Priority 2: Use exponential backoff with jitter
        return CalculateExponentialBackoffWithJitter(attemptCount);
    }

    /// <summary>
    /// Parses the Retry-After header from an HTTP response.
    /// Supports both integer (delay-seconds) and HTTP-date formats per RFC 7231.
    /// </summary>
    /// <param name="response">HTTP response message</param>
    /// <returns>Number of seconds to wait, or null if header is missing or invalid</returns>
    public int? ParseRetryAfter(HttpResponseMessage response) {
        return ParseRetryAfterFromHeaders(response?.Headers);
    }

    /// <summary>
    /// Parses the Retry-After header from HTTP headers.
    /// Supports both integer (delay-seconds) and HTTP-date formats per RFC 7231.
    /// </summary>
    /// <param name="headers">HTTP response headers</param>
    /// <returns>Number of seconds to wait, or null if header is missing or invalid</returns>
    public int? ParseRetryAfterFromHeaders(HttpHeaders? headers) {
        if (headers == null) {
            return null;
        }

        // Try to get Retry-After header
        if (!headers.TryGetValues("Retry-After", out var values)) {
            return null;
        }

        var retryAfterValue = values.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(retryAfterValue)) {
            return null;
        }

        // Try parsing as integer (delay-seconds format)
        if (int.TryParse(retryAfterValue.Trim(), out var delaySeconds)) {
            return ValidateRetryAfterSeconds(delaySeconds);
        }

        // Try parsing as HTTP-date format (e.g., "Wed, 21 Oct 2025 07:28:00 GMT")
        if (DateTimeOffset.TryParse(retryAfterValue.Trim(), CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal, out var retryAfterDate)) {
            var delay = retryAfterDate - DateTimeOffset.UtcNow;
            var delayInSeconds = (int)Math.Ceiling(delay.TotalSeconds);
            return ValidateRetryAfterSeconds(delayInSeconds);
        }

        // Invalid format - return null to fall back to exponential backoff
        return null;
    }

    /// <summary>
    /// Validates that a Retry-After value is within acceptable bounds (1-1800 seconds).
    /// </summary>
    /// <param name="seconds">Retry-After value in seconds</param>
    /// <returns>Validated seconds value, or null if out of range</returns>
    private int? ValidateRetryAfterSeconds(int seconds) {
        if (seconds < MIN_VALID_RETRY_AFTER_SECONDS || seconds > MAX_VALID_RETRY_AFTER_SECONDS) {
            // Out of acceptable range - return null to fall back to exponential backoff
            return null;
        }

        return seconds;
    }

    /// <summary>
    /// Calculates exponential backoff delay with jitter to prevent thundering herd.
    ///
    /// Formula: Random delay between [2^attempt * MinWaitInMs, 2^(attempt+1) * MinWaitInMs]
    /// Maximum delay is capped at MAX_RETRY_DELAY_MS (120 seconds).
    ///
    /// Example delays (assuming MinWaitInMs = 500):
    /// - Attempt 0: 500ms - 1s
    /// - Attempt 1: 1s - 2s
    /// - Attempt 2: 2s - 4s
    /// - Attempt 3: 4s - 8s
    /// - Attempt 4: 8s - 16s
    /// - Attempt 5: 16s - 32s
    /// - Attempt 6: 32s - 64s
    /// - Attempt 7: 64s - 120s
    /// - Attempt 8+: 120s (capped)
    /// </summary>
    /// <param name="attemptCount">Retry attempt count (0-based)</param>
    /// <returns>Calculated delay with jitter</returns>
    private TimeSpan CalculateExponentialBackoffWithJitter(int attemptCount) {
        // Calculate base delay: 2^attempt * MinWaitInMs
        var baseDelayMs = Math.Pow(2, attemptCount) * _retryParams.MinWaitInMs;

        // Calculate upper bound: 2^(attempt+1) * MinWaitInMs
        var upperBoundMs = Math.Pow(2, attemptCount + 1) * _retryParams.MinWaitInMs;

        // Apply jitter: random value between base and upper bound
        var jitterDelayMs = baseDelayMs + (_random.Value!.NextDouble() * (upperBoundMs - baseDelayMs));

        // Cap at maximum delay
        var cappedDelayMs = Math.Min(jitterDelayMs, MAX_RETRY_DELAY_MS);

        return TimeSpan.FromMilliseconds(cappedDelayMs);
    }

    /// <summary>
    /// Extracts Retry-After information from response headers for error reporting.
    /// Returns both the parsed value (if valid) and the raw header value.
    /// </summary>
    /// <param name="response">HTTP response message</param>
    /// <returns>Tuple of (parsed seconds, raw header value)</returns>
    public (int? retryAfterSeconds, string? retryAfterRaw) ExtractRetryAfterInfo(HttpResponseMessage? response) {
        return ExtractRetryAfterInfoFromHeaders(response?.Headers);
    }

    /// <summary>
    /// Extracts Retry-After information from HTTP headers for error reporting.
    /// Returns both the parsed value (if valid) and the raw header value.
    /// </summary>
    /// <param name="headers">HTTP response headers</param>
    /// <returns>Tuple of (parsed seconds, raw header value)</returns>
    public (int? retryAfterSeconds, string? retryAfterRaw) ExtractRetryAfterInfoFromHeaders(HttpHeaders? headers) {
        if (headers == null) {
            return (null, null);
        }

        if (!headers.TryGetValues("Retry-After", out var values)) {
            return (null, null);
        }

        var rawValue = values.FirstOrDefault();
        var parsedValue = ParseRetryAfterFromHeaders(headers);

        return (parsedValue, rawValue);
    }
}
