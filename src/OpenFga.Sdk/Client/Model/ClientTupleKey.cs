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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientTupleKey : IClientTupleKeyWithoutCondition {
    public RelationshipCondition? Condition { get; set; }
}

public interface IClientQueryContextWrapper {
    public List<ClientTupleKey>? ContextualTuples { get; set; }
    public Object? Context { get; set; }
}

public class ClientTupleKey : IClientTupleKey {
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

    /// <summary>
    /// Gets or Sets Condition
    /// </summary>
    [DataMember(Name = "condition", EmitDefaultValue = false)]
    [JsonPropertyName("condition")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public RelationshipCondition? Condition { get; set; }

    public virtual TupleKey ToTupleKey() => new() { User = User, Relation = Relation, Object = Object, Condition = Condition };

    public virtual TupleKeyWithoutCondition ToTupleKeyWithoutCondition() => new() { User = User, Relation = Relation, Object = Object };

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientTupleKey FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientTupleKey>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientTupleKey);

    public bool Equals(ClientTupleKey input) {
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
                Condition == input.Condition ||
                (Condition != null &&
                 Condition.Equals(input.Condition))
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

            if (Condition != null) {
                hashCode = (hashCode * 9923) + Condition.GetHashCode();
            }

            return hashCode;
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }
}