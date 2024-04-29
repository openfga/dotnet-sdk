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
        /// <param name="excludedUsers">excludedUsers (required).</param>
        public ListUsersResponse(List<User> users = default(List<User>), List<ObjectOrUserset> excludedUsers = default(List<ObjectOrUserset>)) {
            // to ensure "users" is required (not null)
            if (users == null) {
                throw new ArgumentNullException("users is a required property for ListUsersResponse and cannot be null");
            }
            this.Users = users;
            // to ensure "excludedUsers" is required (not null)
            if (excludedUsers == null) {
                throw new ArgumentNullException("excludedUsers is a required property for ListUsersResponse and cannot be null");
            }
            this.ExcludedUsers = excludedUsers;
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
        /// Gets or Sets ExcludedUsers
        /// </summary>
        [DataMember(Name = "excluded_users", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("excluded_users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<ObjectOrUserset> ExcludedUsers { get; set; }

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
            return this.Equals(input as ListUsersResponse);
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
            return
                (
                    this.Users == input.Users ||
                    this.Users != null &&
                    input.Users != null &&
                    this.Users.SequenceEqual(input.Users)
                ) &&
                (
                    this.ExcludedUsers == input.ExcludedUsers ||
                    this.ExcludedUsers != null &&
                    input.ExcludedUsers != null &&
                    this.ExcludedUsers.SequenceEqual(input.ExcludedUsers)
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
                if (this.Users != null) {
                    hashCode = (hashCode * 9923) + this.Users.GetHashCode();
                }
                if (this.ExcludedUsers != null) {
                    hashCode = (hashCode * 9923) + this.ExcludedUsers.GetHashCode();
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