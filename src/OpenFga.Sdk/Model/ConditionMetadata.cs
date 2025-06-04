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

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// ConditionMetadata
    /// </summary>
    [DataContract(Name = "ConditionMetadata")]
    public partial class ConditionMetadata : IEquatable<ConditionMetadata>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionMetadata" /> class.
        /// </summary>
        [JsonConstructor]
        public ConditionMetadata() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionMetadata" /> class.
        /// </summary>
        /// <param name="module">module.</param>
        /// <param name="sourceInfo">sourceInfo.</param>
        public ConditionMetadata(string module = default(string), SourceInfo sourceInfo = default(SourceInfo)) {
            this.Module = module;
            this.SourceInfo = sourceInfo;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

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
        /// Builds a ConditionMetadata from the JSON string presentation of the object
        /// </summary>
        /// <returns>ConditionMetadata</returns>
        public static ConditionMetadata FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ConditionMetadata>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            // Proper type checking in the Equals method - don't use 'as' operator
            if (input == null)
                return false;

            if (ReferenceEquals(this, input))
                return true;

            if (this.GetType() != input.GetType())
                return false;

            return Equals((ConditionMetadata)input);
        }

        /// <summary>
        /// Returns true if ConditionMetadata instances are equal
        /// </summary>
        /// <param name="input">Instance of ConditionMetadata to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ConditionMetadata input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ConditionMetadata input) {

            if (!IsPropertyEqual(this.Module, input.Module)) {
                return false;
            }

            if (!IsPropertyEqual(this.SourceInfo, input.SourceInfo)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ConditionMetadata input) {
            if (this.AdditionalProperties.Count != input.AdditionalProperties.Count) {
                return false;
            }

            return !this.AdditionalProperties.Except(input.AdditionalProperties).Any();
        }

        private bool IsPropertyEqual<T>(T thisValue, T otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.Equals(otherValue);
        }

        private bool IsCollectionPropertyEqual<T>(IEnumerable<T> thisValue, IEnumerable<T> otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.SequenceEqual(otherValue);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                if (this.Module != null) {
                    hashCode = (hashCode * 9923) + this.Module.GetHashCode();
                }
                if (this.SourceInfo != null) {
                    hashCode = (hashCode * 9923) + this.SourceInfo.GetHashCode();
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