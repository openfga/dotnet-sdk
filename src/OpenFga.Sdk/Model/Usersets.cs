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
    /// Usersets
    /// </summary>
    [DataContract(Name = "Usersets")]
    public partial class Usersets : IEquatable<Usersets>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Usersets" /> class.
        /// </summary>
        [JsonConstructor]
        public Usersets() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Usersets" /> class.
        /// </summary>
        /// <param name="child">child (required).</param>
        public Usersets(List<Userset> child = default(List<Userset>)) {
            // to ensure "child" is required (not null)
            if (child == null) {
                throw new ArgumentNullException("child is a required property for Usersets and cannot be null");
            }
            this.Child = child;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Child
        /// </summary>
        [DataMember(Name = "child", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("child")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Userset> Child { get; set; }

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
        /// Builds a Usersets from the JSON string presentation of the object
        /// </summary>
        /// <returns>Usersets</returns>
        public static Usersets FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Usersets>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((Usersets)input);
        }

        /// <summary>
        /// Returns true if Usersets instances are equal
        /// </summary>
        /// <param name="input">Instance of Usersets to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Usersets input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(Usersets input) {

            if (!IsCollectionPropertyEqual(this.Child, input.Child)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(Usersets input) {
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
                if (this.Child != null) {
                    hashCode = (hashCode * 9923) + this.Child.GetHashCode();
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