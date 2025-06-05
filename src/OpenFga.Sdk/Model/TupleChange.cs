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
    /// TupleChange
    /// </summary>
    [DataContract(Name = "TupleChange")]
    public partial class TupleChange : IEquatable<TupleChange>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Operation
        /// </summary>
        [DataMember(Name = "operation", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("operation")]
        public TupleOperation Operation { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleChange" /> class.
        /// </summary>
        [JsonConstructor]
        public TupleChange() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TupleChange" /> class.
        /// </summary>
        /// <param name="tupleKey">tupleKey (required).</param>
        /// <param name="operation">operation (required).</param>
        /// <param name="timestamp">timestamp (required).</param>
        public TupleChange(TupleKey tupleKey = default(TupleKey), TupleOperation operation = default(TupleOperation), DateTime timestamp = default(DateTime)) {
            // to ensure "tupleKey" is required (not null)
            if (tupleKey == null) {
                throw new ArgumentNullException("tupleKey is a required property for TupleChange and cannot be null");
            }
            this.TupleKey = tupleKey;
            this.Operation = operation;
            this.Timestamp = timestamp;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TupleKey
        /// </summary>
        [DataMember(Name = "tuple_key", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tuple_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TupleKey TupleKey { get; set; }

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
        public virtual string ToJson() {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a TupleChange from the JSON string presentation of the object
        /// </summary>
        /// <returns>TupleChange</returns>
        public static TupleChange FromJson(string jsonString) {
            return JsonSerializer.Deserialize<TupleChange>(jsonString) ?? throw new InvalidOperationException();
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

            return Equals((TupleChange)input);
        }

        /// <summary>
        /// Returns true if TupleChange instances are equal
        /// </summary>
        /// <param name="input">Instance of TupleChange to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TupleChange input) {
            if (input == null) {
                return false;
            }

            return ArePropertiesEqual(input);
        }

        // Helper methods for property equality
        private bool ArePropertiesEqual(TupleChange input) {

            if (!IsPropertyEqual(this.TupleKey, input.TupleKey)) {
                return false;
            }

            if (!IsPropertyEqual(this.Operation, input.Operation)) {
                return false;
            }

            if (!IsPropertyEqual(this.Timestamp, input.Timestamp)) {
                return false;
            }


            // Check if additional properties are equal
            if (!AreAdditionalPropertiesEqual(input)) {
                return false;
            }

            return true;
        }

        private bool AreAdditionalPropertiesEqual(TupleChange input) {
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
                if (this.TupleKey != null) {
                    hashCode = (hashCode * 9923) + this.TupleKey.GetHashCode();
                }
                hashCode = (hashCode * 9923) + this.Operation.GetHashCode();
                if (this.Timestamp != null) {
                    hashCode = (hashCode * 9923) + this.Timestamp.GetHashCode();
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