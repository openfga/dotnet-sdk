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
    /// PathUnknownErrorMessageResponse
    /// </summary>
    [DataContract(Name = "PathUnknownErrorMessageResponse")]
    public partial class PathUnknownErrorMessageResponse : IEquatable<PathUnknownErrorMessageResponse>, IValidatableObject {

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name = "code", EmitDefaultValue = false)]
        [JsonPropertyName("code")]
        public NotFoundErrorCode? Code { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PathUnknownErrorMessageResponse" /> class.
        /// </summary>
        [JsonConstructor]
        public PathUnknownErrorMessageResponse() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathUnknownErrorMessageResponse" /> class.
        /// </summary>
        /// <param name="code">code.</param>
        /// <param name="message">message.</param>
        public PathUnknownErrorMessageResponse(NotFoundErrorCode? code = default(NotFoundErrorCode?), string message = default(string)) {
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
        /// Builds a PathUnknownErrorMessageResponse from the JSON string presentation of the object
        /// </summary>
        /// <returns>PathUnknownErrorMessageResponse</returns>
        public static PathUnknownErrorMessageResponse FromJson(string jsonString) {
            return JsonSerializer.Deserialize<PathUnknownErrorMessageResponse>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as PathUnknownErrorMessageResponse);
        }

        /// <summary>
        /// Returns true if PathUnknownErrorMessageResponse instances are equal
        /// </summary>
        /// <param name="input">Instance of PathUnknownErrorMessageResponse to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PathUnknownErrorMessageResponse input) {
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