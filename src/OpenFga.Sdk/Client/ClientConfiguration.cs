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


using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;

namespace OpenFga.Sdk.Client;

/// <summary>
/// Class for managing telemetry settings.
/// </summary>
public class Telemetry {
}

/// <summary>
/// Configuration class for the OpenFga.Sdk client.
/// </summary>
public class ClientConfiguration : Configuration.Configuration {
    /// <summary>
    /// Initializes a new instance of the <see cref="ClientConfiguration"/> class with the specified configuration.
    /// </summary>
    /// <param name="config">The base configuration to copy settings from.</param>
    public ClientConfiguration(Configuration.Configuration config) {
        ApiScheme = config.ApiScheme;
        ApiHost = config.ApiHost;
        ApiUrl = config.ApiUrl;
        UserAgent = config.UserAgent;
        Credentials = config.Credentials;
        DefaultHeaders = config.DefaultHeaders;
        Telemetry = config.Telemetry;
        RetryParams = new RetryParams { MaxRetry = config.MaxRetry, MinWaitInMs = config.MinWaitInMs };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientConfiguration"/> class.
    /// </summary>
    public ClientConfiguration() { }

    /// <summary>
    /// Gets or sets the Store ID.
    /// </summary>
    /// <value>The Store ID.</value>
    public string? StoreId { get; set; }

    /// <summary>
    /// Gets or sets the Authorization Model ID.
    /// </summary>
    /// <value>The Authorization Model ID.</value>
    public string? AuthorizationModelId { get; set; }

    /// <summary>
    /// Gets or sets the retry parameters.
    /// </summary>
    /// <value>The retry parameters.</value>
    public RetryParams? RetryParams { get; set; } = new();

    /// <summary>
    /// Ensures the configuration is valid, otherwise throws an error.
    /// </summary>
    /// <exception cref="FgaValidationError">Thrown when the Store ID or Authorization Model ID is not in a valid ULID format.</exception>
    public new void EnsureValid() {
        base.EnsureValid();

        if (StoreId != null && !IsWellFormedUlidString(StoreId)) {
            throw new FgaValidationError("StoreId is not in a valid ulid format");
        }

        if (!string.IsNullOrEmpty(AuthorizationModelId) &&
            !IsWellFormedUlidString(AuthorizationModelId)) {
            throw new FgaValidationError("AuthorizationModelId is not in a valid ulid format");
        }
    }

    /// <summary>
    /// Ensures that a string is in valid [ULID](https://github.com/ulid/spec) format.
    /// </summary>
    /// <param name="ulid">The string to validate as a ULID.</param>
    /// <returns>True if the string is a valid ULID, otherwise false.</returns>
    public static bool IsWellFormedUlidString(string ulid) {
        var regex = new Regex("^[0-7][0-9A-HJKMNP-TV-Z]{25}$");
        return regex.IsMatch(ulid);
    }
}