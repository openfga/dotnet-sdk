//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


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
}