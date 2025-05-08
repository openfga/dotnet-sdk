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
    /// ReadChangesResponse
    /// </summary>
    [DataContract(Name = "ReadChangesResponse")]
    public partial class ReadChangesResponse : IEquatable<ReadChangesResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadChangesResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadChangesResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadChangesResponse" /> class.
        /// </summary>
        /// <param name="changes">changes (required).</param>
        /// <param name="continuationToken">The continuation token will be identical if there are no new changes..</param>
        public ReadChangesResponse(List<TupleChange> changes = default(List<TupleChange>), string continuationToken = default(string)) {
            // to ensure "changes" is required (not null)
            if (changes == null) {
                throw new ArgumentNullException("changes is a required property for ReadChangesResponse and cannot be null");
            }
            this.Changes = changes;
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Changes
        /// </summary>
        [DataMember(Name = "changes", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("changes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TupleChange> Changes { get; set; }

        /// <summary>
        /// The continuation token will be identical if there are no new changes.
        /// </summary>
        /// <value>The continuation token will be identical if there are no new changes.</value>
        [DataMember(Name = "continuation_token", EmitDefaultValue = false)]
        [JsonPropertyName("continuation_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ContinuationToken { get; set; }

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
        /// Builds a ReadChangesResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadChangesResponse</returns>
        public static ReadChangesResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadChangesResponse>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ReadChangesResponse)input);
        }

        /// <summary>
        /// Returns true if ReadChangesResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadChangesResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadChangesResponse input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ReadChangesResponse input) {

            if (!IsCollectionPropertyEqual(this.Changes, input.Changes)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContinuationToken, input.ContinuationToken)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ReadChangesResponse input) {
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
                if (this.Changes != null) {
                    hashCode = (hashCode * 9923) + this.Changes.GetHashCode();
                }
                if (this.ContinuationToken != null) {
                    hashCode = (hashCode * 9923) + this.ContinuationToken.GetHashCode();
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