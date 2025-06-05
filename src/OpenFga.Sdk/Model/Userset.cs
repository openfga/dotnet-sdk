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
    /// Userset
    /// </summary>
    [DataContract(Name = "Userset")]
    public partial class Userset : IEquatable<Userset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Userset" /> class.
        /// </summary>
        [JsonConstructor]
        public Userset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Userset" /> class.
        /// </summary>
        /// <param name="_this">A DirectUserset is a sentinel message for referencing the direct members specified by an object/relation mapping..</param>
        /// <param name="computedUserset">computedUserset.</param>
        /// <param name="tupleToUserset">tupleToUserset.</param>
        /// <param name="union">union.</param>
        /// <param name="intersection">intersection.</param>
        /// <param name="difference">difference.</param>
        public Userset(Object _this = default(Object), ObjectRelation computedUserset = default(ObjectRelation), TupleToUserset tupleToUserset = default(TupleToUserset), Usersets union = default(Usersets), Usersets intersection = default(Usersets), Difference difference = default(Difference)) {
            this.This = _this;
            this.ComputedUserset = computedUserset;
            this.TupleToUserset = tupleToUserset;
            this.Union = union;
            this.Intersection = intersection;
            this.Difference = difference;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// A DirectUserset is a sentinel message for referencing the direct members specified by an object/relation mapping.
        /// </summary>
        /// <value>A DirectUserset is a sentinel message for referencing the direct members specified by an object/relation mapping.</value>
        [DataMember(Name = "this", EmitDefaultValue = false)]
        [JsonPropertyName("this")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? This { get; set; }

        /// <summary>
        /// Gets or Sets ComputedUserset
        /// </summary>
        [DataMember(Name = "computedUserset", EmitDefaultValue = false)]
        [JsonPropertyName("computedUserset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ObjectRelation? ComputedUserset { get; set; }

        /// <summary>
        /// Gets or Sets TupleToUserset
        /// </summary>
        [DataMember(Name = "tupleToUserset", EmitDefaultValue = false)]
        [JsonPropertyName("tupleToUserset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TupleToUserset? TupleToUserset { get; set; }

        /// <summary>
        /// Gets or Sets Union
        /// </summary>
        [DataMember(Name = "union", EmitDefaultValue = false)]
        [JsonPropertyName("union")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Usersets? Union { get; set; }

        /// <summary>
        /// Gets or Sets Intersection
        /// </summary>
        [DataMember(Name = "intersection", EmitDefaultValue = false)]
        [JsonPropertyName("intersection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Usersets? Intersection { get; set; }

        /// <summary>
        /// Gets or Sets Difference
        /// </summary>
        [DataMember(Name = "difference", EmitDefaultValue = false)]
        [JsonPropertyName("difference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Difference? Difference { get; set; }

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
        /// Builds a Userset from the JSON string presentation of the object
        /// </summary>
        /// <returns>Userset</returns>
        public static Userset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Userset>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((Userset)input);
        }

        /// <summary>
        /// Returns true if Userset instances are equal
        /// </summary>
        /// <param name="input">Instance of Userset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Userset input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(Userset input) {

            if (!IsPropertyEqual(this.This, input.This)) {
                return false;
            }

            if (!IsPropertyEqual(this.ComputedUserset, input.ComputedUserset)) {
                return false;
            }

            if (!IsPropertyEqual(this.TupleToUserset, input.TupleToUserset)) {
                return false;
            }

            if (!IsPropertyEqual(this.Union, input.Union)) {
                return false;
            }

            if (!IsPropertyEqual(this.Intersection, input.Intersection)) {
                return false;
            }

            if (!IsPropertyEqual(this.Difference, input.Difference)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(Userset input) {
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
                if (this.This != null) {
                    hashCode = (hashCode * 9923) + this.This.GetHashCode();
                }
                if (this.ComputedUserset != null) {
                    hashCode = (hashCode * 9923) + this.ComputedUserset.GetHashCode();
                }
                if (this.TupleToUserset != null) {
                    hashCode = (hashCode * 9923) + this.TupleToUserset.GetHashCode();
                }
                if (this.Union != null) {
                    hashCode = (hashCode * 9923) + this.Union.GetHashCode();
                }
                if (this.Intersection != null) {
                    hashCode = (hashCode * 9923) + this.Intersection.GetHashCode();
                }
                if (this.Difference != null) {
                    hashCode = (hashCode * 9923) + this.Difference.GetHashCode();
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