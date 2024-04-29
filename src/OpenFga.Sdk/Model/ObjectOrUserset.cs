//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
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
    /// ObjectOrUserset
    /// </summary>
    [DataContract(Name = "ObjectOrUserset")]
    public partial class ObjectOrUserset : IEquatable<ObjectOrUserset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectOrUserset" /> class.
        /// </summary>
        [JsonConstructor]
        public ObjectOrUserset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectOrUserset" /> class.
        /// </summary>
        /// <param name="_object">_object.</param>
        /// <param name="userset">userset.</param>
        public ObjectOrUserset(FgaObject _object = default(FgaObject), UsersetUser userset = default(UsersetUser)) {
            this.Object = _object;
            this.Userset = userset;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FgaObject? Object { get; set; }

        /// <summary>
        /// Gets or Sets Userset
        /// </summary>
        [DataMember(Name = "userset", EmitDefaultValue = false)]
        [JsonPropertyName("userset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public UsersetUser? Userset { get; set; }

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
        /// Builds a ObjectOrUserset from the JSON string presentation of the object
        /// </summary>
        /// <returns>ObjectOrUserset</returns>
        public static ObjectOrUserset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ObjectOrUserset>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as ObjectOrUserset);
        }

        /// <summary>
        /// Returns true if ObjectOrUserset instances are equal
        /// </summary>
        /// <param name="input">Instance of ObjectOrUserset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ObjectOrUserset input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
                ) &&
                (
                    this.Userset == input.Userset ||
                    (this.Userset != null &&
                    this.Userset.Equals(input.Userset))
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
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
                }
                if (this.Userset != null) {
                    hashCode = (hashCode * 9923) + this.Userset.GetHashCode();
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