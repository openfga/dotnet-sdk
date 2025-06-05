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
    /// WriteRequest
    /// </summary>
    [DataContract(Name = "Write_request")]
    public partial class WriteRequest : IEquatable<WriteRequest>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public WriteRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequest" /> class.
        /// </summary>
        /// <param name="writes">writes.</param>
        /// <param name="deletes">deletes.</param>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        public WriteRequest(WriteRequestWrites writes = default(WriteRequestWrites), WriteRequestDeletes deletes = default(WriteRequestDeletes), string authorizationModelId = default(string)) {
            this.Writes = writes;
            this.Deletes = deletes;
            this.AuthorizationModelId = authorizationModelId;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Writes
        /// </summary>
        [DataMember(Name = "writes", EmitDefaultValue = false)]
        [JsonPropertyName("writes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public WriteRequestWrites? Writes { get; set; }

        /// <summary>
        /// Gets or Sets Deletes
        /// </summary>
        [DataMember(Name = "deletes", EmitDefaultValue = false)]
        [JsonPropertyName("deletes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public WriteRequestDeletes? Deletes { get; set; }

        /// <summary>
        /// Gets or Sets AuthorizationModelId
        /// </summary>
        [DataMember(Name = "authorization_model_id", EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AuthorizationModelId { get; set; }

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
        /// Builds a WriteRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteRequest</returns>
        public static WriteRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteRequest>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((WriteRequest)input);
        }

        /// <summary>
        /// Returns true if WriteRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteRequest input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(WriteRequest input) {

            if (!IsPropertyEqual(this.Writes, input.Writes)) {
                return false;
            }

            if (!IsPropertyEqual(this.Deletes, input.Deletes)) {
                return false;
            }

            if (!IsPropertyEqual(this.AuthorizationModelId, input.AuthorizationModelId)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(WriteRequest input) {
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
                if (this.Writes != null) {
                    hashCode = (hashCode * 9923) + this.Writes.GetHashCode();
                }
                if (this.Deletes != null) {
                    hashCode = (hashCode * 9923) + this.Deletes.GetHashCode();
                }
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
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