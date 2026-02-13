using System;
using System.Text.Json;
using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

/// <inheritdoc />
public class ClientWriteAuthorizationModelRequest : WriteAuthorizationModelRequest {
    /// <summary>
    /// Builds a ClientWriteAuthorizationModelRequest from the JSON string presentation of the object
    /// </summary>
    /// <param name="jsonString">JSON string to deserialize</param>
    /// <returns>ClientWriteAuthorizationModelRequest</returns>
    public static new ClientWriteAuthorizationModelRequest FromJson(string jsonString) {
        return JsonSerializer.Deserialize<ClientWriteAuthorizationModelRequest>(jsonString)
            ?? throw new InvalidOperationException("Failed to deserialize ClientWriteAuthorizationModelRequest from JSON");
    }
}