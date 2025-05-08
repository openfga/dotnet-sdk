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
    /// CheckRequest
    /// </summary>
    [DataContract(Name = "Check_request")]
    public partial class CheckRequest : IEquatable<CheckRequest>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public CheckRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRequest" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey (required).</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        /// <param name="context">Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation..</param>
        /// <param name="consistency">consistency.</param>
        public CheckRequest(CheckRequestTupleKey tupleKey = default(CheckRequestTupleKey), ContextualTupleKeys contextualTuples = default(ContextualTupleKeys), string authorizationModelId = default(string), Object context = default(Object), ConsistencyPreference? consistency = default(ConsistencyPreference?)) {
            // to ensure "tupleKey" is required (not null)
            if (tupleKey == null) {
                throw new ArgumentNullException("tupleKey is a required property for CheckRequest and cannot be null");
            }
            this.TupleKey = tupleKey;
            this.ContextualTuples = contextualTuples;
            this.AuthorizationModelId = authorizationModelId;
            this.Context = context;
            this.Consistency = consistency;
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
        /// Gets or Sets AuthorizationModelId
        /// </summary>
        [DataMember(Name = "authorization_model_id", EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AuthorizationModelId { get; set; }

        /// <summary>
        /// Defaults to false. Making it true has performance implications.
        /// </summary>
        /// <value>Defaults to false. Making it true has performance implications.</value>
        [DataMember(Name = "trace", EmitDefaultValue = true)]
        [JsonPropertyName("trace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Trace { get; private set; }

        /// <summary>
        /// Returns false as Trace should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeTrace() {
            return false;
        }
        /// <summary>
        /// Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation.
        /// </summary>
        /// <value>Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation.</value>
        [DataMember(Name = "context", EmitDefaultValue = false)]
        [JsonPropertyName("context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? Context { get; set; }

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
        /// Builds a CheckRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>CheckRequest</returns>
        public static CheckRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<CheckRequest>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((CheckRequest)input);
        }

        /// <summary>
        /// Returns true if CheckRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of CheckRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CheckRequest input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(CheckRequest input) {

            if (!IsPropertyEqual(this.TupleKey, input.TupleKey)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContextualTuples, input.ContextualTuples)) {
                return false;
            }

            if (!IsPropertyEqual(this.AuthorizationModelId, input.AuthorizationModelId)) {
                return false;
            }

            if (!IsPropertyEqual(this.Trace, input.Trace)) {
                return false;
            }

            if (!IsPropertyEqual(this.Context, input.Context)) {
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

        private bool AreAdditionalPropertiesEqual(CheckRequest input) {
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
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.Trace.GetHashCode();
                if (this.Context != null) {
                    hashCode = (hashCode * 9923) + this.Context.GetHashCode();
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
            yield break;
        }

    }

}