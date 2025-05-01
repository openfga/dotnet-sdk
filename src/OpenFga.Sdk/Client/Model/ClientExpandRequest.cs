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

            return hashCode;
        }
    }
}