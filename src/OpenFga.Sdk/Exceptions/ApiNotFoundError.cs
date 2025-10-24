using OpenFga.Sdk.Exceptions.Parsers;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Exceptions;

/// <summary>
/// FGA API Not Found Error - Corresponding to HTTP Error Codes 400 and 422
/// </summary>
public class FgaApiNotFoundError : FgaApiError {
    /// <inheritdoc />
    public FgaApiNotFoundError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError) {
    }

    internal new static async Task<FgaApiNotFoundError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new FgaApiNotFoundError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}