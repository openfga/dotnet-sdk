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
    /// Controls the consistency preferences when calling the query APIs.   - UNSPECIFIED: Default if not set. Behavior will be the same as MINIMIZE_LATENCY.  - MINIMIZE_LATENCY: Minimize latency at the potential expense of lower consistency.  - HIGHER_CONSISTENCY: Prefer higher consistency, at the potential expense of increased latency.
    /// </summary>
    /// <value>Controls the consistency preferences when calling the query APIs.   - UNSPECIFIED: Default if not set. Behavior will be the same as MINIMIZE_LATENCY.  - MINIMIZE_LATENCY: Minimize latency at the potential expense of lower consistency.  - HIGHER_CONSISTENCY: Prefer higher consistency, at the potential expense of increased latency.</value>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<ConsistencyPreference>))]
    public enum ConsistencyPreference {
        /// <summary>
        /// Enum UNSPECIFIED for value: UNSPECIFIED
        /// </summary>
        [EnumMember(Value = "UNSPECIFIED")]
        UNSPECIFIED = 1,

        /// <summary>
        /// Enum MINIMIZELATENCY for value: MINIMIZE_LATENCY
        /// </summary>
        [EnumMember(Value = "MINIMIZE_LATENCY")]
        MINIMIZELATENCY = 2,

        /// <summary>
        /// Enum HIGHERCONSISTENCY for value: HIGHER_CONSISTENCY
        /// </summary>
        [EnumMember(Value = "HIGHER_CONSISTENCY")]
        HIGHERCONSISTENCY = 3

    }

}