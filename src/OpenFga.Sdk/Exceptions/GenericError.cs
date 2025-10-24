using System;

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