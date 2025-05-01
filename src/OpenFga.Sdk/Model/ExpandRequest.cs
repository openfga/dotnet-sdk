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
    /// ExpandRequest
    /// </summary>
    [DataContract(Name = "Expand_request")]
    public partial class ExpandRequest : IEquatable<ExpandRequest>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public ExpandRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandRequest" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey (required).</param>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        /// <param name="consistency">consistency.</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        public ExpandRequest(ExpandRequestTupleKey tupleKey = default(ExpandRequestTupleKey), string authorizationModelId = default(string), ConsistencyPreference? consistency = default(ConsistencyPreference?), ContextualTupleKeys contextualTuples = default(ContextualTupleKeys)) {
            // to ensure "tupleKey" is required (not null)
            if (tupleKey == null) {
                throw new ArgumentNullException("tupleKey is a required property for ExpandRequest and cannot be null");
            }
            this.TupleKey = tupleKey;
            this.AuthorizationModelId = authorizationModelId;
            this.Consistency = consistency;
            this.ContextualTuples = contextualTuples;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKey
        /// </summary>
        [DataMember(Name = "tuple_key", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuple_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ExpandRequestTupleKey TupleKey { get; set; }

        /// <summary>
        /// Gets or Sets AuthorizationModelId
        /// </summary>
        [DataMember(Name = "authorization_model_id", EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AuthorizationModelId { get; set; }

        /// <summary>
        /// Gets or Sets ContextualTuples
        /// </summary>
        [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
        [JsonPropertyName("contextual_tuples")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ContextualTupleKeys? ContextualTuples { get; set; }

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
        /// Builds a ExpandRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>ExpandRequest</returns>
        public static ExpandRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ExpandRequest>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ExpandRequest);
        }

        /// <summary>
        /// Returns true if ExpandRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of ExpandRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ExpandRequest input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.TupleKey == input.TupleKey ||
                    (this.TupleKey != null &&
                    this.TupleKey.Equals(input.TupleKey))
                ) &&
                (
                    this.AuthorizationModelId == input.AuthorizationModelId ||
                    (this.AuthorizationModelId != null &&
                    this.AuthorizationModelId.Equals(input.AuthorizationModelId))
                ) &&
                (
                    this.Consistency == input.Consistency ||
                    this.Consistency.Equals(input.Consistency)
                ) &&
                (
                    this.ContextualTuples == input.ContextualTuples ||
                    (this.ContextualTuples != null &&
                    this.ContextualTuples.Equals(input.ContextualTuples))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && !this.AdditionalProperties.Except(input.AdditionalProperties).Any());
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
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.Consistency.GetHashCode();
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * 9923) + this.ContextualTuples.GetHashCode();
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