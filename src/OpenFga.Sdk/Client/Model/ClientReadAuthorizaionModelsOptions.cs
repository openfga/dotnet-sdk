namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAuthorizationModelsOptions - Client Options for ReadAuthorizationModels
/// </summary>
public interface IClientReadAuthorizationModelsOptions : ClientRequestOptions, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientReadAuthorizationModelsOptions : IClientReadAuthorizationModelsOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }
}