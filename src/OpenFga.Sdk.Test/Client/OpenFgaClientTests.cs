//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Exceptions.Parsers;
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

namespace OpenFga.Sdk.Test.Client;

public class OpenFgaClientTests : IDisposable {
    private readonly string _storeId;
    private readonly string _apiUrl = "https://api.fga.example";
    private readonly ClientConfiguration _config;

    private static class TestHeaders {
        public const string RequestId = "X-Request-ID";
        public const string TraceId = "X-Trace-ID";
        public const string SessionId = "X-Session-ID";
        public const string UserId = "X-User-ID";
        public const string CorrelationId = "X-Correlation-ID";
        public const string CustomHeader = "X-Custom-Header";
    }

    public OpenFgaClientTests() {
        _storeId = "01H0H015178Y2V4CX10C2KGHF4";
        _config = new ClientConfiguration() { StoreId = _storeId, ApiUrl = _apiUrl };
    }

    private HttpResponseMessage GetCheckResponse(CheckResponse content, bool shouldRetry = false) {
        var response = new HttpResponseMessage() {
            StatusCode = shouldRetry ? (HttpStatusCode)429 : HttpStatusCode.OK,
            Content = Utils.CreateJsonStringContent(content),
            Headers = { }
        };

        if (shouldRetry) {
            response.Headers.Add(RateLimitParser.RateLimitHeader.LimitRemaining, "0");
            response.Headers.Add(RateLimitParser.RateLimitHeader.LimitResetIn, "100");
            response.Headers.Add(RateLimitParser.RateLimitHeader.LimitTotalInPeriod, "2");
        }

        return response;
    }

    private (OpenFgaClient client, Mock<HttpMessageHandler> handler) CreateTestClientForHeaders<TResponse>(
        TResponse response,
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
            .ReturnsAsync(() => new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(response),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        return (new OpenFgaClient(_config, httpClient), mockHandler);
    }

    private void AssertHeaderPresent(Mock<HttpMessageHandler> mockHandler, string headerName, string expectedValue) {
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.AtLeastOnce(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Headers.Contains(headerName) &&
                req.Headers.GetValues(headerName).First() == expectedValue),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    public void Dispose() {
        // Cleanup when everything is done.
    }

    /// <summary>
    /// Test StoreId validation
    /// </summary>
    [Fact]
    public async Task ConfigurationInValidStoreIdTest() {
        var config = new ClientConfiguration() {
            ApiUrl = _apiUrl,
            StoreId = "invalid-format"
        };
        void ActionInvalidId() => config.EnsureValid();
        var exception = Assert.Throws<FgaValidationError>(ActionInvalidId);
        Assert.Equal("StoreId is not in a valid ulid format", exception.Message);
    }

    /// <summary>
    /// Test Auth Model ID validation
    /// </summary>
    [Fact]
    public async Task ConfigurationInValidAuthorizationModelIdTest() {
        var config = new ClientConfiguration() {
            ApiUrl = _apiUrl,
            StoreId = _config.StoreId,
            AuthorizationModelId = "invalid-format"
        };
        void ActionInvalidId() => new OpenFgaClient(config);
        var exception = Assert.Throws<FgaValidationError>(ActionInvalidId);
        Assert.Equal("AuthorizationModelId is not in a valid ulid format", exception.Message);
    }

    /// <summary>
    /// Test Auth Model ID validation
    /// </summary>
    [Fact]
    public async Task ConfigurationInValidAuthModelIdInOptionsTest() {
        var config = new ClientConfiguration() {
            ApiUrl = _apiUrl,
            StoreId = _config.StoreId,
        };
        var fgaClient = new OpenFgaClient(config);

        async Task<ReadAuthorizationModelResponse> ActionMissingStoreId() => await fgaClient.ReadAuthorizationModel(new ClientReadAuthorizationModelOptions() {
            AuthorizationModelId = "invalid-format"
        });
        var exception = await Assert.ThrowsAsync<FgaValidationError>(ActionMissingStoreId);
        Assert.Equal("AuthorizationModelId is not in a valid ulid format", exception.Message);
    }

    /// <summary>
    /// Test DefaultHeaders with reserved headers should throw
    /// </summary>
    [Theory]
    [InlineData("Content-Type", "application/xml")]
    [InlineData("content-type", "text/plain")]
    [InlineData("CONTENT-TYPE", "application/json")]
    [InlineData("Authorization", "Bearer fake-token")]
    [InlineData("authorization", "Bearer fake-token")]
    [InlineData("Content-Length", "1234")]
    [InlineData("content-length", "1234")]
    [InlineData("Host", "evil.com")]
    [InlineData("host", "evil.com")]
    [InlineData("Accept", "application/xml")]
    [InlineData("accept", "application/xml")]
    [InlineData("Accept-Encoding", "gzip")]
    [InlineData("accept-encoding", "gzip")]
    [InlineData("Transfer-Encoding", "chunked")]
    [InlineData("transfer-encoding", "chunked")]
    [InlineData("Connection", "close")]
    [InlineData("connection", "close")]
    [InlineData("Cookie", "sessionid=abc123")]
    [InlineData("cookie", "sessionid=abc123")]
    [InlineData("Set-Cookie", "sessionid=abc123")]
    [InlineData("set-cookie", "sessionid=abc123")]
    [InlineData("Date", "Mon, 01 Jan 2024 00:00:00 GMT")]
    [InlineData("date", "Mon, 01 Jan 2024 00:00:00 GMT")]
    public void EnsureValid_WithReservedDefaultHeader_ShouldThrowArgumentException(string headerName, string headerValue) {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders[headerName] = headerValue;

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());

        Assert.Contains("is a reserved HTTP header", exception.Message);
        Assert.Contains("should not be set via custom headers", exception.Message);
        Assert.Contains(headerName, exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Test DefaultHeaders with valid custom headers should not throw
    /// </summary>
    [Fact]
    public void EnsureValid_WithValidCustomDefaultHeaders_ShouldNotThrow() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders["X-Custom-Header"] = "custom-value";
        config.DefaultHeaders["X-Request-ID"] = "req-123";
        config.DefaultHeaders["X-Correlation-ID"] = "corr-456";

        config.EnsureValid();
    }

    /// <summary>
    /// Test DefaultHeaders with empty header name should throw
    /// </summary>
    [Fact]
    public void EnsureValid_WithEmptyDefaultHeaderName_ShouldThrowArgumentException() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders[""] = "value";

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());
        Assert.Contains("Header name cannot be null, empty, or whitespace", exception.Message);
    }

