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
    /// User.  Represents any possible value for a user (subject or principal). Can be a: - Specific user object e.g.: &#39;user:will&#39;, &#39;folder:marketing&#39;, &#39;org:contoso&#39;, ...) - Specific userset (e.g. &#39;group:engineering#member&#39;) - Public-typed wildcard (e.g. &#39;user:*&#39;)  See https://openfga.dev/docs/concepts#what-is-a-user
    /// </summary>
    [DataContract(Name = "User")]
    public partial class User : IEquatable<User>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        [JsonConstructor]
        public User() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User" /> class.
        /// </summary>
        /// <param name="_object">_object.</param>
        /// <param name="userset">userset.</param>
        /// <param name="wildcard">wildcard.</param>
        public User(FgaObject _object = default(FgaObject), UsersetUser userset = default(UsersetUser), TypedWildcard wildcard = default(TypedWildcard)) {
            this.Object = _object;
            this.Userset = userset;
            this.Wildcard = wildcard;
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
        /// Gets or Sets Wildcard
        /// </summary>
        [DataMember(Name = "wildcard", EmitDefaultValue = false)]
        [JsonPropertyName("wildcard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TypedWildcard? Wildcard { get; set; }

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
        /// Builds a User from the JSON string presentation of the object
        /// </summary>
        /// <returns>User</returns>
        public static User FromJson(string jsonString) {
            return JsonSerializer.Deserialize<User>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((User)input);
        }

        /// <summary>
        /// Returns true if User instances are equal
        /// </summary>
        /// <param name="input">Instance of User to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(User input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(User input) {

            if (!IsPropertyEqual(this.Object, input.Object)) {
                return false;
            }

            if (!IsPropertyEqual(this.Userset, input.Userset)) {
                return false;
            }

            if (!IsPropertyEqual(this.Wildcard, input.Wildcard)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(User input) {
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
                if (this.Object != null) {
                    hashCode = (hashCode * 9923) + this.Object.GetHashCode();
                }
                if (this.Userset != null) {
                    hashCode = (hashCode * 9923) + this.Userset.GetHashCode();
                }
                if (this.Wildcard != null) {
                    hashCode = (hashCode * 9923) + this.Wildcard.GetHashCode();
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