using OpenFga.Sdk.Model;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     Behavior for handling duplicate tuple writes
/// </summary>
public enum OnDuplicateWrites {
    /// <summary>
    ///     Return an error when attempting to write a tuple that already exists (default)
    /// </summary>
    Error = 1,

    /// <summary>
    ///     Silently ignore duplicate tuple writes (no-op)
    /// </summary>
    Ignore = 2
}

/// <summary>
///     Behavior for handling missing tuple deletes
/// </summary>
public enum OnMissingDeletes {
    /// <summary>
    ///     Return an error when attempting to delete a tuple that doesn't exist (default)
    /// </summary>
    Error = 1,

    /// <summary>
    ///     Silently ignore missing tuple deletes (no-op)
    /// </summary>
    Ignore = 2
}

/// <summary>
///     ConflictOptions - Controls behavior for duplicate writes and missing deletes
/// </summary>
public interface IConflictOptions {
    /// <summary>
    ///     Controls behavior when writing a tuple that already exists
    /// </summary>
    OnDuplicateWrites? OnDuplicateWrites { get; set; }

    /// <summary>
    ///     Controls behavior when deleting a tuple that doesn't exist
    /// </summary>
    OnMissingDeletes? OnMissingDeletes { get; set; }
}

public class ConflictOptions : IConflictOptions {
    /// <summary>
    ///     Controls behavior when writing a tuple that already exists
    /// </summary>
    public OnDuplicateWrites? OnDuplicateWrites { get; set; }

    /// <summary>
    ///     Controls behavior when deleting a tuple that doesn't exist
    /// </summary>
    public OnMissingDeletes? OnMissingDeletes { get; set; }
}

/// <summary>
///     TransactionOpts
/// </summary>
public interface ITransactionOpts {
    /// <summary>
    ///     Disables full transaction mode (note: if MaxPerChunk > 1, each chunk will be a transaction)
    /// </summary>
    bool Disable { get; set; }

    /// <summary>
    ///     Max number of items to send in a single request (transaction)
    /// </summary>
    int? MaxPerChunk { get; set; }

    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    int? MaxParallelRequests { get; set; }
}

public class TransactionOptions : ITransactionOpts {
    /// <summary>
    ///     Disables full transaction mode (note: if MaxPerChunk > 1, each chunk will be a transaction)
    /// </summary>
    public bool Disable { get; set; }

    /// <summary>
    ///     Max number of items to send in a single request (transaction)
    /// </summary>
    public int? MaxPerChunk { get; set; }

    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    public int? MaxParallelRequests { get; set; }
}

public interface IClientWriteOptions : IClientRequestOptionsWithAuthZModelId {
    ITransactionOpts Transaction { get; set; }
    IConflictOptions? Conflict { get; set; }
}

public class ClientWriteOptions : IClientWriteOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <summary>
    ///     Override the Authorization Model ID for this request
    /// </summary>
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public ITransactionOpts Transaction { get; set; }

    /// <inheritdoc />
    public IConflictOptions? Conflict { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}