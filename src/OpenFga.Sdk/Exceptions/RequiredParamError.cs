//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// An error indicating that a parameter was required but not provided
/// </summary>
public class FgaRequiredParamError : FgaValidationError {

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaRequiredParamError"/> class.
    /// </summary>
    public FgaRequiredParamError()
        : base() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FgaRequiredParamError"/> class with a specified error message.
    /// </summary>
    /// <param name="functionName">The name of the function being called.</param>
    /// <param name="paramName">The name of the problematic param.</param>
    public FgaRequiredParamError(string functionName, string paramName)
        : base($"Required parameter {paramName} was not defined when calling {functionName}.") {
    }
}