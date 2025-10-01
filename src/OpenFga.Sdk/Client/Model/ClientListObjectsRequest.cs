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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientListObjectsRequest : IClientQueryContextWrapper {
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

    /// <summary>
    ///     Gets or Sets Context
    /// </summary>
    [DataMember(Name = "context", EmitDefaultValue = false)]
    [JsonPropertyName("context")]
    public Object? Context { get; set; }

    public bool Equals(ClientListObjectsRequest input) {
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
                Type == input.Type ||
                (Type != null &&
                 Type.Equals(input.Type))
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

            if (Context != null) {
                hashCode = (hashCode * 9923) + Context.GetHashCode();
            }

            return hashCode;
        }
    }
}