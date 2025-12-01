//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Node
    /// </summary>
    [DataContract(Name = "Node")]
    public partial class Node : IEquatable<Node>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        [JsonConstructor]
        public Node() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node" /> class.
        /// </summary>
        /// <param name="name">name (required).</param>
        /// <param name="leaf">leaf.</param>
        /// <param name="difference">difference.</param>
        /// <param name="union">union.</param>
        /// <param name="intersection">intersection.</param>
        public Node(string name = default, Leaf leaf = default, UsersetTreeDifference difference = default, Nodes union = default, Nodes intersection = default) {
            // to ensure "name" is required (not null)
            if (name == null) {
                throw new ArgumentNullException("name is a required property for Node and cannot be null");
            }
            this.Name = name;
            this.Leaf = leaf;
            this.Difference = difference;
            this.Union = union;
            this.Intersection = intersection;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Leaf
        /// </summary>
        [DataMember(Name = "leaf", EmitDefaultValue = false)]
        [JsonPropertyName("leaf")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Leaf? Leaf { get; set; }

        /// <summary>
        /// Gets or Sets Difference
        /// </summary>
        [DataMember(Name = "difference", EmitDefaultValue = false)]
        [JsonPropertyName("difference")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UsersetTreeDifference? Difference { get; set; }

        /// <summary>
        /// Gets or Sets Union
        /// </summary>
        [DataMember(Name = "union", EmitDefaultValue = false)]
        [JsonPropertyName("union")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Nodes? Union { get; set; }

        /// <summary>
        /// Gets or Sets Intersection
        /// </summary>
        [DataMember(Name = "intersection", EmitDefaultValue = false)]
        [JsonPropertyName("intersection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Nodes? Intersection { get; set; }

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
        /// Builds a Node from the JSON string presentation of the object
        /// </summary>
        /// <returns>Node</returns>
        public static Node FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Node>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((Node)input);
        }

        /// <summary>
        /// Returns true if Node instances are equal
        /// </summary>
        /// <param name="input">Instance of Node to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Node input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Name == input.Name ||
                    (this.Name != null &&
                    this.Name.Equals(input.Name))
                ) &&
                (
                    this.Leaf == input.Leaf ||
                    (this.Leaf != null &&
                    this.Leaf.Equals(input.Leaf))
                ) &&
                (
                    this.Difference == input.Difference ||
                    (this.Difference != null &&
                    this.Difference.Equals(input.Difference))
                ) &&
                (
                    this.Union == input.Union ||
                    (this.Union != null &&
                    this.Union.Equals(input.Union))
                ) &&
                (
                    this.Intersection == input.Intersection ||
                    (this.Intersection != null &&
                    this.Intersection.Equals(input.Intersection))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Name != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Name.GetHashCode();
                }
                if (this.Leaf != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Leaf.GetHashCode();
                }
                if (this.Difference != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Difference.GetHashCode();
                }
                if (this.Union != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Union.GetHashCode();
                }
                if (this.Intersection != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Intersection.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AdditionalProperties.GetHashCode();
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