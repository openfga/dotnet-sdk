using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientListStoresOptions - Client Options for ListStores
/// </summary>
public interface IClientListStoresOptions : IClientRequestOptions, AuthorizationModelIdOptions, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientListStoresOptions : IClientListStoresOptions {

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}