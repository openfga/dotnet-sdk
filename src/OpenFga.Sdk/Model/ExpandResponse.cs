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
    /// ExpandResponse
    /// </summary>
    [DataContract(Name = "ExpandResponse")]
    public partial class ExpandResponse : IEquatable<ExpandResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ExpandResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpandResponse" /> class.
        /// </summary>
        /// <param name="tree">tree.</param>
        public ExpandResponse(UsersetTree tree = default(UsersetTree)) {
            this.Tree = tree;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tree
        /// </summary>
        [DataMember(Name = "tree", EmitDefaultValue = false)]
        [JsonPropertyName("tree")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UsersetTree? Tree { get; set; }

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
        /// Builds a ExpandResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ExpandResponse</returns>
        public static ExpandResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ExpandResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ExpandResponse);
        }

        /// <summary>
        /// Returns true if ExpandResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ExpandResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ExpandResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Tree == input.Tree ||
                    (this.Tree != null &&
                    this.Tree.Equals(input.Tree))
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
                if (this.Tree != null) {
                    hashCode = (hashCode * 9923) + this.Tree.GetHashCode();
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