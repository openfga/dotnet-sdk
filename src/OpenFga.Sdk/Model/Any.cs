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
    /// Any
    /// </summary>
    [DataContract(Name = "Any")]
    public partial class Any : Dictionary<String, Object>, IEquatable<Any>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Any" /> class.
        /// </summary>
        [JsonConstructor]
        public Any() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Any" /> class.
        /// </summary>
        /// <param name="type">type.</param>
        public Any(string type = default(string)) : base() {
            this.Type = type;
        }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "@type", EmitDefaultValue = false)]
        [JsonPropertyName("@type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Type { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson() {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a Any from the JSON string presentation of the object
        /// </summary>
        /// <returns>Any</returns>
        public static Any FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Any>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Any);
        }

        /// <summary>
        /// Returns true if Any instances are equal
        /// </summary>
        /// <param name="input">Instance of Any to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Any input) {
            if (input == null) {
                return false;
            }
            return base.Equals(input) &&
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = base.GetHashCode();
                if (this.Type != null) {
                    hashCode = (hashCode * 9923) + this.Type.GetHashCode();
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