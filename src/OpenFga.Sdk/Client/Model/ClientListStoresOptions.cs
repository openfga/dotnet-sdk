namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientListStoresOptions - Client Options for ListStores
/// </summary>
public interface IClientListStoresOptions : ClientRequestOptions, AuthorizationModelIdOptions, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientListStoresOptions : IClientListStoresOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }
}