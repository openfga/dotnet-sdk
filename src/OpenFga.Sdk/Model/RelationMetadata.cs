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
    /// RelationMetadata
    /// </summary>
    [DataContract(Name = "RelationMetadata")]
    public partial class RelationMetadata : IEquatable<RelationMetadata>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationMetadata" /> class.
        /// </summary>
        [JsonConstructor]
        public RelationMetadata() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationMetadata" /> class.
        /// </summary>
        /// <param name="directlyRelatedUserTypes">directlyRelatedUserTypes.</param>
        public RelationMetadata(List<RelationReference> directlyRelatedUserTypes = default(List<RelationReference>)) {
            this.DirectlyRelatedUserTypes = directlyRelatedUserTypes;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets DirectlyRelatedUserTypes
        /// </summary>
        [DataMember(Name = "directly_related_user_types", EmitDefaultValue = false)]
        [JsonPropertyName("directly_related_user_types")]
        public List<RelationReference>? DirectlyRelatedUserTypes { get; set; }

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
        /// Builds a RelationMetadata from the JSON string presentation of the object
        /// </summary>
        /// <returns>RelationMetadata</returns>
        public static RelationMetadata FromJson(string jsonString) {
            return JsonSerializer.Deserialize<RelationMetadata>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as RelationMetadata);
        }

        /// <summary>
        /// Returns true if RelationMetadata instances are equal
        /// </summary>
        /// <param name="input">Instance of RelationMetadata to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RelationMetadata input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.DirectlyRelatedUserTypes == input.DirectlyRelatedUserTypes ||
                    this.DirectlyRelatedUserTypes != null &&
                    input.DirectlyRelatedUserTypes != null &&
                    this.DirectlyRelatedUserTypes.SequenceEqual(input.DirectlyRelatedUserTypes)
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
                if (this.DirectlyRelatedUserTypes != null) {
                    hashCode = (hashCode * 9923) + this.DirectlyRelatedUserTypes.GetHashCode();
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