using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAuthorizationModelsOptions - Client Options for ReadAuthorizationModels
/// </summary>
public interface IClientReadAuthorizationModelsOptions : IClientRequestOptionsWithStoreId, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientReadAuthorizationModelsOptions : IClientReadAuthorizationModelsOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}