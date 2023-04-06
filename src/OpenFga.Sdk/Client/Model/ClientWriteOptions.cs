//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


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
    ///     Max Requests to issue in parallel
    /// </summary>
    int? MaxParallelRequests { get; set; }
}

public interface IClientWriteOptions : IClientRequestOptionsWithAuthZModelId {
    TransactionOpts Transaction { get; set; }
}

public class ClientWriteOptions : IClientWriteOptions {
    /// <summary>
    ///     Override the Authorization Model ID for this request
    /// </summary>
    public string? AuthorizationModelId { get; set; }

    /// <inheritdoc />
    public TransactionOpts Transaction { get; set; }
}