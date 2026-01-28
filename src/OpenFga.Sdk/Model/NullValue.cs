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
    /// &#x60;NullValue&#x60; is a singleton enumeration to represent the null value for the &#x60;Value&#x60; type union.  The JSON representation for &#x60;NullValue&#x60; is JSON &#x60;null&#x60;.   - NULL_VALUE: Null value.
    /// </summary>
    /// <value>&#x60;NullValue&#x60; is a singleton enumeration to represent the null value for the &#x60;Value&#x60; type union.  The JSON representation for &#x60;NullValue&#x60; is JSON &#x60;null&#x60;.   - NULL_VALUE: Null value.</value>
    [JsonConverter(typeof(JsonStringEnumMemberConverter<NullValue>))]
    public enum NullValue
    {
        /// <summary>
        /// Enum NULLVALUE for value: NULL_VALUE
        /// </summary>
        [EnumMember(Value = "NULL_VALUE")]
        NULLVALUE = 1

    }

}
