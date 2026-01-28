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
    /// BatchCheckResponse
    /// </summary>
    [DataContract(Name = "BatchCheckResponse")]
    public partial class BatchCheckResponse : IEquatable<BatchCheckResponse>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public BatchCheckResponse()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCheckResponse" /> class.
        /// </summary>
        /// <param name="result">map keys are the correlation_id values from the BatchCheckItems in the request.</param>
        public BatchCheckResponse(Dictionary<string, BatchCheckSingleResult> result = default)
        {
            this.Result = result;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// map keys are the correlation_id values from the BatchCheckItems in the request
        /// </summary>
        /// <value>map keys are the correlation_id values from the BatchCheckItems in the request</value>
        [DataMember(Name = "result", EmitDefaultValue = false)]
        [JsonPropertyName("result")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, BatchCheckSingleResult>? Result { get; set; }

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
        /// Builds a BatchCheckResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>BatchCheckResponse</returns>
        public static BatchCheckResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<BatchCheckResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((BatchCheckResponse)input);
        }

        /// <summary>
        /// Returns true if BatchCheckResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of BatchCheckResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BatchCheckResponse input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Result == input.Result ||
                    this.Result != null &&
                    input.Result != null &&
                    this.Result.SequenceEqual(input.Result)
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
                if (this.Result != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Result.GetHashCode();
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
