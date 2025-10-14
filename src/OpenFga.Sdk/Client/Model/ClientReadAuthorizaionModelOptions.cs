using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadAuthorizationModelOptions - Client Options for ReadAuthorizationModel
/// </summary>
public interface IClientReadAuthorizationModelOptions : IClientRequestOptionsWithStoreId, AuthorizationModelIdOptions {
}

/// <inheritdoc />
public class ClientReadAuthorizationModelOptions : IClientReadAuthorizationModelOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}