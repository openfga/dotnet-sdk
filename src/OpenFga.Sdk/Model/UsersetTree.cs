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
    /// A UsersetTree contains the result of an Expansion.
    /// </summary>
    [DataContract(Name = "UsersetTree")]
    public partial class UsersetTree : IEquatable<UsersetTree>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTree" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTree() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTree" /> class.
        /// </summary>
        /// <param name="root">root.</param>
        public UsersetTree(Node? root = default(Node)) {
            this.Root = root;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Root
        /// </summary>
        [DataMember(Name = "root", EmitDefaultValue = false)]
        [JsonPropertyName("root")]
        public Node Root { get; set; }

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
        /// Builds a UsersetTree from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTree</returns>
        public static UsersetTree FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTree>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as UsersetTree);
        }

        /// <summary>
        /// Returns true if UsersetTree instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTree to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTree input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Root == input.Root ||
                    (this.Root != null &&
                    this.Root.Equals(input.Root))
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
                if (this.Root != null) {
                    hashCode = (hashCode * 9923) + this.Root.GetHashCode();
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