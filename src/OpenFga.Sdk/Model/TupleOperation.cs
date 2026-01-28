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
    /// Defines TupleOperation
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<TupleOperation>))]
    public enum TupleOperation
    {
        /// <summary>
        /// Enum TUPLEOPERATIONWRITE for value: TUPLE_OPERATION_WRITE
        /// </summary>
        [EnumMember(Value = "TUPLE_OPERATION_WRITE")]
        TUPLEOPERATIONWRITE = 1,

        /// <summary>
        /// Enum TUPLEOPERATIONDELETE for value: TUPLE_OPERATION_DELETE
        /// </summary>
        [EnumMember(Value = "TUPLE_OPERATION_DELETE")]
        TUPLEOPERATIONDELETE = 2

    }

}
