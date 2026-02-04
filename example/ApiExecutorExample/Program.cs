using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.ApiExecutor;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using System.Net.Http;

namespace ApiExecutorExample;

/// <summary>
/// This example demonstrates how to use the ApiExecutor to make raw HTTP requests
/// to the OpenFGA API without using the SDK's typed methods.
///
/// Prerequisites: Run an OpenFGA server on localhost:8080
///   docker run -p 8080:8080 openfga/openfga:latest run
/// </summary>
class Program {
    static async Task Main(string[] args) {
        Console.WriteLine("=== OpenFGA ApiExecutor Example ===\n");

        // Configure client to connect to local OpenFGA instance
        var config = new ClientConfiguration {
            ApiUrl = "http://localhost:8080"
        };

        using var client = new OpenFgaClient(config);

        try {
            // Example 1: List stores using raw GET request
            await ListStoresExample(client);

            // Example 2: Create a store using raw POST request with typed response
            var storeId = await CreateStoreExample(client);

            // Example 3: Get store details using path parameters
            await GetStoreExample(client, storeId);

            // Example 4: Create an authorization model
            var modelId = await CreateAuthorizationModelExample(client, storeId);

            // Example 5: Write relationship tuples
            await WriteTuplesExample(client, storeId);

            // Example 6: Read relationship tuples
            await ReadTuplesExample(client, storeId);

            // Example 7: Check permissions
            await CheckPermissionExample(client, storeId, modelId);

            // Example 8: Use raw JSON response instead of typed
            await RawJsonResponseExample(client);

            // Example 9: Custom headers
            await CustomHeadersExample(client);

            // Cleanup: Delete the store we created
            await DeleteStoreExample(client, storeId);

            Console.WriteLine("\n=== All examples completed successfully! ===");
        } catch (Exception ex) {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.WriteLine("\nMake sure OpenFGA is running on localhost:8080");
            Console.WriteLine("Run: docker run -p 8080:8080 openfga/openfga:latest run");
            Environment.Exit(1);
        }
    }

