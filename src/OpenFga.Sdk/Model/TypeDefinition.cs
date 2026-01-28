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
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


using OpenFga.Sdk.Constants;

namespace OpenFga.Sdk.Model
{
    /// <summary>
    /// TypeDefinition
    /// </summary>
    [DataContract(Name = "TypeDefinition")]
    public partial class TypeDefinition : IEquatable<TypeDefinition>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition" /> class.
        /// </summary>
        [JsonConstructor]
        public TypeDefinition()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="relations">relations.</param>
        /// <param name="metadata">metadata.</param>
        public TypeDefinition(string type = default, Dictionary<string, Userset> relations = default, Metadata metadata = default)
        {
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for TypeDefinition and cannot be null");
            }
            this.Type = type;
            this.Relations = relations;
            this.Metadata = metadata;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Relations
        /// </summary>
        [DataMember(Name = "relations", EmitDefaultValue = false)]
        [JsonPropertyName("relations")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, Userset>? Relations { get; set; }

        /// <summary>
        /// Gets or Sets Metadata
        /// </summary>
        [DataMember(Name = "metadata", EmitDefaultValue = false)]
        [JsonPropertyName("metadata")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Metadata? Metadata { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
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
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((TypeDefinition)input);
        }

        /// <summary>
        /// Returns true if TypeDefinition instances are equal
        /// </summary>
        /// <param name="input">Instance of TypeDefinition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TypeDefinition input)
        {
            if (input == null)
            {
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
                ) && 
                (
                    this.Metadata == input.Metadata ||
                    (this.Metadata != null &&
                    this.Metadata.Equals(input.Metadata))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Type != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Type.GetHashCode();
                }
                if (this.Relations != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Relations.GetHashCode();
                }
                if (this.Metadata != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Metadata.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

    }

}
