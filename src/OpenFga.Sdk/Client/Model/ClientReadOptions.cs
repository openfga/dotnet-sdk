namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadOptions - Client Options for Read
/// </summary>
public interface IClientReadOptions : ClientRequestOptions, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientReadOptions : IClientReadOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }
}