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


using OpenFga.Sdk.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// ReadResponse
    /// </summary>
    [DataContract(Name = "ReadResponse")]
    public partial class ReadResponse : IEquatable<ReadResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ReadResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadResponse" /> class.
        /// </summary>
        /// <param name="tuples">tuples (required).</param>
        /// <param name="continuationToken">The continuation token will be empty if there are no more tuples. (required).</param>
        public ReadResponse(List<Tuple> tuples = default, string continuationToken = default) {
            // to ensure "tuples" is required (not null)
            if (tuples == null) {
                throw new ArgumentNullException("tuples is a required property for ReadResponse and cannot be null");
            }
            this.Tuples = tuples;
            // to ensure "continuationToken" is required (not null)
            if (continuationToken == null) {
                throw new ArgumentNullException("continuationToken is a required property for ReadResponse and cannot be null");
            }
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tuples
        /// </summary>
        [DataMember(Name = "tuples", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuples")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Tuple> Tuples { get; set; }

        /// <summary>
        /// The continuation token will be empty if there are no more tuples.
        /// </summary>
        /// <value>The continuation token will be empty if there are no more tuples.</value>
        [DataMember(Name = "continuation_token", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("continuation_token")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ContinuationToken { get; set; }

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
        /// Builds a ReadResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ReadResponse</returns>
        public static ReadResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ReadResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((ReadResponse)input);
        }

        /// <summary>
        /// Returns true if ReadResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ReadResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReadResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Tuples == input.Tuples ||
                    this.Tuples != null &&
                    input.Tuples != null &&
                    this.Tuples.SequenceEqual(input.Tuples)
                ) &&
                (
                    this.ContinuationToken == input.ContinuationToken ||
                    (this.ContinuationToken != null &&
                    this.ContinuationToken.Equals(input.ContinuationToken))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Tuples != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Tuples.GetHashCode();
                }
                if (this.ContinuationToken != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.ContinuationToken.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            yield break;
        }

    }

}