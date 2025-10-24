using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientListUsersRequest : IClientQueryContextWrapper {
    public FgaObject Object { get; set; }
    public string Relation { get; set; }
    public List<UserTypeFilter> UserFilters { get; set; }
}

/// <summary>
///     ClientListUsersRequest
/// </summary>
public class ClientListUsersRequest : IClientListUsersRequest, IEquatable<ClientListUsersRequest>,
    IValidatableObject {
    /// <summary>
    /// Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public FgaObject Object { get; set; }

    /// <summary>
    /// Gets or Sets Relation
    /// </summary>
    [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Relation { get; set; }

    /// <summary>
    /// Gets or Sets UserFilters
    /// </summary>
    [DataMember(Name = "user_filters", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("user_filters")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<UserTypeFilter> UserFilters { get; set; }

    /// <summary>
    /// Gets or Sets ContextualTuples
    /// </summary>
    [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
    [JsonPropertyName("contextual_tuples")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<ClientTupleKey>? ContextualTuples { get; set; }

    /// <summary>
    /// Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation.
    /// </summary>
    /// <value>Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation.</value>
    [DataMember(Name = "context", EmitDefaultValue = false)]
    [JsonPropertyName("context")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Object? Context { get; set; }

    public bool Equals(ClientListUsersRequest input) {
        if (input == null) {
            return false;
        }

        return
            (
                Object == input.Object ||
                (Object != null &&
                 Object.Equals(input.Object))
            ) &&
            (
                Relation == input.Relation ||
                (Relation != null &&
                 Relation.Equals(input.Relation))
            ) &&
            (
                UserFilters == input.UserFilters ||
                (UserFilters != null &&
                 UserFilters.Equals(input.UserFilters))
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

    public static ClientListUsersRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientListUsersRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientListUsersRequest);

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

            if (UserFilters != null) {
                hashCode = (hashCode * 9923) + UserFilters.GetHashCode();
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