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
    /// UsersetTreeTupleToUserset
    /// </summary>
    [DataContract(Name = "UsersetTree.TupleToUserset")]
    public partial class UsersetTreeTupleToUserset : IEquatable<UsersetTreeTupleToUserset>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        [JsonConstructor]
        public UsersetTreeTupleToUserset() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersetTreeTupleToUserset" /> class.
        /// </summary>
        /// <param name="tupleset">tupleset (required).</param>
        /// <param name="computed">computed (required).</param>
        public UsersetTreeTupleToUserset(string tupleset = default, List<Computed> computed = default) {
            // to ensure "tupleset" is required (not null)
            if (tupleset == null) {
                throw new ArgumentNullException("tupleset is a required property for UsersetTreeTupleToUserset and cannot be null");
            }
            this.Tupleset = tupleset;
            // to ensure "computed" is required (not null)
            if (computed == null) {
                throw new ArgumentNullException("computed is a required property for UsersetTreeTupleToUserset and cannot be null");
            }
            this.Computed = computed;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Tupleset
        /// </summary>
        [DataMember(Name = "tupleset", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("tupleset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Tupleset { get; set; }

        /// <summary>
        /// Gets or Sets Computed
        /// </summary>
        [DataMember(Name = "computed", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("computed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Computed> Computed { get; set; }

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
        /// Builds a UsersetTreeTupleToUserset from the JSON string presentation of the object
        /// </summary>
        /// <returns>UsersetTreeTupleToUserset</returns>
        public static UsersetTreeTupleToUserset FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UsersetTreeTupleToUserset>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as UsersetTreeTupleToUserset);
        }

        /// <summary>
        /// Returns true if UsersetTreeTupleToUserset instances are equal
        /// </summary>
        /// <param name="input">Instance of UsersetTreeTupleToUserset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UsersetTreeTupleToUserset input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Tupleset == input.Tupleset ||
                    (this.Tupleset != null &&
                    this.Tupleset.Equals(input.Tupleset))
                ) &&
                (
                    this.Computed == input.Computed ||
                    this.Computed != null &&
                    input.Computed != null &&
                    this.Computed.SequenceEqual(input.Computed)
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
                if (this.Tupleset != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Tupleset.GetHashCode();
                }
                if (this.Computed != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Computed.GetHashCode();
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