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
    /// TupleKey
    /// </summary>
    [DataContract(Name = "TupleKey")]
    public partial class TupleKey : IEquatable<TupleKey>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleKey" /> class.
        /// </summary>
        [JsonConstructor]
        public TupleKey() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TupleKey" /> class.
        /// </summary>
        /// <param name="_object">_object.</param>
        /// <param name="relation">relation.</param>
        /// <param name="user">user.</param>
        public TupleKey(string? _object = default(string), string? relation = default(string), string? user = default(string)) {
            this.Object = _object;
            this.Relation = relation;
            this.User = user;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        public string Object { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        public string Relation { get; set; }

        /// <summary>
        /// Gets or Sets User
        /// </summary>
        [DataMember(Name = "user", EmitDefaultValue = false)]
        [JsonPropertyName("user")]
        public string User { get; set; }

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
        /// Builds a TupleKey from the JSON string presentation of the object
        /// </summary>
        /// <returns>TupleKey</returns>
        public static TupleKey FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TupleKey>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as TupleKey);
        }

        /// <summary>
        /// Returns true if TupleKey instances are equal
        /// </summary>
        /// <param name="input">Instance of TupleKey to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TupleKey input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
                ) &&
                (
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                ) &&
                (
                    this.User == input.User ||
                    (this.User != null &&
                    this.User.Equals(input.User))
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
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
                }
                if (this.User != null) {
                    hashCode = (hashCode * 9923) + this.User.GetHashCode();
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
            // Object (string) maxLength
            if (this.Object != null && this.Object.Length > 256) {
                yield return new ValidationResult("Invalid value for Object, length must be less than 256.", new[] { "Object" });
            }

            // Relation (string) maxLength
            if (this.Relation != null && this.Relation.Length > 50) {
                yield return new ValidationResult("Invalid value for Relation, length must be less than 50.", new[] { "Relation" });
            }

            // User (string) maxLength
            if (this.User != null && this.User.Length > 512) {
                yield return new ValidationResult("Invalid value for User, length must be less than 512.", new[] { "User" });
            }

            yield break;
        }

    }

}