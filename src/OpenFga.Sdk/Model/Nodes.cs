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
    /// Nodes
    /// </summary>
    [DataContract(Name = "Nodes")]
    public partial class Nodes : IEquatable<Nodes>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes" /> class.
        /// </summary>
        [JsonConstructor]
        public Nodes() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes" /> class.
        /// </summary>
        /// <param name="nodes">nodes.</param>
        public Nodes(List<Node> nodes = default(List<Node>)) {
            this._Nodes = nodes;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets _Nodes
        /// </summary>
        [DataMember(Name = "nodes", EmitDefaultValue = false)]
        [JsonPropertyName("nodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Node>? _Nodes { get; set; }

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
        /// Builds a Nodes from the JSON string presentation of the object
        /// </summary>
        /// <returns>Nodes</returns>
        public static Nodes FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Nodes>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Nodes);
        }

        /// <summary>
        /// Returns true if Nodes instances are equal
        /// </summary>
        /// <param name="input">Instance of Nodes to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Nodes input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this._Nodes == input._Nodes ||
                    this._Nodes != null &&
                    input._Nodes != null &&
                    this._Nodes.SequenceEqual(input._Nodes)
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
                if (this._Nodes != null) {
                    hashCode = (hashCode * 9923) + this._Nodes.GetHashCode();
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