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
        public Node(string name = default(string), Leaf leaf = default(Leaf), UsersetTreeDifference difference = default(UsersetTreeDifference), Nodes union = default(Nodes), Nodes intersection = default(Nodes)) {
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
            // Proper type checking in the Equals method - don't use 'as' operator
            if (input == null)
                return false;

            if (ReferenceEquals(this, input))
                return true;

            if (this.GetType() != input.GetType())
                return false;

            return Equals((Node)input);
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

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(Node input) {

            if (!IsPropertyEqual(this.Name, input.Name)) {
                return false;
            }

            if (!IsPropertyEqual(this.Leaf, input.Leaf)) {
                return false;
            }

            if (!IsPropertyEqual(this.Difference, input.Difference)) {
                return false;
            }

            if (!IsPropertyEqual(this.Union, input.Union)) {
                return false;
            }

            if (!IsPropertyEqual(this.Intersection, input.Intersection)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(Node input) {
            if (this.AdditionalProperties.Count != input.AdditionalProperties.Count) {
                return false;
            }

            return !this.AdditionalProperties.Except(input.AdditionalProperties).Any();
        }

        private bool IsPropertyEqual<T>(T thisValue, T otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.Equals(otherValue);
        }

        private bool IsCollectionPropertyEqual<T>(IEnumerable<T> thisValue, IEnumerable<T> otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.SequenceEqual(otherValue);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                if (this.Name != null) {
                    hashCode = (hashCode * 9923) + this.Name.GetHashCode();
                }
                if (this.Leaf != null) {
                    hashCode = (hashCode * 9923) + this.Leaf.GetHashCode();
                }
                if (this.Difference != null) {
                    hashCode = (hashCode * 9923) + this.Difference.GetHashCode();
                }
                if (this.Union != null) {
                    hashCode = (hashCode * 9923) + this.Union.GetHashCode();
                }
                if (this.Intersection != null) {
                    hashCode = (hashCode * 9923) + this.Intersection.GetHashCode();
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