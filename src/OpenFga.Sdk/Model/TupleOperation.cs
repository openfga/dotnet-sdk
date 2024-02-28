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

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Defines TupleOperation
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<TupleOperation>))]
    public enum TupleOperation {
        /// <summary>
        /// Enum WRITE for value: TUPLE_OPERATION_WRITE
        /// </summary>
        [EnumMember(Value = "TUPLE_OPERATION_WRITE")]
        WRITE = 1,

        /// <summary>
        /// Enum DELETE for value: TUPLE_OPERATION_DELETE
        /// </summary>
        [EnumMember(Value = "TUPLE_OPERATION_DELETE")]
        DELETE = 2

    }

}