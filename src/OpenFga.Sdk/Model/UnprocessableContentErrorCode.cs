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


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Defines UnprocessableContentErrorCode
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<UnprocessableContentErrorCode>))]
    public enum UnprocessableContentErrorCode {
        /// <summary>
        /// Enum NoThrottledErrorCode for value: no_throttled_error_code
        /// </summary>
        [EnumMember(Value = "no_throttled_error_code")]
        NoThrottledErrorCode = 1,

        /// <summary>
        /// Enum ThrottledTimeoutError for value: throttled_timeout_error
        /// </summary>
        [EnumMember(Value = "throttled_timeout_error")]
        ThrottledTimeoutError = 2

    }

}