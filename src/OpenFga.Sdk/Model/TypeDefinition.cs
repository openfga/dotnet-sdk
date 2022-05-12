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
    /// TypeDefinition
    /// </summary>
    [DataContract(Name = "TypeDefinition")]
    public partial class TypeDefinition : IEquatable<TypeDefinition>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition" /> class.
        /// </summary>
        [JsonConstructor]
        protected TypeDefinition() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="relations">relations (required).</param>
        public TypeDefinition(string? type = default(string), Dictionary<string, Userset>? relations = default(Dictionary<string, Userset>)) {
            // to ensure "type" is required (not null)
            if (type == null) {
                throw new ArgumentNullException("type is a required property for TypeDefinition and cannot be null");
            }
            this.Type = type;
            // to ensure "relations" is required (not null)
            if (relations == null) {
                throw new ArgumentNullException("relations is a required property for TypeDefinition and cannot be null");
            }
            this.Relations = relations;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Relations
        /// </summary>
        [DataMember(Name = "relations", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relations")]
        public Dictionary<string, Userset> Relations { get; set; }

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
        /// Builds a TypeDefinition from the JSON string presentation of the object
        /// </summary>
        /// <returns>TypeDefinition</returns>
        public static TypeDefinition FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TypeDefinition>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as TypeDefinition);
        }

        /// <summary>
        /// Returns true if TypeDefinition instances are equal
        /// </summary>
        /// <param name="input">Instance of TypeDefinition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TypeDefinition input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) &&
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
                if (this.Type != null) {
                    hashCode = (hashCode * 9923) + this.Type.GetHashCode();
                }
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