    /// <summary>
    /// Test DefaultHeaders with null header value should throw
    /// </summary>
    [Fact]
    public void EnsureValid_WithNullDefaultHeaderValue_ShouldThrowArgumentException() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders["X-Custom"] = null!;

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());
        Assert.Contains("has a null value", exception.Message);
    }

    /// <summary>
    /// Test DefaultHeaders with header injection should throw
    /// </summary>
    [Fact]
    public void EnsureValid_WithHeaderInjectionInDefaultHeaders_ShouldThrowArgumentException() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders["X-Custom"] = "value\r\nX-Injected: malicious";

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());
        Assert.Contains("CR/LF", exception.Message);
        Assert.Contains("header injection", exception.Message);
    }

    /// <summary>
    /// Test Content-Type in DefaultHeaders should throw with specific error
    /// </summary>
    [Fact]
    public void EnsureValid_ContentTypeInDefaultHeaders_ShouldThrowWithSpecificError() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders["Content-Type"] = "application/xml";

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());

        Assert.Contains("Content-Type", exception.Message);
        Assert.Contains("reserved", exception.Message);
        Assert.Contains("DefaultHeaders", exception.ParamName);
    }

    /// <summary>
    /// Test Content-Type in DefaultHeaders is case-insensitive
    /// </summary>
    [Fact]
    public void EnsureValid_ContentTypeInDefaultHeaders_CaseInsensitive_ShouldThrow() {
        var casings = new[] { "content-type", "CONTENT-TYPE", "Content-type", "CoNtEnT-tYpE" };

        foreach (var casing in casings) {
            var config = new ClientConfiguration {
                ApiUrl = _apiUrl,
                StoreId = _storeId
            };
            config.DefaultHeaders[casing] = "application/xml";

            var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());
            Assert.Contains("reserved", exception.Message);
        }
    }

    /// <summary>
    /// Test Authorization in DefaultHeaders should throw with specific error
    /// </summary>
    [Fact]
    public void EnsureValid_AuthorizationInDefaultHeaders_ShouldThrowWithSpecificError() {
        var config = new ClientConfiguration {
            ApiUrl = _apiUrl,
            StoreId = _storeId
        };
        config.DefaultHeaders["Authorization"] = "Bearer custom-token";

        var exception = Assert.Throws<ArgumentException>(() => config.EnsureValid());

        Assert.Contains("Authorization", exception.Message);
        Assert.Contains("reserved", exception.Message);
        Assert.Contains("authentication failures", exception.Message);
    }

    /// <summary>
    /// Test that updating StoreId after initialization works
    /// </summary>
    [Fact]
    public void UpdateStoreIdTest() {
        var config = new ClientConfiguration() { ApiUrl = _apiUrl };
        var fgaClient = new OpenFgaClient(config);
        Assert.Null(fgaClient.StoreId);
        var storeId = "some-id";
        fgaClient.StoreId = storeId;
        Assert.Equal(storeId, fgaClient.StoreId);
    }

    /// <summary>
    /// Test that updating AuthorizationModelId after initialization works
    /// </summary>
    [Fact]
    public void UpdateAuthorizationModelIdTest() {
        var config = new ClientConfiguration() { ApiUrl = _apiUrl };
        var fgaClient = new OpenFgaClient(config);
        Assert.Null(fgaClient.AuthorizationModelId);
        var modelId = "some-id";
        fgaClient.AuthorizationModelId = modelId;
        Assert.Equal(modelId, fgaClient.AuthorizationModelId);
    }

    /**********
     * Stores *
     **********/

    /// <summary>
    /// Test ListStores
    /// </summary>
    [Fact]
    public async Task ListStoresTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new ListStoresResponse() {
            Stores = new List<Store>() {
                new() {Id = "45678", Name = "TestStore", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now}
            }
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores?name=TestStore&") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ListStores(
            new ClientListStoresRequest() {
                Name = "TestStore"
            },
            new ClientListStoresOptions() { }
        );

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores?name=TestStore&") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<ListStoresResponse>(response);
        Assert.Single(response.Stores);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test ListStores with Null DateTime
    /// </summary>
    [Fact]
    public async Task ListStoresTestNullDeletedAt() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var content =
            "{ \"stores\": [{\"id\": \"xyz123\", \"name\": \"abcdefg\", \"created_at\": \"2022-10-07T14:00:40.205Z\", \"updated_at\": \"2022-10-07T14:00:40.205Z\", \"deleted_at\": null}], \"continuation_token\": \"eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==\"}";
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ListStores(new ClientListStoresRequest());
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<ListStoresResponse>(response);
        Assert.Single(response.Stores);
        Assert.Equal("xyz123", response.Stores[0].Id);
        Assert.Equal("abcdefg", response.Stores[0].Name);
        Assert.Null(response.Stores[0].DeletedAt);
    }

    /// <summary>
    /// Test ListStores for empty array
    /// </summary>
    [Fact]
    public async Task ListStoresEmptyTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new ListStoresResponse() { Stores = new List<Store>() { } };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ListStores(new ClientListStoresRequest());
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<ListStoresResponse>(response);
        Assert.Empty(response.Stores);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test CreateStore
    /// </summary>
    [Fact]
    public async Task CreateStoreTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new CreateStoreResponse() { Id = "45678", Name = "TestStore", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.CreateStore(new ClientCreateStoreRequest() { Name = "FGA Test Store" });
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<CreateStoreResponse>(response);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test GetStore
    /// </summary>
    [Fact]
    public async Task GetStoreTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new GetStoreResponse() { Id = "45678", Name = "TestStore", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_config.StoreId}") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.GetStore();
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{fgaClient.StoreId}") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<GetStoreResponse>(response);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test DeleteStore
    /// </summary>
    [Fact]
    public async Task DeleteStoreTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_config.StoreId}") &&
                    req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.NoContent
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        await fgaClient.DeleteStore();
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{fgaClient.StoreId}") &&
                req.Method == HttpMethod.Delete),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /************************
     * Authorization Models *
     ************************/

    /// <summary>
    /// Test WriteAuthorizationModel
    /// </summary>
    [Fact]
    public async Task WriteAuthorizationModelTest() {
        const string authorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1";
        var body = new ClientWriteAuthorizationModelRequest {
            SchemaVersion = "1.1",
            TypeDefinitions = new List<TypeDefinition> {
                new() {
                    Type = "user", Relations = new Dictionary<string, Userset>()
                },
                new() {
                    Type = "document",
                    Relations = new Dictionary<string, Userset> {
                        {
                            "writer", new Userset {
                                This = new object()
                            }
                        }, {
                            "viewer", new Userset {
                                Union = new Usersets {
                                    Child = new List<Userset> {
                                        new() {
                                            This = new object()
                                        },
                                        new() {
                                            ComputedUserset = new ObjectRelation {
                                                Relation = "writer"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Metadata = new Metadata {
                        Relations = new Dictionary<string, RelationMetadata> {
                            {
                                "writer", new RelationMetadata {
                                    DirectlyRelatedUserTypes = new List<RelationReference> {
                                        new() {
                                            Type = "user"
                                        }
                                    }
                                }
                            }, {
                                "viewer", new RelationMetadata {
                                    DirectlyRelatedUserTypes = new List<RelationReference> {
                                        new() {
                                            Type = "user"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(
                    new WriteAuthorizationModelResponse() { AuthorizationModelId = authorizationModelId }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.WriteAuthorizationModel(body);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<WriteAuthorizationModelResponse>(response);
    }

    /// <summary>
    /// Test ReadAuthorizationModel
    /// </summary>
    [Fact]
    public async Task ReadAuthorizationModelTest() {
        const string authorizationModelId = "01FMJA27YCE3QAT8RDS9VZFN0T";

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri(
                        $"{_config.BasePath}/stores/{_config.StoreId}/authorization-models/{authorizationModelId}") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new ReadAuthorizationModelResponse() {
                    AuthorizationModel = new AuthorizationModel(id: authorizationModelId,
                        typeDefinitions: new List<TypeDefinition>(), schemaVersion: "1.1")
                }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ReadAuthorizationModel(new ClientReadAuthorizationModelOptions {
            AuthorizationModelId = authorizationModelId,
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri ==
                new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models/{authorizationModelId}") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ReadAuthorizationModelResponse>(response);
        Assert.Equal(authorizationModelId, response.AuthorizationModel.Id);
    }
    /// <summary>
    /// Test ReadAuthorizationModel
    /// </summary>
    [Fact]
    public async Task ReadAuthorizationModelModelIdInConfigTest() {
        const string authorizationModelId = "01FMJA27YCE3QAT8RDS9VZFN0T";

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri(
                        $"{_config.BasePath}/stores/{_config.StoreId}/authorization-models/{authorizationModelId}") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new ReadAuthorizationModelResponse() {
                    AuthorizationModel = new AuthorizationModel(id: authorizationModelId,
                        typeDefinitions: new List<TypeDefinition>(), schemaVersion: "1.1")
                }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(new ClientConfiguration(_config) {
            StoreId = _storeId,
            AuthorizationModelId = authorizationModelId,
        }, httpClient);

        var response = await fgaClient.ReadAuthorizationModel();

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri ==
                new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models/{authorizationModelId}") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ReadAuthorizationModelResponse>(response);
        Assert.Equal(authorizationModelId, response.AuthorizationModel.Id);
    }

    /// <summary>
    /// Test ReadAuthorizationModel
    /// </summary>
    [Fact]
    public async Task ReadLatestAuthorizationModelTest() {
        const string authorizationModelId = "01FMJA27YCE3QAT8RDS9VZFN0T";

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri(
                        $"{_config.BasePath}/stores/{_config.StoreId}/authorization-models?page_size=1&") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new ReadAuthorizationModelsResponse() {
                    AuthorizationModels = new List<AuthorizationModel>() {new (id: authorizationModelId,
                        typeDefinitions: new List<TypeDefinition>(), schemaVersion: "1.1")}
                }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ReadLatestAuthorizationModel();

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri ==
                new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models?page_size=1&") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ReadAuthorizationModelResponse>(response);
        Assert.Equal(authorizationModelId, response.AuthorizationModel.Id);
    }

    /***********************
     * Relationship Tuples *
     ***********************/


    /// <summary>
    /// Test ReadChanges
    /// </summary>
    [Fact]
    public async Task ReadChangesTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new ReadChangesResponse() {
            Changes = new List<TupleChange>() {
                new(new TupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                    },
                    TupleOperation.TUPLEOPERATIONWRITE, DateTime.Now),
            },
            ContinuationToken =
                "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ=="
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString()
                        .StartsWith($"{_config.BasePath}/stores/{_config.StoreId}/changes") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var type = "repo";
        var pageSize = 25;
        var startTime = DateTime.Parse("2022-01-01T00:00:00Z");
        var continuationToken =
            "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==";
        var response = await fgaClient.ReadChanges(new ClientReadChangesRequest { Type = type, StartTime = startTime }, new ClientReadChangesOptions {
            PageSize = pageSize,
            ContinuationToken = continuationToken,
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{_config.BasePath}/stores/{_config.StoreId}/changes") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ReadChangesResponse>(response);
        Assert.Single(response.Changes);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test Read
    /// </summary>
    [Fact]
    public async Task ReadTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = "";
        var expectedResponse = new ReadResponse() {
            Tuples = new List<Model.Tuple>() {
                new(new TupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                    },
                    DateTime.Now)
            }
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, token) => {
                capturedContent = req.Content.ReadAsStringAsync().Result;
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientReadRequest() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
        };
        var options = new ClientReadOptions { };
        var response = await fgaClient.Read(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.Contains("tuple", capturedContent);

        Assert.IsType<ReadResponse>(response);
        Assert.Single(response.Tuples);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test Read with Empty Body
    /// </summary>
    [Fact]
    public async Task ReadEmptyTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = "";
        var expectedResponse = new ReadResponse() {
            Tuples = new List<Model.Tuple>() {
                new(new TupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                    },
                    DateTime.Now)
            }
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, token) => {
                capturedContent = req.Content.ReadAsStringAsync().Result;
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientReadRequest() { };
        var options = new ClientReadOptions { };
        var response = await fgaClient.Read(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.DoesNotContain("tuple", capturedContent);

        Assert.IsType<ReadResponse>(response);
        Assert.Single(response.Tuples);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test Write (Write Relationship Tuples)
    /// </summary>
    [Fact]
    public async Task WriteWriteTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new Object()),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientWriteRequest() {
            Writes = new List<ClientTupleKey> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                }
            },
            Deletes = new List<ClientTupleKeyWithoutCondition>(), // should not get passed
        };
        var response = await fgaClient.Write(body, new ClientWriteOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /// <summary>
    /// Test Write (Delete Relationship Tuples)
    /// </summary>
    [Fact]
    public async Task WriteDeleteTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new Object()),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientWriteRequest() {
            Writes = new List<ClientTupleKey> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                }
            },
        };
        var response = await fgaClient.Write(body, new ClientWriteOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /// <summary>
    /// Test Write (Write and Delete Relationship Tuples of a Specific Authorization Model ID)
    /// </summary>
    [Fact]
    public async Task WriteMixedWithAuthorizationModelIdTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new Object()),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientWriteRequest() {
            Writes = new List<ClientTupleKey> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                },
            },
            Deletes = new List<ClientTupleKeyWithoutCondition> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "writer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                }
            },
        };
        var response = await fgaClient.Write(body, new ClientWriteOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /// <summary>
    /// Test Write (Non-transaction mode)
    /// </summary>
    [Fact]
    public async Task WriteNonTransactionTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new Object()),
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.NotFound,
                Content = Utils.CreateJsonStringContent(new Object()),
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new Object()),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientWriteRequest() {
            Writes = new List<ClientTupleKey> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                },
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:budget",
                }
            },
            Deletes = new List<ClientTupleKeyWithoutCondition> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "writer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                }
            },
        };
        var options = new ClientWriteOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
            Transaction = new TransactionOptions() {
                Disable = true,
                MaxParallelRequests = 1,
                MaxPerChunk = 1,
            }
        };
        var response = await fgaClient.Write(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(3),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/write") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
        Assert.IsType<ClientWriteResponse>(response);
    }

    /************************
     * Relationship Queries *
     ************************/
    /// <summary>
    /// Test Check
    /// </summary>
    [Fact]
    public async Task CheckTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientCheckRequest {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            ContextualTuples = new List<ClientTupleKey>() {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "editor",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    Condition = new RelationshipCondition() {
                        Name = "ViewCountLessThan200",
                        Context = new { Name = "Roadmap", Type = "document" }
                    }
                }
            },
            Context = new { ViewCount = 100 }
        };
        var options = new ClientCheckOptions { };
        var response = await fgaClient.Check(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<CheckResponse>(response);
        Assert.True(response.Allowed);
    }

    /// <summary>
    /// Test Check with Consistency
    /// </summary>
    [Fact]
    public async Task CheckWithConsistencyTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = null;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => {
                // Eagerly read the content before disposal
                if (request.Content != null) {
                    capturedContent = request.Content.ReadAsStringAsync().Result;
                }
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientCheckRequest {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            ContextualTuples = new List<ClientTupleKey>() {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "editor",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    Condition = new RelationshipCondition() {
                        Name = "ViewCountLessThan200",
                        Context = new { Name = "Roadmap", Type = "document" }
                    }
                }
            },
            Context = new { ViewCount = 100 },
        };
        var options = new ClientCheckOptions {
            Consistency = ConsistencyPreference.MINIMIZELATENCY
        };
        var response = await fgaClient.Check(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        // Verify the captured content contains the expected consistency setting
        Assert.Contains("MINIMIZE_LATENCY", capturedContent);

        Assert.IsType<CheckResponse>(response);
        Assert.True(response.Allowed);
    }
    /// <summary>
    /// Test BatchCheck
    /// </summary>
    [Fact]
    public async Task BatchCheckTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = null;
        int callCounter = 0;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => {
                // Eagerly read the content before disposal
                if (request.Content != null) {
                    capturedContent = request.Content.ReadAsStringAsync().Result;
                }
            })
            .Returns<HttpRequestMessage, CancellationToken>((request, token) => {
                callCounter++;
                return callCounter switch {
                    1 or 3 => Task.Run(async () => {
                        await Task.Delay(500);
                        return new HttpResponseMessage() {
                            StatusCode = HttpStatusCode.OK,
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                        };
                    }),
                    2 => Task.Run(async () => {
                        await Task.Delay(500);
                        return new HttpResponseMessage() {
                            StatusCode = HttpStatusCode.OK,
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = false }),
                        };
                    }),
                    _ => Task.Run(async () => {
                        await Task.Delay(500);
                        return new HttpResponseMessage() {
                            StatusCode = HttpStatusCode.NotFound,
                            Content = Utils.CreateJsonStringContent(new Object { }),
                        };
                    })
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new List<ClientCheckRequest>(){
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "viewer",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    }
                },
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "admin",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    }
                },
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "creator",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "deleter",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            }
        };
        var options = new ClientBatchCheckOptions {
            Consistency = ConsistencyPreference.HIGHERCONSISTENCY
        };
        var response = await fgaClient.BatchCheck(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(4),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        // Verify the captured content contains the expected consistency setting
        Assert.Contains("HIGHER_CONSISTENCY", capturedContent);

        Assert.IsType<ClientBatchCheckClientResponse>(response);

        var allowedResponses = response.Responses.FindAll(res => res.Allowed);
        Assert.Equal(2, allowedResponses.Count);
        var notAllowedResponses = response.Responses.FindAll(res => !res.Allowed);
        Assert.Equal(2, notAllowedResponses.Count);
        var failedResponses = response.Responses.FindAll(res => res.Error != null);
        Assert.Single(failedResponses);
    }

    /// <summary>
    /// Test Expand
    /// </summary>
    [Fact]
    public async Task ExpandTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = "";

        var jsonResponse =
            "{\"tree\":{\"root\":{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"union\":{\"nodes\":[{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"users\":{\"users\":[\"team:product#member\"]}}}, {\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"tupleToUserset\":{\"tupleset\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"computed\":[{\"userset\":\"org:contoso#admin\"}]}}}]}}}}";
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/expand") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((req, token) => {
                capturedContent = req.Content.ReadAsStringAsync().Result;
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json"),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientExpandRequest {
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            ContextualTuples = new List<ClientTupleKey>() {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "editor",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                }
            },
        };
        var response = await fgaClient.Expand(body, new ClientExpandOptions() {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
            Consistency = ConsistencyPreference.HIGHERCONSISTENCY
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/expand") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.Contains("HIGHER_CONSISTENCY", capturedContent);
        Assert.Contains("user:81684243-9356-4421-8fbf-a4f8d36aa31b", capturedContent);

        Assert.IsType<ExpandResponse>(response);
        ExpandResponse expectedResponse = JsonSerializer.Deserialize<ExpandResponse>(jsonResponse);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test Expand with Complex Response
    /// </summary>
    [Fact]
    public async Task ExpandComplexResponseTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var mockResponse = new ExpandResponse(
            tree: new UsersetTree(
                root: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a1#owner",
                    union: new Nodes(
                        varNodes: new List<Node>() {
                            new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a2#owner",
                                leaf: new Leaf(users: new Users(varUsers: new List<string>() {"team:product#member"}))),
                            new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a3#owner",
                                leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(
                                    tupleset: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner",
                                    computed: new List<Computed>() {new Computed(userset: "org:contoso#admin")}))),
                        }),
                    difference: new UsersetTreeDifference(
                        varBase: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a3#owner",
                            leaf: new Leaf(users: new Users(varUsers: new List<string>() { "team:product#member" }))),
                        subtract: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a4#owner",
                            leaf: new Leaf(users: new Users(varUsers: new List<string>() { "team:product#member" })))
                    ),
                    intersection: new Nodes(
                        varNodes: new List<Node>() {
                            new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a5#owner",
                                leaf: new Leaf(users: new Users(varUsers: new List<string>() {"team:product#commentor"}))),
                            new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a6#owner",
                                leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(
                                    tupleset: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#viewer",
                                    computed: new List<Computed>() {new Computed(userset: "org:contoso#owner")}))),
                        }))
            ));

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/expand") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(mockResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientExpandRequest {
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
        };
        var response = await fgaClient.Expand(body, new ClientExpandOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/expand") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ExpandResponse>(response);
        Assert.Equal(response, mockResponse);
    }

    /// <summary>
    /// Test ListObjects
    /// </summary>
    [Fact]
    public async Task ListObjectsTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new ListObjectsResponse { Objects = new List<string> { "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a" } };
        string capturedContent = null;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/list-objects") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => {
                // Eagerly read the content before disposal
                if (request.Content != null) {
                    capturedContent = request.Content.ReadAsStringAsync().Result;
                }
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientListObjectsRequest {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "can_read",
            Type = "document",
            ContextualTuples = new List<ClientTupleKey> {
                new() {
                    User = "folder:product",
                    Relation = "editor",
                    Object = "folder:product",
                },
                new() {
                    User = "folder:product",
                    Relation = "parent",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                },
            },
        };
        var response = await fgaClient.ListObjects(body, new ClientListObjectsOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
            Consistency = ConsistencyPreference.HIGHERCONSISTENCY
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/list-objects") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        // Verify the captured content contains the expected consistency setting
        Assert.Contains("HIGHER_CONSISTENCY", capturedContent);

        Assert.IsType<ListObjectsResponse>(response);
        Assert.Single(response.Objects);
        Assert.Equal(response, expectedResponse);
    }

    /// <summary>
    /// Test ListRelations
    /// </summary>
    [Fact]
    public async Task ListRelationsTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = null;
        int callCounter = 0;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => {
                // Eagerly read the content before disposal
                if (request.Content != null) {
                    capturedContent = request.Content.ReadAsStringAsync().Result;
                }
            })
            .Returns<HttpRequestMessage, CancellationToken>((request, token) => {
                callCounter++;
                return callCounter switch {
                    1 or 3 => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.OK,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    }),
                    2 or 4 => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.OK,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = false }),
                    }),
                    _ => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = Utils.CreateJsonStringContent(new Object { }),
                    })
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body =
            new ClientListRelationsRequest() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                Relations = new List<string> { "can_view", "can_edit", "can_delete", "can_rename" },
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    }
                }
            };
        var options = new ClientListRelationsOptions { Consistency = ConsistencyPreference.MINIMIZELATENCY };
        var response = await fgaClient.ListRelations(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(4),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        // Verify the captured content contains the expected consistency setting
        Assert.Contains("MINIMIZE_LATENCY", capturedContent);

        Assert.IsType<ListRelationsResponse>(response);
        Assert.Equal(2, response.Relations.Count);
        // TODO: Ensure the relations are correct, currently because the mocks are generic and we process in parallel,
        // we do not know what order they will be processed in
    }

    /// <summary>
    /// Test ListRelations: One of the relations is not found
    /// </summary>
    [Fact]
    public async Task ListRelationsRelationNotFoundTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        string capturedContent = null;
        int callCounter = 0;

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Callback<HttpRequestMessage, CancellationToken>((request, token) => {
                // Eagerly read the content before disposal
                if (request.Content != null) {
                    capturedContent = request.Content.ReadAsStringAsync().Result;
                }
            })
            .Returns<HttpRequestMessage, CancellationToken>((request, token) => {
                callCounter++;
                return callCounter switch {
                    1 or 3 => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.OK,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    }),
                    2 => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.OK,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = false }),
                    }),
                    _ => Task.FromResult(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.NotFound,
                        Content = Utils.CreateJsonStringContent(new Object { }),
                    })
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body =
            new ClientListRelationsRequest() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                Relations = new List<string> { "can_view", "can_edit", "can_delete", "can_rename" },
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                    }
                }
            };
        var options = new ClientListRelationsOptions { Consistency = ConsistencyPreference.MINIMIZELATENCY };
        Task<ListRelationsResponse> ApiError() => fgaClient.ListRelations(body, options);
        var error = await Assert.ThrowsAsync<FgaApiNotFoundError>(ApiError);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(4),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        // Verify the captured content contains the expected consistency setting
        Assert.Contains("MINIMIZE_LATENCY", capturedContent);
    }

    /// <summary>
    /// Test ListRelationsNoRelationsProvided
    /// </summary>
    [Fact]
    public async Task ListRelationsNoRelationsProvidedTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body =
            new ClientListRelationsRequest() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                Relations = new List<string> { },
            };

        Task<ListRelationsResponse> ApiError() => fgaClient.ListRelations(body);
        var error = await Assert.ThrowsAsync<FgaValidationError>(ApiError);
        Assert.Equal("At least 1 relation to check has to be provided when calling ListRelations", error.Message);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(0),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /// <summary>
    /// Test ListUsers
    /// </summary>
    [Fact]
    public async Task ListUsersTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var expectedResponse = new ListUsersResponse {
            // A real API would not return all these for the filter provided, these are just for test purposes
            Users = new List<User> {
                new () {
                    Object = new FgaObject {
                        Type = "user",
                        Id = "81684243-9356-4421-8fbf-a4f8d36aa31b"
                    }
                },
                new () {
                    Userset = new UsersetUser() {
                        Type = "team",
                        Id = "fga",
                        Relation = "member"
                    }
                },
                new () {
                    Wildcard = new TypedWildcard() {
                        Type = "user"
                    }
                }

            },
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-users") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientListUsersRequest {
            Relation = "can_read",
            Object = new FgaObject {
                Type = "document",
                Id = "roadmap"
            },
            UserFilters = new List<UserTypeFilter> {
                // API does not allow sending multiple filters, done here for testing purposes
                new() {
                    Type = "user"
                },
                new() {
                    Type = "team",
                    Relation = "member"
                }
            },
            ContextualTuples = new List<ClientTupleKey> {
                new() {
                    User = "folder:product",
                    Relation = "editor",
                    Object = "folder:product",
                },
                new() {
                    User = "folder:product",
                    Relation = "parent",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
                },
            },
            Context = new { ViewCount = 100 }
        };

        var response = await fgaClient.ListUsers(body, new ClientListUsersOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-users") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ListUsersResponse>(response);
        Assert.Equal(3, response.Users.Count);
        Assert.Equal(response, expectedResponse);
    }

    /**************
     * Assertions *
     **************/

    /// <summary>
    /// Test ReadAssertions
    /// </summary>
    [Fact]
    public async Task ReadAssertionsTest() {
        const string authorizationModelId = "01FMJA27YCE3QAT8RDS9VZFN0T";

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_config.StoreId}/assertions/{authorizationModelId}") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new ReadAssertionsResponse(authorizationModelId,
                    assertions: new List<Assertion>())),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var response = await fgaClient.ReadAssertions(new ClientReadAssertionsOptions {
            AuthorizationModelId = authorizationModelId,
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri ==
                new Uri($"{_config.BasePath}/stores/{_config.StoreId}/assertions/{authorizationModelId}") &&
                req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ReadAssertionsResponse>(response);
        Assert.Equal(authorizationModelId, response.AuthorizationModelId);
        Assert.Empty(response.Assertions);
    }

    /// <summary>
    /// Test WriteAssertions
    /// </summary>
    [Fact]
    public async Task WriteAssertionsTest() {
        const string authorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1";
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_config.StoreId}/assertions/{authorizationModelId}") &&
                    req.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.NoContent, });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new List<ClientAssertion>() {new ClientAssertion() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a",
            Expectation = true,
        }};

        await fgaClient.WriteAssertions(body, new ClientWriteAssertionsOptions {
            AuthorizationModelId = authorizationModelId,
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri ==
                new Uri($"{_config.BasePath}/stores/{_config.StoreId}/assertions/{authorizationModelId}") &&
                req.Method == HttpMethod.Put),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    #region Custom Headers Tests

    /// <summary>
    /// Test that Client*Options classes implement Headers property
    /// </summary>
    [Theory]
    [InlineData(typeof(ClientCheckOptions))]
    [InlineData(typeof(ClientWriteOptions))]
    [InlineData(typeof(ClientExpandOptions))]
    [InlineData(typeof(ClientListObjectsOptions))]
    [InlineData(typeof(ClientReadOptions))]
    public void ClientOptions_ShouldImplementHeadersProperty(Type optionsType) {
        var options = Activator.CreateInstance(optionsType) as IClientRequestOptions;
        Assert.NotNull(options);

        var headers = new Dictionary<string, string> {
            { TestHeaders.RequestId, "test-123" }
        };

        options!.Headers = headers;
        Assert.NotNull(options.Headers);
        Assert.Equal("test-123", options.Headers[TestHeaders.RequestId]);
    }

    /// <summary>
    /// Test that Client*Options classes allow null headers
    /// </summary>
    [Fact]
    public void ClientOptions_ShouldAllowNullHeaders() {
        var options = new ClientCheckOptions { Headers = null };
        Assert.Null(options.Headers);
    }

    /// <summary>
    /// Test Check with custom headers includes headers in request
    /// </summary>
    [Fact]
    public async Task Check_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new CheckResponse() { Allowed = true };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains(TestHeaders.RequestId) &&
            req.Headers.GetValues(TestHeaders.RequestId).First() == "test-123" &&
            req.Headers.Contains(TestHeaders.CustomHeader) &&
            req.Headers.GetValues(TestHeaders.CustomHeader).First() == "custom-value"
        );

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.RequestId, "test-123" },
                { TestHeaders.CustomHeader, "custom-value" }
            }
        };

        var response = await client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        Assert.True(response.Allowed);
        AssertHeaderPresent(mockHandler, TestHeaders.RequestId, "test-123");
        AssertHeaderPresent(mockHandler, TestHeaders.CustomHeader, "custom-value");
    }

    /// <summary>
    /// Test Check with null headers should not fail
    /// </summary>
    [Fact]
    public async Task Check_WithNullHeaders_ShouldNotFail() {
        var expectedResponse = new CheckResponse() { Allowed = false };
        var (client, _) = CreateTestClientForHeaders(expectedResponse);

        var options = new ClientCheckOptions { Headers = null };

        var response = await client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        Assert.False(response.Allowed);
    }

    /// <summary>
    /// Test Check with invalid header value (CRLF injection) should throw
    /// </summary>
    [Fact]
    public async Task Check_WithInvalidHeaderValue_ShouldThrowArgumentException() {
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.RequestId, "value\r\nX-Injected: malicious" }
            }
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                options
            )
        );

        Assert.Contains("CR/LF", exception.Message);
        Assert.Contains("header injection", exception.Message);
    }

    /// <summary>
    /// Test Write with custom headers
    /// </summary>
    [Fact]
    public async Task Write_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var (client, mockHandler) = CreateTestClientForHeaders(new { }, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains(TestHeaders.TraceId) &&
            req.Headers.GetValues(TestHeaders.TraceId).First() == "trace-456"
        );

        var options = new ClientWriteOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.TraceId, "trace-456" }
            },
            Transaction = new TransactionOptions()
        };

        await client.Write(
            new ClientWriteRequest {
                Writes = new List<ClientTupleKey> {
                    new() { User = "user:anne", Relation = "writer", Object = "document:budget" }
                }
            },
            options
        );

        AssertHeaderPresent(mockHandler, TestHeaders.TraceId, "trace-456");
    }

    /// <summary>
    /// Test Read with custom headers
    /// </summary>
    [Fact]
    public async Task Read_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ReadResponse() { Tuples = new List<Model.Tuple>() };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/read") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains(TestHeaders.SessionId)
        );

        var options = new ClientReadOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.SessionId, "session-xyz" }
            }
        };

        var response = await client.Read(
            new ClientReadRequest { User = "user:anne" },
            options
        );

        Assert.NotNull(response.Tuples);
        AssertHeaderPresent(mockHandler, TestHeaders.SessionId, "session-xyz");
    }

    /// <summary>
    /// Test Expand with custom headers
    /// </summary>
    [Fact]
    public async Task Expand_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ExpandResponse() {
            Tree = new UsersetTree() { Root = new Node() }
        };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/expand") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains(TestHeaders.UserId)
        );

        var options = new ClientExpandOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.UserId, "user-789" }
            }
        };

        var response = await client.Expand(
            new ClientExpandRequest {
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        Assert.NotNull(response.Tree);
        AssertHeaderPresent(mockHandler, TestHeaders.UserId, "user-789");
    }

    /// <summary>
    /// Test ListObjects with custom headers
    /// </summary>
    [Fact]
    public async Task ListObjects_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ListObjectsResponse() { Objects = new List<string>() };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-objects") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains(TestHeaders.CorrelationId)
        );

        var options = new ClientListObjectsOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.CorrelationId, "corr-abc" }
            }
        };

        var response = await client.ListObjects(
            new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "reader",
                Type = "document"
            },
            options
        );

        Assert.NotNull(response.Objects);
        AssertHeaderPresent(mockHandler, TestHeaders.CorrelationId, "corr-abc");
    }

    /// <summary>
    /// Test ListUsers with custom headers
    /// </summary>
    [Fact]
    public async Task ListUsers_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ListUsersResponse() { Users = new List<User>() };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-users") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains("X-List-Users")
        );

        var options = new ClientListUsersOptions {
            Headers = new Dictionary<string, string> {
                { "X-List-Users", "users-123" }
            }
        };

        var response = await client.ListUsers(
            new ClientListUsersRequest {
                Object = new FgaObject { Type = "document", Id = "budget" },
                Relation = "reader",
                UserFilters = new List<UserTypeFilter>()
            },
            options
        );

        Assert.NotNull(response.Users);
        AssertHeaderPresent(mockHandler, "X-List-Users", "users-123");
    }

    /// <summary>
    /// Test CreateStore with custom headers
    /// </summary>
    [Fact]
    public async Task CreateStore_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new CreateStoreResponse() {
            Id = "01H0H015178Y2V4CX10C2KGHF6",
            Name = "Test Store"
        };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri == new Uri($"{_config.BasePath}/stores") &&
            req.Method == HttpMethod.Post &&
            req.Headers.Contains("X-Store-Create")
        );

        var options = new ClientCreateStoreOptions {
            Headers = new Dictionary<string, string> {
                { "X-Store-Create", "create-456" }
            }
        };

        var response = await client.CreateStore(
            new ClientCreateStoreRequest { Name = "Test Store" },
            options
        );

        Assert.Equal("Test Store", response.Name);
        AssertHeaderPresent(mockHandler, "X-Store-Create", "create-456");
    }

    /// <summary>
    /// Test ListStores with custom headers
    /// </summary>
    [Fact]
    public async Task ListStores_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ListStoresResponse() { Stores = new List<Store>() };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri.ToString().StartsWith($"{_config.BasePath}/stores") &&
            req.Method == HttpMethod.Get &&
            req.Headers.Contains("X-List-Stores")
        );

        var options = new ClientListStoresOptions {
            Headers = new Dictionary<string, string> {
                { "X-List-Stores", "list-789" }
            }
        };

        var response = await client.ListStores(
            new ClientListStoresRequest { },
            options
        );

        Assert.NotNull(response.Stores);
        AssertHeaderPresent(mockHandler, "X-List-Stores", "list-789");
    }

    /// <summary>
    /// Test ReadAuthorizationModels with custom headers
    /// </summary>
    [Fact]
    public async Task ReadAuthorizationModels_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ReadAuthorizationModelsResponse() {
            AuthorizationModels = new List<AuthorizationModel>()
        };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri.ToString().StartsWith($"{_config.BasePath}/stores/{_storeId}/authorization-models") &&
            req.Method == HttpMethod.Get &&
            req.Headers.Contains("X-Model-Request")
        );

        var options = new ClientReadAuthorizationModelsOptions {
            Headers = new Dictionary<string, string> {
                { "X-Model-Request", "model-123" }
            }
        };

        var response = await client.ReadAuthorizationModels(options);

        Assert.NotNull(response.AuthorizationModels);
        AssertHeaderPresent(mockHandler, "X-Model-Request", "model-123");
    }

    /// <summary>
    /// Test ReadLatestAuthorizationModel with custom headers
    /// </summary>
    [Fact]
    public async Task ReadLatestAuthorizationModel_WithCustomHeaders_ShouldPropagateHeaders() {
        var authModelId = "01GXSA8YR785C4FYS3C0RTG7B1";
        var expectedResponse = new ReadAuthorizationModelsResponse() {
            AuthorizationModels = new List<AuthorizationModel> {
                new AuthorizationModel { Id = authModelId }
            }
        };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri.ToString().Contains("page_size=1") &&
            req.Method == HttpMethod.Get &&
            req.Headers.Contains("X-Latest-Model")
        );

        var options = new ClientWriteOptions {
            Headers = new Dictionary<string, string> {
                { "X-Latest-Model", "latest-xyz" }
            },
            Transaction = new TransactionOptions()
        };

        var response = await client.ReadLatestAuthorizationModel(options);

        Assert.NotNull(response);
        AssertHeaderPresent(mockHandler, "X-Latest-Model", "latest-xyz");
    }

    /// <summary>
    /// Test ReadChanges with custom headers
    /// </summary>
    [Fact]
    public async Task ReadChanges_WithCustomHeaders_ShouldIncludeHeadersInRequest() {
        var expectedResponse = new ReadChangesResponse() {
            Changes = new List<TupleChange>()
        };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.RequestUri.ToString().StartsWith($"{_config.BasePath}/stores/{_storeId}/changes") &&
            req.Method == HttpMethod.Get &&
            req.Headers.Contains("X-Changes-Request")
        );

        var options = new ClientReadChangesOptions {
            Headers = new Dictionary<string, string> {
                { "X-Changes-Request", "changes-abc" }
            }
        };

        var response = await client.ReadChanges(
            new ClientReadChangesRequest { },
            options
        );

        Assert.NotNull(response.Changes);
        AssertHeaderPresent(mockHandler, "X-Changes-Request", "changes-abc");
    }

    /// <summary>
    /// Test Check with empty header name should throw
    /// </summary>
    [Fact]
    public async Task Check_WithEmptyHeaderName_ShouldThrowArgumentException() {
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { "", "value" }
            }
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                options
            )
        );

        Assert.Contains("Header name cannot be null, empty, or whitespace", exception.Message);
    }

    /// <summary>
    /// Test Check with null header value should throw
    /// </summary>
    [Fact]
    public async Task Check_WithNullHeaderValue_ShouldThrowArgumentException() {
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.RequestId, null! }
            }
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                options
            )
        );

        Assert.Contains("has a null value", exception.Message);
    }

    /// <summary>
    /// Test Check with reserved headers should throw
    /// </summary>
    [Theory]
    [InlineData("Authorization")]
    [InlineData("authorization")]
    [InlineData("Content-Type")]
    [InlineData("content-type")]
    [InlineData("Content-Length")]
    [InlineData("Host")]
    [InlineData("Accept")]
    [InlineData("Accept-Encoding")]
    public async Task Check_WithReservedHeader_ShouldThrowArgumentException(string headerName) {
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { headerName, "some-value" }
            }
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                options
            )
        );

        Assert.Contains("is a reserved HTTP header", exception.Message);
        Assert.Contains("should not be set via custom headers", exception.Message);
    }

    /// <summary>
    /// Test concurrent requests with different headers should not interfere
    /// </summary>
    [Fact]
    public async Task ConcurrentRequests_WithDifferentHeaders_ShouldNotInterfere() {
        var expectedResponse = new CheckResponse() { Allowed = true };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse);

        var task1 = client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            new ClientCheckOptions {
                Headers = new Dictionary<string, string> {
                    { TestHeaders.RequestId, "request-1" }
                }
            }
        );

        var task2 = client.Check(
            new ClientCheckRequest {
                User = "user:bob",
                Relation = "writer",
                Object = "document:report"
            },
            new ClientCheckOptions {
                Headers = new Dictionary<string, string> {
                    { TestHeaders.RequestId, "request-2" }
                }
            }
        );

        var results = await Task.WhenAll(task1, task2);

        Assert.All(results, r => Assert.True(r.Allowed));
        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(2),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    /// <summary>
    /// Test per-request header overriding default header
    /// </summary>
    [Fact]
    public async Task Check_WithHeaderOverridingDefaultHeader_ShouldUsePerRequestValue() {
        // Setup client with default headers
        var configWithDefaults = new ClientConfiguration() {
            StoreId = _storeId,
            ApiUrl = _apiUrl
        };
        configWithDefaults.DefaultHeaders[TestHeaders.RequestId] = "default-request-id";

        var expectedResponse = new CheckResponse() { Allowed = true };
        var (client, mockHandler) = CreateTestClientForHeaders(expectedResponse, req =>
            req.Headers.Contains(TestHeaders.RequestId) &&
            req.Headers.GetValues(TestHeaders.RequestId).First() == "override-request-id"
        );

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { TestHeaders.RequestId, "override-request-id" }
            }
        };

        var response = await client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        Assert.True(response.Allowed);
        AssertHeaderPresent(mockHandler, TestHeaders.RequestId, "override-request-id");
    }

    /// <summary>
    /// Test header precedence: per-request headers override default headers
    /// </summary>
    [Fact]
    public async Task Check_HeaderPrecedence_PerRequestOverridesDefault() {
        // Setup: Configure client with default headers
        var configWithDefaults = new ClientConfiguration() {
            StoreId = _storeId,
            ApiUrl = _apiUrl
        };
        configWithDefaults.DefaultHeaders["X-Default-Header"] = "default-value";
        configWithDefaults.DefaultHeaders["X-Override-Test"] = "default-value";

        var expectedResponse = new CheckResponse() { Allowed = true };

        // Track all headers that were actually sent
        IDictionary<string, IEnumerable<string>>? sentHeaders = null;
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                sentHeaders = req.Headers.ToDictionary(h => h.Key, h => h.Value);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(expectedResponse),
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenFgaClient(configWithDefaults, httpClient);

        // Per-request headers should override the default header with same name
        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { "X-Override-Test", "per-request-value" },
                { "X-Per-Request-Only", "per-request-only-value" }
            }
        };

        await client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        // Verify per-request header overrides default
        Assert.NotNull(sentHeaders);
        Assert.True(sentHeaders.TryGetValue("X-Override-Test", out var headerOverride));
        Assert.Equal("per-request-value", headerOverride.First());

        // Verify per-request-only header is present
        Assert.True(sentHeaders.TryGetValue("X-Per-Request-Only", out var headerPerRequestOnly));
        Assert.Equal("per-request-only-value", headerPerRequestOnly.First());

        // Verify default header is still present when not overridden
        Assert.True(sentHeaders.TryGetValue("X-Default-Header", out var headerDefault));
        Assert.Equal("default-value", headerDefault.First());
    }

    /// <summary>
    /// Test header precedence across all layers: per-request > OAuth > default
    /// </summary>
    [Fact]
    public async Task Check_HeaderPrecedence_AllLayersIntegration() {
        // This test verifies the complete header precedence chain:
        // Per-request headers > Runtime headers (OAuth) > Default headers

        var configWithDefaults = new ClientConfiguration() {
            StoreId = _storeId,
            ApiUrl = _apiUrl
        };

        // Layer 1: Default headers (lowest priority)
        configWithDefaults.DefaultHeaders["X-Layer"] = "default";
        configWithDefaults.DefaultHeaders["X-Default-Only"] = "default-only";

        var expectedResponse = new CheckResponse() { Allowed = true };
        IDictionary<string, IEnumerable<string>>? sentHeaders = null;

        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                sentHeaders = req.Headers.ToDictionary(h => h.Key, h => h.Value);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(expectedResponse),
                };
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var client = new OpenFgaClient(configWithDefaults, httpClient);

        // Layer 3: Per-request headers (highest priority)
        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { "X-Layer", "per-request" },
                { "X-Per-Request-Only", "per-request-only" }
            }
        };

        await client.Check(
            new ClientCheckRequest {
                User = "user:anne",
                Relation = "reader",
                Object = "document:budget"
            },
            options
        );

        Assert.NotNull(sentHeaders);

        // Verify per-request header wins the precedence battle
        Assert.Equal("per-request", sentHeaders["X-Layer"].First());

        // Verify per-request-only header is present
        Assert.Equal("per-request-only", sentHeaders["X-Per-Request-Only"].First());

        // Verify default-only header is present (not overridden)
        Assert.Equal("default-only", sentHeaders["X-Default-Only"].First());

        // Verify exactly the right number of custom headers (not counting standard HTTP headers)
        var customHeaders = sentHeaders.Where(h => h.Key.StartsWith("X-")).ToList();
        Assert.Equal(3, customHeaders.Count);
    }

    /// <summary>
    /// Test per-request Content-Type header should be blocked before HTTP request
    /// </summary>
    [Fact]
    public async Task PerRequestHeaders_ContentType_ShouldFailBeforeHttpRequest() {
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var options = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/xml" }
            }
        };

        var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                options
            )
        );

        Assert.Contains("Content-Type", exception.Message);
        Assert.Contains("reserved HTTP header", exception.Message);
    }

    /// <summary>
    /// Comprehensive test: Content-Type cannot be overridden through ANY path
    /// </summary>
    [Fact]
    public async Task IntegrationTest_ContentTypeCannotBeOverriddenAnyPath() {
        // This comprehensive test verifies Content-Type cannot be overridden through ANY path:
        // 1. Via DefaultHeaders - validated at configuration time
        // 2. Via per-request Headers - validated at request time

        // Test Path 1: DefaultHeaders
        var configWithContentType = new ClientConfiguration() {
            StoreId = _storeId,
            ApiUrl = _apiUrl
        };
        configWithContentType.DefaultHeaders["Content-Type"] = "text/plain";

        var configException = Assert.Throws<ArgumentException>(() => configWithContentType.EnsureValid());
        Assert.Contains("Content-Type", configException.Message);
        Assert.Contains("reserved", configException.Message);

        // Test Path 2: Per-request headers
        var (client, _) = CreateTestClientForHeaders(new CheckResponse());

        var optionsWithContentType = new ClientCheckOptions {
            Headers = new Dictionary<string, string> {
                { "Content-Type", "application/xml" }
            }
        };

        var requestException = await Assert.ThrowsAsync<ArgumentException>(async () =>
            await client.Check(
                new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "reader",
                    Object = "document:budget"
                },
                optionsWithContentType
            )
        );

        Assert.Contains("Content-Type", requestException.Message);
        Assert.Contains("reserved", requestException.Message);
    }

    /// <summary>
    /// Comprehensive test: all reserved headers are protected in both config and per-request paths
    /// </summary>
    [Fact]
    public async Task IntegrationTest_AllReservedHeadersProtected() {
        // Comprehensive test: ensure ALL reserved headers are protected in both paths
        // Note: User-Agent is intentionally excluded as the SDK allows customization

        var reservedHeaders = new[] {
            "Authorization",
            "Content-Type",
            "Content-Length",
            "Host",
            "Accept",
            "Accept-Encoding"
        };

        foreach (var reservedHeader in reservedHeaders) {
            // Test DefaultHeaders path
            var config = new ClientConfiguration() {
                StoreId = _storeId,
                ApiUrl = _apiUrl
            };
            config.DefaultHeaders[reservedHeader] = "test-value";

            var configException = Assert.Throws<ArgumentException>(() => config.EnsureValid());
            Assert.Contains(reservedHeader, configException.Message, StringComparison.OrdinalIgnoreCase);

            // Test per-request headers path
            var (client, _) = CreateTestClientForHeaders(new CheckResponse());

            var options = new ClientCheckOptions {
                Headers = new Dictionary<string, string> {
                    { reservedHeader, "test-value" }
                }
            };

            var requestException = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await client.Check(
                    new ClientCheckRequest {
                        User = "user:anne",
                        Relation = "reader",
                        Object = "document:budget"
                    },
                    options
                )
            );

            Assert.Contains(reservedHeader, requestException.Message, StringComparison.OrdinalIgnoreCase);
        }
    }

    #endregion
}