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
    /// RelationshipCondition
    /// </summary>
    [DataContract(Name = "RelationshipCondition")]
    public partial class RelationshipCondition : IEquatable<RelationshipCondition>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipCondition" /> class.
        /// </summary>
        [JsonConstructor]
        public RelationshipCondition() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationshipCondition" /> class.
        /// </summary>
        /// <param name="name">A reference (by name) of the relationship condition defined in the authorization model. (required).</param>
        /// <param name="context">Additional context/data to persist along with the condition. The keys must match the parameters defined by the condition, and the value types must match the parameter type definitions..</param>
        public RelationshipCondition(string name = default(string), Object context = default(Object)) {
            // to ensure "name" is required (not null)
            if (name == null) {
                throw new ArgumentNullException("name is a required property for RelationshipCondition and cannot be null");
            }
            this.Name = name;
            this.Context = context;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// A reference (by name) of the relationship condition defined in the authorization model.
        /// </summary>
        /// <value>A reference (by name) of the relationship condition defined in the authorization model.</value>
        [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// Additional context/data to persist along with the condition. The keys must match the parameters defined by the condition, and the value types must match the parameter type definitions.
        /// </summary>
        /// <value>Additional context/data to persist along with the condition. The keys must match the parameters defined by the condition, and the value types must match the parameter type definitions.</value>
        [DataMember(Name = "context", EmitDefaultValue = false)]
        [JsonPropertyName("context")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? Context { get; set; }

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
        /// Builds a RelationshipCondition from the JSON string presentation of the object
        /// </summary>
        /// <returns>RelationshipCondition</returns>
        public static RelationshipCondition FromJson(string jsonString) {
            return JsonSerializer.Deserialize<RelationshipCondition>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as RelationshipCondition);
        }

        /// <summary>
        /// Returns true if RelationshipCondition instances are equal
        /// </summary>
        /// <param name="input">Instance of RelationshipCondition to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RelationshipCondition input) {
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
                    this.Context == input.Context ||
                    (this.Context != null &&
                    this.Context.Equals(input.Context))
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
                if (this.Name != null) {
                    hashCode = (hashCode * 9923) + this.Name.GetHashCode();
                }
                if (this.Context != null) {
                    hashCode = (hashCode * 9923) + this.Context.GetHashCode();
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
            // Name (string) maxLength
            if (this.Name != null && this.Name.Length > 256) {
                yield return new ValidationResult("Invalid value for Name, length must be less than 256.", new[] { "Name" });
            }

            yield break;
        }

    }

}