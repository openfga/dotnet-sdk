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

namespace OpenFga.Sdk.Exceptions;

public class OpenFgaApiInternalError : OpenFgaApiError {
    public OpenFgaApiInternalError(HttpResponseMessage response, HttpRequestMessage request, string? apiName,
        ApiErrorParser? apiError = null) : base(response, request, apiName, apiError) {
    }

    internal new static async Task<OpenFgaApiInternalError> CreateAsync(HttpResponseMessage response, HttpRequestMessage request, string? apiName) {
        return new OpenFgaApiInternalError(response, request, apiName,
            await ApiErrorParser.Parse(response).ConfigureAwait(false));
    }
}