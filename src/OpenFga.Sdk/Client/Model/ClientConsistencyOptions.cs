using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

public interface IClientConsistencyOptions {
    /// <summary>
    ///     ConsistencyPreference - Can be used to indicate preference for lower latency or higher consistency
    /// </summary>
    public ConsistencyPreference? Consistency { get; set; }
}