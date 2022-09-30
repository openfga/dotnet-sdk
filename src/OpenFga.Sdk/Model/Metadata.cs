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
    /// Metadata
    /// </summary>
    [DataContract(Name = "Metadata")]
    public partial class Metadata : IEquatable<Metadata>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata" /> class.
        /// </summary>
        [JsonConstructor]
        public Metadata() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Metadata" /> class.
        /// </summary>
        /// <param name="relations">relations.</param>
        public Metadata(Dictionary<string, RelationMetadata> relations = default(Dictionary<string, RelationMetadata>)) {
            this.Relations = relations;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Relations
        /// </summary>
        [DataMember(Name = "relations", EmitDefaultValue = false)]
        [JsonPropertyName("relations")]
        public Dictionary<string, RelationMetadata> Relations { get; set; }

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
        /// Builds a Metadata from the JSON string presentation of the object
        /// </summary>
        /// <returns>Metadata</returns>
        public static Metadata FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Metadata>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Metadata);
        }

        /// <summary>
        /// Returns true if Metadata instances are equal
        /// </summary>
        /// <param name="input">Instance of Metadata to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Metadata input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Relations == input.Relations ||
                    this.Relations != null &&
                    input.Relations != null &&
                    this.Relations.SequenceEqual(input.Relations)
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
                if (this.Relations != null) {
                    hashCode = (hashCode * 9923) + this.Relations.GetHashCode();
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