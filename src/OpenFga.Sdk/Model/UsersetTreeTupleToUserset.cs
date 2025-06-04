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
    /// UsersetTreeTupleToUserset
    /// </summary>
    [DataContract(Name = "UsersetTree.TupleToUserset")]
    public partial class UsersetTreeTupleToUserset : IEquatable<UsersetTreeTupleToUserset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeTupleToUserset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        /// <param name="tupleset">tupleset (required).</param>
        /// <param name="computed">computed (required).</param>
        public UsersetTreeTupleToUserset(string tupleset = default(string), List<Computed> computed = default(List<Computed>)) {
            // to ensure "tupleset" is required (not null)
            if (tupleset == null) {
                throw new ArgumentNullException("tupleset is a required property for UsersetTreeTupleToUserset and cannot be null");
            }
            this.Tupleset = tupleset;
            // to ensure "computed" is required (not null)
            if (computed == null) {
                throw new ArgumentNullException("computed is a required property for UsersetTreeTupleToUserset and cannot be null");
            }
            this.Computed = computed;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tupleset
        /// </summary>
        [DataMember(Name = "tupleset", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tupleset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Tupleset { get; set; }

        /// <summary>
        /// Gets or Sets Computed
        /// </summary>
        [DataMember(Name = "computed", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("computed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Computed> Computed { get; set; }

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
        /// Builds a UsersetTreeTupleToUserset from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTreeTupleToUserset</returns>
        public static UsersetTreeTupleToUserset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTreeTupleToUserset>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((UsersetTreeTupleToUserset)input);
        }

        /// <summary>
        /// Returns true if UsersetTreeTupleToUserset instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeTupleToUserset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeTupleToUserset input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(UsersetTreeTupleToUserset input) {

            if (!IsPropertyEqual(this.Tupleset, input.Tupleset)) {
                return false;
            }

            if (!IsCollectionPropertyEqual(this.Computed, input.Computed)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(UsersetTreeTupleToUserset input) {
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
                if (this.Tupleset != null) {
                    hashCode = (hashCode * 9923) + this.Tupleset.GetHashCode();
                }
                if (this.Computed != null) {
                    hashCode = (hashCode * 9923) + this.Computed.GetHashCode();
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