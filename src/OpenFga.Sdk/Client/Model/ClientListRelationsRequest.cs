using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace OpenFga.Sdk.Client.Model;

internal interface IListRelationsRequest {
    public string User { get; set; }

    public string Object { get; set; }

    public List<ClientTupleKey> ContextualTuples { get; set; }

    public List<string> Relations { get; set; }
}

/// <summary>
///     ListRelationsRequest
/// </summary>
public class ListRelationsRequest : IListRelationsRequest, IEquatable<ListRelationsRequest>, IValidatableObject {
    public ListRelationsRequest(List<string> relations) {
        Relations = relations;
    }

    public ListRelationsRequest() {
        Relations = new List<string>();
    }

    public bool Equals(ListRelationsRequest input) {
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

    public static ListRelationsRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ListRelationsRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ListRelationsRequest);

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