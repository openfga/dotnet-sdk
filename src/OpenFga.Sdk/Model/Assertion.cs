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
    /// Assertion
    /// </summary>
    [DataContract(Name = "Assertion")]
    public partial class Assertion : IEquatable<Assertion>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Assertion" /> class.
        /// </summary>
        [JsonConstructor]
        protected Assertion() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Assertion" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey.</param>
        /// <param name="expectation">expectation (required).</param>
        public Assertion(TupleKey? tupleKey = default(TupleKey), bool expectation = default(bool)) {
            this.Expectation = expectation;
            this.TupleKey = tupleKey;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKey
        /// </summary>
        [DataMember(Name = "tuple_key", EmitDefaultValue = false)]
        [JsonPropertyName("tuple_key")]
        public TupleKey TupleKey { get; set; }

        /// <summary>
        /// Gets or Sets Expectation
        /// </summary>
        [DataMember(Name = "expectation", IsRequired = true, EmitDefaultValue = true)]
        [JsonPropertyName("expectation")]
        public bool Expectation { get; set; }

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
        /// Builds a Assertion from the JSON string presentation of the object
        /// </summary>
        /// <returns>Assertion</returns>
        public static Assertion FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Assertion>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Assertion);
        }

        /// <summary>
        /// Returns true if Assertion instances are equal
        /// </summary>
        /// <param name="input">Instance of Assertion to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Assertion input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.TupleKey == input.TupleKey ||
                    (this.TupleKey != null &&
                    this.TupleKey.Equals(input.TupleKey))
                ) &&
                (
                    this.Expectation == input.Expectation ||
                    this.Expectation.Equals(input.Expectation)
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
                if (this.TupleKey != null) {
                    hashCode = (hashCode * 9923) + this.TupleKey.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.Expectation.GetHashCode();
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