using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientExpandRequest
/// </summary>
public interface IClientExpandRequest {
    /// <summary>
    ///     Relation
    /// </summary>
    public string Relation { get; set; }

    /// <summary>
    ///     Object
    /// </summary>
    public string Object { get; set; }

    public List<ClientTupleKey>? ContextualTuples { get; set; }
}

/// <inheritdoc />
public class ClientExpandRequest : IClientExpandRequest, IEquatable<ClientExpandRequest>, IValidatableObject {
    /// <inheritdoc />
    [DataMember(Name = "relation", EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    public string Relation { get; set; }

    /// <inheritdoc />
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]

    public string Object { get; set; }

    /// <summary>
    ///     Gets or Sets Contextual Tuples
    /// </summary>
    [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
    [JsonPropertyName("contextual_tuples")]
    public List<ClientTupleKey>? ContextualTuples { get; set; }

    public bool Equals(ClientExpandRequest input) {
        if (input == null) {
            return false;
        }

        return
            (
                Relation == input.Relation ||
                (Relation != null &&
                 Relation.Equals(input.Relation))
            ) &&
            (
                Object == input.Object ||
                (Object != null &&
                 Object.Equals(input.Object))
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

    public static ClientExpandRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientExpandRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientExpandRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Relation != null) {
                hashCode = (hashCode * 9923) + Relation.GetHashCode();
            }

            if (Object != null) {
                hashCode = (hashCode * 9923) + Object.GetHashCode();
            }

            if (ContextualTuples != null) {
                hashCode = (hashCode * 9923) + ContextualTuples.GetHashCode();
            }

            return hashCode;
        }
    }
}