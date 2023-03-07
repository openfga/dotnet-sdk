namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     TransactionOpts
/// </summary>
public interface TransactionOpts {
    /// <summary>
    ///     Disables full transaction mode (note: if MaxPerChunk > 1, each chunk will be a transaction)
    /// </summary>
    bool Disable { get; set; }

    /// <summary>
    ///     Max number of items to send in a single request (transaction)
    /// </summary>
    int? MaxPerChunk { get; set; }

    /// <summary>
    ///     Time to wait between writing chunks
    /// </summary>
    int? WaitTimeBetweenChunksInMs { get; set; }
}

public interface IClientWriteOptions : IClientRequestOptionsWithAuthZModelId {
    TransactionOpts Transaction { get; set; }
}

public class ClientWriteOptions : IClientWriteOptions {
    /// <inheritdoc />
    public RetryParams? RetryParams { get; set; }

    public Dictionary<string, string>? Headers { get; set; }

    /// <summary>
    ///     Override the Authorization Model ID for this request
    /// </summary>
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public TransactionOpts Transaction { get; set; }
}