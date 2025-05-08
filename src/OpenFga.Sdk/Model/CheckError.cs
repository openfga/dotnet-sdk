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
    /// CheckError
    /// </summary>
    [DataContract(Name = "CheckError")]
    public partial class CheckError : IEquatable<CheckError>, IValidatableObject {

        /// <summary>
        /// Gets or Sets InputError
        /// </summary>
        [DataMember(Name = "input_error", EmitDefaultValue = false)]
        [JsonPropertyName("input_error")]
        public ErrorCode? InputError { get; set; }

        /// <summary>
        /// Gets or Sets InternalError
        /// </summary>
        [DataMember(Name = "internal_error", EmitDefaultValue = false)]
        [JsonPropertyName("internal_error")]
        public InternalErrorCode? InternalError { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckError" /> class.
        /// </summary>
        [JsonConstructor]
        public CheckError() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckError" /> class.
        /// </summary>
        /// <param name="inputError">inputError.</param>
        /// <param name="internalError">internalError.</param>
        /// <param name="message">message.</param>
        public CheckError(ErrorCode? inputError = default(ErrorCode?), InternalErrorCode? internalError = default(InternalErrorCode?), string message = default(string)) {
            this.InputError = inputError;
            this.InternalError = internalError;
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
        /// Builds a CheckError from the JSON string presentation of the object
        /// </summary>
        /// <returns>CheckError</returns>
        public static CheckError FromJson(string jsonString) {
            return JsonSerializer.Deserialize<CheckError>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((CheckError)input);
        }

        /// <summary>
        /// Returns true if CheckError instances are equal
        /// </summary>
        /// <param name="input">Instance of CheckError to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CheckError input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(CheckError input) {

            if (!IsPropertyEqual(this.InputError, input.InputError)) {
                return false;
            }

            if (!IsPropertyEqual(this.InternalError, input.InternalError)) {
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

        private bool AreAdditionalPropertiesEqual(CheckError input) {
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
                hashCode = (hashCode * 9923) + this.InputError.GetHashCode();
                hashCode = (hashCode * 9923) + this.InternalError.GetHashCode();
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