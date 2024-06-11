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
/// A generic validation error
/// </summary>
public class FgaValidationError : FgaError {
    /// <summary>
    /// Initializes a new instance of the <see cref="FgaValidationError"/> class.
    /// </summary>
    public FgaValidationError() : base() {
    }

    /// <inheritdoc />
    public FgaValidationError(string message) : base(message) {
    }

    /// <inheritdoc />
    public FgaValidationError(string message, Exception innerException) : base(message, innerException) {
    }
}