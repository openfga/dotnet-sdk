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


using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Users
    /// </summary>
    [DataContract(Name = "Users")]
    public partial class Users : IEquatable<Users>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="Users" /> class.
        /// </summary>
        [JsonConstructor]
        public Users() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Users" /> class.
        /// </summary>
        /// <param name="users">users (required).</param>
        public Users(List<string> users = default(List<string>)) {
            // to ensure "users" is required (not null)
            if (users == null) {
                throw new ArgumentNullException("users is a required property for Users and cannot be null");
            }
            this._Users = users;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets _Users
        /// </summary>
        [DataMember(Name = "users", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("users")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<string> _Users { get; set; }

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
        /// Builds a Users from the JSON string presentation of the object
        /// </summary>
        /// <returns>Users</returns>
        public static Users FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Users>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as Users);
        }

        /// <summary>
        /// Returns true if Users instances are equal
        /// </summary>
        /// <param name="input">Instance of Users to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Users input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this._Users == input._Users ||
                    this._Users != null &&
                    input._Users != null &&
                    this._Users.SequenceEqual(input._Users)
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
                if (this._Users != null) {
                    hashCode = (hashCode * 9923) + this._Users.GetHashCode();
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