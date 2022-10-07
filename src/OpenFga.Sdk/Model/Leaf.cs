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
    /// A leaf node contains either - a set of users (which may be individual users, or usersets   referencing other relations) - a computed node, which is the result of a computed userset   value in the authorization model - a tupleToUserset nodes, containing the result of expanding   a tupleToUserset value in a authorization model.
    /// </summary>
    [DataContract(Name = "Leaf")]
    public partial class Leaf : IEquatable<Leaf>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf" /> class.
        /// </summary>
        [JsonConstructor]
        public Leaf() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf" /> class.
        /// </summary>
        /// <param name="users">users.</param>
        /// <param name="computed">computed.</param>
        /// <param name="tupleToUserset">tupleToUserset.</param>
        public Leaf(Users users = default(Users), Computed computed = default(Computed), UsersetTreeTupleToUserset tupleToUserset = default(UsersetTreeTupleToUserset)) {
            this.Users = users;
            this.Computed = computed;
            this.TupleToUserset = tupleToUserset;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Users
        /// </summary>
        [DataMember(Name = "users", EmitDefaultValue = false)]
        [JsonPropertyName("users")]
        public Users? Users { get; set; }

        /// <summary>
        /// Gets or Sets Computed
        /// </summary>
        [DataMember(Name = "computed", EmitDefaultValue = false)]
        [JsonPropertyName("computed")]
        public Computed? Computed { get; set; }

        /// <summary>
        /// Gets or Sets TupleToUserset
        /// </summary>
        [DataMember(Name = "tupleToUserset", EmitDefaultValue = false)]
        [JsonPropertyName("tupleToUserset")]
        public UsersetTreeTupleToUserset? TupleToUserset { get; set; }

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
        /// Builds a Leaf from the JSON string presentation of the object
        /// </summary>
        /// <returns>Leaf</returns>
        public static Leaf FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Leaf>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Leaf);
        }

        /// <summary>
        /// Returns true if Leaf instances are equal
        /// </summary>
        /// <param name="input">Instance of Leaf to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Leaf input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Users == input.Users ||
                    (this.Users != null &&
                    this.Users.Equals(input.Users))
                ) &&
                (
                    this.Computed == input.Computed ||
                    (this.Computed != null &&
                    this.Computed.Equals(input.Computed))
                ) &&
                (
                    this.TupleToUserset == input.TupleToUserset ||
                    (this.TupleToUserset != null &&
                    this.TupleToUserset.Equals(input.TupleToUserset))
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
                if (this.Users != null) {
                    hashCode = (hashCode * 9923) + this.Users.GetHashCode();
                }
                if (this.Computed != null) {
                    hashCode = (hashCode * 9923) + this.Computed.GetHashCode();
                }
                if (this.TupleToUserset != null) {
                    hashCode = (hashCode * 9923) + this.TupleToUserset.GetHashCode();
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