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
using System.Text.Json;

namespace OpenFga.Sdk.Client.Model;
internal interface IClientListRelationsRequest {
    public string User { get; set; }

    public string Object { get; set; }

    public List<ClientTupleKey> ContextualTuples { get; set; }

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

    public bool Equals(ClientListRelationsRequest input) {
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
                Relations == input.Relations ||
                (Relations != null &&
                 Relations.Equals(input.Relations))
            ) &&
            (
                ContextualTuples == input.ContextualTuples ||
                (ContextualTuples != null &&
                 ContextualTuples.Equals(input.ContextualTuples))
            ) &&
            (
                User == input.User ||
                (User != null &&
                 User.Equals(input.User))
            );
    }

    public string User { get; set; }

    public string Object { get; set; }

    public List<ClientTupleKey> ContextualTuples { get; set; }
    public List<string> Relations { get; set; }

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

            return hashCode;
        }
    }
}