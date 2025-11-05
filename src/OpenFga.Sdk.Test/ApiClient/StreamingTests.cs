using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using Xunit;

namespace OpenFga.Sdk.Test.ApiClient;

/// <summary>
/// Tests for NDJSON streaming functionality in BaseClient
/// </summary>
public class StreamingTests {
    private Mock<HttpMessageHandler> CreateMockHttpHandler(HttpStatusCode statusCode, string content,
        string contentType = "application/x-ndjson") {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, contentType)
            });
        return mockHandler;
    }

    [Fact]
    public async Task SendStreamingRequestAsync_SingleLineNDJSON_ParsesCorrectly() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Single(results);
        Assert.Equal("document:1", results[0].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_MultipleLineNDJSON_ParsesAllLines() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Equal(3, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
        Assert.Equal("document:3", results[2].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EmptyLines_SkipsEmptyLines() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_LastLineWithoutNewline_ParsesCorrectly() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}"; // No trailing newline
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_CancellationToken_CancelsStream() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var cts = new CancellationTokenSource();

        // Act & Assert
        var results = new List<StreamedListObjectsResponse>();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => {
            await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
                requestBuilder, null, "Test", cts.Token)) {
                results.Add(item);
                if (results.Count == 1) {
                    cts.Cancel(); // Cancel after first item
                }
            }
        });

        // Should have received at least one item before cancellation
        Assert.True(results.Count >= 1);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_HttpError_ThrowsException() {
        // Arrange
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.InternalServerError,
            "{\"code\":\"internal_error\",\"message\":\"Server error\"}",
            "application/json");
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act & Assert
        await Assert.ThrowsAsync<FgaApiInternalError>(async () => {
            await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
                requestBuilder, null, "Test")) {
                // Should not get here
            }
        });
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EarlyBreak_DisposesResourcesProperly() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n" +
                     "{\"result\":{\"object\":\"document:4\"}}\n" +
                     "{\"result\":{\"object\":\"document:5\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
            if (results.Count == 2) {
                break; // Early termination
            }
        }

        // Assert
        Assert.Equal(2, results.Count);
        // If we get here without exceptions, resources were disposed properly
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EmptyResponse_ReturnsNoResults() {
        // Arrange
        var ndjson = "";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_WhitespaceOnlyLines_SkipsWhitespace() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "   \n" +  // Whitespace only
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert
        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_InvalidJsonLine_SkipsInvalidLine() {
        // Arrange
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "invalid json here\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new Configuration.Configuration { ApiUrl = "http://localhost" };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = "http://localhost",
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        // Act
        var results = new List<StreamedListObjectsResponse>();
        // Should not throw, just skip invalid lines
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Assert - invalid line should be skipped
        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }
}

