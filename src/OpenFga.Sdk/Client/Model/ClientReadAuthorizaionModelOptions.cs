namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAuthorizationModelOptions - Client Options for ReadAuthorizationModel
/// </summary>
public interface IClientReadAuthorizationModelOptions : ClientRequestOptions, AuthorizationModelIdOptions {
}

/// <inheritdoc />
public class ClientReadAuthorizationModelOptions : IClientReadAuthorizationModelOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }
}