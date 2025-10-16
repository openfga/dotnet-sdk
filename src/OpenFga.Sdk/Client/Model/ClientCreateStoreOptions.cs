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

public interface IClientCreateStoreOptions : IClientRequestOptions { }

public class ClientCreateStoreOptions : IClientCreateStoreOptions {
    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}