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
    /// BatchCheckItem
    /// </summary>
    [DataContract(Name = "BatchCheckItem")]
    public partial class BatchCheckItem : IEquatable<BatchCheckItem>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckItem" /> class.
        /// </summary>
        [JsonConstructor]
        public BatchCheckItem() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckItem" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey (required).</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        /// <param name="context">context.</param>
        /// <param name="correlationId">correlation_id must be a string containing only letters, numbers, or hyphens, with length ≤ 36 characters. (required).</param>
        public BatchCheckItem(CheckRequestTupleKey tupleKey = default(CheckRequestTupleKey), ContextualTupleKeys contextualTuples = default(ContextualTupleKeys), Object context = default(Object), string correlationId = default(string)) {
            // to ensure "tupleKey" is required (not null)
            if (tupleKey == null) {
                throw new ArgumentNullException("tupleKey is a required property for BatchCheckItem and cannot be null");
            }
            this.TupleKey = tupleKey;
            // to ensure "correlationId" is required (not null)
            if (correlationId == null) {
                throw new ArgumentNullException("correlationId is a required property for BatchCheckItem and cannot be null");
            }
            this.CorrelationId = correlationId;
            this.ContextualTuples = contextualTuples;
            this.Context = context;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKey
        /// </summary>
        [DataMember(Name = "tuple_key", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuple_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CheckRequestTupleKey TupleKey { get; set; }

        /// <summary>
        /// Gets or Sets ContextualTuples
        /// </summary>
        [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
        [JsonPropertyName("contextual_tuples")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ContextualTupleKeys? ContextualTuples { get; set; }

        /// <summary>
        /// Gets or Sets Context
        /// </summary>
        [DataMember(Name = "context", EmitDefaultValue = false)]
        [JsonPropertyName("context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? Context { get; set; }

        /// <summary>
        /// correlation_id must be a string containing only letters, numbers, or hyphens, with length ≤ 36 characters.
        /// </summary>
        /// <value>correlation_id must be a string containing only letters, numbers, or hyphens, with length ≤ 36 characters.</value>
        [DataMember(Name = "correlation_id", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("correlation_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CorrelationId { get; set; }

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
        /// Builds a BatchCheckItem from the JSON string presentation of the object
        /// </summary>
        /// <returns>BatchCheckItem</returns>
        public static BatchCheckItem FromJson(string jsonString) {
            return JsonSerializer.Deserialize<BatchCheckItem>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((BatchCheckItem)input);
        }

        /// <summary>
        /// Returns true if BatchCheckItem instances are equal
        /// </summary>
        /// <param name="input">Instance of BatchCheckItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BatchCheckItem input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(BatchCheckItem input) {

            if (!IsPropertyEqual(this.TupleKey, input.TupleKey)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContextualTuples, input.ContextualTuples)) {
                return false;
            }

            if (!IsPropertyEqual(this.Context, input.Context)) {
                return false;
            }

            if (!IsPropertyEqual(this.CorrelationId, input.CorrelationId)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(BatchCheckItem input) {
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
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * 9923) + this.ContextualTuples.GetHashCode();
                }
                if (this.Context != null) {
                    hashCode = (hashCode * 9923) + this.Context.GetHashCode();
                }
                if (this.CorrelationId != null) {
                    hashCode = (hashCode * 9923) + this.CorrelationId.GetHashCode();
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