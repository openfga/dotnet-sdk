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


using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


using OpenFga.Sdk.Constants;

namespace OpenFga.Sdk.Model
{
    /// <summary>
    /// Defines TypeName
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<TypeName>))]
    public enum TypeName
    {
        /// <summary>
        /// Enum TYPENAMEUNSPECIFIED for value: TYPE_NAME_UNSPECIFIED
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_UNSPECIFIED")]
        TYPENAMEUNSPECIFIED = 1,

        /// <summary>
        /// Enum TYPENAMEANY for value: TYPE_NAME_ANY
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_ANY")]
        TYPENAMEANY = 2,

        /// <summary>
        /// Enum TYPENAMEBOOL for value: TYPE_NAME_BOOL
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_BOOL")]
        TYPENAMEBOOL = 3,

        /// <summary>
        /// Enum TYPENAMESTRING for value: TYPE_NAME_STRING
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_STRING")]
        TYPENAMESTRING = 4,

        /// <summary>
        /// Enum TYPENAMEINT for value: TYPE_NAME_INT
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_INT")]
        TYPENAMEINT = 5,

        /// <summary>
        /// Enum TYPENAMEUINT for value: TYPE_NAME_UINT
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_UINT")]
        TYPENAMEUINT = 6,

        /// <summary>
        /// Enum TYPENAMEDOUBLE for value: TYPE_NAME_DOUBLE
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_DOUBLE")]
        TYPENAMEDOUBLE = 7,

        /// <summary>
        /// Enum TYPENAMEDURATION for value: TYPE_NAME_DURATION
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_DURATION")]
        TYPENAMEDURATION = 8,

        /// <summary>
        /// Enum TYPENAMETIMESTAMP for value: TYPE_NAME_TIMESTAMP
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_TIMESTAMP")]
        TYPENAMETIMESTAMP = 9,

        /// <summary>
        /// Enum TYPENAMEMAP for value: TYPE_NAME_MAP
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_MAP")]
        TYPENAMEMAP = 10,

        /// <summary>
        /// Enum TYPENAMELIST for value: TYPE_NAME_LIST
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_LIST")]
        TYPENAMELIST = 11,

        /// <summary>
        /// Enum TYPENAMEIPADDRESS for value: TYPE_NAME_IPADDRESS
        /// </summary>
        [EnumMember(Value = "TYPE_NAME_IPADDRESS")]
        TYPENAMEIPADDRESS = 12

    }

}
