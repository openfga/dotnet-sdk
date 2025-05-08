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

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// RelationReference represents a relation of a particular object type (e.g. &#39;document#viewer&#39;).
    /// </summary>
    [DataContract(Name = "RelationReference")]
    public partial class RelationReference : IEquatable<RelationReference>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationReference" /> class.
        /// </summary>
        [JsonConstructor]
        public RelationReference() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationReference" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="relation">relation.</param>
        /// <param name="wildcard">wildcard.</param>
        /// <param name="condition">The name of a condition that is enforced over the allowed relation..</param>
        public RelationReference(string type = default(string), string relation = default(string), Object wildcard = default(Object), string condition = default(string)) {
            // to ensure "type" is required (not null)
            if (type == null) {
                throw new ArgumentNullException("type is a required property for RelationReference and cannot be null");
            }
            this.Type = type;
            this.Relation = relation;
            this.Wildcard = wildcard;
            this.Condition = condition;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Relation { get; set; }

        /// <summary>
        /// Gets or Sets Wildcard
        /// </summary>
        [DataMember(Name = "wildcard", EmitDefaultValue = false)]
        [JsonPropertyName("wildcard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? Wildcard { get; set; }

        /// <summary>
        /// The name of a condition that is enforced over the allowed relation.
        /// </summary>
        /// <value>The name of a condition that is enforced over the allowed relation.</value>
        [DataMember(Name = "condition", EmitDefaultValue = false)]
        [JsonPropertyName("condition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Condition { get; set; }

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
        /// Builds a RelationReference from the JSON string presentation of the object
        /// </summary>
        /// <returns>RelationReference</returns>
        public static RelationReference FromJson(string jsonString) {
            return JsonSerializer.Deserialize<RelationReference>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            // Proper type checking in the Equals method - don't use 'as' operator
            if (input == null)
                return false;

            if (ReferenceEquals(this, input))
                return true;

            if (this.GetType() != input.GetType())
                return false;

            return Equals((RelationReference)input);
        }

        /// <summary>
        /// Returns true if RelationReference instances are equal
        /// </summary>
        /// <param name="input">Instance of RelationReference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RelationReference input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(RelationReference input) {

            if (!IsPropertyEqual(this.Type, input.Type)) {
                return false;
            }

            if (!IsPropertyEqual(this.Relation, input.Relation)) {
                return false;
            }

            if (!IsPropertyEqual(this.Wildcard, input.Wildcard)) {
                return false;
            }

            if (!IsPropertyEqual(this.Condition, input.Condition)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(RelationReference input) {
            if (this.AdditionalProperties.Count != input.AdditionalProperties.Count) {
                return false;
            }

            return !this.AdditionalProperties.Except(input.AdditionalProperties).Any();
        }

        private bool IsPropertyEqual<T>(T thisValue, T otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.Equals(otherValue);
        }

        private bool IsCollectionPropertyEqual<T>(IEnumerable<T> thisValue, IEnumerable<T> otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.SequenceEqual(otherValue);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                if (this.Type != null) {
                    hashCode = (hashCode * 9923) + this.Type.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
                }
                if (this.Wildcard != null) {
                    hashCode = (hashCode * 9923) + this.Wildcard.GetHashCode();
                }
                if (this.Condition != null) {
                    hashCode = (hashCode * 9923) + this.Condition.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
                    hashCode = (hashCode * 9923) + this.AdditionalProperties.GetHashCode();
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