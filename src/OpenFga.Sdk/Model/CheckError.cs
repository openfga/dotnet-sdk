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
    /// CheckError
    /// </summary>
    [DataContract(Name = "CheckError")]
    public partial class CheckError : IEquatable<CheckError>, IValidatableObject {

        /// <summary>
        /// Gets or Sets InputError
        /// </summary>
        [DataMember(Name = "input_error", EmitDefaultValue = false)]
        [JsonPropertyName("input_error")]
        public ErrorCode? InputError { get; set; }

        /// <summary>
        /// Gets or Sets InternalError
        /// </summary>
        [DataMember(Name = "internal_error", EmitDefaultValue = false)]
        [JsonPropertyName("internal_error")]
        public InternalErrorCode? InternalError { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckError" /> class.
        /// </summary>
        [JsonConstructor]
        public CheckError() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckError" /> class.
        /// </summary>
        /// <param name="inputError">inputError.</param>
        /// <param name="internalError">internalError.</param>
        /// <param name="message">message.</param>
        public CheckError(ErrorCode? inputError = default, InternalErrorCode? internalError = default, string message = default) {
            this.InputError = inputError;
            this.InternalError = internalError;
            this.Message = message;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets Message
        /// </summary>
        [DataMember(Name = "message", EmitDefaultValue = false)]
        [JsonPropertyName("message")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Message { get; set; }

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
        /// Builds a CheckError from the JSON string presentation of the object
        /// </summary>
        /// <returns>CheckError</returns>
        public static CheckError FromJson(string jsonString) {
            return JsonSerializer.Deserialize<CheckError>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((CheckError)input);
        }

        /// <summary>
        /// Returns true if CheckError instances are equal
        /// </summary>
        /// <param name="input">Instance of CheckError to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CheckError input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.InputError == input.InputError ||
                    this.InputError.Equals(input.InputError)
                ) &&
                (
                    this.InternalError == input.InternalError ||
                    this.InternalError.Equals(input.InternalError)
                ) &&
                (
                    this.Message == input.Message ||
                    (this.Message != null &&
                    this.Message.Equals(input.Message))
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
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.InputError.GetHashCode();
                hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.InternalError.GetHashCode();
                if (this.Message != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Message.GetHashCode();
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