    static async Task ListStoresExample(OpenFgaClient client) {
        Console.WriteLine("📋 Example 1: List Stores");
        Console.WriteLine("Making GET request to /stores");

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores")
            .Build();

        var response = await client.GetApiExecutor().SendAsync<ListStoresResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Stores?.Count ?? 0} store(s)");
        Console.WriteLine();
    }

    static async Task<string> CreateStoreExample(OpenFgaClient client) {
        Console.WriteLine("🏪 Example 2: Create Store");
        Console.WriteLine("Making POST request to /stores");

        var storeName = "ApiExecutor-Example-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Body(requestBody)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<CreateStoreResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Store ID: {response.Data.Id}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine();

        return response.Data.Id!;
    }

    static async Task GetStoreExample(OpenFgaClient client, string storeId) {
        Console.WriteLine("🔍 Example 3: Get Store Details");
        Console.WriteLine($"Making GET request to /stores/{{store_id}}");

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores/{store_id}")
            .PathParam("store_id", storeId)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<GetStoreResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine($"   Created At: {response.Data.CreatedAt}");
        Console.WriteLine();
    }

    static async Task<string> CreateAuthorizationModelExample(OpenFgaClient client, string storeId) {
        Console.WriteLine("📝 Example 4: Create Authorization Model");
        Console.WriteLine("Making POST request to /stores/{store_id}/authorization-models");

        var requestBody = new Dictionary<string, object> {
            { "schema_version", "1.1" },
            {
                "type_definitions", new List<Dictionary<string, object>> {
                    new() {
                        { "type", "user" },
                        { "relations", new Dictionary<string, object>() }
                    },
                    new() {
                        { "type", "document" },
                        {
                            "relations", new Dictionary<string, object> {
                                {
                                    "reader", new Dictionary<string, object> {
                                        { "this", new Dictionary<string, object>() }
                                    }
                                },
                                {
                                    "writer", new Dictionary<string, object> {
                                        { "this", new Dictionary<string, object>() }
                                    }
                                }
                            }
                        },
                        {
                            "metadata", new Dictionary<string, object> {
                                {
                                    "relations", new Dictionary<string, object> {
                                        {
                                            "reader", new Dictionary<string, object> {
                                                {
                                                    "directly_related_user_types", new List<Dictionary<string, string>> {
                                                        new() { { "type", "user" } }
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            "writer", new Dictionary<string, object> {
                                                {
                                                    "directly_related_user_types", new List<Dictionary<string, string>> {
                                                        new() { { "type", "user" } }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/authorization-models")
            .PathParam("store_id", storeId)
            .Body(requestBody)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<WriteAuthorizationModelResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Model ID: {response.Data.AuthorizationModelId}");
        Console.WriteLine();

        return response.Data.AuthorizationModelId!;
    }

    static async Task WriteTuplesExample(OpenFgaClient client, string storeId) {
        Console.WriteLine("✍️  Example 5: Write Relationship Tuples");
        Console.WriteLine("Making POST request to /stores/{store_id}/write");

        var requestBody = new Dictionary<string, object> {
            {
                "writes", new Dictionary<string, object> {
                    {
                        "tuple_keys", new List<Dictionary<string, string>> {
                            new() {
                                { "user", "user:alice" },
                                { "relation", "writer" },
                                { "object", "document:roadmap" }
                            },
                            new() {
                                { "user", "user:bob" },
                                { "relation", "reader" },
                                { "object", "document:roadmap" }
                            }
                        }
                    }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/write")
            .PathParam("store_id", storeId)
            .Body(requestBody)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<object>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Tuples written successfully");
        Console.WriteLine();
    }

    static async Task ReadTuplesExample(OpenFgaClient client, string storeId) {
        Console.WriteLine("📖 Example 6: Read Relationship Tuples");
        Console.WriteLine("Making POST request to /stores/{store_id}/read");

        var requestBody = new Dictionary<string, object> {
            {
                "tuple_key", new Dictionary<string, string> {
                    { "object", "document:roadmap" }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/read")
            .PathParam("store_id", storeId)
            .Body(requestBody)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<ReadResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Tuples?.Count ?? 0} tuple(s):");
        if (response.Data.Tuples != null) {
            foreach (var tuple in response.Data.Tuples) {
                Console.WriteLine($"     - {tuple.Key.User} is {tuple.Key.Relation} of {tuple.Key.Object}");
            }
        }
        Console.WriteLine();
    }

    static async Task CheckPermissionExample(OpenFgaClient client, string storeId, string modelId) {
        Console.WriteLine("🔐 Example 7: Check Permission");
        Console.WriteLine("Making POST request to /stores/{store_id}/check");

        var requestBody = new Dictionary<string, object> {
            { "authorization_model_id", modelId },
            {
                "tuple_key", new Dictionary<string, string> {
                    { "user", "user:alice" },
                    { "relation", "writer" },
                    { "object", "document:roadmap" }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/check")
            .PathParam("store_id", storeId)
            .Body(requestBody)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<CheckResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Allowed: {response.Data.Allowed}");
        Console.WriteLine();
    }

    static async Task RawJsonResponseExample(OpenFgaClient client) {
        Console.WriteLine("📄 Example 8: Raw JSON Response");
        Console.WriteLine("Getting response as raw JSON string instead of typed object");

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores")
            .QueryParam("page_size", "5")
            .Build();

        // Use SendAsync without type parameter to get raw JSON string
        var response = await client.GetApiExecutor().SendAsync(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Raw JSON: {response.Data?.Substring(0, Math.Min(100, response.Data.Length))}...");
        Console.WriteLine();
    }

    static async Task CustomHeadersExample(OpenFgaClient client) {
        Console.WriteLine("📨 Example 9: Custom Headers");
        Console.WriteLine("Making request with custom headers");

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores")
            .Header("X-Custom-Header", "example-value")
            .Header("X-Request-ID", Guid.NewGuid().ToString())
            .Build();

        var response = await client.GetApiExecutor().SendAsync<ListStoresResponse>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Custom headers sent successfully");
        Console.WriteLine();
    }

    static async Task DeleteStoreExample(OpenFgaClient client, string storeId) {
        Console.WriteLine("🗑️  Cleanup: Delete Store");
        Console.WriteLine($"Making DELETE request to /stores/{{store_id}}");

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Delete, "/stores/{store_id}")
            .PathParam("store_id", storeId)
            .Build();

        var response = await client.GetApiExecutor().SendAsync<object>(request);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Store deleted successfully");
        Console.WriteLine();
    }
}
