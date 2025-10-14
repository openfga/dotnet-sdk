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
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
    /// <exception cref="ArgumentException">Thrown when DefaultHeaders contain reserved or invalid headers.</exception>
    public new void EnsureValid() {
        base.EnsureValid();

        if (StoreId != null && !IsWellFormedUlidString(StoreId)) {
            throw new FgaValidationError("StoreId is not in a valid ulid format");
        }

        if (!string.IsNullOrEmpty(AuthorizationModelId) &&
            !IsWellFormedUlidString(AuthorizationModelId)) {
            throw new FgaValidationError("AuthorizationModelId is not in a valid ulid format");
        }

        // Validate that DefaultHeaders don't contain reserved headers
        ValidateHeaders(DefaultHeaders, nameof(DefaultHeaders));
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

    /// <summary>
    ///     Reserved HTTP headers that should not be overridden via custom headers.
    ///     Note: User-Agent is intentionally excluded as the SDK sets a default value
    ///     but allows users to customize it via DefaultHeaders.
    /// </summary>
    private static readonly HashSet<string> ReservedHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
        "Authorization",
        "Content-Type",
        "Content-Length",
        "Host",
        "Accept",
        "Accept-Encoding"
    };

    /// <summary>
    ///     Validates that HTTP headers are safe to use with HTTP requests
    /// </summary>
    /// <param name="headers">The headers dictionary to validate</param>
    /// <param name="paramName">The parameter name for exception messages</param>
    /// <exception cref="ArgumentException">Thrown when headers contain invalid data</exception>
    internal static void ValidateHeaders(IDictionary<string, string>? headers, string paramName = "headers") {
        if (headers == null) {
            return;
        }

        foreach (var header in headers) {
            if (string.IsNullOrWhiteSpace(header.Key)) {
                throw new ArgumentException("Header name cannot be null, empty, or whitespace.", paramName);
            }

            if (header.Value == null) {
                throw new ArgumentException($"Header '{header.Key}' has a null value. Header values cannot be null.", paramName);
            }

            // Prevent HTTP header injection attacks by checking for newline characters
            if (header.Value.Contains("\r") || header.Value.Contains("\n")) {
                throw new ArgumentException(
                    $"Header '{header.Key}' contains invalid characters (CR/LF). Header values cannot contain newline characters as this may lead to header injection vulnerabilities.",
                    paramName);
            }

            // Warn about reserved headers that may cause unexpected behavior
            if (ReservedHeaders.Contains(header.Key)) {
                throw new ArgumentException(
                    $"Header '{header.Key}' is a reserved HTTP header and should not be set via custom headers. " +
                    $"Setting this header may cause authentication failures, request corruption, or other unexpected behavior. " +
                    $"Reserved headers include: {string.Join(", ", ReservedHeaders)}.",
                    paramName);
            }
        }
    }
}