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
    /// A leaf node contains either - a set of users (which may be individual users, or usersets   referencing other relations) - a computed node, which is the result of a computed userset   value in the authorization model - a tupleToUserset nodes, containing the result of expanding   a tupleToUserset value in a authorization model.
    /// </summary>
    [DataContract(Name = "Leaf")]
    public partial class Leaf : IEquatable<Leaf>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf" /> class.
        /// </summary>
        [JsonConstructor]
        public Leaf() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf" /> class.
        /// </summary>
        /// <param name="users">users.</param>
        /// <param name="computed">computed.</param>
        /// <param name="tupleToUserset">tupleToUserset.</param>
        public Leaf(Users users = default(Users), Computed computed = default(Computed), UsersetTreeTupleToUserset tupleToUserset = default(UsersetTreeTupleToUserset)) {
            this.Users = users;
            this.Computed = computed;
            this.TupleToUserset = tupleToUserset;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Users
        /// </summary>
        [DataMember(Name = "users", EmitDefaultValue = false)]
        [JsonPropertyName("users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Users? Users { get; set; }

        /// <summary>
        /// Gets or Sets Computed
        /// </summary>
        [DataMember(Name = "computed", EmitDefaultValue = false)]
        [JsonPropertyName("computed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Computed? Computed { get; set; }

        /// <summary>
        /// Gets or Sets TupleToUserset
        /// </summary>
        [DataMember(Name = "tupleToUserset", EmitDefaultValue = false)]
        [JsonPropertyName("tupleToUserset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UsersetTreeTupleToUserset? TupleToUserset { get; set; }

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
        /// Builds a Leaf from the JSON string presentation of the object
        /// </summary>
        /// <returns>Leaf</returns>
        public static Leaf FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Leaf>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((Leaf)input);
        }

        /// <summary>
        /// Returns true if Leaf instances are equal
        /// </summary>
        /// <param name="input">Instance of Leaf to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Leaf input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(Leaf input) {

            if (!IsPropertyEqual(this.Users, input.Users)) {
                return false;
            }

            if (!IsPropertyEqual(this.Computed, input.Computed)) {
                return false;
            }

            if (!IsPropertyEqual(this.TupleToUserset, input.TupleToUserset)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(Leaf input) {
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
                if (this.Users != null) {
                    hashCode = (hashCode * 9923) + this.Users.GetHashCode();
                }
                if (this.Computed != null) {
                    hashCode = (hashCode * 9923) + this.Computed.GetHashCode();
                }
                if (this.TupleToUserset != null) {
                    hashCode = (hashCode * 9923) + this.TupleToUserset.GetHashCode();
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