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
public interface ClientRequestOptions {
    /// <summary>
    ///     Custom headers to include with this specific request.
    ///     These headers will be merged with DefaultHeaders from ClientConfiguration.
    ///     If a header key exists in both, the per-request header takes precedence.
    /// </summary>
    IDictionary<string, string>? Headers { get; set; }
}