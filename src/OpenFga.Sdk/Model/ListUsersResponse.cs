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
    /// ListUsersResponse
    /// </summary>
    [DataContract(Name = "ListUsersResponse")]
    public partial class ListUsersResponse : IEquatable<ListUsersResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListUsersResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ListUsersResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListUsersResponse" /> class.
        /// </summary>
        /// <param name="users">users (required).</param>
        public ListUsersResponse(List<User> users = default(List<User>)) {
            // to ensure "users" is required (not null)
            if (users == null) {
                throw new ArgumentNullException("users is a required property for ListUsersResponse and cannot be null");
            }
            this.Users = users;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Users
        /// </summary>
        [DataMember(Name = "users", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<User> Users { get; set; }

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
        /// Builds a ListUsersResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ListUsersResponse</returns>
        public static ListUsersResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ListUsersResponse>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((ListUsersResponse)input);
        }

        /// <summary>
        /// Returns true if ListUsersResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ListUsersResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ListUsersResponse input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ListUsersResponse input) {

            if (!IsCollectionPropertyEqual(this.Users, input.Users)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ListUsersResponse input) {
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
                if (this.Users != null) {
                    hashCode = (hashCode * 9923) + this.Users.GetHashCode();
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