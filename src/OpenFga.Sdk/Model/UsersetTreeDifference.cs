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
    /// UsersetTreeDifference
    /// </summary>
    [DataContract(Name = "UsersetTree.Difference")]
    public partial class UsersetTreeDifference : IEquatable<UsersetTreeDifference>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeDifference() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeDifference" /> class.
        /// </summary>
        /// <param name="_base">_base.</param>
        /// <param name="subtract">subtract.</param>
        public UsersetTreeDifference(Node? _base = default(Node), Node? subtract = default(Node)) {
            this.Base = _base;
            this.Subtract = subtract;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Base
        /// </summary>
        [DataMember(Name = "base", EmitDefaultValue = false)]
        [JsonPropertyName("base")]
        public Node Base { get; set; }

        /// <summary>
        /// Gets or Sets Subtract
        /// </summary>
        [DataMember(Name = "subtract", EmitDefaultValue = false)]
        [JsonPropertyName("subtract")]
        public Node Subtract { get; set; }

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
        /// Builds a UsersetTreeDifference from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTreeDifference</returns>
        public static UsersetTreeDifference FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTreeDifference>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as UsersetTreeDifference);
        }

        /// <summary>
        /// Returns true if UsersetTreeDifference instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeDifference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeDifference input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Base == input.Base ||
                    (this.Base != null &&
                    this.Base.Equals(input.Base))
                ) &&
                (
                    this.Subtract == input.Subtract ||
                    (this.Subtract != null &&
                    this.Subtract.Equals(input.Subtract))
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
                if (this.Base != null) {
                    hashCode = (hashCode * 9923) + this.Base.GetHashCode();
                }
                if (this.Subtract != null) {
                    hashCode = (hashCode * 9923) + this.Subtract.GetHashCode();
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