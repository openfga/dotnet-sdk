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

    public bool Equals(BatchCheckSingleResponse? other) => throw new NotImplementedException();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
        throw new NotImplementedException();
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

    public bool Equals(ClientBatchCheckClientResponse? other) => throw new NotImplementedException();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
        throw new NotImplementedException();

    /// <summary>
    ///     Appends a response to the list of responses
    /// </summary>
    /// <param name="response"></param>
    public void AppendResponse(BatchCheckSingleResponse response) => Responses.Add(response);
}