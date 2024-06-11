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

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// Base FGA Exception
/// </summary>
public class FgaError : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="FgaError"/> class.
    /// </summary>
    public FgaError() : base() {
    }

    /// <inheritdoc />
    public FgaError(string message) : base(message) {
    }

    /// <inheritdoc />
    public FgaError(string message, Exception innerException) : base(message, innerException) {
    }
}