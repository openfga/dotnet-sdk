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

internal interface IClientWriteSingleResponse {
    public TupleKey TupleKey { get; set; }
    public ClientWriteStatus Status { get; set; }
    public Exception? Error { get; set; }
}

/// <summary>
///     ClientWriteSingleResponse
/// </summary>
[DataContract(Name = "ClientWriteSingleResponse")]
public class ClientWriteSingleResponse : IClientWriteSingleResponse, IEquatable<ClientWriteSingleResponse>,
    IValidatableObject {
    [DataMember(Name = "tuple_key", EmitDefaultValue = false)]
    [JsonPropertyName("tuple_key")]
    public TupleKey TupleKey { get; set; }

    [DataMember(Name = "status", EmitDefaultValue = false)]
    [JsonPropertyName("status")]
    public ClientWriteStatus Status { get; set; }

    [DataMember(Name = "error", EmitDefaultValue = false)]
    [JsonPropertyName("error")]
    public Exception? Error { get; set; }

    public bool Equals(ClientWriteSingleResponse input) {
        if (input == null) {
            return false;
        }

        return
            (
                TupleKey == input.TupleKey ||
                (TupleKey != null &&
                 TupleKey.Equals(input.TupleKey))
            ) &&
            (
                Status == input.Status ||
                (Status != null &&
                 Status.Equals(input.Status))
            ) &&
            (
                Error == input.Error ||
                (Error != null &&
                 Error.Equals(input.Error))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientWriteSingleResponse FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientWriteSingleResponse>(jsonString) ?? throw new InvalidOperationException();


    public override bool Equals(object input) => Equals(input as ClientWriteSingleResponse);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (TupleKey != null) {
                hashCode = (hashCode * 9923) + TupleKey.GetHashCode();
            }

            if (Status != null) {
                hashCode = (hashCode * 9923) + Status.GetHashCode();
            }

            if (Error != null) {
                hashCode = (hashCode * 9923) + Error.GetHashCode();
            }

            return hashCode;
        }
    }
}

internal interface IClientWriteResponse {
    public List<ClientWriteSingleResponse> Writes { get; set; }
    public List<ClientWriteSingleResponse> Deletes { get; set; }
}

/// <summary>
///     ClientWriteResponse
/// </summary>
[DataContract(Name = "ClientWriteResponse")]
public class ClientWriteResponse : IClientWriteResponse, IEquatable<ClientWriteResponse>, IValidatableObject {
    [DataMember(Name = "writes", EmitDefaultValue = false)]
    [JsonPropertyName("writes")]
    public List<ClientWriteSingleResponse> Writes { get; set; }

    [DataMember(Name = "deletes", EmitDefaultValue = false)]
    [JsonPropertyName("deletes")]
    public List<ClientWriteSingleResponse> Deletes { get; set; }

    public bool Equals(ClientWriteResponse input) {
        if (input == null) {
            return false;
        }

        return
            (
                Writes == input.Writes ||
                (Writes != null &&
                 Writes.Equals(input.Writes))
            ) &&
            (
                Deletes == input.Deletes ||
                (Deletes != null &&
                 Deletes.Equals(input.Deletes))
            );
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public virtual string ToJson() => JsonSerializer.Serialize(this);

    public static ClientWriteResponse FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientWriteResponse>(jsonString) ?? throw new InvalidOperationException();

    public override bool Equals(object input) => Equals(input as ClientWriteResponse);

    public override int GetHashCode() {
        unchecked // Overflow is fine, just wrap
        {
            var hashCode = 9661;
            if (Writes != null) {
                hashCode = (hashCode * 9923) + Writes.GetHashCode();
            }

            if (Deletes != null) {
                hashCode = (hashCode * 9923) + Deletes.GetHashCode();
            }

            return hashCode;
        }
    }
}