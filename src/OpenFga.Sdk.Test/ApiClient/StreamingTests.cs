using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Constants;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using Xunit;

namespace OpenFga.Sdk.Test.ApiClient;

/// <summary>
/// Tests for NDJSON streaming functionality in BaseClient
/// </summary>
public class StreamingTests {
    /// <summary>
    /// Custom HttpContent that emits data in controlled chunks to test partial NDJSON handling
    /// </summary>
    private class ChunkedStreamContent : HttpContent {
        private readonly string[] _chunks;
        private readonly int _delayMs;

        public ChunkedStreamContent(string[] chunks, int delayMs = 0) {
            _chunks = chunks;
            _delayMs = delayMs;
            Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-ndjson");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context) {
            foreach (var chunk in _chunks) {
                if (_delayMs > 0) {
                    await Task.Delay(_delayMs);
                }
                var bytes = Encoding.UTF8.GetBytes(chunk);
                await stream.WriteAsync(bytes, 0, bytes.Length);
                await stream.FlushAsync();
            }
        }

        protected override bool TryComputeLength(out long length) {
            length = 0;
            return false; // Unknown length, force streaming
        }
    }

    private Mock<HttpMessageHandler> CreateMockHttpHandler(HttpStatusCode statusCode, string content,
        string contentType = "application/x-ndjson") {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(() => new HttpResponseMessage {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, contentType)
            });
        return mockHandler;
    }

    private Mock<HttpMessageHandler> CreateMockHttpHandlerWithChunks(HttpStatusCode statusCode, string[] chunks, 
        int delayMs = 0) {
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(() => new HttpResponseMessage {
                StatusCode = statusCode,
                Content = new ChunkedStreamContent(chunks, delayMs)
            });
        return mockHandler;
    }

    [Fact]
    public async Task SendStreamingRequestAsync_SingleLineNDJSON_ParsesCorrectly() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Single(results);
        Assert.Equal("document:1", results[0].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_MultipleLineNDJSON_ParsesAllLines() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(3, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
        Assert.Equal("document:3", results[2].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EmptyLines_SkipsEmptyLines() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_LastLineWithoutNewline_ParsesCorrectly() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}"; // No trailing newline
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_CancellationToken_CancelsStream() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        using var cts = new CancellationTokenSource();

        var results = new List<StreamedListObjectsResponse>();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => {
            await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
                requestBuilder, null, "Test", cts.Token)) {
                results.Add(item);
                if (results.Count == 1) {
                    cts.Cancel(); // Cancel after the first item
                }
            }
        });

        // Cancellation happens after the first item, but timing may allow more items before cancellation takes effect
        Assert.True(results.Count >= 1, "At least one item should be processed before cancellation");
    }

    [Fact]
    public async Task SendStreamingRequestAsync_HttpError_ThrowsException() {
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.InternalServerError,
            "{\"code\":\"internal_error\",\"message\":\"Server error\"}",
            "application/json");
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        await Assert.ThrowsAsync<FgaApiInternalError>(async () => {
            await foreach (var _ in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
                requestBuilder, null, "Test")) {
                // Should not get here
            }
        });
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EarlyBreak_DisposesResourcesProperly() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n" +
                     "{\"result\":{\"object\":\"document:3\"}}\n" +
                     "{\"result\":{\"object\":\"document:4\"}}\n" +
                     "{\"result\":{\"object\":\"document:5\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
            if (results.Count == 2) {
                break; // Early termination
            }
        }

        Assert.Equal(2, results.Count);
        // If we get here without exceptions, resources were disposed of properly
    }

    [Fact]
    public async Task SendStreamingRequestAsync_EmptyResponse_ReturnsNoResults() {
        var ndjson = "";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Empty(results);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_WhitespaceOnlyLines_SkipsWhitespace() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "   \n" +  // Whitespace only
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_InvalidJsonLine_SkipsInvalidLine() {
        var ndjson = "{\"result\":{\"object\":\"document:1\"}}\n" +
                     "invalid json here\n" +
                     "{\"result\":{\"object\":\"document:2\"}}\n";
        var mockHandler = CreateMockHttpHandler(HttpStatusCode.OK, ndjson);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

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

    // ============================================================
    // Partial NDJSON Handling Tests
    // ============================================================
    
    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataSplitsJsonMidObject_ParsesCorrectly() {
        // This is the critical test case: data arrives in chunks that split JSON objects mid-line
        // Simulates real-world streaming where network packets don't align with JSON boundaries
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n{\"res", // Ends mid-JSON object
            "ult\":{\"object\":\"document:2\"}}\n"              // Completes the JSON object
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataMultipleSplits_ParsesAllCorrectly() {
        // Test multiple partial chunks across many small reads
        var chunks = new[] {
            "{\"result\":{\"ob",                    // First chunk: partial first object
            "ject\":\"document:1\"}}\n{\"result", // Second chunk: completes first, starts second
            "\":{\"object\":\"document:",           // Third chunk: middle of second object
            "2\"}}\n{\"result\":{\"object\":\"do",  // Fourth chunk: completes second, starts third
            "cument:3\"}}\n"                         // Fifth chunk: completes third
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(3, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
        Assert.Equal("document:3", results[2].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataWithEmptyLines_SkipsEmptyLines() {
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n\n{\"r", // Has empty line, splits second object
            "esult\":{\"object\":\"document:2\"}}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataLastChunkNoNewline_ParsesFinalObject() {
        // Test that final buffered content is parsed even without trailing newline
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n{\"result\":{\"ob",
            "ject\":\"document:2\"}}" // No trailing newline
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataInvalidPartialJson_SkipsInvalidAndContinues() {
        // Test that invalid JSON in the final buffer is skipped gracefully
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n{\"result\":{\"ob",
            "ject\":\"document:2\"}}\n{\"invalid\":"  // Incomplete/invalid JSON at end
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Should parse the two valid objects and skip the invalid trailing fragment
        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataInvalidLineInMiddle_SkipsAndContinues() {
        // Test that invalid JSON in the middle is skipped
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\ninvalid ",
            "json line here\n{\"result\":{\"object\":\"document:2\"}}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_VerySmallChunks_ParsesCorrectly() {
        // Test with very small chunks (even single character chunks)
        var chunks = new[] {
            "{", "\"result\":{\"object\":\"document:1\"}}\n",
            "{\"result\":{\"object\":",
            "\"",
            "document:2\"}",
            "}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataWithLargeObjects_ParsesCorrectly() {
        // Test with larger JSON objects to ensure buffer handles them correctly
        var largeValue = new string('x', 10000); // 10KB of data
        var chunks = new[] {
            $"{{\"result\":{{\"object\":\"document:1\"}}}}\n{{\"result\":{{\"object\":\"",
            $"{largeValue}",
            "\"}}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal(largeValue, results[1].Object);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataCancellation_CancelsCorrectly() {
        // Test cancellation with chunked streaming
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n{\"res",
            "ult\":{\"object\":\"document:2\"}}\n",
            "{\"result\":{\"object\":\"document:3\"}}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks, delayMs: 50);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        using var cts = new CancellationTokenSource();

        var results = new List<StreamedListObjectsResponse>();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => {
            await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
                requestBuilder, null, "Test", cts.Token)) {
                results.Add(item);
                if (results.Count == 1) {
                    cts.Cancel();
                }
            }
        });

        Assert.True(results.Count >= 1);
    }

    [Fact]
    public async Task SendStreamingRequestAsync_ChunkedDataResultPropertyMissing_SkipsObject() {
        // Test that objects without "result" property are skipped
        var chunks = new[] {
            "{\"result\":{\"object\":\"document:1\"}}\n{\"no",
            "_result\":true}\n{\"result\":{\"object\":\"document:2\"}}\n"
        };
        
        var mockHandler = CreateMockHttpHandlerWithChunks(HttpStatusCode.OK, chunks);
        var httpClient = new HttpClient(mockHandler.Object);
        var config = new OpenFga.Sdk.Configuration.Configuration { ApiUrl = FgaConstants.TestApiUrl };
        var baseClient = new BaseClient(config, httpClient);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = FgaConstants.TestApiUrl,
            PathTemplate = "/test",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = new { }
        };

        var results = new List<StreamedListObjectsResponse>();
        await foreach (var item in baseClient.SendStreamingRequestAsync<object, StreamedListObjectsResponse>(
            requestBuilder, null, "Test")) {
            results.Add(item);
        }

        // Should only get the two objects with "result" property
        Assert.Equal(2, results.Count);
        Assert.Equal("document:1", results[0].Object);
        Assert.Equal("document:2", results[1].Object);
    }
}

