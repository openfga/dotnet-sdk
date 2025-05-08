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
    /// UsersetTreeDifference
    /// </summary>
    [DataContract(Name = "UsersetTree.Difference")]
    public partial class UsersetTreeDifference : IEquatable<UsersetTreeDifference>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeDifference() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        /// <param name="_base">_base (required).</param>
        /// <param name="subtract">subtract (required).</param>
        public UsersetTreeDifference(Node _base = default(Node), Node subtract = default(Node)) {
            // to ensure "_base" is required (not null)
            if (_base == null) {
                throw new ArgumentNullException("_base is a required property for UsersetTreeDifference and cannot be null");
            }
            this.Base = _base;
            // to ensure "subtract" is required (not null)
            if (subtract == null) {
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
        public virtual string ToJson() {
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
        public override bool Equals(object input) {
            // Proper type checking in the Equals method - don't use 'as' operator
            if (input == null)
                return false;

            if (ReferenceEquals(this, input))
                return true;

            if (this.GetType() != input.GetType())
                return false;

            return Equals((UsersetTreeDifference)input);
        }

        /// <summary>
        /// Returns true if UsersetTreeDifference instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeDifference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeDifference input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(UsersetTreeDifference input) {

            if (!IsPropertyEqual(this.Base, input.Base)) {
                return false;
            }

            if (!IsPropertyEqual(this.Subtract, input.Subtract)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(UsersetTreeDifference input) {
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
                if (this.Base != null) {
                    hashCode = (hashCode * 9923) + this.Base.GetHashCode();
                }
                if (this.Subtract != null) {
                    hashCode = (hashCode * 9923) + this.Subtract.GetHashCode();
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