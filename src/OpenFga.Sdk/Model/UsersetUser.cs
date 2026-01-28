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


using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


using OpenFga.Sdk.Constants;

namespace OpenFga.Sdk.Model
{
    /// <summary>
    /// Userset.  A set or group of users, represented in the &#x60;&lt;type&gt;:&lt;id&gt;#&lt;relation&gt;&#x60; format  &#x60;group:fga#member&#x60; represents all members of group FGA, not to be confused by &#x60;group:fga&#x60; which represents the group itself as a specific object.  See: https://openfga.dev/docs/modeling/building-blocks/usersets#what-is-a-userset
    /// </summary>
    [DataContract(Name = "UsersetUser")]
    public partial class UsersetUser : IEquatable<UsersetUser>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetUser" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetUser()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetUser" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="id">id (required).</param>
        /// <param name="relation">relation (required).</param>
        public UsersetUser(string type = default, string id = default, string relation = default)
        {
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for UsersetUser and cannot be null");
            }
            this.Type = type;
            // to ensure "id" is required (not null)
            if (id == null)
            {
                throw new ArgumentNullException("id is a required property for UsersetUser and cannot be null");
            }
            this.Id = id;
            // to ensure "relation" is required (not null)
            if (relation == null)
            {
                throw new ArgumentNullException("relation is a required property for UsersetUser and cannot be null");
            }
            this.Relation = relation;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Relation { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a UsersetUser from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetUser</returns>
        public static UsersetUser FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetUser>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((UsersetUser)input);
        }

        /// <summary>
        /// Returns true if UsersetUser instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetUser to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetUser input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Id == input.Id ||
                    (this.Id != null &&
                    this.Id.Equals(input.Id))
                ) && 
                (
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Type != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Type.GetHashCode();
                }
                if (this.Id != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Id.GetHashCode();
                }
                if (this.Relation != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Relation.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.AdditionalProperties.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

    }

}
