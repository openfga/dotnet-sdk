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
        public CheckRequest(CheckRequestTupleKey tupleKey = default, ContextualTupleKeys contextualTuples = default, string authorizationModelId = default, Object context = default, ConsistencyPreference? consistency = default) {
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
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((CheckRequest)input);
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
            return
                (
                    this.TupleKey == input.TupleKey ||
                    (this.TupleKey != null &&
                    this.TupleKey.Equals(input.TupleKey))
                ) &&
                (
                    this.ContextualTuples == input.ContextualTuples ||
                    (this.ContextualTuples != null &&
                    this.ContextualTuples.Equals(input.ContextualTuples))
                ) &&
                (
                    this.AuthorizationModelId == input.AuthorizationModelId ||
                    (this.AuthorizationModelId != null &&
                    this.AuthorizationModelId.Equals(input.AuthorizationModelId))
                ) &&
                (
                    this.Trace == input.Trace ||
                    this.Trace.Equals(input.Trace)
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
                if (this.TupleKey != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.TupleKey.GetHashCode();
                }
                if (this.ContextualTuples != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.ContextualTuples.GetHashCode();
                }
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AuthorizationModelId.GetHashCode();
                }
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Trace.GetHashCode();
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