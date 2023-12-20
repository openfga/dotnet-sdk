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
    /// WriteRequestWrites
    /// </summary>
    [DataContract(Name = "WriteRequestWrites")]
    public partial class WriteRequestWrites : IEquatable<WriteRequestWrites>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequestWrites" /> class.
        /// </summary>
        [JsonConstructor]
        public WriteRequestWrites() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequestWrites" /> class.
        /// </summary>
        /// <param name="tupleKeys">tupleKeys (required).</param>
        public WriteRequestWrites(List<TupleKey> tupleKeys = default(List<TupleKey>)) {
            // to ensure "tupleKeys" is required (not null)
            if (tupleKeys == null) {
                throw new ArgumentNullException("tupleKeys is a required property for WriteRequestWrites and cannot be null");
            }
            this.TupleKeys = tupleKeys;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKeys
        /// </summary>
        [DataMember(Name = "tuple_keys", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuple_keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TupleKey> TupleKeys { get; set; }

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
        /// Builds a WriteRequestWrites from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteRequestWrites</returns>
        public static WriteRequestWrites FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteRequestWrites>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as WriteRequestWrites);
        }

        /// <summary>
        /// Returns true if WriteRequestWrites instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteRequestWrites to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteRequestWrites input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.TupleKeys == input.TupleKeys ||
                    this.TupleKeys != null &&
                    input.TupleKeys != null &&
                    this.TupleKeys.SequenceEqual(input.TupleKeys)
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
                if (this.TupleKeys != null) {
                    hashCode = (hashCode * 9923) + this.TupleKeys.GetHashCode();
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