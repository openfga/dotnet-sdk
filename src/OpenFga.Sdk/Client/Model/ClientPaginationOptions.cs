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