using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Client.ApiExecutor;

public class ApiExecutorTests : IDisposable {
    private readonly string _storeId = "01H0H015178Y2V4CX10C2KGHF4";
    private readonly string _apiUrl = "https://api.fga.example";

    public void Dispose() {
        // Cleanup when everything is done.
    }

    private (OpenFgaClient client, Mock<HttpMessageHandler> handler) CreateMockClient(
        HttpResponseMessage response,
        Func<HttpRequestMessage, bool>? requestValidator = null) {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                requestValidator != null
                    ? ItExpr.Is<HttpRequestMessage>(req => requestValidator(req))
                    : ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(response);

        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        var client = new OpenFgaClient(config, new HttpClient(mockHandler.Object));
        return (client, mockHandler);
    }

    [Fact]
    public async Task ExecuteAsync_ValidGetRequest_ReturnsSuccessResponse() {
        // Arrange
        var expectedResponse = new { id = _storeId, name = "test-store" };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                JsonSerializer.Serialize(expectedResponse),
                Encoding.UTF8,
                "application/json")
        };
        responseMessage.Headers.Add("X-Request-Id", "req-123");

        var (client, _) = CreateMockClient(responseMessage, req =>
            req.Method == HttpMethod.Get &&
            req.RequestUri.ToString().Contains("/stores/" + _storeId));

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        // Act
        var response = await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "GetStore");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.Contains("test-store", response.RawResponse);
        Assert.True(response.Headers.ContainsKey("X-Request-Id"));
    }

    [Fact]
    public async Task ExecuteAsync_ValidPostRequest_ReturnsSuccessResponse() {
        // Arrange
        var requestBody = new { user = "user:anne", relation = "reader", @object = "document:2021-budget" };
        var expectedResponse = new { allowed = true };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                JsonSerializer.Serialize(expectedResponse),
                Encoding.UTF8,
                "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage, req =>
            req.Method == HttpMethod.Post &&
            req.RequestUri.ToString().Contains("/stores/" + _storeId + "/check"));

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        // Act
        var response = await client.GetApiClient().ExecuteAsync<object, dynamic>(requestBuilder, "Check");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public async Task ExecuteAsync_WithPathParams_ReplacesInUrl() {
        // Arrange
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };

        var (client, mockHandler) = CreateMockClient(responseMessage, req =>
            req.RequestUri.ToString().Contains("/stores/" + _storeId + "/check"));

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        // Act
        await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "Check");

        // Assert
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString().Contains("/stores/" + _storeId + "/check")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithQueryParams_AppendsToUrl() {
        // Arrange
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("[]", Encoding.UTF8, "application/json")
        };

        var (client, mockHandler) = CreateMockClient(responseMessage, req =>
            req.RequestUri.ToString().Contains("page_size=20") &&
            req.RequestUri.ToString().Contains("continuation_token=abc123"));

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = _apiUrl,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string> {
                { "page_size", "20" },
                { "continuation_token", "abc123" }
            }
        };

        // Act
        await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "ListStores");

        // Assert
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString().Contains("page_size=20") &&
                req.RequestUri.ToString().Contains("continuation_token=abc123")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithCustomHeaders_IncludesInRequest() {
        // Arrange
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };

        var (client, mockHandler) = CreateMockClient(responseMessage);

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        var options = new ClientRequestOptions {
            Headers = new Dictionary<string, string> {
                { "X-Custom-Header", "custom-value" },
                { "X-Another-Header", "another-value" }
            }
        };

        // Act
        await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "CreateStore", options);

        // Assert
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Headers.Contains("X-Custom-Header") &&
                req.Headers.GetValues("X-Custom-Header").First() == "custom-value" &&
                req.Headers.Contains("X-Another-Header") &&
                req.Headers.GetValues("X-Another-Header").First() == "another-value"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithBody_SerializesToJson() {
        // Arrange
        var requestBody = new { name = "test-store" };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.Created,
            Content = new StringContent(
                JsonSerializer.Serialize(new { id = "new-store-123" }),
                Encoding.UTF8,
                "application/json")
        };

        var (client, mockHandler) = CreateMockClient(responseMessage);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>(),
            Body = requestBody
        };

        // Act
        await client.GetApiClient().ExecuteAsync<object, dynamic>(requestBuilder, "CreateStore");

        // Assert
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Content != null &&
                req.Content.Headers.ContentType.MediaType == "application/json"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteAsync_RawResponse_ReturnsJsonString() {
        // Arrange
        var expectedJson = "{\"id\":\"123\",\"name\":\"test\"}";
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(expectedJson, Encoding.UTF8, "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage);

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        // Act
        var response = await client.GetApiClient().ExecuteAsync(requestBuilder, "GetStore");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedJson, response.Data);
        Assert.Equal(expectedJson, response.RawResponse);
    }

    [Fact]
    public async Task ExecuteAsync_ApiError_ThrowsFgaApiError() {
        // Arrange
        var errorResponse = new {
            code = "invalid_request",
            message = "Invalid request parameters"
        };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent(
                JsonSerializer.Serialize(errorResponse),
                Encoding.UTF8,
                "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage);

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>()
        };

        // Act & Assert
        await Assert.ThrowsAsync<FgaApiValidationError>(async () =>
            await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "Check"));
    }

    [Fact]
    public void GetApiClient_CalledMultipleTimes_ReturnsSameInstance() {
        // Arrange
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        var client = new OpenFgaClient(config);

        // Act
        var apiClient1 = client.GetApiClient();
        var apiClient2 = client.GetApiClient();

        // Assert
        Assert.Same(apiClient1, apiClient2);
    }

    [Fact]
    public async Task ExecuteAsync_TypedResponse_DeserializesCorrectly() {
        // Arrange
        var expectedResponse = new CheckResponse {
            Allowed = true
        };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                JsonSerializer.Serialize(expectedResponse),
                Encoding.UTF8,
                "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage);

        var requestBuilder = new RequestBuilder<object> {
            Method = HttpMethod.Post,
            BasePath = _apiUrl,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = new Dictionary<string, string> { { "store_id", _storeId } },
            QueryParameters = new Dictionary<string, string>(),
            Body = new { user = "user:anne", relation = "reader", @object = "document:test" }
        };

        // Act
        var response = await client.GetApiClient().ExecuteAsync<object, CheckResponse>(requestBuilder, "Check");

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Data);
        Assert.True(response.Data.Allowed);
    }

    [Fact]
    public async Task ExecuteAsync_CancellationToken_CancelsRequest() {
        // Arrange
        using var cts = new CancellationTokenSource();
        cts.Cancel(); // Cancel immediately

        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        var client = new OpenFgaClient(config);

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = _apiUrl,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "ListStores", null, cts.Token));
    }

    [Fact]
    public async Task ExecuteAsync_WithCredentials_IncludesAuthorizationHeader() {
        // Arrange
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        };

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId,
            Credentials = new Credentials {
                Method = CredentialsMethod.ApiToken,
                Config = new CredentialsConfig {
                    ApiToken = "test-token-123"
                }
            }
        };
        var client = new OpenFgaClient(config, new HttpClient(mockHandler.Object));

        var requestBuilder = new RequestBuilder<Any> {
            Method = HttpMethod.Get,
            BasePath = _apiUrl,
            PathTemplate = "/stores",
            PathParameters = new Dictionary<string, string>(),
            QueryParameters = new Dictionary<string, string>()
        };

        // Act
        await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "ListStores");

        // Assert
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Headers.Authorization != null &&
                req.Headers.Authorization.Scheme == "Bearer" &&
                req.Headers.Authorization.Parameter == "test-token-123"),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    [Fact]
    public async Task ExecuteAsync_UsingFluentApi_WorksCorrectly() {
        // Arrange
        var expectedResponse = new { id = _storeId, name = "test-store" };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                JsonSerializer.Serialize(expectedResponse),
                Encoding.UTF8,
                "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage, req =>
            req.Method == HttpMethod.Get &&
            req.RequestUri.ToString().Contains("/stores/" + _storeId));

        // Demonstrate the new fluent API inspired by ApiExecutorRequestBuilder
        var requestBuilder = RequestBuilder<Any>
            .Create(HttpMethod.Get, _apiUrl, "/stores/{store_id}")
            .WithPathParameter("store_id", _storeId);

        // Act
        var response = await client.GetApiClient().ExecuteAsync<Any, dynamic>(requestBuilder, "GetStore");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
        Assert.NotNull(response.Data);
        Assert.Contains("test-store", response.RawResponse);
    }

    [Fact]
    public async Task ExecuteAsync_FluentApiWithMultipleParams_WorksCorrectly() {
        // Arrange
        var requestBody = new { user = "user:anne", relation = "reader", @object = "document:2021-budget" };
        var expectedResponse = new { allowed = true };
        using var responseMessage = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(
                JsonSerializer.Serialize(expectedResponse),
                Encoding.UTF8,
                "application/json")
        };

        var (client, _) = CreateMockClient(responseMessage, req =>
            req.Method == HttpMethod.Post &&
            req.RequestUri.ToString().Contains("/stores/" + _storeId + "/check") &&
            req.RequestUri.ToString().Contains("consistency="));

        // Fluent API with method chaining
        var requestBuilder = RequestBuilder<object>
            .Create(HttpMethod.Post, _apiUrl, "/stores/{store_id}/check")
            .WithPathParameter("store_id", _storeId)
            .WithQueryParameter("consistency", "HIGHER_CONSISTENCY")
            .WithBody(requestBody);

        // Act
        var response = await client.GetApiClient().ExecuteAsync<object, dynamic>(requestBuilder, "Check");

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessful);
    }
}