using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadChangesOptions - Client Options for ReadChanges
/// </summary>
public interface IClientReadChangesOptions : IClientRequestOptionsWithStoreId, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientReadChangesOptions : IClientReadChangesOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}