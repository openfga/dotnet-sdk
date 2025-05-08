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
    /// ReadRequest
    /// </summary>
    [DataContract(Name = "Read_request")]
    public partial class ReadRequest : IEquatable<ReadRequest>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadRequest" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey.</param>
        /// <param name="pageSize">pageSize.</param>
        /// <param name="continuationToken">continuationToken.</param>
        /// <param name="consistency">consistency.</param>
        public ReadRequest(ReadRequestTupleKey tupleKey = default(ReadRequestTupleKey), int pageSize = default(int), string continuationToken = default(string), ConsistencyPreference? consistency = default(ConsistencyPreference?)) {
            this.TupleKey = tupleKey;
            this.PageSize = pageSize;
            this.ContinuationToken = continuationToken;
            this.Consistency = consistency;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKey
        /// </summary>
        [DataMember(Name = "tuple_key", EmitDefaultValue = false)]
        [JsonPropertyName("tuple_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ReadRequestTupleKey? TupleKey { get; set; }

        /// <summary>
        /// Gets or Sets PageSize
        /// </summary>
        [DataMember(Name = "page_size", EmitDefaultValue = false)]
        [JsonPropertyName("page_size")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or Sets ContinuationToken
        /// </summary>
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
        /// Builds a ReadRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadRequest</returns>
        public static ReadRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadRequest>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ReadRequest)input);
        }

        /// <summary>
        /// Returns true if ReadRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadRequest input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ReadRequest input) {

            if (!IsPropertyEqual(this.TupleKey, input.TupleKey)) {
                return false;
            }

            if (!IsPropertyEqual(this.PageSize, input.PageSize)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContinuationToken, input.ContinuationToken)) {
                return false;
            }

            if (!IsPropertyEqual(this.Consistency, input.Consistency)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ReadRequest input) {
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
                if (this.TupleKey != null) {
                    hashCode = (hashCode * 9923) + this.TupleKey.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.PageSize.GetHashCode();
                if (this.ContinuationToken != null) {
                    hashCode = (hashCode * 9923) + this.ContinuationToken.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.Consistency.GetHashCode();
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
            // PageSize (int) maximum
            if (this.PageSize > 100) {
                yield return new ValidationResult("Invalid value for PageSize, must be a value less than or equal to 100.", new[] { "PageSize" });
            }

            // PageSize (int) minimum
            if (this.PageSize < 1) {
                yield return new ValidationResult("Invalid value for PageSize, must be a value greater than or equal to 1.", new[] { "PageSize" });
            }

            yield break;
        }

    }

}