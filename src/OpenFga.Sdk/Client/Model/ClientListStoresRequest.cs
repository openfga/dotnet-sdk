using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

public interface IClientListStoresRequest {
    public string? Name { get; set; }
}

/// <summary>
///     ClientListStoresRequest
/// </summary>
public class ClientListStoresRequest : IClientListStoresRequest, IEquatable<ClientListStoresRequest>,
    IValidatableObject {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", EmitDefaultValue = false)]
    [JsonPropertyName("name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Name { get; set; }

    public bool Equals(ClientListStoresRequest input) {
        if (input == null) {
            return false;
        }

        return
            (
                Name == input.Name ||
                (Name != null &&
                 Name.Equals(input.Name))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientListStoresRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientListStoresRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientListStoresRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Name != null) {
                hashCode = (hashCode * 9923) + Name.GetHashCode();
            }

            return hashCode;
        }
    }
}