namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     Base Client Request Options
/// </summary>
public interface ClientRequestOptions {
    /// <summary>
    ///     RetryParams - Retry parameter overrides
    /// </summary>
    RetryParams? RetryParams { get; set; }

    /// <summary>
    ///     Headers - Headers to send alongside the request
    /// </summary>
    Dictionary<string, string>? Headers { get; set; }
}