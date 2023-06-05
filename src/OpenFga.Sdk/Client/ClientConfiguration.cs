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


using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;

namespace OpenFga.Sdk.Client;

public class ClientConfiguration : Configuration.Configuration {
    public ClientConfiguration(Configuration.Configuration config) {
        ApiScheme = config.ApiScheme;
        ApiHost = config.ApiHost;
        UserAgent = config.UserAgent;
        Credentials = config.Credentials;
        DefaultHeaders = config.DefaultHeaders;
        StoreId = config.StoreId;
        RetryParams = new RetryParams { MaxRetry = config.MaxRetry, MinWaitInMs = config.MinWaitInMs };
    }

    public ClientConfiguration() { }

    /// <summary>
    ///     Gets or sets the Authorization Model ID.
    /// </summary>
    /// <value>Authorization Model ID.</value>
    public string? AuthorizationModelId { get; set; }

    public RetryParams? RetryParams { get; set; } = new();

    public new void IsValid() {
        base.IsValid();

        if (AuthorizationModelId != null && !IsWellFormedUlidString(AuthorizationModelId)) {
          throw new FgaValidationError("AuthorizationModelId is not in a valid ulid format");
        }
    }
}