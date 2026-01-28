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
    /// BatchCheckSingleResult
    /// </summary>
    [DataContract(Name = "BatchCheckSingleResult")]
    public partial class BatchCheckSingleResult : IEquatable<BatchCheckSingleResult>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckSingleResult" /> class.
        /// </summary>
        [JsonConstructor]
        public BatchCheckSingleResult()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckSingleResult" /> class.
        /// </summary>
        /// <param name="allowed">allowed.</param>
        /// <param name="error">error.</param>
        public BatchCheckSingleResult(bool allowed = default, CheckError error = default)
        {
            this.Allowed = allowed;
            this.Error = error;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Allowed
        /// </summary>
        [DataMember(Name = "allowed", EmitDefaultValue = true)]
        [JsonPropertyName("allowed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Allowed { get; set; }

        /// <summary>
        /// Gets or Sets Error
        /// </summary>
        [DataMember(Name = "error", EmitDefaultValue = false)]
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CheckError? Error { get; set; }

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
        /// Builds a BatchCheckSingleResult from the JSON string presentation of the object
        /// </summary>
        /// <returns>BatchCheckSingleResult</returns>
        public static BatchCheckSingleResult FromJson(string jsonString) {
            return JsonSerializer.Deserialize<BatchCheckSingleResult>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((BatchCheckSingleResult)input);
        }

        /// <summary>
        /// Returns true if BatchCheckSingleResult instances are equal
        /// </summary>
        /// <param name="input">Instance of BatchCheckSingleResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BatchCheckSingleResult input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Allowed == input.Allowed ||
                    this.Allowed.Equals(input.Allowed)
                ) && 
                (
                    this.Error == input.Error ||
                    (this.Error != null &&
                    this.Error.Equals(input.Error))
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
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Allowed.GetHashCode();
                if (this.Error != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Error.GetHashCode();
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
