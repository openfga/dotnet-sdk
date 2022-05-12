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
    /// WriteAssertionsRequestParams
    /// </summary>
    [DataContract(Name = "WriteAssertionsRequestParams")]
    public partial class WriteAssertionsRequestParams : IEquatable<WriteAssertionsRequestParams>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAssertionsRequestParams" /> class.
        /// </summary>
        [JsonConstructor]
        protected WriteAssertionsRequestParams() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAssertionsRequestParams" /> class.
        /// </summary>
        /// <param name="assertions">assertions (required).</param>
        public WriteAssertionsRequestParams(List<Assertion>? assertions = default(List<Assertion>)) {
            // to ensure "assertions" is required (not null)
            if (assertions == null) {
                throw new ArgumentNullException("assertions is a required property for WriteAssertionsRequestParams and cannot be null");
            }
            this.Assertions = assertions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Assertions
        /// </summary>
        [DataMember(Name = "assertions", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("assertions")]
        public List<Assertion> Assertions { get; set; }

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
        /// Builds a WriteAssertionsRequestParams from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteAssertionsRequestParams</returns>
        public static WriteAssertionsRequestParams FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteAssertionsRequestParams>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as WriteAssertionsRequestParams);
        }

        /// <summary>
        /// Returns true if WriteAssertionsRequestParams instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteAssertionsRequestParams to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteAssertionsRequestParams input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Assertions == input.Assertions ||
                    this.Assertions != null &&
                    input.Assertions != null &&
                    this.Assertions.SequenceEqual(input.Assertions)
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
                if (this.Assertions != null) {
                    hashCode = (hashCode * 9923) + this.Assertions.GetHashCode();
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