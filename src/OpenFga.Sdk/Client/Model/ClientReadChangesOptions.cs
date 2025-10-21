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