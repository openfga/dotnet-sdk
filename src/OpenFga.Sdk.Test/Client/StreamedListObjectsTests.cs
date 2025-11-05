using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using Xunit;

namespace OpenFga.Sdk.Test.Client;

/// <summary>
/// Integration tests for StreamedListObjects functionality
/// </summary>
public class StreamedListObjectsTests {
    private const string StoreId = "01HVMMBYVFD2W7C21S9TW5XPWT";
    private const string AuthorizationModelId = "01HVMMBZ2EMDA86PXWBQJSVQFK";
    private const string ApiUrl = "http://localhost:8080";

    private Mock<HttpMessageHandler> CreateMockHttpHandler(
        HttpStatusCode statusCode,
        string ndjsonContent,
        Action<HttpRequestMessage>? requestValidator = null) {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                requestValidator?.Invoke(req);
                return new HttpResponseMessage {
                    StatusCode = statusCode,
                    Content = new StringContent(ndjsonContent, Encoding.UTF8, "application/x-ndjson")
                };
            });
        return mockHandler;
    }

    private string CreateNDJSONResponse(params string[] objects) {
        return string.Join("\n",
            objects.Select(obj => $"{{\"result\":{{\"object\":\"{obj}\"}}}}")) + "\n";
    }

    [Fact]
    public async Task StreamedListObjects_BasicRequest_StreamsObjectsIncrementally() {
        // Arrange
        var objects = new[] { "document:1", "document:2", "document:3" };
        var ndjson = CreateNDJSONResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson,
            req => {
                // Verify the request
                Assert.Equal(HttpMethod.Post, req.Method);
                Assert.Contains($"/stores/{StoreId}/streamed-list-objects", req.RequestUri!.ToString());
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.Equal(3, results.Count);
        Assert.Equal(objects, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_WithAuthorizationModelId_IncludesModelIdInRequest() {
        // Arrange
        var objects = new[] { "document:1" };
        var ndjson = CreateNDJSONResponse(objects);

        string? requestBody = null;
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson,
            async req => {
                requestBody = await req.Content!.ReadAsStringAsync();
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId,
            AuthorizationModelId = AuthorizationModelId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.NotNull(requestBody);
        Assert.Contains(AuthorizationModelId, requestBody);
    }

    [Fact]
    public async Task StreamedListObjects_WithConsistency_IncludesConsistencyInRequest() {
        // Arrange
        var objects = new[] { "document:1" };
        var ndjson = CreateNDJSONResponse(objects);

        string? requestBody = null;
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson,
            async req => {
                requestBody = await req.Content!.ReadAsStringAsync();
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            },
            new ClientListObjectsOptions {
                Consistency = ConsistencyPreference.HIGHERCONSISTENCY
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.NotNull(requestBody);
        Assert.Contains("HIGHER_CONSISTENCY", requestBody);
    }

    [Fact]
    public async Task StreamedListObjects_WithContextualTuples_IncludesContextualTuplesInRequest() {
        // Arrange
        var objects = new[] { "document:1" };
        var ndjson = CreateNDJSONResponse(objects);

        string? requestBody = null;
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson,
            async req => {
                requestBody = await req.Content!.ReadAsStringAsync();
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document",
                ContextualTuples = new List<ClientTupleKey> {
                    new() {
                        User = "user:anne",
                        Relation = "writer",
                        Object = "document:temp"
                    }
                }
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.NotNull(requestBody);
        Assert.Contains("contextual_tuples", requestBody);
        Assert.Contains("document:temp", requestBody);
    }

    [Fact]
    public async Task StreamedListObjects_ServerError_ThrowsException() {
        // Arrange
        var mockHandler = CreateMockHttpHandler(
            HttpStatusCode.InternalServerError,
            "{\"code\":\"internal_error\",\"message\":\"Server error\"}");

        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<FgaApiInternalError>(async () => {
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                })) {
                // Should not reach here
            }
        });
    }

    [Fact]
    public async Task StreamedListObjects_EarlyBreak_DisposesResourcesCleanly() {
        // Arrange
        var objects = new[] { "document:1", "document:2", "document:3", "document:4", "document:5" };
        var ndjson = CreateNDJSONResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
            if (results.Count == 2) {
                break; // Early termination - should not throw or leak resources
            }
        }

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal(new[] { "document:1", "document:2" }, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_WithCancellationToken_SupportsCancellation() {
        // Arrange
        var objects = new[] { "document:1", "document:2", "document:3" };
        var ndjson = CreateNDJSONResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        var cts = new CancellationTokenSource();

        // Act & Assert
        var results = new List<string>();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => {
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                },
                cancellationToken: cts.Token)) {
                results.Add(response.Object);
                if (results.Count == 1) {
                    cts.Cancel();
                }
            }
        });

        Assert.True(results.Count >= 1);
    }

    [Fact]
    public async Task StreamedListObjects_EmptyResult_ReturnsNoObjects() {
        // Arrange
        var ndjson = ""; // Empty response
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task StreamedListObjects_MissingStoreId_ThrowsValidationError() {
        // Arrange
        var httpClient = new HttpClient();
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl
            // No StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<FgaValidationError>(async () => {
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                })) {
                // Should not reach here
            }
        });
    }

    [Fact]
    public async Task StreamedListObjects_LargeNumberOfObjects_StreamsEfficiently() {
        // Arrange - simulate a large response
        var objects = Enumerable.Range(1, 100).Select(i => $"document:{i}").ToArray();
        var ndjson = CreateNDJSONResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient = new OpenFgaClient(config, httpClient);

        // Act
        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        // Assert
        Assert.Equal(100, results.Count);
        Assert.Equal(objects, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_MultipleIterations_WorksCorrectly() {
        // Arrange
        var objects = new[] { "document:1", "document:2" };
        var ndjson = CreateNDJSONResponse(objects);

        // Create a new mock handler for each call
        var mockHandler1 = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient1 = new HttpClient(mockHandler1.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        var fgaClient1 = new OpenFgaClient(config, httpClient1);

        var mockHandler2 = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient2 = new HttpClient(mockHandler2.Object);
        var fgaClient2 = new OpenFgaClient(config, httpClient2);

        // Act - Call twice
        var results1 = new List<string>();
        await foreach (var response in fgaClient1.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results1.Add(response.Object);
        }

        var results2 = new List<string>();
        await foreach (var response in fgaClient2.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results2.Add(response.Object);
        }

        // Assert
        Assert.Equal(objects, results1.ToArray());
        Assert.Equal(objects, results2.ToArray());
    }
}

