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
using System.Text.Json;

namespace OpenFga.Sdk.Client.Model;

public interface IClientReadChangesRequest {
    string Type { get; set; }
}

public class ClientReadChangesRequest : IClientReadChangesRequest, IEquatable<ClientReadChangesRequest>,
    IValidatableObject {
    public string Type { get; set; }

    public bool Equals(ClientReadChangesRequest input) {
        if (input == null) {
            return false;
        }

        return
            Type == input.Type ||
            (Type != null &&
             Type.Equals(input.Type));
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }


    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientReadChangesRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientReadChangesRequest>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientReadChangesRequest);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Type != null) {
                hashCode = (hashCode * 9923) + Type.GetHashCode();
            }

            return hashCode;
        }
    }
}