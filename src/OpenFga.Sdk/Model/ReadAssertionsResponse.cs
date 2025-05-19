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
    /// ReadAssertionsResponse
    /// </summary>
    [DataContract(Name = "ReadAssertionsResponse")]
    public partial class ReadAssertionsResponse : IEquatable<ReadAssertionsResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadAssertionsResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadAssertionsResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadAssertionsResponse" /> class.
        /// </summary>
        /// <param name="authorizationModelId">authorizationModelId (required).</param>
        /// <param name="assertions">assertions.</param>
        public ReadAssertionsResponse(string authorizationModelId = default(string), List<Assertion> assertions = default(List<Assertion>)) {
            // to ensure "authorizationModelId" is required (not null)
            if (authorizationModelId == null) {
                throw new ArgumentNullException("authorizationModelId is a required property for ReadAssertionsResponse and cannot be null");
            }
            this.AuthorizationModelId = authorizationModelId;
            this.Assertions = assertions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets AuthorizationModelId
        /// </summary>
        [DataMember(Name = "authorization_model_id", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AuthorizationModelId { get; set; }

        /// <summary>
        /// Gets or Sets Assertions
        /// </summary>
        [DataMember(Name = "assertions", EmitDefaultValue = false)]
        [JsonPropertyName("assertions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Assertion>? Assertions { get; set; }

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
        /// Builds a ReadAssertionsResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadAssertionsResponse</returns>
        public static ReadAssertionsResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadAssertionsResponse>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ReadAssertionsResponse)input);
        }

        /// <summary>
        /// Returns true if ReadAssertionsResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadAssertionsResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadAssertionsResponse input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ReadAssertionsResponse input) {

            if (!IsPropertyEqual(this.AuthorizationModelId, input.AuthorizationModelId)) {
                return false;
            }

            if (!IsCollectionPropertyEqual(this.Assertions, input.Assertions)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ReadAssertionsResponse input) {
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
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
                }
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