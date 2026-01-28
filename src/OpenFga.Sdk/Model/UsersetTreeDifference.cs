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
    /// UsersetTreeDifference
    /// </summary>
    [DataContract(Name = "UsersetTree.Difference")]
    public partial class UsersetTreeDifference : IEquatable<UsersetTreeDifference>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeDifference()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        /// <param name="varBase">varBase (required).</param>
        /// <param name="subtract">subtract (required).</param>
        public UsersetTreeDifference(Node varBase = default, Node subtract = default)
        {
            // to ensure "varBase" is required (not null)
            if (varBase == null)
            {
                throw new ArgumentNullException("varBase is a required property for UsersetTreeDifference and cannot be null");
            }
            this.Base = varBase;
            // to ensure "subtract" is required (not null)
            if (subtract == null)
            {
                throw new ArgumentNullException("subtract is a required property for UsersetTreeDifference and cannot be null");
            }
            this.Subtract = subtract;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Base
        /// </summary>
        [DataMember(Name = "base", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("base")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Node Base { get; set; }

        /// <summary>
        /// Gets or Sets Subtract
        /// </summary>
        [DataMember(Name = "subtract", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("subtract")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Node Subtract { get; set; }

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
        /// Builds a UsersetTreeDifference from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTreeDifference</returns>
        public static UsersetTreeDifference FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTreeDifference>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((UsersetTreeDifference)input);
        }

        /// <summary>
        /// Returns true if UsersetTreeDifference instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeDifference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeDifference input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Base == input.Base ||
                    (this.Base != null &&
                    this.Base.Equals(input.Base))
                ) && 
                (
                    this.Subtract == input.Subtract ||
                    (this.Subtract != null &&
                    this.Subtract.Equals(input.Subtract))
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
                if (this.Base != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Base.GetHashCode();
                }
                if (this.Subtract != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Subtract.GetHashCode();
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
