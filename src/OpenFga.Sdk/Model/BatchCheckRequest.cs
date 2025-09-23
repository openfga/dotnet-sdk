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


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace OpenFga.Sdk.Model {
    /// <summary>
    /// BatchCheckRequest
    /// </summary>
    [DataContract(Name = "BatchCheck_request")]
    public partial class BatchCheckRequest : IEquatable<BatchCheckRequest>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Consistency
        /// </summary>
        [DataMember(Name = "consistency", EmitDefaultValue = false)]
        [JsonPropertyName("consistency")]
        public ConsistencyPreference? Consistency { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public BatchCheckRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckRequest" /> class.
        /// </summary>
        /// <param name="checks">checks (required).</param>
        /// <param name="authorizationModelId">authorizationModelId.</param>
        /// <param name="consistency">consistency.</param>
        public BatchCheckRequest(List<BatchCheckItem> checks = default, string authorizationModelId = default, ConsistencyPreference? consistency = default) {
            // to ensure "checks" is required (not null)
            if (checks == null) {
                throw new ArgumentNullException("checks is a required property for BatchCheckRequest and cannot be null");
            }
            this.Checks = checks;
            this.AuthorizationModelId = authorizationModelId;
            this.Consistency = consistency;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Checks
        /// </summary>
        [DataMember(Name = "checks", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("checks")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<BatchCheckItem> Checks { get; set; }

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
        /// Builds a BatchCheckRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>BatchCheckRequest</returns>
        public static BatchCheckRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<BatchCheckRequest>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as BatchCheckRequest);
        }

        /// <summary>
        /// Returns true if BatchCheckRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of BatchCheckRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BatchCheckRequest input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Checks == input.Checks ||
                    this.Checks != null &&
                    input.Checks != null &&
                    this.Checks.SequenceEqual(input.Checks)
                ) &&
                (
                    this.AuthorizationModelId == input.AuthorizationModelId ||
                    (this.AuthorizationModelId != null &&
                    this.AuthorizationModelId.Equals(input.AuthorizationModelId))
                ) &&
                (
                    this.Consistency == input.Consistency ||
                    this.Consistency.Equals(input.Consistency)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.ContainsKey(kv.Key) && Equals(kv.Value, input.AdditionalProperties[kv.Key])));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                if (this.Checks != null) {
                    hashCode = (hashCode * 9923) + this.Checks.GetHashCode();
                }
                if (this.AuthorizationModelId != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelId.GetHashCode();
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