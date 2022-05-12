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
    /// - no_internal_error: no error  - internal_error: generic internal error.  - auth_internal_error: internal error due to authentication error.  - auth_failed_error_fetching_well_known_jwks: authentication failure due to internal error in fetching well-known/jwks.json.  - cancelled: internal error - request being cancelled.  - deadline_exceeded: internal error - deadline exceeded.  - already_exists: internal error - already exists.  - resource_exhausted: internal error - resource exhausted.  - failed_precondition: internal error - failed precondition.  - aborted: internal error - aborted.  - out_of_range: internal error - out of range.  - unavailable: internal error - unavailable.  - data_loss: internal error - data loss.
    /// </summary>
    /// <value>- no_internal_error: no error  - internal_error: generic internal error.  - auth_internal_error: internal error due to authentication error.  - auth_failed_error_fetching_well_known_jwks: authentication failure due to internal error in fetching well-known/jwks.json.  - cancelled: internal error - request being cancelled.  - deadline_exceeded: internal error - deadline exceeded.  - already_exists: internal error - already exists.  - resource_exhausted: internal error - resource exhausted.  - failed_precondition: internal error - failed precondition.  - aborted: internal error - aborted.  - out_of_range: internal error - out of range.  - unavailable: internal error - unavailable.  - data_loss: internal error - data loss.</value>
    [JsonConverter(typeof(JsonStringEnumConverter))]
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
        /// Enum AuthInternalError for value: auth_internal_error
        /// </summary>
        [EnumMember(Value = "auth_internal_error")]
        AuthInternalError = 3,

        /// <summary>
        /// Enum AuthFailedErrorFetchingWellKnownJwks for value: auth_failed_error_fetching_well_known_jwks
        /// </summary>
        [EnumMember(Value = "auth_failed_error_fetching_well_known_jwks")]
        AuthFailedErrorFetchingWellKnownJwks = 4,

        /// <summary>
        /// Enum Cancelled for value: cancelled
        /// </summary>
        [EnumMember(Value = "cancelled")]
        Cancelled = 5,

        /// <summary>
        /// Enum DeadlineExceeded for value: deadline_exceeded
        /// </summary>
        [EnumMember(Value = "deadline_exceeded")]
        DeadlineExceeded = 6,

        /// <summary>
        /// Enum AlreadyExists for value: already_exists
        /// </summary>
        [EnumMember(Value = "already_exists")]
        AlreadyExists = 7,

        /// <summary>
        /// Enum ResourceExhausted for value: resource_exhausted
        /// </summary>
        [EnumMember(Value = "resource_exhausted")]
        ResourceExhausted = 8,

        /// <summary>
        /// Enum FailedPrecondition for value: failed_precondition
        /// </summary>
        [EnumMember(Value = "failed_precondition")]
        FailedPrecondition = 9,

        /// <summary>
        /// Enum Aborted for value: aborted
        /// </summary>
        [EnumMember(Value = "aborted")]
        Aborted = 10,

        /// <summary>
        /// Enum OutOfRange for value: out_of_range
        /// </summary>
        [EnumMember(Value = "out_of_range")]
        OutOfRange = 11,

        /// <summary>
        /// Enum Unavailable for value: unavailable
        /// </summary>
        [EnumMember(Value = "unavailable")]
        Unavailable = 12,

        /// <summary>
        /// Enum DataLoss for value: data_loss
        /// </summary>
        [EnumMember(Value = "data_loss")]
        DataLoss = 13

    }

}