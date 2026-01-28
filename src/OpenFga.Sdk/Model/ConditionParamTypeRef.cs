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
    /// ConditionParamTypeRef
    /// </summary>
    [DataContract(Name = "ConditionParamTypeRef")]
    public partial class ConditionParamTypeRef : IEquatable<ConditionParamTypeRef>, IValidatableObject
    {

        /// <summary>
        /// Gets or Sets TypeName
        /// </summary>
        [DataMember(Name = "type_name", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type_name")]
        public TypeName TypeName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionParamTypeRef" /> class.
        /// </summary>
        [JsonConstructor]
        public ConditionParamTypeRef()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionParamTypeRef" /> class.
        /// </summary>
        /// <param name="typeName">typeName (required).</param>
        /// <param name="genericTypes">genericTypes.</param>
        public ConditionParamTypeRef(TypeName typeName = default, List<ConditionParamTypeRef> genericTypes = default)
        {
            this.TypeName = typeName;
            this.GenericTypes = genericTypes;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets GenericTypes
        /// </summary>
        [DataMember(Name = "generic_types", EmitDefaultValue = false)]
        [JsonPropertyName("generic_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<ConditionParamTypeRef>? GenericTypes { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a ConditionParamTypeRef from the JSON string presentation of the object
        /// </summary>
        /// <returns>ConditionParamTypeRef</returns>
        public static ConditionParamTypeRef FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ConditionParamTypeRef>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((ConditionParamTypeRef)input);
        }

        /// <summary>
        /// Returns true if ConditionParamTypeRef instances are equal
        /// </summary>
        /// <param name="input">Instance of ConditionParamTypeRef to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ConditionParamTypeRef input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.TypeName == input.TypeName ||
                    this.TypeName.Equals(input.TypeName)
                ) && 
                (
                    this.GenericTypes == input.GenericTypes ||
                    this.GenericTypes != null &&
                    input.GenericTypes != null &&
                    this.GenericTypes.SequenceEqual(input.GenericTypes)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.TypeName.GetHashCode();
                if (this.GenericTypes != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.GenericTypes.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

    }

}
