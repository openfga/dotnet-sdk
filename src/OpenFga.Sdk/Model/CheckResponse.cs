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
    /// CheckResponse
    /// </summary>
    [DataContract(Name = "CheckResponse")]
    public partial class CheckResponse : IEquatable<CheckResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public CheckResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckResponse" /> class.
        /// </summary>
        /// <param name="allowed">allowed.</param>
        /// <param name="resolution">For internal use only..</param>
        public CheckResponse(bool allowed = default, string resolution = default) {
            this.Allowed = allowed;
            this.Resolution = resolution;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Allowed
        /// </summary>
        [DataMember(Name = "allowed", EmitDefaultValue = true)]
        [JsonPropertyName("allowed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Allowed { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <value>For internal use only.</value>
        [DataMember(Name = "resolution", EmitDefaultValue = false)]
        [JsonPropertyName("resolution")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Resolution { get; set; }

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
        /// Builds a CheckResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>CheckResponse</returns>
        public static CheckResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<CheckResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as CheckResponse);
        }

        /// <summary>
        /// Returns true if CheckResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of CheckResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CheckResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Allowed == input.Allowed ||
                    this.Allowed.Equals(input.Allowed)
                ) &&
                (
                    this.Resolution == input.Resolution ||
                    (this.Resolution != null &&
                    this.Resolution.Equals(input.Resolution))
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
                hashCode = (hashCode * 9923) + this.Allowed.GetHashCode();
                if (this.Resolution != null) {
                    hashCode = (hashCode * 9923) + this.Resolution.GetHashCode();
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