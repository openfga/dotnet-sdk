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
    /// ReadResponse
    /// </summary>
    [DataContract(Name = "ReadResponse")]
    public partial class ReadResponse : IEquatable<ReadResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadResponse" /> class.
        /// </summary>
        /// <param name="tuples">tuples.</param>
        /// <param name="continuationToken">continuationToken.</param>
        public ReadResponse(List<Tuple> tuples = default(List<Tuple>), string continuationToken = default(string)) {
            this.Tuples = tuples;
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tuples
        /// </summary>
        [DataMember(Name = "tuples", EmitDefaultValue = false)]
        [JsonPropertyName("tuples")]
        public List<Tuple> Tuples { get; set; }

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
        /// Builds a ReadResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadResponse</returns>
        public static ReadResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ReadResponse);
        }

        /// <summary>
        /// Returns true if ReadResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Tuples == input.Tuples ||
                    this.Tuples != null &&
                    input.Tuples != null &&
                    this.Tuples.SequenceEqual(input.Tuples)
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
                if (this.Tuples != null) {
                    hashCode = (hashCode * 9923) + this.Tuples.GetHashCode();
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