//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

public interface IClientConsistencyOptions {
    /// <summary>
    ///     ConsistencyPreference - Can be used to indicate preference for lower latency or higher consistency
    /// </summary>
    public ConsistencyPreference? Consistency { get; set; }
}