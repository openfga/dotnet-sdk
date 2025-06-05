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
    /// ListObjectsRequest
    /// </summary>
    [DataContract(Name = "ListObjects_request")]
    public partial class ListObjectsRequest : IEquatable<ListObjectsRequest>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ListObjectsRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public ListObjectsRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListObjectsRequest" /> class.
        /// </summary>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        /// <param name="type">type (required).</param>
        /// <param name="relation">relation (required).</param>
        /// <param name="user">user (required).</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        /// <param name="context">Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation..</param>
        /// <param name="consistency">consistency.</param>
        public ListObjectsRequest(string authorizationModelId = default(string), string type = default(string), string relation = default(string), string user = default(string), ContextualTupleKeys contextualTuples = default(ContextualTupleKeys), Object context = default(Object), ConsistencyPreference? consistency = default(ConsistencyPreference?)) {
            // to ensure "type" is required (not null)
            if (type == null) {
                throw new ArgumentNullException("type is a required property for ListObjectsRequest and cannot be null");
            }
            this.Type = type;
            // to ensure "relation" is required (not null)
            if (relation == null) {
                throw new ArgumentNullException("relation is a required property for ListObjectsRequest and cannot be null");
            }
            this.Relation = relation;
            // to ensure "user" is required (not null)
            if (user == null) {
                throw new ArgumentNullException("user is a required property for ListObjectsRequest and cannot be null");
            }
            this.User = user;
            this.AuthorizationModelId = authorizationModelId;
            this.ContextualTuples = contextualTuples;
            this.Context = context;
            this.Consistency = consistency;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets AuthorizationModelId
        /// </summary>
        [DataMember(Name = "authorization_model_id", EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AuthorizationModelId { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Relation { get; set; }

        /// <summary>
        /// Gets or Sets User
        /// </summary>
        [DataMember(Name = "user", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string User { get; set; }

        /// <summary>
        /// Gets or Sets ContextualTuples
        /// </summary>
        [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
        [JsonPropertyName("contextual_tuples")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ContextualTupleKeys? ContextualTuples { get; set; }

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
        /// Builds a ListObjectsRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>ListObjectsRequest</returns>
        public static ListObjectsRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ListObjectsRequest>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ListObjectsRequest)input);
        }

        /// <summary>
        /// Returns true if ListObjectsRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of ListObjectsRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ListObjectsRequest input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ListObjectsRequest input) {

            if (!IsPropertyEqual(this.AuthorizationModelId, input.AuthorizationModelId)) {
                return false;
            }

            if (!IsPropertyEqual(this.Type, input.Type)) {
                return false;
            }

            if (!IsPropertyEqual(this.Relation, input.Relation)) {
                return false;
            }

            if (!IsPropertyEqual(this.User, input.User)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContextualTuples, input.ContextualTuples)) {
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

        private bool AreAdditionalPropertiesEqual(ListObjectsRequest input) {
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
                if (this.Type != null) {
                    hashCode = (hashCode * 9923) + this.Type.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
                }
                if (this.User != null) {
                    hashCode = (hashCode * 9923) + this.User.GetHashCode();
                }
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * 9923) + this.ContextualTuples.GetHashCode();
                }
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
            // User (string) maxLength
            if (this.User != null && this.User.Length > 512) {
                yield return new ValidationResult("Invalid value for User, length must be less than 512.", new[] { "User" });
            }

            // User (string) minLength
            if (this.User != null && this.User.Length < 1) {
                yield return new ValidationResult("Invalid value for User, length must be greater than 1.", new[] { "User" });
            }

            yield break;
        }

    }

}