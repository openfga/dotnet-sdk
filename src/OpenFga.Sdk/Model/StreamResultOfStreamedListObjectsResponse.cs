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
    /// StreamResultOfStreamedListObjectsResponse
    /// </summary>
    [DataContract(Name = "Stream_result_of_StreamedListObjectsResponse")]
    public partial class StreamResultOfStreamedListObjectsResponse : IEquatable<StreamResultOfStreamedListObjectsResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResultOfStreamedListObjectsResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public StreamResultOfStreamedListObjectsResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResultOfStreamedListObjectsResponse" /> class.
        /// </summary>
        /// <param name="result">result.</param>
        /// <param name="error">error.</param>
        public StreamResultOfStreamedListObjectsResponse(StreamedListObjectsResponse result = default, Status error = default) {
            this.Result = result;
            this.Error = error;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name = "result", EmitDefaultValue = false)]
        [JsonPropertyName("result")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public StreamedListObjectsResponse? Result { get; set; }

        /// <summary>
        /// Gets or Sets Error
        /// </summary>
        [DataMember(Name = "error", EmitDefaultValue = false)]
        [JsonPropertyName("error")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Status? Error { get; set; }

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
        /// Builds a StreamResultOfStreamedListObjectsResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>StreamResultOfStreamedListObjectsResponse</returns>
        public static StreamResultOfStreamedListObjectsResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<StreamResultOfStreamedListObjectsResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as StreamResultOfStreamedListObjectsResponse);
        }

        /// <summary>
        /// Returns true if StreamResultOfStreamedListObjectsResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of StreamResultOfStreamedListObjectsResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StreamResultOfStreamedListObjectsResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Result == input.Result ||
                    (this.Result != null &&
                    this.Result.Equals(input.Result))
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
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.Result != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Result.GetHashCode();
                }
                if (this.Error != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Error.GetHashCode();
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