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
    /// ObjectRelation
    /// </summary>
    [DataContract(Name = "ObjectRelation")]
    public partial class ObjectRelation : IEquatable<ObjectRelation>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectRelation" /> class.
        /// </summary>
        [JsonConstructor]
        public ObjectRelation() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectRelation" /> class.
        /// </summary>
        /// <param name="_object">_object.</param>
        /// <param name="relation">relation.</param>
        public ObjectRelation(string _object = default(string), string relation = default(string)) {
            this.Object = _object;
            this.Relation = relation;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Object { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Relation { get; set; }

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
        /// Builds a ObjectRelation from the JSON string presentation of the object
        /// </summary>
        /// <returns>ObjectRelation</returns>
        public static ObjectRelation FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ObjectRelation>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ObjectRelation)input);
        }

        /// <summary>
        /// Returns true if ObjectRelation instances are equal
        /// </summary>
        /// <param name="input">Instance of ObjectRelation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ObjectRelation input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ObjectRelation input) {

            if (!IsPropertyEqual(this.Object, input.Object)) {
                return false;
            }

            if (!IsPropertyEqual(this.Relation, input.Relation)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ObjectRelation input) {
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
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
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