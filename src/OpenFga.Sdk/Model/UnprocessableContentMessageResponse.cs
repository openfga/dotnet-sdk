//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// UnprocessableContentMessageResponse
    /// </summary>
    [DataContract(Name = "UnprocessableContentMessageResponse")]
    public partial class UnprocessableContentMessageResponse : IEquatable<UnprocessableContentMessageResponse>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        [JsonPropertyName("code")]
        public UnprocessableContentErrorCode? Code { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableContentMessageResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public UnprocessableContentMessageResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnprocessableContentMessageResponse" /> class.
        /// </summary>
        /// <param name="code">code.</param>
        /// <param name="message">message.</param>
        public UnprocessableContentMessageResponse(UnprocessableContentErrorCode? code = default(UnprocessableContentErrorCode?), string message = default(string)) {
            this.Code = code;
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
        /// Builds a UnprocessableContentMessageResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>UnprocessableContentMessageResponse</returns>
        public static UnprocessableContentMessageResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<UnprocessableContentMessageResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as UnprocessableContentMessageResponse);
        }

        /// <summary>
        /// Returns true if UnprocessableContentMessageResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of UnprocessableContentMessageResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UnprocessableContentMessageResponse input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.Code == input.Code ||
                    this.Code.Equals(input.Code)
                ) &&
                (
                    this.Message == input.Message ||
                    (this.Message != null &&
                    this.Message.Equals(input.Message))
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && !this.AdditionalProperties.Except(input.AdditionalProperties).Any());
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 9661;
                hashCode = (hashCode * 9923) + this.Code.GetHashCode();
                if (this.Message != null) {
                    hashCode = (hashCode * 9923) + this.Message.GetHashCode();
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