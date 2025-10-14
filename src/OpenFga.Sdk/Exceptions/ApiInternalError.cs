using OpenFga.Sdk.Exceptions.Parsers;
using System.Net.Http;
using System.Threading.Tasks;
namespace OpenFga.Sdk.Exceptions;

public class FgaApiInternalError : FgaApiError {
    public FgaApiInternalError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError, true) {
    }

    internal new static async Task<FgaApiInternalError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new FgaApiInternalError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}