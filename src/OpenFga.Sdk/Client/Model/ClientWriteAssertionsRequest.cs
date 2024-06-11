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


using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientAssertion {
    public string User { get; set; }
    public string Relation { get; set; }
    public string Object { get; set; }
}

/// <summary>
///     ClientAssertion
/// </summary>
[DataContract(Name = "ClientAssertion")]
public class ClientAssertion : IClientAssertion, IEquatable<ClientAssertion>, IValidatableObject {
    /// <summary>
    ///     Gets or Sets Expectation
    /// </summary>
    [DataMember(Name = "expectation", IsRequired = true, EmitDefaultValue = true)]
    [JsonPropertyName("expectation")]
    public bool Expectation { get; set; }

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
    ///     Gets or Sets Object
    /// </summary>
    [DataMember(Name = "object", EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    public bool Equals(ClientAssertion input) {
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
                Expectation == input.Expectation ||
                (Expectation != null &&
                 Expectation.Equals(input.Expectation))
            ) &&
            (
                User == input.User ||
                (User != null &&
                 User.Equals(input.User))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientAssertion FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientAssertion>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientAssertion);

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

            if (Expectation != null) {
                hashCode = (hashCode * 9923) + Expectation.GetHashCode();
            }

            return hashCode;
        }
    }
}