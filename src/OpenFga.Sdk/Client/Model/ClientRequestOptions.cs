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
///     Base Client Request Options
/// </summary>
public interface IClientRequestOptions {
    /// <summary>
    ///     Custom headers to include with this specific request.
    ///     These headers will be merged with DefaultHeaders from ClientConfiguration.
    ///     If a header key exists in both, the per-request header takes precedence.
    /// </summary>
    IDictionary<string, string>? Headers { get; set; }
}

/// <summary>
///     Base Client Request Options
/// </summary>
public partial class ClientRequestOptions : IClientRequestOptions {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientRequestOptions" /> class.
    /// </summary>
    public ClientRequestOptions() {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientRequestOptions" /> class.
    /// </summary>
    /// <param name="headers">Custom headers to include with this specific request.</param>
    public ClientRequestOptions(IDictionary<string, string>? headers = default) {
        this.Headers = headers;
    }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}