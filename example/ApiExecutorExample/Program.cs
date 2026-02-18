using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using System.Net.Http;
using FgaApiClient = OpenFga.Sdk.ApiClient.ApiClient;

namespace ApiExecutorExample;

/// <summary>
/// This example demonstrates how to use ApiClient.ExecuteAsync to make custom HTTP requests
/// to the OpenFGA API with full response details (status code, headers, raw response, typed data).
///
/// Prerequisites: Run an OpenFGA server on localhost:8080
///   docker run -p 8080:8080 openfga/openfga:latest run
/// </summary>
class Program {
    static async Task Main(string[] args) {
        Console.WriteLine("=== OpenFGA Custom API Requests Example ===\n");

        // Configure client to connect to local OpenFGA instance
        var config = new ClientConfiguration {
            ApiUrl = "http://localhost:8080"
        };

        using var client = new OpenFgaClient(config);
        var apiClient = client.GetApiClient(); // Get the ApiClient for custom requests

        try {
            // Example 1: List stores using raw GET request
            await ListStoresExample(apiClient, config.ApiUrl);

            // Example 2: Create a store using raw POST request with typed response
            var storeId = await CreateStoreExample(apiClient, config.ApiUrl);

            // Example 3: Get store details using path parameters
            await GetStoreExample(apiClient, config.ApiUrl, storeId);

            // Example 4: Create an authorization model
            var modelId = await CreateAuthorizationModelExample(apiClient, config.ApiUrl, storeId);

            // Example 5: Write relationship tuples
            await WriteTuplesExample(apiClient, config.ApiUrl, storeId);

            // Example 6: Read relationship tuples
            await ReadTuplesExample(apiClient, config.ApiUrl, storeId);

            // Example 7: Check permissions
            await CheckPermissionExample(apiClient, config.ApiUrl, storeId, modelId);

            // Example 8: Use raw JSON response instead of typed
            await RawJsonResponseExample(apiClient, config.ApiUrl);

            // Example 9: Custom headers
            await CustomHeadersExample(apiClient, config.ApiUrl);

            // Example 10: Fluent API for building requests
            await FluentApiExample(apiClient, config.ApiUrl);

            // Cleanup: Delete the store we created
            await DeleteStoreExample(apiClient, config.ApiUrl, storeId);

            Console.WriteLine("\n=== All examples completed successfully! ===");
        } catch (Exception ex) {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.WriteLine("\nMake sure OpenFGA is running on localhost:8080");
            Console.WriteLine("Run: docker run -p 8080:8080 openfga/openfga:latest run");
            Environment.Exit(1);
        }
    }

    static async Task ListStoresExample(FgaApiClient apiClient, string basePath) {
        Console.WriteLine("📋 Example 1: List Stores");
        Console.WriteLine("Making GET request to /stores");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await apiClient.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Is Successful: {response.IsSuccessful}");
        Console.WriteLine($"   Found {response.Data.Stores?.Count ?? 0} store(s)");
        Console.WriteLine();
    }

    static async Task<string> CreateStoreExample(FgaApiClient apiClient, string basePath) {
        Console.WriteLine("🏪 Example 2: Create Store");
        Console.WriteLine("Making POST request to /stores");

        var storeName = "ApiExecutor-Example-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        var request = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        var response = await apiClient.ExecuteAsync<object, CreateStoreResponse>(request, "CreateStore");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Store ID: {response.Data.Id}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine($"   Raw Response Length: {response.RawResponse.Length} chars");
        Console.WriteLine();

        return response.Data.Id!;
    }

    static async Task GetStoreExample(FgaApiClient apiClient, string basePath, string storeId) {
        Console.WriteLine("🔍 Example 3: Get Store Details");
        Console.WriteLine($"Making GET request to /stores/{{store_id}}");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await apiClient.ExecuteAsync<Any, GetStoreResponse>(request, "GetStore");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine($"   Created At: {response.Data.CreatedAt}");
        Console.WriteLine($"   Response Headers: {response.Headers.Count}");
        Console.WriteLine();
    }

