using Moq;
using Moq.Protected;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Constants;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Client;

/// <summary>
/// Integration tests for StreamedListObjects functionality
/// </summary>
public class StreamedListObjectsTests {
    private const string StoreId = "01HVMMBYVFD2W7C21S9TW5XPWT";
    private const string AuthorizationModelId = "01HVMMBZ2EMDA86PXWBQJSVQFK";
    private static readonly string ApiUrl = FgaConstants.TestApiUrl;

    /// <summary>
    /// Builds a newline-delimited JSON stream matching OpenFGA's streaming response format.
    /// Each line is: {"result":{"object":"document:1"}}
    /// </summary>
    private static string CreateStreamingResponse(IEnumerable<string> objects) {
        var lines = objects.Select(obj =>
            $"{{\"result\":{{\"object\":\"{obj}\"}}}}");
        return string.Join("\n", lines);
    }

    /// <summary>
    /// Creates a StringContent with application/json content type in a way that is
    /// compatible across all target frameworks (net48, netcoreapp3.1, net8+).
    /// Setting the header after construction avoids the StringContent constructor
    /// overload ambiguity between string and MediaTypeHeaderValue across TFMs.
    /// </summary>
    private static StringContent JsonContent(string content) {
        var sc = new StringContent(content, Encoding.UTF8);
        sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return sc;
    }

    private static Mock<HttpMessageHandler> CreateMockHttpHandler(
        HttpStatusCode statusCode,
        string streamContent,
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
                    Content = JsonContent(streamContent)
                };
            });

        return mockHandler;
    }

    [Fact]
    public async Task StreamedListObjects_WithAuthorizationModelId_IncludesModelIdInRequest() {
        var objects = new[] { "document:1" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId,
            AuthorizationModelId = AuthorizationModelId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        Assert.Single(results);
        Assert.Equal("document:1", results[0]);
    }

    [Fact]
    public async Task StreamedListObjects_WithConsistency_IncludesConsistencyInRequest() {
        var objects = new[] { "document:1" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

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

        Assert.Single(results);
        Assert.Equal("document:1", results[0]);
    }

    [Fact]
    public async Task StreamedListObjects_WithContextualTuples_IncludesContextualTuplesInRequest() {
        var objects = new[] { "document:1" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

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

        Assert.Single(results);
        Assert.Equal("document:1", results[0]);
    }

    [Fact]
    public async Task StreamedListObjects_ServerError_ThrowsException() {
        var mockHandler = CreateMockHttpHandler(
            HttpStatusCode.InternalServerError,
            "{\"code\":\"internal_error\",\"message\":\"Server error\"}");

        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        await Assert.ThrowsAsync<FgaApiInternalError>(async () => {
            await foreach (var _ in fgaClient.StreamedListObjects(
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
        var objects = new[] { "document:1", "document:2", "document:3", "document:4", "document:5" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
            if (results.Count == 2) {
                break;
            }
        }

        Assert.Equal(2, results.Count);
        Assert.Equal(new[] { "document:1", "document:2" }, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_WithCancellationToken_SupportsCancellation() {
        var objects = new[] { "document:1", "document:2", "document:3" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        using var cts = new CancellationTokenSource();

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

        Assert.True(results.Count >= 1, "At least one item should be processed before cancellation");
    }

    [Fact]
    public async Task StreamedListObjects_EmptyResult_ReturnsNoObjects() {
        var streamContent = "";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        Assert.Empty(results);
    }

    [Fact]
    public async Task StreamedListObjects_MissingStoreId_ThrowsValidationError() {
        using var httpClient = new HttpClient();
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl
            // No StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        await Assert.ThrowsAsync<FgaRequiredParamError>(async () => {
            await foreach (var _ in fgaClient.StreamedListObjects(
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
        var objects = Enumerable.Range(1, 100).Select(i => $"document:{i}").ToArray();
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        Assert.Equal(100, results.Count);
        Assert.Equal(objects, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_MultipleIterations_WorksCorrectly() {
        var objects = new[] { "document:1", "document:2" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler1 = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient1 = new HttpClient(mockHandler1.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient1 = new OpenFgaClient(config, httpClient1);

        var mockHandler2 = CreateMockHttpHandler(HttpStatusCode.OK, streamContent);
        using var httpClient2 = new HttpClient(mockHandler2.Object);
        using var fgaClient2 = new OpenFgaClient(config, httpClient2);

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

        Assert.Equal(objects, results1.ToArray());
        Assert.Equal(objects, results2.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_WithCustomHeaders_IncludesHeadersInRequest() {
        var objects = new[] { "document:1", "document:2" };
        var streamContent = CreateStreamingResponse(objects);

        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, streamContent,
            req => {
                Assert.True(req.Headers.Contains("X-Custom-Header"));
                Assert.Equal("custom-value", req.Headers.GetValues("X-Custom-Header").First());
                Assert.True(req.Headers.Contains("X-Request-Id"));
                Assert.Equal("req-123", req.Headers.GetValues("X-Request-Id").First());
            });

        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            },
            new ClientListObjectsOptions {
                Headers = new Dictionary<string, string> {
                    { "X-Custom-Header", "custom-value" },
                    { "X-Request-Id", "req-123" }
                }
            })) {
            results.Add(response.Object);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal(objects, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_RateLimitError_ThrowsException() {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(() => new HttpResponseMessage {
                StatusCode = (HttpStatusCode)429,
                Content = JsonContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"Too many requests\"}")
            });

        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId,
            RetryParams = new RetryParams { MaxRetry = 0, MinWaitInMs = 0 }
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(async () => {
            await foreach (var _ in fgaClient.StreamedListObjects(
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
    public async Task StreamedListObjects_TransientConnectionError_RetriesAndSucceeds() {
        var objects = new[] { "document:1", "document:2" };
        var streamContent = CreateStreamingResponse(objects);

        var callCount = 0;
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                callCount++;
                if (callCount == 1) {
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = JsonContent("{\"code\":\"internal_error\",\"message\":\"Transient error\"}")
                    };
                }
                return new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent(streamContent)
                };
            });

        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId,
            RetryParams = new RetryParams { MaxRetry = 1, MinWaitInMs = 0 }
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        Assert.Equal(2, callCount);
        Assert.Equal(objects, results.ToArray());
    }

    [Fact]
    public async Task StreamedListObjects_RateLimitError_RetriesAndSucceeds() {
        var objects = new[] { "document:1" };
        var streamContent = CreateStreamingResponse(objects);

        var callCount = 0;
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                callCount++;
                if (callCount == 1) {
                    return new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = JsonContent("{\"code\":\"rate_limit_exceeded\",\"message\":\"Too many requests\"}")
                    };
                }
                return new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent(streamContent)
                };
            });

        using var httpClient = new HttpClient(mockHandler.Object);
        var config = new ClientConfiguration {
            ApiUrl = ApiUrl,
            StoreId = StoreId,
            RetryParams = new RetryParams { MaxRetry = 1, MinWaitInMs = 0 }
        };
        using var fgaClient = new OpenFgaClient(config, httpClient);

        var results = new List<string>();
        await foreach (var response in fgaClient.StreamedListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            })) {
            results.Add(response.Object);
        }

        Assert.Equal(2, callCount);
        Assert.Equal(objects, results.ToArray());
    }
}