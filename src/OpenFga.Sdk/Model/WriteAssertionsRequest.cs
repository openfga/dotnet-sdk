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
    /// WriteAssertionsRequest
    /// </summary>
    [DataContract(Name = "WriteAssertions_request")]
    public partial class WriteAssertionsRequest : IEquatable<WriteAssertionsRequest>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAssertionsRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public WriteAssertionsRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAssertionsRequest" /> class.
        /// </summary>
        /// <param name="assertions">assertions (required).</param>
        public WriteAssertionsRequest(List<Assertion> assertions = default(List<Assertion>)) {
            // to ensure "assertions" is required (not null)
            if (assertions == null) {
                throw new ArgumentNullException("assertions is a required property for WriteAssertionsRequest and cannot be null");
            }
            this.Assertions = assertions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Assertions
        /// </summary>
        [DataMember(Name = "assertions", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("assertions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Assertion> Assertions { get; set; }

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
        /// Builds a WriteAssertionsRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteAssertionsRequest</returns>
        public static WriteAssertionsRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteAssertionsRequest>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((WriteAssertionsRequest)input);
        }

        /// <summary>
        /// Returns true if WriteAssertionsRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteAssertionsRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteAssertionsRequest input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(WriteAssertionsRequest input) {

            if (!IsCollectionPropertyEqual(this.Assertions, input.Assertions)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(WriteAssertionsRequest input) {
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
                if (this.Assertions != null) {
                    hashCode = (hashCode * 9923) + this.Assertions.GetHashCode();
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