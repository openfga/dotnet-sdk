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
    /// TupleToUserset
    /// </summary>
    [DataContract(Name = "TupleToUserset")]
    public partial class TupleToUserset : IEquatable<TupleToUserset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleToUserset" /> class.
        /// </summary>
        [JsonConstructor]
        public TupleToUserset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TupleToUserset" /> class.
        /// </summary>
        /// <param name="tupleset">tupleset.</param>
        /// <param name="computedUserset">computedUserset.</param>
        public TupleToUserset(ObjectRelation tupleset = default(ObjectRelation), ObjectRelation computedUserset = default(ObjectRelation)) {
            this.Tupleset = tupleset;
            this.ComputedUserset = computedUserset;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tupleset
        /// </summary>
        [DataMember(Name = "tupleset", EmitDefaultValue = false)]
        [JsonPropertyName("tupleset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ObjectRelation? Tupleset { get; set; }

        /// <summary>
        /// Gets or Sets ComputedUserset
        /// </summary>
        [DataMember(Name = "computedUserset", EmitDefaultValue = false)]
        [JsonPropertyName("computedUserset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ObjectRelation? ComputedUserset { get; set; }

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
        /// Builds a TupleToUserset from the JSON string presentation of the object
        /// </summary>
        /// <returns>TupleToUserset</returns>
        public static TupleToUserset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TupleToUserset>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as TupleToUserset);
        }

        /// <summary>
        /// Returns true if TupleToUserset instances are equal
        /// </summary>
        /// <param name="input">Instance of TupleToUserset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TupleToUserset input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Tupleset == input.Tupleset ||
                    (this.Tupleset != null &&
                    this.Tupleset.Equals(input.Tupleset))
                ) &&
                (
                    this.ComputedUserset == input.ComputedUserset ||
                    (this.ComputedUserset != null &&
                    this.ComputedUserset.Equals(input.ComputedUserset))
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
                if (this.Tupleset != null) {
                    hashCode = (hashCode * 9923) + this.Tupleset.GetHashCode();
                }
                if (this.ComputedUserset != null) {
                    hashCode = (hashCode * 9923) + this.ComputedUserset.GetHashCode();
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