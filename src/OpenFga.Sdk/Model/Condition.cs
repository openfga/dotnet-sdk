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


using OpenFga.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Condition
    /// </summary>
    [DataContract(Name = "Condition")]
    public partial class Condition : IEquatable<Condition>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Condition" /> class.
        /// </summary>
        [JsonConstructor]
        public Condition() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition" /> class.
        /// </summary>
        /// <param name="name">name (required).</param>
        /// <param name="expression">A Google CEL expression, expressed as a string. (required).</param>
        /// <param name="parameters">A map of parameter names to the parameter&#39;s defined type reference..</param>
        /// <param name="metadata">metadata.</param>
        public Condition(string name = default, string expression = default, Dictionary<string, ConditionParamTypeRef> parameters = default, ConditionMetadata metadata = default) {
            // to ensure "name" is required (not null)
            if (name == null) {
                throw new ArgumentNullException("name is a required property for Condition and cannot be null");
            }
            this.Name = name;
            // to ensure "expression" is required (not null)
            if (expression == null) {
                throw new ArgumentNullException("expression is a required property for Condition and cannot be null");
            }
            this.Expression = expression;
            this.Parameters = parameters;
            this.Metadata = metadata;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// A Google CEL expression, expressed as a string.
        /// </summary>
        /// <value>A Google CEL expression, expressed as a string.</value>
        [DataMember(Name = "expression", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("expression")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Expression { get; set; }

        /// <summary>
        /// A map of parameter names to the parameter&#39;s defined type reference.
        /// </summary>
        /// <value>A map of parameter names to the parameter&#39;s defined type reference.</value>
        [DataMember(Name = "parameters", EmitDefaultValue = false)]
        [JsonPropertyName("parameters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, ConditionParamTypeRef>? Parameters { get; set; }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name = "metadata", EmitDefaultValue = false)]
        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ConditionMetadata? Metadata { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson() {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a Condition from the JSON string presentation of the object
        /// </summary>
        /// <returns>Condition</returns>
        public static Condition FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Condition>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((Condition)input);
        }

        /// <summary>
        /// Returns true if Condition instances are equal
        /// </summary>
        /// <param name="input">Instance of Condition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Condition input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Expression == input.Expression ||
                    (this.Expression != null &&
                    this.Expression.Equals(input.Expression))
                ) &&
                (
                    this.Parameters == input.Parameters ||
                    this.Parameters != null &&
                    input.Parameters != null &&
                    this.Parameters.SequenceEqual(input.Parameters)
                ) &&
                (
                    this.Metadata == input.Metadata ||
                    (this.Metadata != null &&
                    this.Metadata.Equals(input.Metadata))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Name != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Name.GetHashCode();
                }
                if (this.Expression != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Expression.GetHashCode();
                }
                if (this.Parameters != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Parameters.GetHashCode();
                }
                if (this.Metadata != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Metadata.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            yield break;
        }

    }

}