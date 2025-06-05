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


using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

public interface IClientTupleKeyWithoutCondition {
    public string User { get; set; }
    public string Relation { get; set; }
    public string Object { get; set; }
}

public class ClientTupleKeyWithoutCondition : IClientTupleKeyWithoutCondition {
    /// <summary>
    ///     Gets or Sets User
    /// </summary>
    [DataMember(Name = "user", EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    public new string User { get; set; }

    /// <summary>
    ///     Gets or Sets Relation
    /// </summary>
    [DataMember(Name = "relation", EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    public new string Relation { get; set; }

    /// <summary>
    ///     Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public new string Object { get; set; }

    public virtual TupleKey ToTupleKey() => new() { User = User, Relation = Relation, Object = Object };

    public virtual TupleKeyWithoutCondition ToTupleKeyWithoutCondition() => new() { User = User, Relation = Relation, Object = Object };

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientTupleKeyWithoutCondition FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientTupleKeyWithoutCondition>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientTupleKey);

    public bool Equals(ClientTupleKeyWithoutCondition input) {
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
            );
    }

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;

            if (User != null) {
                hashCode = (hashCode * 9923) + User.GetHashCode();
            }

            if (Object != null) {
                hashCode = (hashCode * 9923) + Object.GetHashCode();
            }

            if (Relation != null) {
                hashCode = (hashCode * 9923) + Relation.GetHashCode();
            }

            return hashCode;
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }
}