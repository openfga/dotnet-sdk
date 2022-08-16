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
    /// TypeDefinitions
    /// </summary>
    [DataContract(Name = "TypeDefinitions")]
    public partial class TypeDefinitions : IEquatable<TypeDefinitions>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinitions" /> class.
        /// </summary>
        [JsonConstructor]
        public TypeDefinitions() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinitions" /> class.
        /// </summary>
        /// <param name="typeDefinitions">typeDefinitions.</param>
        public TypeDefinitions(List<TypeDefinition> typeDefinitions = default(List<TypeDefinition>)) {
            this._TypeDefinitions = typeDefinitions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets _TypeDefinitions
        /// </summary>
        [DataMember(Name = "type_definitions", EmitDefaultValue = false)]
        [JsonPropertyName("type_definitions")]
        public List<TypeDefinition> _TypeDefinitions { get; set; }

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
        /// Builds a TypeDefinitions from the JSON string presentation of the object
        /// </summary>
        /// <returns>TypeDefinitions</returns>
        public static TypeDefinitions FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TypeDefinitions>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as TypeDefinitions);
        }

        /// <summary>
        /// Returns true if TypeDefinitions instances are equal
        /// </summary>
        /// <param name="input">Instance of TypeDefinitions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TypeDefinitions input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this._TypeDefinitions == input._TypeDefinitions ||
                    this._TypeDefinitions != null &&
                    input._TypeDefinitions != null &&
                    this._TypeDefinitions.SequenceEqual(input._TypeDefinitions)
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
                if (this._TypeDefinitions != null) {
                    hashCode = (hashCode * 9923) + this._TypeDefinitions.GetHashCode();
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