    static async Task<string> CreateAuthorizationModelExample(FgaApiClient apiClient, string basePath, string storeId) {
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

        var request = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}/authorization-models",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        var response = await apiClient.ExecuteAsync<object, WriteAuthorizationModelResponse>(request, "WriteAuthorizationModel");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Model ID: {response.Data.AuthorizationModelId}");
        Console.WriteLine();

        return response.Data.AuthorizationModelId!;
    }

    static async Task WriteTuplesExample(FgaApiClient apiClient, string basePath, string storeId) {
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

        var request = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}/write",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        var response = await apiClient.ExecuteAsync<object, object>(request, "Write");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Tuples written successfully");
        Console.WriteLine();
    }

    static async Task ReadTuplesExample(FgaApiClient apiClient, string basePath, string storeId) {
        Console.WriteLine("📖 Example 6: Read Relationship Tuples");
        Console.WriteLine("Making POST request to /stores/{store_id}/read");

        var requestBody = new Dictionary<string, object> {
            {
                "tuple_key", new Dictionary<string, string> {
                    { "object", "document:roadmap" }
                }
            }
        };

        var request = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}/read",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        var response = await apiClient.ExecuteAsync<object, ReadResponse>(request, "Read");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Tuples?.Count ?? 0} tuple(s):");
        if (response.Data.Tuples != null) {
            foreach (var tuple in response.Data.Tuples) {
                Console.WriteLine($"     - {tuple.Key.User} is {tuple.Key.Relation} of {tuple.Key.Object}");
            }
        }
        Console.WriteLine();
    }

    static async Task CheckPermissionExample(FgaApiClient apiClient, string basePath, string storeId, string modelId) {
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

        var request = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        var response = await apiClient.ExecuteAsync<object, CheckResponse>(request, "Check");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Allowed: {response.Data.Allowed}");
        Console.WriteLine();
    }

    static async Task RawJsonResponseExample(FgaApiClient apiClient, string basePath) {
        Console.WriteLine("📄 Example 8: Raw JSON Response");
        Console.WriteLine("Getting response as raw JSON string instead of typed object");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string> { { "page_size", "5" } }
        };

        // Use ExecuteAsync without second type parameter to get raw JSON string
        var response = await apiClient.ExecuteAsync(request, "ListStores");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Raw JSON (first 100 chars): {response.Data?.Substring(0, Math.Min(100, response.Data.Length))}...");
        Console.WriteLine($"   RawResponse and Data are the same: {response.RawResponse == response.Data}");
        Console.WriteLine();
    }

    static async Task CustomHeadersExample(FgaApiClient apiClient, string basePath) {
        Console.WriteLine("📨 Example 9: Custom Headers");
        Console.WriteLine("Making request with custom headers");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        // Pass custom headers via ClientRequestOptions
        var options = new ClientRequestOptions {
            Headers = new Dictionary<string, string> {
                { "X-Custom-Header", "example-value" },
                { "X-Request-ID", Guid.NewGuid().ToString() }
            }
        };

        var response = await apiClient.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores", options);

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Custom headers sent successfully");
        Console.WriteLine($"   Response has {response.Headers.Count} headers");
        Console.WriteLine();
    }

    static async Task FluentApiExample(FgaApiClient apiClient, string basePath) {
        Console.WriteLine("🎯 Example 10: Fluent API for Request Building");
        Console.WriteLine("Using the enhanced RequestBuilder with fluent methods");

        // Use the new fluent API - much cleaner!
        var request = RequestBuilder<Any>
            .Create(HttpMethod.Get, basePath, "/stores")
            .WithQueryParameter("page_size", "10")
            .WithQueryParameter("continuation_token", "");

        var response = await apiClient.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Stores?.Count ?? 0} store(s) using fluent API");
        Console.WriteLine("   Note: Fluent API provides better validation and cleaner syntax!");
        Console.WriteLine();
    }

    static async Task DeleteStoreExample(FgaApiClient apiClient, string basePath, string storeId) {
        Console.WriteLine("🗑️  Cleanup: Delete Store");
        Console.WriteLine($"Making DELETE request to /stores/{{store_id}}");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Delete,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await apiClient.ExecuteAsync<Any, object>(request, "DeleteStore");

        Console.WriteLine($"✅ Status: {response.StatusCode}");
        Console.WriteLine("   Store deleted successfully");
        Console.WriteLine();
    }
}
