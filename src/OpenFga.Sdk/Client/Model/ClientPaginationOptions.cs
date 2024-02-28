//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientPaginationOptions - Pagination specific options
/// </summary>
public interface ClientPaginationOptions {
    /// <summary>
    ///     PageSize - The number of results to return in each page
    /// </summary>
    int? PageSize { get; set; }

    /// <summary>
    ///     ContinuationToken - The continuation token from the last response
    /// </summary>
    string? ContinuationToken { get; set; }
}