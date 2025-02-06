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
    /// Defines AuthErrorCode
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<AuthErrorCode>))]
    public enum AuthErrorCode {
        /// <summary>
        /// Enum NoAuthError for value: no_auth_error
        /// </summary>
        [EnumMember(Value = "no_auth_error")]
        NoAuthError = 1,

        /// <summary>
        /// Enum AuthFailedInvalidSubject for value: auth_failed_invalid_subject
        /// </summary>
        [EnumMember(Value = "auth_failed_invalid_subject")]
        AuthFailedInvalidSubject = 2,

        /// <summary>
        /// Enum AuthFailedInvalidAudience for value: auth_failed_invalid_audience
        /// </summary>
        [EnumMember(Value = "auth_failed_invalid_audience")]
        AuthFailedInvalidAudience = 3,

        /// <summary>
        /// Enum AuthFailedInvalidIssuer for value: auth_failed_invalid_issuer
        /// </summary>
        [EnumMember(Value = "auth_failed_invalid_issuer")]
        AuthFailedInvalidIssuer = 4,

        /// <summary>
        /// Enum InvalidClaims for value: invalid_claims
        /// </summary>
        [EnumMember(Value = "invalid_claims")]
        InvalidClaims = 5,

        /// <summary>
        /// Enum AuthFailedInvalidBearerToken for value: auth_failed_invalid_bearer_token
        /// </summary>
        [EnumMember(Value = "auth_failed_invalid_bearer_token")]
        AuthFailedInvalidBearerToken = 6,

        /// <summary>
        /// Enum BearerTokenMissing for value: bearer_token_missing
        /// </summary>
        [EnumMember(Value = "bearer_token_missing")]
        BearerTokenMissing = 7,

        /// <summary>
        /// Enum Unauthenticated for value: unauthenticated
        /// </summary>
        [EnumMember(Value = "unauthenticated")]
        Unauthenticated = 8,

        /// <summary>
        /// Enum Forbidden for value: forbidden
        /// </summary>
        [EnumMember(Value = "forbidden")]
        Forbidden = 9

    }

}