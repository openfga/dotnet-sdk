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
    /// UsersetTreeTupleToUserset
    /// </summary>
    [DataContract(Name = "UsersetTree.TupleToUserset")]
    public partial class UsersetTreeTupleToUserset : IEquatable<UsersetTreeTupleToUserset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeTupleToUserset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        /// <param name="tupleset">tupleset.</param>
        /// <param name="computed">computed.</param>
        public UsersetTreeTupleToUserset(string? tupleset = default(string), List<Computed>? computed = default(List<Computed>)) {
            this.Tupleset = tupleset;
            this.Computed = computed;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tupleset
        /// </summary>
        [DataMember(Name = "tupleset", EmitDefaultValue = false)]
        [JsonPropertyName("tupleset")]
        public string Tupleset { get; set; }

        /// <summary>
        /// Gets or Sets Computed
        /// </summary>
        [DataMember(Name = "computed", EmitDefaultValue = false)]
        [JsonPropertyName("computed")]
        public List<Computed> Computed { get; set; }

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
        /// Builds a UsersetTreeTupleToUserset from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTreeTupleToUserset</returns>
        public static UsersetTreeTupleToUserset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTreeTupleToUserset>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as UsersetTreeTupleToUserset);
        }

        /// <summary>
        /// Returns true if UsersetTreeTupleToUserset instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeTupleToUserset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeTupleToUserset input) {
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
                    this.Computed == input.Computed ||
                    this.Computed != null &&
                    input.Computed != null &&
                    this.Computed.SequenceEqual(input.Computed)
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
                if (this.Computed != null) {
                    hashCode = (hashCode * 9923) + this.Computed.GetHashCode();
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