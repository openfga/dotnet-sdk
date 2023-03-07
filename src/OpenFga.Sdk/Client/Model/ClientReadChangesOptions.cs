namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     ClientReadChangesOptions - Client Options for ReadChanges
/// </summary>
public interface IClientReadChangesOptions : ClientRequestOptions, ClientPaginationOptions {
}

/// <inheritdoc />
public class ClientReadChangesOptions : IClientReadChangesOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    /// <inheritdoc />
    public Dictionary<string, string>? Headers { get; set; }

    /// <inheritdoc />
    public int? PageSize { get; set; }

    /// <inheritdoc />
    public string? ContinuationToken { get; set; }
}