using OpenFga.Sdk.Exceptions.Parsers;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// FGA API Validation Error - Corresponding to HTTP Error Codes 400 and 422
/// </summary>
public class FgaApiValidationError : FgaApiError {
    /// <inheritdoc />
    public FgaApiValidationError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError) {
    }

    internal new static async Task<FgaApiValidationError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new FgaApiValidationError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}