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


using OpenFga.Sdk.Exceptions.Parsers;
using System.Net;

namespace OpenFga.Sdk.Exceptions;

public class OpenFgaAuthenticationError : OpenFgaApiError {
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenFgaAuthenticationError"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="apiName">The name of the API called</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.</param>
    public OpenFgaAuthenticationError(string message, string apiName, Exception innerException)
        : base(HttpStatusCode.Unauthorized, message, innerException) {
        ApiName = apiName;
        Method = HttpMethod.Post;
    }

    public OpenFgaAuthenticationError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError) {
    }

    internal new static async Task<OpenFgaAuthenticationError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new OpenFgaAuthenticationError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}