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
    /// ListStoresResponse
    /// </summary>
    [DataContract(Name = "ListStoresResponse")]
    public partial class ListStoresResponse : IEquatable<ListStoresResponse>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListStoresResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public ListStoresResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListStoresResponse" /> class.
        /// </summary>
        /// <param name="stores">stores (required).</param>
        /// <param name="continuationToken">The continuation token will be empty if there are no more stores. (required).</param>
        public ListStoresResponse(List<Store> stores = default, string continuationToken = default) {
            // to ensure "stores" is required (not null)
            if (stores == null) {
                throw new ArgumentNullException("stores is a required property for ListStoresResponse and cannot be null");
            }
            this.Stores = stores;
            // to ensure "continuationToken" is required (not null)
            if (continuationToken == null) {
                throw new ArgumentNullException("continuationToken is a required property for ListStoresResponse and cannot be null");
            }
            this.ContinuationToken = continuationToken;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Stores
        /// </summary>
        [DataMember(Name = "stores", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("stores")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Store> Stores { get; set; }

        /// <summary>
        /// The continuation token will be empty if there are no more stores.
        /// </summary>
        /// <value>The continuation token will be empty if there are no more stores.</value>
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
        /// Builds a ListStoresResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>ListStoresResponse</returns>
        public static ListStoresResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<ListStoresResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((ListStoresResponse)input);
        }

        /// <summary>
        /// Returns true if ListStoresResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of ListStoresResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ListStoresResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Stores == input.Stores ||
                    this.Stores != null &&
                    input.Stores != null &&
                    this.Stores.SequenceEqual(input.Stores)
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
                if (this.Stores != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Stores.GetHashCode();
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