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
    /// ValidationErrorMessageResponse
    /// </summary>
    [DataContract(Name = "ValidationErrorMessageResponse")]
    public partial class ValidationErrorMessageResponse : IEquatable<ValidationErrorMessageResponse>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        [JsonPropertyName("code")]
        public ErrorCode? Code { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorMessageResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ValidationErrorMessageResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrorMessageResponse" /> class.
        /// </summary>
        /// <param name="code">code.</param>
        /// <param name="message">message.</param>
        public ValidationErrorMessageResponse(ErrorCode? code = default(ErrorCode?), string message = default(string)) {
            this.Code = code;
            this.Message = message;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Message { get; set; }

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
        /// Builds a ValidationErrorMessageResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ValidationErrorMessageResponse</returns>
        public static ValidationErrorMessageResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ValidationErrorMessageResponse>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ValidationErrorMessageResponse)input);
        }

        /// <summary>
        /// Returns true if ValidationErrorMessageResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ValidationErrorMessageResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ValidationErrorMessageResponse input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ValidationErrorMessageResponse input) {

            if (!IsPropertyEqual(this.Code, input.Code)) {
                return false;
            }

            if (!IsPropertyEqual(this.Message, input.Message)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ValidationErrorMessageResponse input) {
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
                hashCode = (hashCode * 9923) + this.Code.GetHashCode();
                if (this.Message != null) {
                    hashCode = (hashCode * 9923) + this.Message.GetHashCode();
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