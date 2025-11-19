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


using OpenFga.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
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
        /// <param name="varObject">varObject (required).</param>
        /// <param name="relation">relation (required).</param>
        /// <param name="userFilters">The type of results returned. Only accepts exactly one value. (required).</param>
        /// <param name="contextualTuples">contextualTuples.</param>
        /// <param name="context">Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation..</param>
        /// <param name="consistency">consistency.</param>
        public ListUsersRequest(string authorizationModelId = default, FgaObject varObject = default, string relation = default, List<UserTypeFilter> userFilters = default, List<TupleKey> contextualTuples = default, Object context = default, ConsistencyPreference? consistency = default) {
            // to ensure "varObject" is required (not null)
            if (varObject == null) {
                throw new ArgumentNullException("varObject is a required property for ListUsersRequest and cannot be null");
            }
            this.Object = varObject;
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
                ) &&
                (
                    this.Consistency == input.Consistency ||
                    this.Consistency.Equals(input.Consistency)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AuthorizationModelId.GetHashCode();
                }
                if (this.Object != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Object.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Relation.GetHashCode();
                }
                if (this.UserFilters != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.UserFilters.GetHashCode();
                }
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.ContextualTuples.GetHashCode();
                }
                if (this.Context != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Context.GetHashCode();
                }
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Consistency.GetHashCode();
                if (this.AdditionalProperties != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AdditionalProperties.GetHashCode();
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