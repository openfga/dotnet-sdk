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


using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// ListUsersRequest
    /// </summary>
    [DataContract(Name = "ListUsers_request")]
    public partial class ListUsersRequest : IEquatable<ListUsersRequest>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListUsersRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public ListUsersRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUsersRequest" /> class.
        /// </summary>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        /// <param name="_object">_object (required).</param>
        /// <param name="relation">relation (required).</param>
        /// <param name="userFilters">The type of results returned. Only accepts exactly one value. (required).</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        /// <param name="context">Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation..</param>
        public ListUsersRequest(string authorizationModelId = default(string), FgaObject _object = default(FgaObject), string relation = default(string), List<UserTypeFilter> userFilters = default(List<UserTypeFilter>), List<TupleKey> contextualTuples = default(List<TupleKey>), Object context = default(Object)) {
            // to ensure "_object" is required (not null)
            if (_object == null) {
                throw new ArgumentNullException("_object is a required property for ListUsersRequest and cannot be null");
            }
            this.Object = _object;
            // to ensure "relation" is required (not null)
            if (relation == null) {
                throw new ArgumentNullException("relation is a required property for ListUsersRequest and cannot be null");
            }
            this.Relation = relation;
            // to ensure "userFilters" is required (not null)
            if (userFilters == null) {
                throw new ArgumentNullException("userFilters is a required property for ListUsersRequest and cannot be null");
            }
            this.UserFilters = userFilters;
            this.AuthorizationModelId = authorizationModelId;
            this.ContextualTuples = contextualTuples;
            this.Context = context;
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
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FgaObject Object { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Relation { get; set; }

        /// <summary>
        /// The type of results returned. Only accepts exactly one value.
        /// </summary>
        /// <value>The type of results returned. Only accepts exactly one value.</value>
        [DataMember(Name = "user_filters", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("user_filters")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<UserTypeFilter> UserFilters { get; set; }

        /// <summary>
        /// Gets or Sets ContextualTuples
        /// </summary>
        [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
        [JsonPropertyName("contextual_tuples")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TupleKey>? ContextualTuples { get; set; }

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
        /// Builds a ListUsersRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>ListUsersRequest</returns>
        public static ListUsersRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ListUsersRequest>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ListUsersRequest);
        }

        /// <summary>
        /// Returns true if ListUsersRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of ListUsersRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ListUsersRequest input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.AuthorizationModelId == input.AuthorizationModelId ||
                    (this.AuthorizationModelId != null &&
                    this.AuthorizationModelId.Equals(input.AuthorizationModelId))
                ) &&
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
                ) &&
                (
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                ) &&
                (
                    this.UserFilters == input.UserFilters ||
                    this.UserFilters != null &&
                    input.UserFilters != null &&
                    this.UserFilters.SequenceEqual(input.UserFilters)
                ) &&
                (
                    this.ContextualTuples == input.ContextualTuples ||
                    this.ContextualTuples != null &&
                    input.ContextualTuples != null &&
                    this.ContextualTuples.SequenceEqual(input.ContextualTuples)
                ) &&
                (
                    this.Context == input.Context ||
                    (this.Context != null &&
                    this.Context.Equals(input.Context))
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
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
                }
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
                }
                if (this.UserFilters != null) {
                    hashCode = (hashCode * 9923) + this.UserFilters.GetHashCode();
                }
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * 9923) + this.ContextualTuples.GetHashCode();
                }
                if (this.Context != null) {
                    hashCode = (hashCode * 9923) + this.Context.GetHashCode();
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