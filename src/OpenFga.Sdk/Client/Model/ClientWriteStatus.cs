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