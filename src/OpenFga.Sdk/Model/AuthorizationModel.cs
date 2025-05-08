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
    /// AuthorizationModel
    /// </summary>
    [DataContract(Name = "AuthorizationModel")]
    public partial class AuthorizationModel : IEquatable<AuthorizationModel>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationModel" /> class.
        /// </summary>
        [JsonConstructor]
        public AuthorizationModel() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationModel" /> class.
        /// </summary>
        /// <param name="id">id (required).</param>
        /// <param name="schemaVersion">schemaVersion (required).</param>
        /// <param name="typeDefinitions">typeDefinitions (required).</param>
        /// <param name="conditions">conditions.</param>
        public AuthorizationModel(string id = default(string), string schemaVersion = default(string), List<TypeDefinition> typeDefinitions = default(List<TypeDefinition>), Dictionary<string, Condition> conditions = default(Dictionary<string, Condition>)) {
            // to ensure "id" is required (not null)
            if (id == null) {
                throw new ArgumentNullException("id is a required property for AuthorizationModel and cannot be null");
            }
            this.Id = id;
            // to ensure "schemaVersion" is required (not null)
            if (schemaVersion == null) {
                throw new ArgumentNullException("schemaVersion is a required property for AuthorizationModel and cannot be null");
            }
            this.SchemaVersion = schemaVersion;
            // to ensure "typeDefinitions" is required (not null)
            if (typeDefinitions == null) {
                throw new ArgumentNullException("typeDefinitions is a required property for AuthorizationModel and cannot be null");
            }
            this.TypeDefinitions = typeDefinitions;
            this.Conditions = conditions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets SchemaVersion
        /// </summary>
        [DataMember(Name = "schema_version", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("schema_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SchemaVersion { get; set; }

        /// <summary>
        /// Gets or Sets TypeDefinitions
        /// </summary>
        [DataMember(Name = "type_definitions", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type_definitions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TypeDefinition> TypeDefinitions { get; set; }

        /// <summary>
        /// Gets or Sets Conditions
        /// </summary>
        [DataMember(Name = "conditions", EmitDefaultValue = false)]
        [JsonPropertyName("conditions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, Condition>? Conditions { get; set; }

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
        /// Builds a AuthorizationModel from the JSON string presentation of the object
        /// </summary>
        /// <returns>AuthorizationModel</returns>
        public static AuthorizationModel FromJson(string jsonString) {
            return JsonSerializer.Deserialize<AuthorizationModel>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((AuthorizationModel)input);
        }

        /// <summary>
        /// Returns true if AuthorizationModel instances are equal
        /// </summary>
        /// <param name="input">Instance of AuthorizationModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AuthorizationModel input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(AuthorizationModel input) {

            if (!IsPropertyEqual(this.Id, input.Id)) {
                return false;
            }

            if (!IsPropertyEqual(this.SchemaVersion, input.SchemaVersion)) {
                return false;
            }

            if (!IsCollectionPropertyEqual(this.TypeDefinitions, input.TypeDefinitions)) {
                return false;
            }

            if (!IsCollectionPropertyEqual(this.Conditions, input.Conditions)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(AuthorizationModel input) {
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
                if (this.Id != null) {
                    hashCode = (hashCode * 9923) + this.Id.GetHashCode();
                }
                if (this.SchemaVersion != null) {
                    hashCode = (hashCode * 9923) + this.SchemaVersion.GetHashCode();
                }
                if (this.TypeDefinitions != null) {
                    hashCode = (hashCode * 9923) + this.TypeDefinitions.GetHashCode();
                }
                if (this.Conditions != null) {
                    hashCode = (hashCode * 9923) + this.Conditions.GetHashCode();
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