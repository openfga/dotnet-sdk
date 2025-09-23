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
    /// ReadAuthorizationModelsResponse
    /// </summary>
    [DataContract(Name = "ReadAuthorizationModelsResponse")]
    public partial class ReadAuthorizationModelsResponse : IEquatable<ReadAuthorizationModelsResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadAuthorizationModelsResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadAuthorizationModelsResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadAuthorizationModelsResponse" /> class.
        /// </summary>
        /// <param name="authorizationModels">authorizationModels (required).</param>
        /// <param name="continuationToken">The continuation token will be empty if there are no more models..</param>
        public ReadAuthorizationModelsResponse(List<AuthorizationModel> authorizationModels = default, string continuationToken = default) {
            // to ensure "authorizationModels" is required (not null)
            if (authorizationModels == null) {
                throw new ArgumentNullException("authorizationModels is a required property for ReadAuthorizationModelsResponse and cannot be null");
            }
            this.AuthorizationModels = authorizationModels;
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets AuthorizationModels
        /// </summary>
        [DataMember(Name = "authorization_models", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("authorization_models")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<AuthorizationModel> AuthorizationModels { get; set; }

        /// <summary>
        /// The continuation token will be empty if there are no more models.
        /// </summary>
        /// <value>The continuation token will be empty if there are no more models.</value>
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
                    this.AuthorizationModels == input.AuthorizationModels ||
                    this.AuthorizationModels != null &&
                    input.AuthorizationModels != null &&
                    this.AuthorizationModels.SequenceEqual(input.AuthorizationModels)
                ) &&
                (
                    this.ContinuationToken == input.ContinuationToken ||
                    (this.ContinuationToken != null &&
                    this.ContinuationToken.Equals(input.ContinuationToken))
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
                if (this.AuthorizationModels != null) {
                    hashCode = (hashCode * 9923) + this.AuthorizationModels.GetHashCode();
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