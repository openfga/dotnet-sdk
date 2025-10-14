using System;

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