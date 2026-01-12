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
    /// The response for a StreamedListObjects RPC.
    /// </summary>
    [DataContract(Name = "StreamedListObjectsResponse")]
    public partial class StreamedListObjectsResponse : IEquatable<StreamedListObjectsResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamedListObjectsResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public StreamedListObjectsResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamedListObjectsResponse" /> class.
        /// </summary>
        /// <param name="varObject">varObject (required).</param>
        public StreamedListObjectsResponse(string varObject = default) {
            // to ensure "varObject" is required (not null)
            if (varObject == null) {
                throw new ArgumentNullException("varObject is a required property for StreamedListObjectsResponse and cannot be null");
            }
            this.Object = varObject;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Object
        /// </summary>
        [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("object")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Object { get; set; }

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
        /// Builds a StreamedListObjectsResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>StreamedListObjectsResponse</returns>
        public static StreamedListObjectsResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<StreamedListObjectsResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((StreamedListObjectsResponse)input);
        }

        /// <summary>
        /// Returns true if StreamedListObjectsResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of StreamedListObjectsResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(StreamedListObjectsResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Object == input.Object ||
                    (this.Object != null &&
                    this.Object.Equals(input.Object))
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
                if (this.Object != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Object.GetHashCode();
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