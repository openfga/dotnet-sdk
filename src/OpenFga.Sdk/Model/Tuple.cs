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
    /// Tuple
    /// </summary>
    [DataContract(Name = "Tuple")]
    public partial class Tuple : IEquatable<Tuple>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tuple" /> class.
        /// </summary>
        [JsonConstructor]
        public Tuple()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tuple" /> class.
        /// </summary>
        /// <param name="key">key (required).</param>
        /// <param name="timestamp">timestamp (required).</param>
        public Tuple(TupleKey key = default, DateTime timestamp = default)
        {
            // to ensure "key" is required (not null)
            if (key == null)
            {
                throw new ArgumentNullException("key is a required property for Tuple and cannot be null");
            }
            this.Key = key;
            this.Timestamp = timestamp;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Key
        /// </summary>
        [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TupleKey Key { get; set; }

        /// <summary>
        /// Gets or Sets Timestamp
        /// </summary>
        [DataMember(Name = "timestamp", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("timestamp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime Timestamp { get; set; }

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
        /// Builds a Tuple from the JSON string presentation of the object
        /// </summary>
        /// <returns>Tuple</returns>
        public static Tuple FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Tuple>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((Tuple)input);
        }

        /// <summary>
        /// Returns true if Tuple instances are equal
        /// </summary>
        /// <param name="input">Instance of Tuple to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Tuple input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Key == input.Key ||
                    (this.Key != null &&
                    this.Key.Equals(input.Key))
                ) && 
                (
                    this.Timestamp == input.Timestamp ||
                    (this.Timestamp != null &&
                    this.Timestamp.Equals(input.Timestamp))
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
                if (this.Key != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Key.GetHashCode();
                }
                if (this.Timestamp != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Timestamp.GetHashCode();
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
