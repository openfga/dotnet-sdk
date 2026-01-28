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
    /// RelationReference represents a relation of a particular object type (e.g. &#39;document#viewer&#39;).
    /// </summary>
    [DataContract(Name = "RelationReference")]
    public partial class RelationReference : IEquatable<RelationReference>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationReference" /> class.
        /// </summary>
        [JsonConstructor]
        public RelationReference()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationReference" /> class.
        /// </summary>
        /// <param name="type">type (required).</param>
        /// <param name="relation">relation.</param>
        /// <param name="wildcard">wildcard.</param>
        /// <param name="condition">The name of a condition that is enforced over the allowed relation..</param>
        public RelationReference(string type = default, string relation = default, Object wildcard = default, string condition = default)
        {
            // to ensure "type" is required (not null)
            if (type == null)
            {
                throw new ArgumentNullException("type is a required property for RelationReference and cannot be null");
            }
            this.Type = type;
            this.Relation = relation;
            this.Wildcard = wildcard;
            this.Condition = condition;
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
        /// Gets or Sets Relation
        /// </summary>
        [DataMember(Name = "relation", EmitDefaultValue = false)]
        [JsonPropertyName("relation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Relation { get; set; }

        /// <summary>
        /// Gets or Sets Wildcard
        /// </summary>
        [DataMember(Name = "wildcard", EmitDefaultValue = false)]
        [JsonPropertyName("wildcard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Object? Wildcard { get; set; }

        /// <summary>
        /// The name of a condition that is enforced over the allowed relation.
        /// </summary>
        /// <value>The name of a condition that is enforced over the allowed relation.</value>
        [DataMember(Name = "condition", EmitDefaultValue = false)]
        [JsonPropertyName("condition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Condition { get; set; }

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
        /// Builds a RelationReference from the JSON string presentation of the object
        /// </summary>
        /// <returns>RelationReference</returns>
        public static RelationReference FromJson(string jsonString) {
            return JsonSerializer.Deserialize<RelationReference>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((RelationReference)input);
        }

        /// <summary>
        /// Returns true if RelationReference instances are equal
        /// </summary>
        /// <param name="input">Instance of RelationReference to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(RelationReference input)
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
                    this.Relation == input.Relation ||
                    (this.Relation != null &&
                    this.Relation.Equals(input.Relation))
                ) && 
                (
                    this.Wildcard == input.Wildcard ||
                    (this.Wildcard != null &&
                    this.Wildcard.Equals(input.Wildcard))
                ) && 
                (
                    this.Condition == input.Condition ||
                    (this.Condition != null &&
                    this.Condition.Equals(input.Condition))
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
                if (this.Relation != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Relation.GetHashCode();
                }
                if (this.Wildcard != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Wildcard.GetHashCode();
                }
                if (this.Condition != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Condition.GetHashCode();
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
