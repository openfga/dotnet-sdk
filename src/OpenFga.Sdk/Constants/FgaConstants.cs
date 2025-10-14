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

public static class FgaConstants {
    // SdkVersion is the version of this service
    public const string SdkVersion = "0.8.0";

    // UserAgent is the user agent used in HTTP requests
    public const string UserAgent = "openfga-sdk dotnet/0.8.0";

    // SampleBaseDomain is the sample API Domain
    public const string SampleBaseDomain = "fga.example";

    // RetryMaxAllowedNumber is the maximum allowed number of retries for HTTP requests
    public const int RetryMaxAllowedNumber = 15;

    // DefaultMaxRetry is the default maximum number of retries for HTTP requests
    public const int DefaultMaxRetry = 3;

    // DefaultMinWaitInMs is the default minimum wait time between retries in milliseconds
    public const int DefaultMinWaitInMs = 100;

    // MaxBackoffTimeInSec is the maximum backoff time in seconds
    public const int MaxBackoffTimeInSec = 120;

    // RetryHeaderMaxAllowableDurationInSec is the maximum allowable duration for retry headers in seconds
    public const int RetryHeaderMaxAllowableDurationInSec = 1800;

    // ClientMaxMethodParallelRequests is the maximum number of parallel requests for a single method
    public const int ClientMaxMethodParallelRequests = 10;

    // ClientMaxBatchSize is the maximum batch size for batch requests
    public const int ClientMaxBatchSize = 50;

    // DefaultConnectionTimeoutInMs is the default connection timeout in milliseconds
    public const int DefaultConnectionTimeoutInMs = 10000;

    // TokenExpiryThresholdBufferInSec is the buffer time in seconds before token expiry to consider it expired
    public const int TokenExpiryThresholdBufferInSec = 300;

    // TokenExpiryJitterInSec is the jitter time in seconds to add randomness to token expiry checks
    public const int TokenExpiryJitterInSec = 300;

    // ClientMethodHeader is the header used to identify the client method
    public const string ClientMethodHeader = "X-OpenFGA-Client-Method";

    // ClientBulkRequestIdHeader is the header used to identify bulk request IDs
    public const string ClientBulkRequestIdHeader = "X-OpenFGA-Client-Bulk-Request-Id";

    // RetryAfterHeaderName is the standard HTTP header for retry after
    public const string RetryAfterHeaderName = "Retry-After";

    // RateLimitResetHeaderName is a common header for rate limit reset time
    public const string RateLimitResetHeaderName = "X-RateLimit-Reset";

    // RateLimitResetAltHeaderName is an alternative header for rate limit reset time
    public const string RateLimitResetAltHeaderName = "X-Rate-Limit-Reset";

    // HashCodeBasePrimeNumber is a prime number used as a base in hash code calculations
    public const int HashCodeBasePrimeNumber = 9661;

    // HashCodeMultiplierPrimeNumber is a prime number used as a multiplier in hash code calculations
    public const int HashCodeMultiplierPrimeNumber = 9923;
}