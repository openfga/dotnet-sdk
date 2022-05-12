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
    /// ReadChangesResponse
    /// </summary>
    [DataContract(Name = "ReadChangesResponse")]
    public partial class ReadChangesResponse : IEquatable<ReadChangesResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadChangesResponse" /> class.
        /// </summary>
        /// <param name="changes">changes.</param>
        /// <param name="continuationToken">continuationToken.</param>
        public ReadChangesResponse(List<TupleChange>? changes = default(List<TupleChange>), string? continuationToken = default(string)) {
            this.Changes = changes;
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Changes
        /// </summary>
        [DataMember(Name = "changes", EmitDefaultValue = false)]
        [JsonPropertyName("changes")]
        public List<TupleChange> Changes { get; set; }

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
        /// Builds a ReadChangesResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadChangesResponse</returns>
        public static ReadChangesResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadChangesResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ReadChangesResponse);
        }

        /// <summary>
        /// Returns true if ReadChangesResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadChangesResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadChangesResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Changes == input.Changes ||
                    this.Changes != null &&
                    input.Changes != null &&
                    this.Changes.SequenceEqual(input.Changes)
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
                if (this.Changes != null) {
                    hashCode = (hashCode * 9923) + this.Changes.GetHashCode();
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