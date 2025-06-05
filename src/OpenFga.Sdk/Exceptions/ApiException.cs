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

public class ApiException : Exception {
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorApiException"/> class.
    /// </summary>
    protected ApiException()
        : base() {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    protected ApiException(string message)
        : base(message) {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiException"/> class with a specified error message
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null
    /// reference if no inner exception is specified.</param>
    protected ApiException(string message, Exception innerException)
        : base(message, innerException) {
    }

    /// <inheritdoc />
    protected ApiException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext) {
    }

    /// <summary>
    /// Create an instance of the specific exception related to a particular response.
    /// </summary>
    /// <param name="response"><see cref="HttpResponseMessage"/></param>
    /// <param name="request"><see cref="HttpRequestMessage"/></param>
    /// <param name="apiName"></param>
    /// <returns>An instance of a <see cref="ApiException"/> subclass containing the appropriate exception for this response.</returns>
    public static async Task<ApiException> CreateSpecificExceptionAsync(HttpResponseMessage? response, HttpRequestMessage request,
        string? apiName = null) {
        var statusCode = response?.StatusCode;

        // Handle authentication errors
        if (statusCode == HttpStatusCode.Unauthorized || statusCode == HttpStatusCode.Forbidden) {
            return await FgaApiAuthenticationError.CreateAsync(response, request, apiName).ConfigureAwait(false);
        }

        // Handle validation errors
        if (statusCode == HttpStatusCode.BadRequest ||
            statusCode == HttpStatusCodeExtensions.UnprocessableEntity) {
            return await FgaApiValidationError.CreateAsync(response, request, apiName).ConfigureAwait(false);
        }

        // Handle not found errors
        if (statusCode == HttpStatusCode.NotFound) {
            return await FgaApiNotFoundError.CreateAsync(response, request, apiName).ConfigureAwait(false);
        }

        // Handle rate limit errors
        if (statusCode == HttpStatusCodeExtensions.TooManyRequests) {
            return await FgaApiRateLimitExceededError.CreateAsync(response, request, apiName).ConfigureAwait(false);
        }

        // Handle other errors
        if (statusCode >= HttpStatusCode.InternalServerError && statusCode != HttpStatusCode.NotImplemented) {
            return await FgaApiInternalError.CreateAsync(response, request, apiName).ConfigureAwait(false);
        }

        // Default case
        return await FgaApiError.CreateAsync(response, request, apiName).ConfigureAwait(false);
    }
}