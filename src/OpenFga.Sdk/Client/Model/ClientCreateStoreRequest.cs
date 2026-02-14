using OpenFga.Sdk.Model;
using System;
using System.Text.Json;

namespace OpenFga.Sdk.Client.Model;

/// <inheritdoc />
public class ClientCreateStoreRequest : CreateStoreRequest {
    /// <summary>
    /// Builds a ClientCreateStoreRequest from the JSON string presentation of the object
    /// </summary>
    /// <param name="jsonString">JSON string to deserialize</param>
    /// <returns>ClientCreateStoreRequest</returns>
    public static new ClientCreateStoreRequest FromJson(string jsonString) {
        return JsonSerializer.Deserialize<ClientCreateStoreRequest>(jsonString)
            ?? throw new InvalidOperationException("Failed to deserialize ClientCreateStoreRequest from JSON");
    }
}