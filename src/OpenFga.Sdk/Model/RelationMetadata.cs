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


using OpenFga.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        /// <param name="module">module.</param>
        /// <param name="sourceInfo">sourceInfo.</param>
        public RelationMetadata(List<RelationReference> directlyRelatedUserTypes = default, string module = default, SourceInfo sourceInfo = default) {
            this.DirectlyRelatedUserTypes = directlyRelatedUserTypes;
            this.Module = module;
            this.SourceInfo = sourceInfo;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets DirectlyRelatedUserTypes
        /// </summary>
        [DataMember(Name = "directly_related_user_types", EmitDefaultValue = false)]
        [JsonPropertyName("directly_related_user_types")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<RelationReference>? DirectlyRelatedUserTypes { get; set; }

        /// <summary>
        /// Gets or Sets Module
        /// </summary>
        [DataMember(Name = "module", EmitDefaultValue = false)]
        [JsonPropertyName("module")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Module { get; set; }

        /// <summary>
        /// Gets or Sets SourceInfo
        /// </summary>
        [DataMember(Name = "source_info", EmitDefaultValue = false)]
        [JsonPropertyName("source_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public SourceInfo? SourceInfo { get; set; }

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
                ) &&
                (
                    this.Module == input.Module ||
                    (this.Module != null &&
                    this.Module.Equals(input.Module))
                ) &&
                (
                    this.SourceInfo == input.SourceInfo ||
                    (this.SourceInfo != null &&
                    this.SourceInfo.Equals(input.SourceInfo))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.DirectlyRelatedUserTypes != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.DirectlyRelatedUserTypes.GetHashCode();
                }
                if (this.Module != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Module.GetHashCode();
                }
                if (this.SourceInfo != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.SourceInfo.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            yield break;
        }

    }

}