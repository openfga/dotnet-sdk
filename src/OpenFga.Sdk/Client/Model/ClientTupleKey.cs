using OpenFga.Sdk.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public class ClientTupleKey : TupleKey {
    public ClientTupleKey(string user, string relation, string _object) {
        Object = _object;
        Relation = relation;
        User = user;
    }

    public ClientTupleKey() {
    }

    /// <summary>
    ///     Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public new string Object { get; set; }

    /// <summary>
    ///     Gets or Sets Relation
    /// </summary>
    [DataMember(Name = "relation", EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    public new string Relation { get; set; }

    /// <summary>
    ///     Gets or Sets User
    /// </summary>
    [DataMember(Name = "user", EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    public new string User { get; set; }

    public virtual TupleKey ToTupleKey() => new TupleKey {User = User, Relation = Relation, Object = Object};

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
                User == input.User ||
                (User != null &&
                 User.Equals(input.User))
            );
    }

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

            return hashCode;
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }
}