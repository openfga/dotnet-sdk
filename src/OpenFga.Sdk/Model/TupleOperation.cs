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
    /// Defines TupleOperation
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TupleOperation {
        /// <summary>
        /// Enum Write for value: write
        /// </summary>
        [EnumMember(Value = "write")]
        Write = 1,

        /// <summary>
        /// Enum Delete for value: delete
        /// </summary>
        [EnumMember(Value = "delete")]
        Delete = 2

    }

}