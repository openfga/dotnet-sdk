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


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Client.Model;

[DataContract(Name = "BatchCheckSingleResponse")]
public class BatchCheckSingleResponse : IEquatable<BatchCheckSingleResponse>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="BatchCheckSingleResponse" /> class.
    /// </summary>
    /// <param name="allowed">allowed.</param>
    /// <param name="request"></param>
    /// <param name="error"></param>
    public BatchCheckSingleResponse(bool allowed, ClientCheckRequest request, Exception? error = default) {
        Allowed = allowed;
        Request = request;
        Error = error;
    }

    public BatchCheckSingleResponse() { }

    /// <summary>
    ///     Gets or Sets Allowed
    /// </summary>
    [DataMember(Name = "allowed", EmitDefaultValue = true)]
    [JsonPropertyName("allowed")]
    public bool Allowed { get; set; }

    /// <summary>
    ///     Gets or Sets the Check Request
    /// </summary>
    [DataMember(Name = "request", EmitDefaultValue = true)]
    [JsonPropertyName("request")]
    public ClientCheckRequest Request { get; set; }

    /// <summary>
    ///     Gets or Sets the Check Request
    /// </summary>
    [DataMember(Name = "error", EmitDefaultValue = true)]
    [JsonPropertyName("error")]
    public Exception? Error { get; set; }

    /// <inheritdoc />
    public bool Equals(BatchCheckSingleResponse? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Allowed == other.Allowed &&
               EqualityComparer<ClientCheckRequest>.Default.Equals(Request, other.Request) &&
               (ReferenceEquals(Error, other.Error) || 
                (Error != null && other.Error != null && Error.Message == other.Error.Message));
    }

    /// <inheritdoc />
    public override bool Equals(object obj) {
        if (obj is BatchCheckSingleResponse other) {
            return Equals(other);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            hashCode = (hashCode * 9923) + Allowed.GetHashCode();
            if (Request != null) {
                hashCode = (hashCode * 9923) + Request.GetHashCode();
            }
            if (Error != null) {
                hashCode = (hashCode * 9923) + Error.Message.GetHashCode();
            }
            return hashCode;
        }
    }
}

/// <summary>
///     CheckResponse
/// </summary>
[DataContract(Name = "ClientBatchCheckClientResponse")]
public class ClientBatchCheckClientResponse : IEquatable<ClientBatchCheckClientResponse>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckClientResponse" /> class.
    /// </summary>
    public ClientBatchCheckClientResponse() {
        Responses = new List<BatchCheckSingleResponse>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckClientResponse" /> class.
    /// </summary>
    public ClientBatchCheckClientResponse(List<BatchCheckSingleResponse> responses) {
        Responses = responses;
    }

    [DataMember(Name = "responses", EmitDefaultValue = true)]
    [JsonPropertyName("responses")]
    public List<BatchCheckSingleResponse> Responses { get; set; }

    /// <inheritdoc />
    public bool Equals(ClientBatchCheckClientResponse? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Responses.SequenceEqual(other.Responses);
    }

    /// <inheritdoc />
    public override bool Equals(object obj) {
        if (obj is ClientBatchCheckClientResponse other) {
            return Equals(other);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    /// <summary>
    ///     Appends a response to the list of responses
    /// </summary>
    /// <param name="response"></param>
    public void AppendResponse(BatchCheckSingleResponse response) => Responses.Add(response);

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            if (Responses != null) {
                foreach (var response in Responses) {
                    hashCode = (hashCode * 9923) + (response?.GetHashCode() ?? 0);
                }
            }
            return hashCode;
        }
    }
}