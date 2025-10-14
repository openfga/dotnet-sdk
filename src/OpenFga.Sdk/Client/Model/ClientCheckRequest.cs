using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientCheckRequest : IClientQueryContextWrapper {
    public string User { get; set; }
    public string Relation { get; set; }
    public string Object { get; set; }
}

/// <summary>
/// </summary>
public class ClientCheckRequest : IClientCheckRequest, IEquatable<ClientCheckRequest>, IValidatableObject {
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
    ///     Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    ///     Gets or Sets Contextual Tuples
    /// </summary>
    [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
    [JsonPropertyName("contextual_tuples")]
    public List<ClientTupleKey>? ContextualTuples { get; set; }

    /// <summary>
    ///     Gets or Sets Context
    /// </summary>
    [DataMember(Name = "context", EmitDefaultValue = false)]
    [JsonPropertyName("context")]
    public Object? Context { get; set; }

    public bool Equals(ClientCheckRequest input) {
        if (input == null) {
            return false;
        }

        return
            (
                User == input.User ||
                (User != null &&
                 User.Equals(input.User))
            ) &&
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
            ) &&
            (
                this.Context == input.Context ||
                (this.Context != null &&
                this.Context.Equals(input.Context))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientCheckRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientCheckRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientCheckRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Object != null) {
                hashCode = (hashCode * 9923) + Object.GetHashCode();
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

            if (Context != null) {
                hashCode = (hashCode * 9923) + Context.GetHashCode();
            }

            return hashCode;
        }
    }
}