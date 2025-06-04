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

namespace OpenFga.Sdk.Client.Model;

public interface IClientListRelationsRequest : IClientQueryContextWrapper {
    public string User { get; set; }

    public string Object { get; set; }

    public List<string> Relations { get; set; }
}

/// <summary>
///     ClientListRelationsRequest
/// </summary>
public class ClientListRelationsRequest : IClientListRelationsRequest, IEquatable<ClientListRelationsRequest>, IValidatableObject {
    public ClientListRelationsRequest(List<string> relations) {
        Relations = relations;
    }

    public ClientListRelationsRequest() {
        Relations = new List<string>();
    }

    /// <summary>
    ///     Gets or Sets User
    /// </summary>
    [DataMember(Name = "user", EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    ///     Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    ///     Gets or Sets Relation
    /// </summary>
    [DataMember(Name = "relations", EmitDefaultValue = false)]
    [JsonPropertyName("relations")]
    public List<string> Relations { get; set; }

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

    public bool Equals(ClientListRelationsRequest input) {
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
                Relations == input.Relations ||
                (Relations != null &&
                 Relations.Equals(input.Relations))
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

    public static ClientListRelationsRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientListRelationsRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientListRelationsRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Object != null) {
                hashCode = (hashCode * 9923) + Object.GetHashCode();
            }

            if (Relations != null) {
                hashCode = (hashCode * 9923) + Relations.GetHashCode();
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