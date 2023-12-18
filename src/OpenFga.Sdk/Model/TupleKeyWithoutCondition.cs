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
    /// TupleKeyWithoutCondition
    /// </summary>
    [DataContract(Name = "TupleKeyWithoutCondition")]
    public partial class TupleKeyWithoutCondition : IEquatable<TupleKeyWithoutCondition>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleKeyWithoutCondition" /> class.
        /// </summary>
        [JsonConstructor]
        public TupleKeyWithoutCondition() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TupleKeyWithoutCondition" /> class.
        /// </summary>
        /// <param name="user">user (required).</param>
        /// <param name="relation">relation (required).</param>
        /// <param name="_object">_object (required).</param>
        public TupleKeyWithoutCondition(string user = default(string), string relation = default(string), string _object = default(string)) {
            // to ensure "user" is required (not null)
            if (user == null) {
                throw new ArgumentNullException("user is a required property for TupleKeyWithoutCondition and cannot be null");
            }
            this.User = user;
            // to ensure "relation" is required (not null)
            if (relation == null) {
                throw new ArgumentNullException("relation is a required property for TupleKeyWithoutCondition and cannot be null");
            }
            this.Relation = relation;
            // to ensure "_object" is required (not null)
            if (_object == null) {
                throw new ArgumentNullException("_object is a required property for TupleKeyWithoutCondition and cannot be null");
            }
            this.Object = _object;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets User
        /// </summary>
        [DataMember(Name = "user", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string User { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Relation { get; set; }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Object { get; set; }

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
        /// Builds a TupleKeyWithoutCondition from the JSON string presentation of the object
        /// </summary>
        /// <returns>TupleKeyWithoutCondition</returns>
        public static TupleKeyWithoutCondition FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TupleKeyWithoutCondition>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as TupleKeyWithoutCondition);
        }

        /// <summary>
        /// Returns true if TupleKeyWithoutCondition instances are equal
        /// </summary>
        /// <param name="input">Instance of TupleKeyWithoutCondition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TupleKeyWithoutCondition input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.User == input.User ||
                    (this.User != null &&
                    this.User.Equals(input.User))
                ) &&
                (
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                ) &&
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
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
                if (this.User != null) {
                    hashCode = (hashCode * 9923) + this.User.GetHashCode();
                }
                if (this.Relation != null) {
                    hashCode = (hashCode * 9923) + this.Relation.GetHashCode();
                }
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
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
            // User (string) maxLength
            if (this.User != null && this.User.Length > 512) {
                yield return new ValidationResult("Invalid value for User, length must be less than 512.", new[] { "User" });
            }

            // Relation (string) maxLength
            if (this.Relation != null && this.Relation.Length > 50) {
                yield return new ValidationResult("Invalid value for Relation, length must be less than 50.", new[] { "Relation" });
            }

            // Object (string) maxLength
            if (this.Object != null && this.Object.Length > 256) {
                yield return new ValidationResult("Invalid value for Object, length must be less than 256.", new[] { "Object" });
            }

            yield break;
        }

    }

}