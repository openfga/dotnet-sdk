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
    /// ExpandRequestTupleKey
    /// </summary>
    [DataContract(Name = "ExpandRequestTupleKey")]
    public partial class ExpandRequestTupleKey : IEquatable<ExpandRequestTupleKey>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandRequestTupleKey" /> class.
        /// </summary>
        [JsonConstructor]
        public ExpandRequestTupleKey() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandRequestTupleKey" /> class.
        /// </summary>
        /// <param name="relation">relation (required).</param>
        /// <param name="varObject">varObject (required).</param>
        public ExpandRequestTupleKey(string relation = default, string varObject = default) {
            // to ensure "relation" is required (not null)
            if (relation == null) {
                throw new ArgumentNullException("relation is a required property for ExpandRequestTupleKey and cannot be null");
            }
            this.Relation = relation;
            // to ensure "varObject" is required (not null)
            if (varObject == null) {
                throw new ArgumentNullException("varObject is a required property for ExpandRequestTupleKey and cannot be null");
            }
            this.Object = varObject;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Relation { get; set; }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Object { get; set; }

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
        /// Builds a ExpandRequestTupleKey from the JSON string presentation of the object
        /// </summary>
        /// <returns>ExpandRequestTupleKey</returns>
        public static ExpandRequestTupleKey FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ExpandRequestTupleKey>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ExpandRequestTupleKey);
        }

        /// <summary>
        /// Returns true if ExpandRequestTupleKey instances are equal
        /// </summary>
        /// <param name="input">Instance of ExpandRequestTupleKey to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ExpandRequestTupleKey input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                ) &&
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.ContainsKey(kv.Key) && Equals(kv.Value, input.AdditionalProperties[kv.Key])));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Relation != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Relation.GetHashCode();
                }
                if (this.Object != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Object.GetHashCode();
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
            // Relation (string) maxLength
            if (this.Relation != null && this.Relation.Length > 50) {
                yield return new ValidationResult("Invalid value for Relation, length must be less than 50.", new[] { "Relation" });
            }

            // Object (string) maxLength
            if (this.Object != null && this.Object.Length > 256) {
                yield return new ValidationResult("Invalid value for Object, length must be less than 256.", new[] { "Object" });
            }

            yield break;
        }

    }

}