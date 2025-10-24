using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAssertionsOptions - Client Options for ReadAssertions
/// </summary>
public interface IClientReadAssertionsOptions : IClientRequestOptionsWithStoreId, AuthorizationModelIdOptions {
}

/// <inheritdoc />
public class ClientReadAssertionsOptions : IClientReadAssertionsOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}