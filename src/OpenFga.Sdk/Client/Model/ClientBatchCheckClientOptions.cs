using OpenFga.Sdk.Model;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

public interface IClientBatchCheckClientOptions : IClientCheckOptions {
    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    public int? MaxParallelRequests { get; set; }
}

public class ClientBatchCheckClientOptions : IClientBatchCheckClientOptions {
    /// <inheritdoc />
    public string? StoreId { get; set; }

    /// <inheritdoc />
    public string? AuthorizationModelId { get; set; }

    /// <summary>
    ///     Max Requests to issue in parallel
    /// </summary>
    public int? MaxParallelRequests { get; set; }

    /// <inheritdoc />
    public ConsistencyPreference? Consistency { get; set; }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}

