using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace OpenFga.Sdk.Client.Model;

internal interface IListRelationsResponse {
    public List<string> Relations { get; set; }
}

/// <summary>
///     ListRelationsResponse
/// </summary>
public class ListRelationsResponse : IListRelationsResponse, IEquatable<ListRelationsResponse>, IValidatableObject {
    public ListRelationsResponse(List<string> relations) {
        Relations = relations;
    }

    public ListRelationsResponse() {
        Relations = new List<string>();
    }

    public bool Equals(ListRelationsResponse input) {
        if (input == null) {
            return false;
        }

        return
            Relations == input.Relations ||
            (Relations != null &&
             Relations.SequenceEqual(input.Relations));
    }

    public List<string> Relations { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public void AddRelation(string relation) => Relations.Add(relation);

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ListRelationsResponse FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ListRelationsResponse>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ListRelationsResponse);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Relations != null) {
                hashCode = (hashCode * 9923) + Relations.GetHashCode();
            }

            return hashCode;
        }
    }
}