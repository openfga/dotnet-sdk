using OpenFga.Sdk.Model;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

internal interface IClientWriteRequest {
    public List<TupleKey>? Writes { get; set; }
    public List<TupleKey>? Deletes { get; set; }
}

/// <summary>
///     ClientWriteRequest - Wraps around <see cref="WriteRequest" />
/// </summary>
[DataContract(Name = "ClientWriteRequest")]
public class ClientWriteRequest : IClientWriteRequest, IEquatable<ClientWriteRequest>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientWriteRequest" /> class.
    /// </summary>
    /// <param name="writes">writes.</param>
    /// <param name="deletes">deletes.</param>
    public ClientWriteRequest(List<TupleKey>? writes = default, List<TupleKey>? deletes = default) {
        Writes = writes;
        Deletes = deletes;
    }

    /// <summary>
    ///     Gets or Sets Writes
    /// </summary>
    [DataMember(Name = "writes", EmitDefaultValue = false)]
    [JsonPropertyName("writes")]
    public List<TupleKey>? Writes { get; set; }

    /// <summary>
    ///     Gets or Sets Deletes
    /// </summary>
    [DataMember(Name = "deletes", EmitDefaultValue = false)]
    [JsonPropertyName("deletes")]
    public List<TupleKey>? Deletes { get; set; }

    /// <summary>
    ///     Returns true if WriteRequest instances are equal
    /// </summary>
    /// <param name="input">Instance of WriteRequest to be compared</param>
    /// <returns>Boolean</returns>
    public bool Equals(ClientWriteRequest input) {
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

    /// <summary>
    ///     To validate all properties of the instance
    /// </summary>
    /// <param name="validationContext">Validation context</param>
    /// <returns>Validation Result</returns>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }


    /// <summary>
    ///     Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson() => JsonSerializer.Serialize(this);

    /// <summary>
    ///     Builds a WriteRequest from the JSON string presentation of the object
    /// </summary>
    /// <returns>WriteRequest</returns>
    public static ClientWriteRequest FromJson(string jsonString) =>
        JsonSerializer.Deserialize<ClientWriteRequest>(jsonString) ?? throw new InvalidOperationException();

    /// <summary>
    ///     Returns true if objects are equal
    /// </summary>
    /// <param name="input">Object to be compared</param>
    /// <returns>Boolean</returns>
    public override bool Equals(object input) => Equals(input as ClientWriteRequest);

    /// <summary>
    ///     Gets the hash code
    /// </summary>
    /// <returns>Hash code</returns>
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