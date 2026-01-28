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


using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;


using OpenFga.Sdk.Constants;

namespace OpenFga.Sdk.Model
{
    /// <summary>
    /// Nodes
    /// </summary>
    [DataContract(Name = "Nodes")]
    public partial class Nodes : IEquatable<Nodes>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes" /> class.
        /// </summary>
        [JsonConstructor]
        public Nodes()
        {
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nodes" /> class.
        /// </summary>
        /// <param name="varNodes">varNodes (required).</param>
        public Nodes(List<Node> varNodes = default)
        {
            // to ensure "varNodes" is required (not null)
            if (varNodes == null)
            {
                throw new ArgumentNullException("varNodes is a required property for Nodes and cannot be null");
            }
            this.VarNodes = varNodes;
            this.AdditionalProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or Sets VarNodes
        /// </summary>
        [DataMember(Name = "nodes", IsRequired = true, EmitDefaultValue = false)]
        [JsonPropertyName("nodes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<Node> VarNodes { get; set; }

        /// <summary>
        /// Gets or Sets additional properties
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalProperties { get; set; }


        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Builds a Nodes from the JSON string presentation of the object
        /// </summary>
        /// <returns>Nodes</returns>
        public static Nodes FromJson(string jsonString) {
            return JsonSerializer.Deserialize<Nodes>(jsonString) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            if (input == null || input.GetType() != this.GetType()) return false;
            return this.Equals((Nodes)input);
        }

        /// <summary>
        /// Returns true if Nodes instances are equal
        /// </summary>
        /// <param name="input">Instance of Nodes to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Nodes input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.VarNodes == input.VarNodes ||
                    this.VarNodes != null &&
                    input.VarNodes != null &&
                    this.VarNodes.SequenceEqual(input.VarNodes)
                )
                && (this.AdditionalProperties.Count == input.AdditionalProperties.Count && this.AdditionalProperties.All(kv => input.AdditionalProperties.TryGetValue(kv.Key, out var inputValue) && Equals(kv.Value, inputValue)));
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = FgaConstants.HashCodeBasePrimeNumber;
                if (this.VarNodes != null)
                {
                    hashCode = (hashCode * FgaConstants.HashCodeMultiplierPrimeNumber) + this.VarNodes.GetHashCode();
                }
                if (this.AdditionalProperties != null)
                {
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
        public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }

    }

}
