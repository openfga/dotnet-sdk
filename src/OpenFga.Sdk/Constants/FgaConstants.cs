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


namespace OpenFga.Sdk.Constants;

/// <summary>
/// Centralized constants for the OpenFGA .NET SDK.
/// </summary>
public static class FgaConstants {
    /// <summary>
    /// Version of the OpenFGA .NET SDK.
    /// </summary>
    public const string SdkVersion = "0.8.0";

    /// <summary>
    /// User agent used in HTTP requests.
    /// </summary>
    public const string UserAgent = "openfga-sdk dotnet/0.8.0";

    /// <summary>
    /// Example API domain for documentation/tests.
    /// </summary>
    public static readonly string SampleBaseDomain = "fga.example";

    /// <summary>
    /// API URL used for tests.
    /// </summary>
    public static readonly string TestApiUrl = "https://api." + SampleBaseDomain;

    /// <summary>
    /// API Token Issuer URL used for tests.
    /// </summary>
    public static readonly string TestIssuerUrl = "https://issuer." + SampleBaseDomain;

    /// <summary>
    /// Default API URL.
    /// </summary>
    public static readonly string DefaultApiUrl = "http://localhost:8080";

    // Retry configuration

    /// <summary>
    /// Maximum allowed number of retries for HTTP requests.
    /// </summary>
    public const int RetryMaxAllowedNumber = 15;

    /// <summary>
    /// Default maximum number of retries for HTTP requests.
    /// </summary>
    public const int DefaultMaxRetry = 3;

    /// <summary>
    /// Default minimum wait time between retries in milliseconds.
    /// </summary>
    public const int DefaultMinWaitInMs = 100;

    /// <summary>
    /// Maximum backoff time in seconds.
    /// </summary>
    public const int MaxBackoffTimeInSec = 120;

    /// <summary>
    /// Maximum allowable duration for retry headers in seconds.
    /// </summary>
    public const int RetryHeaderMaxAllowableDurationInSec = 1800;

    /// <summary>
    /// Standard HTTP header for retry-after.
    /// </summary>
    public static readonly string RetryAfterHeaderName = "Retry-After";

    /// <summary>
    /// Rate limit reset header name.
    /// </summary>
    public static readonly string RateLimitResetHeaderName = "X-RateLimit-Reset";

    /// <summary>
    /// Alternative rate limit reset header name.
    /// </summary>
    public static readonly string RateLimitResetAltHeaderName = "X-Rate-Limit-Reset";

    // Client methods

    /// <summary>
    /// Maximum number of parallel requests for a single method.
    /// </summary>
    public const int ClientMaxMethodParallelRequests = 10;

    /// <summary>
    /// Maximum batch size for batch requests.
    /// </summary>
    public const int ClientMaxBatchSize = 50;

    /// <summary>
    /// Header used to identify the client method.
    /// </summary>
    public static readonly string ClientMethodHeader = "X-OpenFGA-Client-Method";

    /// <summary>
    /// Header used to identify bulk requests.
    /// </summary>
    public static readonly string ClientBulkRequestIdHeader = "X-OpenFGA-Client-Bulk-Request-Id";

    // Connection options

    /// <summary>
    /// Default timeout for HTTP requests in milliseconds.
    /// </summary>
    public const int DefaultRequestTimeoutInMs = 10000;

    /// <summary>
    /// Default connection timeout in milliseconds.
    /// </summary>
    public const int DefaultConnectionTimeoutInMs = 10000;

    // Token management

    /// <summary>
    /// Buffer time in seconds before token expiry to consider it expired.
    /// </summary>
    public const int TokenExpiryThresholdBufferInSec = 300;

    /// <summary>
    /// Jitter time in seconds to add randomness to token expiry checks.
    /// </summary>
    public const int TokenExpiryJitterInSec = 300;

    // FGA Response Headers

    /// <summary>
    /// Response header name for query duration in milliseconds.
    /// </summary>
    public static readonly string QueryDurationHeaderName = "fga-query-duration-ms";

    // .NET Specific constants

    /// <summary>
    /// Prime number used as a base in hash code calculations.
    /// </summary>
    public const int HashCodeBasePrimeNumber = 9661;

    /// <summary>
    /// Prime number used as a multiplier in hash code calculations.
    /// </summary>
    public const int HashCodeMultiplierPrimeNumber = 9923;
}