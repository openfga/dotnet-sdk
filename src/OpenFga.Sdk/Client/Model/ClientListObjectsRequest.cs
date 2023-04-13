//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientListObjectsRequest: IClientContextualTuplesWrapper {
    public string User { get; set; }
    public string Relation { get; set; }
    public string Type { get; set; }
}

/// <summary>
///     ClientListObjectsRequest
/// </summary>
public class ClientListObjectsRequest : IClientListObjectsRequest, IEquatable<ClientListObjectsRequest>,
    IValidatableObject {
    /// <summary>
    ///     Gets or Sets User
    /// </summary>
    [DataMember(Name = "user", EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    ///     Gets or Sets Relation
    /// </summary>
    [DataMember(Name = "relation", EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    public string Relation { get; set; }

    /// <summary>
    ///     Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    ///     Gets or Sets Contextual Tuples
    /// </summary>
    [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
    [JsonPropertyName("contextual_tuples")]
    public List<ClientTupleKey>? ContextualTuples { get; set; }

    public bool Equals(ClientListObjectsRequest input) {
        if (input == null) {
            return false;
        }

        return
            (
                Type == input.Type ||
                (Type != null &&
                 Type.Equals(input.Type))
            ) &&
            (
                Relation == input.Relation ||
                (Relation != null &&
                 Relation.Equals(input.Relation))
            ) &&
            (
                User == input.User ||
                (User != null &&
                 User.Equals(input.User))
            ) &&
            (
                ContextualTuples == input.ContextualTuples ||
                (ContextualTuples != null &&
                 ContextualTuples.Equals(input.ContextualTuples))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientListObjectsRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientListObjectsRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientListObjectsRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Type != null) {
                hashCode = (hashCode * 9923) + Type.GetHashCode();
            }

            if (Relation != null) {
                hashCode = (hashCode * 9923) + Relation.GetHashCode();
            }

            if (User != null) {
                hashCode = (hashCode * 9923) + User.GetHashCode();
            }

            if (ContextualTuples != null) {
                hashCode = (hashCode * 9923) + ContextualTuples.GetHashCode();
            }

            return hashCode;
        }
    }
}