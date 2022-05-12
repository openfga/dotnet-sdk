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


namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// A generic validation error
/// </summary>
public class OpenFgaValidationError : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenFgaValidationError"/> class.
    /// </summary>
    public OpenFgaValidationError() : base() {
    }

    /// <inheritdoc />
    public OpenFgaValidationError(string message) : base(message) {
    }

    /// <inheritdoc />
    public OpenFgaValidationError(string message, Exception innerException) : base(message, innerException) {
    }
}