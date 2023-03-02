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
    /// Defines InternalErrorCode
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<InternalErrorCode>))]
    public enum InternalErrorCode {
        /// <summary>
        /// Enum NoInternalError for value: no_internal_error
        /// </summary>
        [EnumMember(Value = "no_internal_error")]
        NoInternalError = 1,

        /// <summary>
        /// Enum InternalError for value: internal_error
        /// </summary>
        [EnumMember(Value = "internal_error")]
        InternalError = 2,

        /// <summary>
        /// Enum Cancelled for value: cancelled
        /// </summary>
        [EnumMember(Value = "cancelled")]
        Cancelled = 3,

        /// <summary>
        /// Enum DeadlineExceeded for value: deadline_exceeded
        /// </summary>
        [EnumMember(Value = "deadline_exceeded")]
        DeadlineExceeded = 4,

        /// <summary>
        /// Enum AlreadyExists for value: already_exists
        /// </summary>
        [EnumMember(Value = "already_exists")]
        AlreadyExists = 5,

        /// <summary>
        /// Enum ResourceExhausted for value: resource_exhausted
        /// </summary>
        [EnumMember(Value = "resource_exhausted")]
        ResourceExhausted = 6,

        /// <summary>
        /// Enum FailedPrecondition for value: failed_precondition
        /// </summary>
        [EnumMember(Value = "failed_precondition")]
        FailedPrecondition = 7,

        /// <summary>
        /// Enum Aborted for value: aborted
        /// </summary>
        [EnumMember(Value = "aborted")]
        Aborted = 8,

        /// <summary>
        /// Enum OutOfRange for value: out_of_range
        /// </summary>
        [EnumMember(Value = "out_of_range")]
        OutOfRange = 9,

        /// <summary>
        /// Enum Unavailable for value: unavailable
        /// </summary>
        [EnumMember(Value = "unavailable")]
        Unavailable = 10,

        /// <summary>
        /// Enum DataLoss for value: data_loss
        /// </summary>
        [EnumMember(Value = "data_loss")]
        DataLoss = 11

    }

}