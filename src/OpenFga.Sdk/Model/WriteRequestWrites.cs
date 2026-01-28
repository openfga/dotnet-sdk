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
    /// WriteRequestWrites
    /// </summary>
    [DataContract(Name = "WriteRequestWrites")]
    public partial class WriteRequestWrites : IEquatable<WriteRequestWrites>, IValidatableObject
    {
        /// <summary>
        /// On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition).
        /// </summary>
        /// <value>On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition).</value>
        [JsonConverter(typeof(JsonStringEnumMemberConverter<OnDuplicateEnum>))]
        public enum OnDuplicateEnum
        {
            /// <summary>
            /// Enum Error for value: error
            /// </summary>
            [EnumMember(Value = "error")]
            Error = 1,

            /// <summary>
            /// Enum Ignore for value: ignore
            /// </summary>
            [EnumMember(Value = "ignore")]
            Ignore = 2

        }


        /// <summary>
        /// On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition).
        /// </summary>
        /// <value>On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition).</value>
        [DataMember(Name = "on_duplicate", EmitDefaultValue = false)]
        [JsonPropertyName("on_duplicate")]
        public OnDuplicateEnum? OnDuplicate { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequestWrites" /> class.
        /// </summary>
        [JsonConstructor]
        public WriteRequestWrites()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRequestWrites" /> class.
        /// </summary>
        /// <param name="tupleKeys">tupleKeys (required).</param>
        /// <param name="onDuplicate">On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition). (default to OnDuplicateEnum.Error).</param>
        public WriteRequestWrites(List<TupleKey> tupleKeys = default, OnDuplicateEnum? onDuplicate = OnDuplicateEnum.Error)
        {
            // to ensure "tupleKeys" is required (not null)
            if (tupleKeys == null)
            {
                throw new ArgumentNullException("tupleKeys is a required property for WriteRequestWrites and cannot be null");
            }
            this.TupleKeys = tupleKeys;
            this.OnDuplicate = onDuplicate;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKeys
        /// </summary>
        [DataMember(Name = "tuple_keys", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuple_keys")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TupleKey> TupleKeys { get; set; }

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
        /// Builds a WriteRequestWrites from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteRequestWrites</returns>
        public static WriteRequestWrites FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteRequestWrites>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((WriteRequestWrites)input);
        }

        /// <summary>
        /// Returns true if WriteRequestWrites instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteRequestWrites to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteRequestWrites input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.TupleKeys == input.TupleKeys ||
                    this.TupleKeys != null &&
                    input.TupleKeys != null &&
                    this.TupleKeys.SequenceEqual(input.TupleKeys)
                ) && 
                (
                    this.OnDuplicate == input.OnDuplicate ||
                    this.OnDuplicate.Equals(input.OnDuplicate)
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
                if (this.TupleKeys != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.TupleKeys.GetHashCode();
                }
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.OnDuplicate.GetHashCode();
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
