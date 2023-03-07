namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAssertionsOptions - Client Options for ReadAssertions
/// </summary>
public interface IClientWriteAssertionsOptions : ClientRequestOptions, AuthorizationModelIdOptions {
}

/// <inheritdoc />
public class ClientWriteAssertionsOptions : IClientWriteAssertionsOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }
}