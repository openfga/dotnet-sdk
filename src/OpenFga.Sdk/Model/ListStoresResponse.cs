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
        public ListStoresResponse(List<Store> stores = default(List<Store>), string continuationToken = default(string)) {
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
            // Proper type checking in the Equals method - don't use 'as' operator
            if (input == null)
                return false;

            if (ReferenceEquals(this, input))
                return true;

            if (this.GetType() != input.GetType())
                return false;

            return Equals((ListStoresResponse)input);
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

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(ListStoresResponse input) {

            if (!IsCollectionPropertyEqual(this.Stores, input.Stores)) {
                return false;
            }

            if (!IsPropertyEqual(this.ContinuationToken, input.ContinuationToken)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(ListStoresResponse input) {
            if (this.AdditionalProperties.Count != input.AdditionalProperties.Count) {
                return false;
            }

            return !this.AdditionalProperties.Except(input.AdditionalProperties).Any();
        }

        private bool IsPropertyEqual<T>(T thisValue, T otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.Equals(otherValue);
        }

        private bool IsCollectionPropertyEqual<T>(IEnumerable<T> thisValue, IEnumerable<T> otherValue) {
            if (thisValue == null && otherValue == null) {
                return true;
            }

            if (thisValue == null || otherValue == null) {
                return false;
            }

            return thisValue.SequenceEqual(otherValue);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                if (this.Stores != null) {
                    hashCode = (hashCode * 9923) + this.Stores.GetHashCode();
                }
                if (this.ContinuationToken != null) {
                    hashCode = (hashCode * 9923) + this.ContinuationToken.GetHashCode();
                }
                if (this.AdditionalProperties != null) {
                    hashCode = (hashCode * 9923) + this.AdditionalProperties.GetHashCode();
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