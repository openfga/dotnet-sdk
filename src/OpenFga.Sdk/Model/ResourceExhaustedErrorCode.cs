//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// - no_resource_exhausted_error: no error  - rate_limit_exceeded: operation failed due to exceeding rate limit.  - auth_rate_limit_exceeded: rate limit error during authentication.
    /// </summary>
    /// <value>- no_resource_exhausted_error: no error  - rate_limit_exceeded: operation failed due to exceeding rate limit.  - auth_rate_limit_exceeded: rate limit error during authentication.</value>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResourceExhaustedErrorCode {
        /// <summary>
        /// Enum NoResourceExhaustedError for value: no_resource_exhausted_error
        /// </summary>
        [EnumMember(Value = "no_resource_exhausted_error")]
        NoResourceExhaustedError = 1,

        /// <summary>
        /// Enum RateLimitExceeded for value: rate_limit_exceeded
        /// </summary>
        [EnumMember(Value = "rate_limit_exceeded")]
        RateLimitExceeded = 2,

        /// <summary>
        /// Enum AuthRateLimitExceeded for value: auth_rate_limit_exceeded
        /// </summary>
        [EnumMember(Value = "auth_rate_limit_exceeded")]
        AuthRateLimitExceeded = 3

    }

}