//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
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
    /// ReadAuthorizationModelsResponse
    /// </summary>
    [DataContract(Name = "ReadAuthorizationModelsResponse")]
    public partial class ReadAuthorizationModelsResponse : IEquatable<ReadAuthorizationModelsResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadAuthorizationModelsResponse" /> class.
        /// </summary>
        /// <param name="authorizationModelIds">authorizationModelIds.</param>
        /// <param name="continuationToken">continuationToken.</param>
        public ReadAuthorizationModelsResponse(List<string>? authorizationModelIds = default(List<string>), string? continuationToken = default(string)) {
            this.AuthorizationModelIds = authorizationModelIds;
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets AuthorizationModelIds
        /// </summary>
        [DataMember(Name = "authorization_model_ids", EmitDefaultValue = false)]
        [JsonPropertyName("authorization_model_ids")]
        public List<string> AuthorizationModelIds { get; set; }

        /// <summary>
        /// Gets or Sets ContinuationToken
        /// </summary>
        [DataMember(Name = "continuation_token", EmitDefaultValue = false)]
        [JsonPropertyName("continuation_token")]
        public string ContinuationToken { get; set; }

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
        /// Builds a ReadAuthorizationModelsResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadAuthorizationModelsResponse</returns>
        public static ReadAuthorizationModelsResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadAuthorizationModelsResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ReadAuthorizationModelsResponse);
        }

        /// <summary>
        /// Returns true if ReadAuthorizationModelsResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadAuthorizationModelsResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadAuthorizationModelsResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.AuthorizationModelIds == input.AuthorizationModelIds ||
                    this.AuthorizationModelIds != null &&
                    input.AuthorizationModelIds != null &&
                    this.AuthorizationModelIds.SequenceEqual(input.AuthorizationModelIds)
                ) &&
                (
                    this.ContinuationToken == input.ContinuationToken ||
                    (this.ContinuationToken != null &&
                    this.ContinuationToken.Equals(input.ContinuationToken))
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
                if (this.AuthorizationModelIds != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModelIds.GetHashCode();
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