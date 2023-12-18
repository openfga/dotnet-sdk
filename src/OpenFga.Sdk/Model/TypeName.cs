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
    /// Defines TypeName
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<TypeName>))]
    public enum TypeName {
        /// <summary>
        /// Enum UNSPECIFIED for value: TYPE_NAME_UNSPECIFIED
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_UNSPECIFIED")]
        UNSPECIFIED = 1,

        /// <summary>
        /// Enum ANY for value: TYPE_NAME_ANY
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_ANY")]
        ANY = 2,

        /// <summary>
        /// Enum BOOL for value: TYPE_NAME_BOOL
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_BOOL")]
        BOOL = 3,

        /// <summary>
        /// Enum STRING for value: TYPE_NAME_STRING
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_STRING")]
        STRING = 4,

        /// <summary>
        /// Enum INT for value: TYPE_NAME_INT
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_INT")]
        INT = 5,

        /// <summary>
        /// Enum UINT for value: TYPE_NAME_UINT
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_UINT")]
        UINT = 6,

        /// <summary>
        /// Enum DOUBLE for value: TYPE_NAME_DOUBLE
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_DOUBLE")]
        DOUBLE = 7,

        /// <summary>
        /// Enum DURATION for value: TYPE_NAME_DURATION
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_DURATION")]
        DURATION = 8,

        /// <summary>
        /// Enum TIMESTAMP for value: TYPE_NAME_TIMESTAMP
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_TIMESTAMP")]
        TIMESTAMP = 9,

        /// <summary>
        /// Enum MAP for value: TYPE_NAME_MAP
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_MAP")]
        MAP = 10,

        /// <summary>
        /// Enum LIST for value: TYPE_NAME_LIST
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_LIST")]
        LIST = 11,

        /// <summary>
        /// Enum IPADDRESS for value: TYPE_NAME_IPADDRESS
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_IPADDRESS")]
        IPADDRESS = 12

    }

}