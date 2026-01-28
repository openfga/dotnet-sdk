using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.ApiExecutor;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Client.ApiExecutor;

/// <summary>
/// Integration tests for ApiExecutor functionality.
/// These tests demonstrate how to use API Executor to call OpenFGA endpoints
/// without using the SDK's typed methods.
/// Runs against an actual OpenFGA server using Testcontainers.
/// </summary>
public class ApiExecutorIntegrationTests : IAsyncLifetime {
    private IContainer? _openFgaContainer;
    private OpenFgaClient? _fga;
    private const string OpenFgaImage = "openfga/openfga:v1.10.2";
    private const int OpenFgaPort = 8080;

    public async Task InitializeAsync() {
        // Create and start OpenFGA container
        _openFgaContainer = new ContainerBuilder()
            .WithImage(OpenFgaImage)
            .WithPortBinding(OpenFgaPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r
                .ForPort(OpenFgaPort)
                .ForPath("/healthz")
            ))
            .Build();

        await _openFgaContainer.StartAsync();

        // Initialize OpenFGA client
        var apiUrl = $"http://{_openFgaContainer.Hostname}:{_openFgaContainer.GetMappedPublicPort(OpenFgaPort)}";
        var config = new ClientConfiguration {
            ApiUrl = apiUrl
        };
        _fga = new OpenFgaClient(config);
    }

    public async Task DisposeAsync() {
        _fga?.Dispose();
        if (_openFgaContainer != null) {
            await _openFgaContainer.DisposeAsync();
        }
    }

    /// <summary>
    /// Test listing stores using ApiExecutor instead of fga.ListStores().
    /// </summary>
    [Fact]
    public async Task RawRequest_ListStores() {
        // Create a store first so we have something to list
        var storeName = "test-store-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        await CreateStoreUsingRawRequest(storeName);

        // Use ApiExecutor to list stores (equivalent to GET /stores)
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores")
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<ListStoresResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Stores);
        Assert.NotEmpty(response.Data.Stores);

        // Verify we can find our store
        var foundStore = false;
        foreach (var store in response.Data.Stores) {
            if (store.Name == storeName) {
                foundStore = true;
                break;
            }
        }
        Assert.True(foundStore, "Should find the store we created");
    }

    /// <summary>
    /// Test creating a store using ApiExecutor with typed response.
    /// </summary>
    [Fact]
    public async Task RawRequest_CreateStore_TypedResponse() {
        var storeName = "raw-test-store-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Build request body
        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        // Use ApiExecutor to create store (equivalent to POST /stores)
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Body(requestBody)
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<CreateStoreResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Id);
        Assert.Equal(storeName, response.Data.Name);
    }

    /// <summary>
    /// Test creating a store using ApiExecutor with raw JSON string response.
    /// </summary>
    [Fact]
    public async Task RawRequest_CreateStore_RawJsonResponse() {
        var storeName = "raw-json-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // Build request body
        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        // Use ApiExecutor to create store and get raw JSON response
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Body(requestBody)
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.RawResponse);

        // Parse the JSON manually
        var rawJson = response.Data;
        Assert.Contains("\"id\"", rawJson);
        Assert.Contains("\"name\"", rawJson);
        Assert.Contains(storeName, rawJson);
    }

    /// <summary>
    /// Test getting a specific store using ApiExecutor with path parameters.
    /// </summary>
    [Fact]
    public async Task RawRequest_GetStore_WithPathParams() {
        // Create a store first
        var storeName = "get-test-store-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);

        // Use ApiExecutor to get store details (equivalent to GET /stores/{store_id})
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores/{store_id}")
            .PathParam("store_id", storeId)
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<GetStoreResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.Equal(storeId, response.Data.Id);
        Assert.Equal(storeName, response.Data.Name);
    }

    /// <summary>
    /// Test automatic {store_id} replacement when store ID is configured.
    /// </summary>
    [Fact]
    public async Task RawRequest_AutomaticStoreIdReplacement() {
        // Create a store and configure it
        var storeName = "auto-store-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;

        // Use ApiExecutor WITHOUT providing store_id path param - it should be auto-replaced
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores/{store_id}")
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<GetStoreResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.Equal(storeId, response.Data.Id);

        // Clean up - reset store ID
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test writing authorization model using ApiExecutor.
    /// </summary>
    [Fact]
    public async Task RawRequest_WriteAuthorizationModel() {
        // Create a store first
        var storeName = "auth-model-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;

        // Build authorization model with proper metadata
        var requestBody = new Dictionary<string, object> {
            { "schema_version", "1.1" },
            {
                "type_definitions", new List<Dictionary<string, object>> {
                    new Dictionary<string, object> {
                        { "type", "user" },
                        { "relations", new Dictionary<string, object>() }
                    },
                    new Dictionary<string, object> {
                        { "type", "document" },
                        {
                            "relations", new Dictionary<string, object> {
                                {
                                    "reader", new Dictionary<string, object> {
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
                                                        new Dictionary<string, string> { { "type", "user" } }
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

        // Use ApiExecutor to write authorization model
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/authorization-models")
            .Body(requestBody)
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<WriteAuthorizationModelResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.AuthorizationModelId);

        // Clean up
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test reading authorization models with query parameters.
    /// </summary>
    [Fact]
    public async Task RawRequest_ReadAuthorizationModels_WithQueryParams() {
        // Create a store and write a model
        var storeName = "read-models-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;

        // Create an authorization model first
        await WriteSimpleAuthorizationModel(storeId);

        // Use ApiExecutor to read authorization models with query parameters
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores/{store_id}/authorization-models")
            .QueryParam("page_size", "10")
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<ReadAuthorizationModelsResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.AuthorizationModels);
        Assert.NotEmpty(response.Data.AuthorizationModels);

        // Clean up
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test Check API using raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_Check() {
        // Setup: Create store and authorization model
        var storeName = "check-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;
        var modelId = await WriteSimpleAuthorizationModel(storeId);

        // Write a tuple
        await WriteTupleUsingRawRequest(storeId, "user:alice", "reader", "document:budget");

        // Use ApiExecutor to perform check
        var checkBody = new Dictionary<string, object> {
            { "authorization_model_id", modelId },
            {
                "tuple_key", new Dictionary<string, string> {
                    { "user", "user:alice" },
                    { "relation", "reader" },
                    { "object", "document:budget" }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/check")
            .Body(checkBody)
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<CheckResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Allowed, "Alice should be allowed to read the document");

        // Clean up
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test custom headers with raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_WithCustomHeaders() {
        var storeName = "headers-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        // Use ApiExecutor with custom headers
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Body(requestBody)
            .Header("X-Custom-Header", "custom-value")
            .Header("X-Request-ID", "test-123")
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<CreateStoreResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.IsSuccessful);
    }

    /// <summary>
    /// Test error handling with raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_ErrorHandling_NotFound() {
        // Try to get a non-existent store
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores/{store_id}")
            .PathParam("store_id", "non-existent-store-id")
            .Build();

        // Should throw an exception
        await Assert.ThrowsAnyAsync<FgaApiError>(async () =>
            await _fga!.ApiExecutor().SendAsync<GetStoreResponse>(request));
    }

    /// <summary>
    /// Test list stores with pagination using query parameters.
    /// </summary>
    [Fact]
    public async Task RawRequest_ListStores_WithPagination() {
        // Create multiple stores
        for (int i = 0; i < 3; i++) {
            await CreateStoreUsingRawRequest("pagination-test-" + i + "-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            // Small delay to ensure unique timestamps
            await Task.Delay(10);
        }

        // Use ApiExecutor to list stores with pagination
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Get, "/stores")
            .QueryParam("page_size", "2")
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<ListStoresResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Stores);
    }

    /// <summary>
    /// Test Write API using raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_Write_WriteTuples() {
        // Setup: Create store and authorization model
        var storeName = "write-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;
        await WriteSimpleAuthorizationModel(storeId);

        // Use ApiExecutor to write tuples
        var writeBody = new Dictionary<string, object> {
            {
                "writes", new Dictionary<string, object> {
                    {
                        "tuple_keys", new List<Dictionary<string, string>> {
                            new Dictionary<string, string> {
                                { "user", "user:bob" },
                                { "relation", "reader" },
                                { "object", "document:report" }
                            }
                        }
                    }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/write")
            .Body(writeBody)
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<object>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);

        // Clean up
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test Read API using raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_Read_ReadTuples() {
        // Setup: Create store, authorization model, and write a tuple
        var storeName = "read-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);
        _fga!.StoreId = storeId;
        await WriteSimpleAuthorizationModel(storeId);
        await WriteTupleUsingRawRequest(storeId, "user:charlie", "reader", "document:plan");

        // Use ApiExecutor to read tuples
        var readBody = new Dictionary<string, object> {
            {
                "tuple_key", new Dictionary<string, string> {
                    { "user", "user:charlie" },
                    { "relation", "reader" },
                    { "object", "document:plan" }
                }
            }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/read")
            .Body(readBody)
            .Build();

        var response = await _fga.ApiExecutor().SendAsync<ReadResponse>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Tuples);
        Assert.NotEmpty(response.Data.Tuples);

        // Verify the tuple we wrote is present
        var foundTuple = false;
        foreach (var tuple in response.Data.Tuples) {
            if (tuple.Key.User == "user:charlie" &&
                tuple.Key.Relation == "reader" &&
                tuple.Key.Object == "document:plan") {
                foundTuple = true;
                break;
            }
        }
        Assert.True(foundTuple, "Should find the tuple we wrote");

        // Clean up
        _fga.StoreId = null;
    }

    /// <summary>
    /// Test Delete Store using raw request.
    /// </summary>
    [Fact]
    public async Task RawRequest_DeleteStore() {
        // Create a store first
        var storeName = "delete-test-" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var storeId = await CreateStoreUsingRawRequest(storeName);

        // Use ApiExecutor to delete the store
        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Delete, "/stores/{store_id}")
            .PathParam("store_id", storeId)
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<object>(request);

        // Verify response
        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        Assert.True(response.IsSuccessful);
    }

    // Helper methods

    private async Task<string> CreateStoreUsingRawRequest(string storeName) {
        var requestBody = new Dictionary<string, object> {
            { "name", storeName }
        };

        var request = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Body(requestBody)
            .Build();

        var response = await _fga!.ApiExecutor().SendAsync<CreateStoreResponse>(request);
        return response.Data.Id;
    }

    private async Task<string> WriteSimpleAuthorizationModel(string storeId) {
        var requestBody = new Dictionary<string, object> {
            { "schema_version", "1.1" },
            {
                "type_definitions", new List<Dictionary<string, object>> {
                    new Dictionary<string, object> {
                        { "type", "user" },
                        { "relations", new Dictionary<string, object>() }
                    },
                    new Dictionary<string, object> {
                        { "type", "document" },
                        {
                            "relations", new Dictionary<string, object> {
                                {
                                    "reader", new Dictionary<string, object> {
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
                                                        new Dictionary<string, string> { { "type", "user" } }
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

        var response = await _fga!.ApiExecutor().SendAsync<WriteAuthorizationModelResponse>(request);
        return response.Data.AuthorizationModelId;
    }

    private async Task WriteTupleUsingRawRequest(string storeId, string user, string relation, string obj) {
        var requestBody = new Dictionary<string, object> {
            {
                "writes", new Dictionary<string, object> {
                    {
                        "tuple_keys", new List<Dictionary<string, string>> {
                            new Dictionary<string, string> {
                                { "user", user },
                                { "relation", relation },
                                { "object", obj }
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

        await _fga!.ApiExecutor().SendAsync<object>(request);
    }
}
