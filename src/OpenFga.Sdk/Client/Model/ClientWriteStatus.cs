//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientWriteStatus
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter<ClientWriteStatus>))]
public enum ClientWriteStatus {
    /// <summary>
    ///     Enum SUCCESS for value: CLIENT_WRITE_STATUS_SUCCESS
    /// </summary>
    [EnumMember(Value = "CLIENT_WRITE_STATUS_SUCCESS")]
    SUCCESS = 1,

    /// <summary>
    ///     Enum FAILURE for value: CLIENT_WRITE_STATUS_FAILURE
    /// </summary>
    [EnumMember(Value = "CLIENT_WRITE_STATUS_FAILURE")]
    FAILURE = 2
}