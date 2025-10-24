using OpenFga.Sdk.Model;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadOptions - Client Options for Read
/// </summary>
public interface IClientReadOptions : IClientRequestOptionsWithStoreId, ClientPaginationOptions, IClientConsistencyOptions {
}

/// <inheritdoc />
public class ClientReadOptions : IClientReadOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }

    /// <inheritdoc />
    public ConsistencyPreference? Consistency { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}