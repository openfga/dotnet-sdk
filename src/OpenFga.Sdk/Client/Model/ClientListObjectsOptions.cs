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


using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

public interface IClientListObjectsOptions : IClientRequestOptionsWithAuthZModelIdAndConsistency {
}

public class ClientListObjectsOptions : IClientListObjectsOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public ConsistencyPreference? Consistency { get; set; }
}