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
    /// WriteAuthorizationModelRequest
    /// </summary>
    [DataContract(Name = "WriteAuthorizationModel_request")]
    public partial class WriteAuthorizationModelRequest : IEquatable<WriteAuthorizationModelRequest>, IValidatableObject {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAuthorizationModelRequest" /> class.
        /// </summary>
        [JsonConstructor]
        public WriteAuthorizationModelRequest() {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteAuthorizationModelRequest" /> class.
        /// </summary>
        /// <param name="typeDefinitions">typeDefinitions (required).</param>
        /// <param name="schemaVersion">schemaVersion (required).</param>
        /// <param name="conditions">conditions.</param>
        public WriteAuthorizationModelRequest(List<TypeDefinition> typeDefinitions = default, string schemaVersion = default, Dictionary<string, Condition> conditions = default) {
            // to ensure "typeDefinitions" is required (not null)
            if (typeDefinitions == null) {
                throw new ArgumentNullException("typeDefinitions is a required property for WriteAuthorizationModelRequest and cannot be null");
            }
            this.TypeDefinitions = typeDefinitions;
            // to ensure "schemaVersion" is required (not null)
            if (schemaVersion == null) {
                throw new ArgumentNullException("schemaVersion is a required property for WriteAuthorizationModelRequest and cannot be null");
            }
            this.SchemaVersion = schemaVersion;
            this.Conditions = conditions;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets TypeDefinitions
        /// </summary>
        [DataMember(Name = "type_definitions", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("type_definitions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<TypeDefinition> TypeDefinitions { get; set; }

        /// <summary>
        /// Gets or Sets SchemaVersion
        /// </summary>
        [DataMember(Name = "schema_version", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("schema_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string SchemaVersion { get; set; }

        /// <summary>
        /// Gets or Sets Conditions
        /// </summary>
        [DataMember(Name = "conditions", EmitDefaultValue = false)]
        [JsonPropertyName("conditions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, Condition>? Conditions { get; set; }

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
        /// Builds a WriteAuthorizationModelRequest from the JSON string presentation of the object
        /// </summary>
        /// <returns>WriteAuthorizationModelRequest</returns>
        public static WriteAuthorizationModelRequest FromJson(string jsonString) {
            return JsonSerializer.Deserialize<WriteAuthorizationModelRequest>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input) {
            return this.Equals(input as WriteAuthorizationModelRequest);
        }

        /// <summary>
        /// Returns true if WriteAuthorizationModelRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of WriteAuthorizationModelRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(WriteAuthorizationModelRequest input) {
            if (input == null) {
                return false;
            }
            return
                (
                    this.TypeDefinitions == input.TypeDefinitions ||
                    this.TypeDefinitions != null &&
                    input.TypeDefinitions != null &&
                    this.TypeDefinitions.SequenceEqual(input.TypeDefinitions)
                ) &&
                (
                    this.SchemaVersion == input.SchemaVersion ||
                    (this.SchemaVersion != null &&
                    this.SchemaVersion.Equals(input.SchemaVersion))
                ) &&
                (
                    this.Conditions == input.Conditions ||
                    this.Conditions != null &&
                    input.Conditions != null &&
                    this.Conditions.SequenceEqual(input.Conditions)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.ContainsKey(kv.Key) && Equals(kv.Value, input.AdditionalProperties[kv.Key])));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.TypeDefinitions != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.TypeDefinitions.GetHashCode();
                }
                if (this.SchemaVersion != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.SchemaVersion.GetHashCode();
                }
                if (this.Conditions != null) {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.Conditions.GetHashCode();
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