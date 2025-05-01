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
    /// Computed
    /// </summary>
    [DataContract(Name = "Computed")]
    public partial class Computed : IEquatable<Computed>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Computed" /> class.
        /// </summary>
        [JsonConstructor]
        public Computed() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computed" /> class.
        /// </summary>
        /// <param name="userset">userset (required).</param>
        public Computed(string userset = default(string)) {
            // to ensure "userset" is required (not null)
            if (userset == null) {
                throw new ArgumentNullException("userset is a required property for Computed and cannot be null");
            }
            this.Userset = userset;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Userset
        /// </summary>
        [DataMember(Name = "userset", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("userset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Userset { get; set; }

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
        /// Builds a Computed from the JSON string presentation of the object
        /// </summary>
        /// <returns>Computed</returns>
        public static Computed FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Computed>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Computed);
        }

        /// <summary>
        /// Returns true if Computed instances are equal
        /// </summary>
        /// <param name="input">Instance of Computed to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Computed input) {
            if (input == null) {
                return false;
            }
            return
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