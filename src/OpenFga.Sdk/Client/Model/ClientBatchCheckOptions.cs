using OpenFga.Sdk.Model;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

public interface IClientBatchCheckOptions : IClientCheckOptions {
    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    public int? MaxParallelRequests { get; set; }

    /// <summary>
    ///     Max number of checks to include in a single batch check request
    /// </summary>
    public int? MaxBatchSize { get; set; }
}

public class ClientBatchCheckOptions : IClientBatchCheckOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    public int? MaxParallelRequests { get; set; }

    /// <summary>
    ///     Max number of checks to include in a single batch check request
    /// </summary>
    public int? MaxBatchSize { get; set; }

    /// <inheritdoc />
    public ConsistencyPreference? Consistency { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}