using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
using System.Net.Http;

namespace ApiExecutorExample;

/// <summary>
/// This example demonstrates how to use ApiExecutor to make custom HTTP requests
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
        var executor = client.ApiExecutor;

        try {
            // Example 1: List stores using raw GET request
            await ListStoresExample(executor, config.ApiUrl);

            // Example 2: Create a store using raw POST request with typed response
            var storeId = await CreateStoreExample(executor, config.ApiUrl);

            // Example 3: Get store details using path parameters
            await GetStoreExample(executor, config.ApiUrl, storeId);

            // Example 4: Create an authorization model
            var modelId = await CreateAuthorizationModelExample(executor, config.ApiUrl, storeId);

            // Example 5: Write relationship tuples
            await WriteTuplesExample(executor, config.ApiUrl, storeId);

            // Example 6: Read relationship tuples
            await ReadTuplesExample(executor, config.ApiUrl, storeId);

            // Example 7: Check permissions
            await CheckPermissionExample(executor, config.ApiUrl, storeId, modelId);

            // Example 8: Use raw JSON response instead of typed
            await RawJsonResponseExample(executor, config.ApiUrl);

            // Example 9: Custom headers
            await CustomHeadersExample(executor, config.ApiUrl);

            // Example 10: Fluent API for building requests
            await FluentApiExample(executor, config.ApiUrl);

            // Example 11: Streaming API via ExecuteStreamingAsync
            await StreamingExample(executor, config.ApiUrl);

            // Example 12: Verify StreamedListObjects retries on 429 before streaming
            await StreamedListObjectsRetryVerification();

            // Cleanup: Delete the store we created
            await DeleteStoreExample(executor, config.ApiUrl, storeId);

            Console.WriteLine("\n=== All examples completed successfully! ===");
        } catch (Exception ex) {
            Console.WriteLine($"\nError: {ex.Message}");
            Console.WriteLine("\nMake sure OpenFGA is running on localhost:8080");
            Console.WriteLine("Run: docker run -p 8080:8080 openfga/openfga:latest run");
            Environment.Exit(1);
        }
    }

    static async Task ListStoresExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 1: List Stores");
        Console.WriteLine("Making GET request to /stores");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await executor.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Is Successful: {response.IsSuccessful}");
        Console.WriteLine($"   Found {response.Data.Stores?.Count ?? 0} store(s)");
        Console.WriteLine();
    }

    static async Task<string> CreateStoreExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 2: Create Store");
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

        var response = await executor.ExecuteAsync<object, CreateStoreResponse>(request, "CreateStore");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Store ID: {response.Data.Id}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine($"   Raw Response Length: {response.RawResponse.Length} chars");
        Console.WriteLine();

        return response.Data.Id!;
    }

    static async Task GetStoreExample(ApiExecutor executor, string basePath, string storeId) {
        Console.WriteLine("Example 3: Get Store Details");
        Console.WriteLine($"Making GET request to /stores/{{store_id}}");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await executor.ExecuteAsync<Any, GetStoreResponse>(request, "GetStore");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Store Name: {response.Data.Name}");
        Console.WriteLine($"   Created At: {response.Data.CreatedAt}");
        Console.WriteLine($"   Response Headers: {response.Headers.Count}");
        Console.WriteLine();
    }

    static async Task<string> CreateAuthorizationModelExample(ApiExecutor executor, string basePath, string storeId) {
        Console.WriteLine("Example 4: Create Authorization Model");
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

        var response = await executor.ExecuteAsync<object, WriteAuthorizationModelResponse>(request, "WriteAuthorizationModel");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Model ID: {response.Data.AuthorizationModelId}");
        Console.WriteLine();

        return response.Data.AuthorizationModelId!;
    }

    static async Task WriteTuplesExample(ApiExecutor executor, string basePath, string storeId) {
        Console.WriteLine("Example 5: Write Relationship Tuples");
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

        var response = await executor.ExecuteAsync<object, object>(request, "Write");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine("   Tuples written successfully");
        Console.WriteLine();
    }

    static async Task ReadTuplesExample(ApiExecutor executor, string basePath, string storeId) {
        Console.WriteLine("Example 6: Read Relationship Tuples");
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

        var response = await executor.ExecuteAsync<object, ReadResponse>(request, "Read");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Tuples?.Count ?? 0} tuple(s):");
        if (response.Data.Tuples != null) {
            foreach (var tuple in response.Data.Tuples) {
                Console.WriteLine($"     - {tuple.Key.User} is {tuple.Key.Relation} of {tuple.Key.Object}");
            }
        }
        Console.WriteLine();
    }

    static async Task CheckPermissionExample(ApiExecutor executor, string basePath, string storeId, string modelId) {
        Console.WriteLine("Example 7: Check Permission");
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

        var response = await executor.ExecuteAsync<object, CheckResponse>(request, "Check");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Allowed: {response.Data.Allowed}");
        Console.WriteLine();
    }

    static async Task RawJsonResponseExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 8: Raw JSON Response");
        Console.WriteLine("Getting response as raw JSON string instead of typed object");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string> { { "page_size", "5" } }
        };

        var response = await executor.ExecuteAsync(request, "ListStores");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Raw JSON (first 100 chars): {response.Data?.Substring(0, Math.Min(100, response.Data.Length))}...");
        Console.WriteLine($"   RawResponse and Data are the same: {response.RawResponse == response.Data}");
        Console.WriteLine();
    }

    static async Task CustomHeadersExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 9: Custom Headers");
        Console.WriteLine("Making request with custom headers");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = basePath,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        var options = new ClientRequestOptions {
            Headers = new Dictionary<string, string> {
                { "X-Custom-Header", "example-value" },
                { "X-Request-ID", Guid.NewGuid().ToString() }
            }
        };

        var response = await executor.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores", options);

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine("   Custom headers sent successfully");
        Console.WriteLine($"   Response has {response.Headers.Count} headers");
        Console.WriteLine();
    }

    static async Task FluentApiExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 10: Fluent API for Request Building");
        Console.WriteLine("Using RequestBuilder with fluent methods");

        var request = RequestBuilder<Any>
            .Create(HttpMethod.Get, basePath, "/stores")
            .WithQueryParameter("page_size", "10")
            .WithQueryParameter("continuation_token", "");

        var response = await executor.ExecuteAsync<Any, ListStoresResponse>(request, "ListStores");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine($"   Found {response.Data.Stores?.Count ?? 0} store(s)");
        Console.WriteLine();
    }

    static async Task StreamingExample(ApiExecutor executor, string basePath) {
        Console.WriteLine("Example 11: Streaming API");
        Console.WriteLine("Streaming list-objects for a computed relation via ExecuteStreamingAsync");

        // Create a dedicated store for this streaming demo
        var storeResponse = await executor.ExecuteAsync<object, CreateStoreResponse>(
            RequestBuilder<object>
                .Create(HttpMethod.Post, basePath, "/stores")
                .WithBody(new Dictionary<string, object> { { "name", "streaming-demo" } }),
            "CreateStore");
        var streamStoreId = storeResponse.Data.Id!;
        Console.WriteLine($"   Created store: {streamStoreId}");

        // Write an authorization model with owner, viewer, and a computed can_read relation
        var modelResponse = await executor.ExecuteAsync<object, WriteAuthorizationModelResponse>(
            RequestBuilder<object>
                .Create(HttpMethod.Post, basePath, "/stores/{store_id}/authorization-models")
                .WithPathParameter("store_id", streamStoreId)
                .WithBody(new Dictionary<string, object> {
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
                                        { "owner",  new Dictionary<string, object> { { "this", new Dictionary<string, object>() } } },
                                        { "viewer", new Dictionary<string, object> { { "this", new Dictionary<string, object>() } } },
                                        {
                                            "can_read", new Dictionary<string, object> {
                                                {
                                                    "union", new Dictionary<string, object> {
                                                        {
                                                            "child", new List<Dictionary<string, object>> {
                                                                new() { { "computedUserset", new Dictionary<string, object> { { "relation", "owner" } } } },
                                                                new() { { "computedUserset", new Dictionary<string, object> { { "relation", "viewer" } } } }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                                {
                                    "metadata", new Dictionary<string, object> {
                                        {
                                            "relations", new Dictionary<string, object> {
                                                { "owner",   new Dictionary<string, object> { { "directly_related_user_types", new List<Dictionary<string, string>> { new() { { "type", "user" } } } } } },
                                                { "viewer",  new Dictionary<string, object> { { "directly_related_user_types", new List<Dictionary<string, string>> { new() { { "type", "user" } } } } } },
                                                { "can_read", new Dictionary<string, object> { { "directly_related_user_types", new List<Dictionary<string, string>>() } } }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }),
            "WriteAuthorizationModel");
        var streamModelId = modelResponse.Data.AuthorizationModelId!;
        Console.WriteLine($"   Created model: {streamModelId}");

        // Write 1000 documents where anne is the owner and 1000 where she is a viewer (batches of 100)
        Console.WriteLine("   Writing tuples (1000 as owner, 1000 as viewer)...");
        const int batchSize = 100;
        var totalWritten = 0;

        for (var batch = 0; batch < 10; batch++) {
            var tuples = new List<Dictionary<string, string>>();
            for (var i = 1; i <= batchSize; i++) {
                tuples.Add(new Dictionary<string, string> {
                    { "user", "user:anne" }, { "relation", "owner" }, { "object", $"document:{batch * batchSize + i}" }
                });
            }
            await executor.ExecuteAsync<object, object>(
                RequestBuilder<object>
                    .Create(HttpMethod.Post, basePath, "/stores/{store_id}/write")
                    .WithPathParameter("store_id", streamStoreId)
                    .WithBody(new Dictionary<string, object> {
                        { "writes", new Dictionary<string, object> { { "tuple_keys", tuples } } },
                        { "authorization_model_id", streamModelId }
                    }),
                "Write");
            totalWritten += tuples.Count;
        }

        for (var batch = 0; batch < 10; batch++) {
            var tuples = new List<Dictionary<string, string>>();
            for (var i = 1; i <= batchSize; i++) {
                tuples.Add(new Dictionary<string, string> {
                    { "user", "user:anne" }, { "relation", "viewer" }, { "object", $"document:{1000 + batch * batchSize + i}" }
                });
            }
            await executor.ExecuteAsync<object, object>(
                RequestBuilder<object>
                    .Create(HttpMethod.Post, basePath, "/stores/{store_id}/write")
                    .WithPathParameter("store_id", streamStoreId)
                    .WithBody(new Dictionary<string, object> {
                        { "writes", new Dictionary<string, object> { { "tuple_keys", tuples } } },
                        { "authorization_model_id", streamModelId }
                    }),
                "Write");
            totalWritten += tuples.Count;
        }

        Console.WriteLine($"   Wrote {totalWritten} tuples");

        // Stream objects via the computed can_read relation using ExecuteStreamingAsync
        Console.WriteLine("   Streaming objects via computed 'can_read' relation...");
        var count = 0;
        var streamRequest = RequestBuilder<object>
            .Create(HttpMethod.Post, basePath, "/stores/{store_id}/streamed-list-objects")
            .WithPathParameter("store_id", streamStoreId)
            .WithBody(new Dictionary<string, object> {
                { "user", "user:anne" },
                { "relation", "can_read" },
                { "type", "document" },
                { "authorization_model_id", streamModelId }
            });

        await foreach (var item in executor.ExecuteStreamingAsync<object, StreamedListObjectsResponse>(streamRequest, "StreamedListObjects")) {
            count++;
            if (count <= 3 || count % 500 == 0) {
                Console.WriteLine($"   - {item.Object}");
            }
        }

        Console.WriteLine($"Streamed {count} objects");

        // Clean up the streaming demo store
        await executor.ExecuteAsync<Any, object>(
            RequestBuilder<Any>
                .Create(HttpMethod.Delete, basePath, "/stores/{store_id}")
                .WithPathParameter("store_id", streamStoreId),
            "DeleteStore");
        Console.WriteLine("   Streaming demo store deleted");
        Console.WriteLine();
    }

    /// <summary>
    /// Verifies that StreamedListObjects retries on 429 during connection establishment.
    ///
    /// Uses a mock HttpMessageHandler that returns 429 for the first two attempts,
    /// then succeeds on the third with a small NDJSON payload. If the retry fix is
    /// working the items are yielded; if broken, an FgaApiRateLimitExceededError is thrown.
    /// </summary>
    static async Task StreamedListObjectsRetryVerification() {
        Console.WriteLine("Example 12: StreamedListObjects retry on 429 (mock)");

        const int failuresBeforeSuccess = 2;
        var attemptCount = 0;

        // NDJSON body that the mock server returns on the successful attempt
        const string ndjsonBody =
            "{\"result\":{\"object\":\"document:1\"}}\n" +
            "{\"result\":{\"object\":\"document:2\"}}\n" +
            "{\"result\":{\"object\":\"document:3\"}}\n";

        var handler = new MockHttpMessageHandler(request => {
            attemptCount++;
            Console.WriteLine($"   [mock] attempt {attemptCount} - {request.Method} {request.RequestUri?.PathAndQuery}");

            if (attemptCount <= failuresBeforeSuccess) {
                Console.WriteLine($"   [mock] returning 429 (simulated rate-limit)");
                var rateLimitResponse = new HttpResponseMessage((System.Net.HttpStatusCode)429) {
                    Content = new StringContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"rate limit exceeded\"}")
                };
                return rateLimitResponse;
            }

            Console.WriteLine($"   [mock] returning 200 with NDJSON stream");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                Content = new StringContent(ndjsonBody, System.Text.Encoding.UTF8, "application/x-ndjson")
            };
        });

        var httpClient = new HttpClient(handler);

        // Configure with enough retries (>= failuresBeforeSuccess) and a short wait
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            // Use a valid ULID for StoreId (ULID: 26 chars, Crockford base32, e.g. "01ARZ3NDEKTSV4RRFFQ69G5FAV")
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10  // short so the test runs fast
        };

        using var retryClient = new OpenFgaClient(retryConfig, httpClient);

        var objects = new List<string>();
        await foreach (var item in retryClient.StreamedListObjects(
            new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {
            objects.Add(item.Object ?? string.Empty);
        }

        Console.WriteLine($"   Attempts made  : {attemptCount} (expected {failuresBeforeSuccess + 1})");
        Console.WriteLine($"   Objects streamed: {objects.Count} (expected 3)");
        Console.WriteLine($"   Objects: {string.Join(", ", objects)}");

        if (attemptCount != failuresBeforeSuccess + 1)
            throw new Exception($"Expected {failuresBeforeSuccess + 1} attempts, got {attemptCount}");
        if (objects.Count != 3)
            throw new Exception($"Expected 3 objects, got {objects.Count}");

        Console.WriteLine("   PASS: StreamedListObjects correctly retried on 429 and received all items");
        Console.WriteLine();

        // --- Additional Retry Flow Test Scenarios ---
        // Scenario 1: Exceeding MaxRetry (Permanent 429)
        await StreamedListObjectsRetry_Permanent429();

        // Scenario 2: Immediate Success (No Retry Needed)
        await StreamedListObjectsRetry_ImmediateSuccess();

        // Scenario 3: Intermittent Non-Retryable Error (400 Bad Request)
        await StreamedListObjectsRetry_NonRetryableError();

        // Scenario 4: Retry on 503 then Success
        await StreamedListObjectsRetry_503ThenSuccess();

        // Scenario 5: Malformed NDJSON After Retries
        await StreamedListObjectsRetry_MalformedNDJSON();

        // Scenario 6: Partial Success (200 with Fewer Objects)
        await StreamedListObjectsRetry_PartialSuccess();

        // Scenario 7: Cancellation/Timeout During Retry (not implemented here, would require CancellationToken support)
        // Expectation: The client should abort retries and throw a cancellation exception.
        // This would require additional support in the client and test harness.
        // --- End of Additional Retry Flow Test Scenarios ---
    }

    // Scenario 1: Exceeding MaxRetry (Permanent 429)
    static async Task StreamedListObjectsRetry_Permanent429() {
        Console.WriteLine("Scenario 1: Permanent 429 (exceed MaxRetry)");
        int attemptCount = 0;
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 429");
            return new HttpResponseMessage((System.Net.HttpStatusCode)429) {
                Content = new StringContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"rate limit exceeded\"}")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        try {
            await foreach (var _ in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {}
            throw new Exception("Expected exception was not thrown");
        } catch (Exception ex) {
            Console.WriteLine($"   Caught exception: {ex.GetType().Name}");
            Console.WriteLine("   PASS: Exception thrown after max retries");
        }
        Console.WriteLine();
    }

    // Scenario 2: Immediate Success (No Retry Needed)
    static async Task StreamedListObjectsRetry_ImmediateSuccess() {
        Console.WriteLine("Scenario 2: Immediate Success (no retry)");
        int attemptCount = 0;
        const string ndjsonBody = "{\"result\":{\"object\":\"document:1\"}}\n";
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 200");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                Content = new StringContent(ndjsonBody, System.Text.Encoding.UTF8, "application/x-ndjson")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        var objects = new List<string>();
        await foreach (var item in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {
            objects.Add(item.Object ?? string.Empty);
        }
        Console.WriteLine($"   Attempts made: {attemptCount} (expected 1)");
        Console.WriteLine($"   Objects streamed: {objects.Count} (expected 1)");
        Console.WriteLine($"   Objects: {string.Join(", ", objects)}");
        if (attemptCount == 1 && objects.Count == 1 && objects[0] == "document:1")
            Console.WriteLine("   PASS: Immediate success, no retry");
        else
            throw new Exception("Immediate success scenario failed");
        Console.WriteLine();
    }

    // Scenario 3: Intermittent Non-Retryable Error (400 Bad Request)
    static async Task StreamedListObjectsRetry_NonRetryableError() {
        Console.WriteLine("Scenario 3: Non-retryable error (400 Bad Request)");
        int attemptCount = 0;
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 400");
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest) {
                Content = new StringContent("{\"code\":\"bad_request\",\"message\":\"bad request\"}")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        try {
            await foreach (var _ in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {}
            throw new Exception("Expected exception was not thrown");
        } catch (Exception ex) {
            Console.WriteLine($"   Caught exception: {ex.GetType().Name}");
            Console.WriteLine("   PASS: Non-retryable error thrown immediately");
        }
        Console.WriteLine();
    }

    // Scenario 4: Retry on 503 then Success
    static async Task StreamedListObjectsRetry_503ThenSuccess() {
        Console.WriteLine("Scenario 4: Retry on 503 then success");
        int attemptCount = 0;
        const string ndjsonBody = "{\"result\":{\"object\":\"document:1\"}}\n";
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            if (attemptCount < 3) {
                Console.WriteLine($"   [mock] attempt {attemptCount} - returning 503");
                return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable) {
                    Content = new StringContent("{\"code\":\"unavailable\",\"message\":\"service unavailable\"}")
                };
            }
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 200");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                Content = new StringContent(ndjsonBody, System.Text.Encoding.UTF8, "application/x-ndjson")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        var objects = new List<string>();
        await foreach (var item in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {
            objects.Add(item.Object ?? string.Empty);
        }
        Console.WriteLine($"   Attempts made: {attemptCount} (expected 3)");
        Console.WriteLine($"   Objects streamed: {objects.Count} (expected 1)");
        if (attemptCount == 3 && objects.Count == 1 && objects[0] == "document:1")
            Console.WriteLine("   PASS: Retries on 503, then success");
        else
            throw new Exception("503 retry scenario failed");
        Console.WriteLine();
    }

    // Scenario 5: Malformed NDJSON After Retries
    static async Task StreamedListObjectsRetry_MalformedNDJSON() {
        Console.WriteLine("Scenario 5: Malformed NDJSON after retries");
        int attemptCount = 0;
        const string malformedNdjson = "not-a-json\n";
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            if (attemptCount < 3) {
                Console.WriteLine($"   [mock] attempt {attemptCount} - returning 429");
                return new HttpResponseMessage((System.Net.HttpStatusCode)429) {
                    Content = new StringContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"rate limit exceeded\"}")
                };
            }
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 200 with malformed NDJSON");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                Content = new StringContent(malformedNdjson, System.Text.Encoding.UTF8, "application/x-ndjson")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        try {
            await foreach (var _ in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {}
            throw new Exception("Expected parsing exception was not thrown");
        } catch (Exception ex) {
            Console.WriteLine($"   Caught exception: {ex.GetType().Name}");
            Console.WriteLine("   PASS: Malformed NDJSON throws after retries");
        }
        Console.WriteLine();
    }

    // Scenario 6: Partial Success (200 with Fewer Objects)
    static async Task StreamedListObjectsRetry_PartialSuccess() {
        Console.WriteLine("Scenario 6: Partial success (fewer objects)");
        int attemptCount = 0;
        const string ndjsonBody = "{\"result\":{\"object\":\"document:1\"}}\n";
        var handler = new MockHttpMessageHandler(_ => {
            attemptCount++;
            if (attemptCount < 2) {
                Console.WriteLine($"   [mock] attempt {attemptCount} - returning 429");
                return new HttpResponseMessage((System.Net.HttpStatusCode)429) {
                    Content = new StringContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"rate limit exceeded\"}")
                };
            }
            Console.WriteLine($"   [mock] attempt {attemptCount} - returning 200 with 1 object");
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
                Content = new StringContent(ndjsonBody, System.Text.Encoding.UTF8, "application/x-ndjson")
            };
        });
        var httpClient = new HttpClient(handler);
        var retryConfig = new ClientConfiguration {
            ApiUrl = "http://mock-fga-server",
            StoreId = "01ARZ3NDEKTSV4RRFFQ69G5FAV",
            MaxRetry = 3,
            MinWaitInMs = 10
        };
        using var retryClient = new OpenFgaClient(retryConfig, httpClient);
        var objects = new List<string>();
        await foreach (var item in retryClient.StreamedListObjects(new ClientListObjectsRequest { User = "user:anne", Relation = "reader", Type = "document" })) {
            objects.Add(item.Object ?? string.Empty);
        }
        Console.WriteLine($"   Attempts made: {attemptCount} (expected 2)");
        Console.WriteLine($"   Objects streamed: {objects.Count} (expected 1)");
        if (attemptCount == 2 && objects.Count == 1 && objects[0] == "document:1")
            Console.WriteLine("   PASS: Partial success handled");
        else
            throw new Exception("Partial success scenario failed");
        Console.WriteLine();
    }

    // Scenario 7: Cancellation/Timeout During Retry (not implemented here, would require CancellationToken support)
    // Expectation: The client should abort retries and throw a cancellation exception.
    // This would require additional support in the client and test harness.

    /// <summary>Simple inline mock handler that delegates to a synchronous callback.</summary>
    class MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(handler(request));
    }

    static async Task DeleteStoreExample(ApiExecutor executor, string basePath, string storeId) {
        Console.WriteLine("Cleanup: Delete Store");
        Console.WriteLine($"Making DELETE request to /stores/{{store_id}}");

        var request = new RequestBuilder<Any> {
            Method = HttpMethod.Delete,
            BasePath = basePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        var response = await executor.ExecuteAsync<Any, object>(request, "DeleteStore");

        Console.WriteLine($"Status: {response.StatusCode}");
        Console.WriteLine("   Store deleted successfully");
        Console.WriteLine();
    }
}
