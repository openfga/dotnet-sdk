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


using OpenFga.Sdk.Exceptions.Parsers;
using System.Net;

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// FGA API Authentication Error - Corresponding to HTTP Error Codes 401 and 403
/// </summary>
public class FgaApiAuthenticationError : FgaApiError {
    /// <summary>
    /// Initializes a new instance of the <see cref="FgaApiAuthenticationError"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="apiName">The name of the API called</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.</param>
    public FgaApiAuthenticationError(string message, string apiName, Exception innerException)
        : base(HttpStatusCode.Unauthorized, message, innerException) {
        ApiName = apiName;
        Method = HttpMethod.Post;
    }

    public FgaApiAuthenticationError(HttpResponseMessage? response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError) {
    }

    internal new static async Task<FgaApiAuthenticationError> CreateAsync(HttpResponseMessage? response, HttpRequestMessage request, string? apiName) {
        return new FgaApiAuthenticationError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}