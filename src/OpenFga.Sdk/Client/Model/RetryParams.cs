namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     Client Retry Params
/// </summary>
public interface IRetryParams {
    /// <summary>
    ///     Max number of times to retry after a request is rate limited
    /// </summary>
    /// <value>MaxRetry</value>
    public int MaxRetry { get; set; }

    /// <summary>
    ///     Minimum time in ms to wait before retrying
    /// </summary>
    /// <value>MinWaitInMs</value>
    public int MinWaitInMs { get; set; }
}

/// <inheritdoc />
public class RetryParams : IRetryParams {
    /// <inheritdoc />
    public int MaxRetry { get; set; }

    /// <inheritdoc />
    public int MinWaitInMs { get; set; }
}