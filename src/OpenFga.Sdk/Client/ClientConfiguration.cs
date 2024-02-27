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


using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;
using System.Text.RegularExpressions;

namespace OpenFga.Sdk.Client;

public class ClientConfiguration : Configuration.Configuration {
    public ClientConfiguration(Configuration.Configuration config) {
        ApiScheme = config.ApiScheme;
        ApiHost = config.ApiHost;
        ApiUrl = config.ApiUrl;
        UserAgent = config.UserAgent;
        Credentials = config.Credentials;
        DefaultHeaders = config.DefaultHeaders;
        RetryParams = new RetryParams { MaxRetry = config.MaxRetry, MinWaitInMs = config.MinWaitInMs };
    }

    public ClientConfiguration() { }

    /// <summary>
    ///     Gets or sets the Store  ID.
    /// </summary>
    /// <value>Store ID.</value>
    public string? StoreId { get; set; }

    /// <summary>
    ///     Gets or sets the Authorization Model ID.
    /// </summary>
    /// <value>Authorization Model ID.</value>
    public string? AuthorizationModelId { get; set; }

    public RetryParams? RetryParams { get; set; } = new();

    public new void IsValid() {
        base.IsValid();

        if (StoreId != null && !IsWellFormedUlidString(StoreId)) {
            throw new FgaValidationError("StoreId is not in a valid ulid format");
        }

        if (AuthorizationModelId != null && AuthorizationModelId != "" && !IsWellFormedUlidString(AuthorizationModelId)) {
            throw new FgaValidationError("AuthorizationModelId is not in a valid ulid format");
        }
    }

    /// <summary>
    /// Ensures that a string is in valid [ULID](https://github.com/ulid/spec) format
    /// </summary>
    /// <param name="ulid"></param>
    /// <returns></returns>
    public static bool IsWellFormedUlidString(string ulid) {
        var regex = new Regex("^[0-7][0-9A-HJKMNP-TV-Z]{25}$");
        return regex.IsMatch(ulid);
    }
}