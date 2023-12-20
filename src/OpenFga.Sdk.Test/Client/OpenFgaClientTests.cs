//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Client;

public class OpenFgaClientTests {
    private readonly string _storeId;
    private readonly string _apiUrl = "https://api.fga.example";
    private readonly ClientConfiguration _config;

    public OpenFgaClientTests() {
        _storeId = "6c181474-aaa1-4df7-8929-6e7b3a992754-test";
        _config = new ClientConfiguration() { StoreId = _storeId, ApiUrl = _apiUrl };
    }

    private HttpResponseMessage GetCheckResponse(CheckResponse content, bool shouldRetry = false) {
        var response = new HttpResponseMessage() {
            StatusCode = shouldRetry ? HttpStatusCode.TooManyRequests : HttpStatusCode.OK,
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

    [Fact]
    public void Dispose() {
        // Cleanup when everything is done.
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

        var response = await fgaClient.ListStores(new ClientListStoresOptions() { });
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

        var response = await fgaClient.ListStores();
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

        var response = await fgaClient.ListStores();
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
                        Object = "document:roadmap"
                    },
                    TupleOperation.WRITE, DateTime.Now),
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
        var continuationToken =
            "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==";
        var response = await fgaClient.ReadChanges(new ClientReadChangesRequest { Type = type }, new ClientReadChangesOptions {
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
        var expectedResponse = new ReadResponse() {
            Tuples = new List<Model.Tuple>() {
                new(new TupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:roadmap"
                    },
                    DateTime.Now)
            }
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                    req.Method == HttpMethod.Post &&
                    req.Content.ReadAsStringAsync().Result.Contains("tuple")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(expectedResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientReadRequest() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:roadmap",
        };
        var options = new ClientReadOptions { };
        var response = await fgaClient.Read(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                req.Method == HttpMethod.Post &&
                req.Content.ReadAsStringAsync().Result.Contains("tuple")),
            ItExpr.IsAny<CancellationToken>()
        );

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
        var expectedResponse = new ReadResponse() {
            Tuples = new List<Model.Tuple>() {
                new(new TupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:roadmap"
                    },
                    DateTime.Now)
            }
        };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                    req.Method == HttpMethod.Post &&
                    !req.Content.ReadAsStringAsync().Result.Contains("tuple")),
                ItExpr.IsAny<CancellationToken>()
            )
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
                req.Method == HttpMethod.Post &&
                !req.Content.ReadAsStringAsync().Result.Contains("tuple")),
            ItExpr.IsAny<CancellationToken>()
        );

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
                    Object = "document:roadmap",
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
                    Object = "document:roadmap",
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
                    Object = "document:roadmap",
                },
            },
            Deletes = new List<ClientTupleKeyWithoutCondition> {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "writer",
                    Object = "document:roadmap",
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
                    Object = "document:roadmap",
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
                    Object = "document:roadmap",
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
    /// Test Batch Check
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
            Object = "document:roadmap",
            ContextualTuples = new List<ClientTupleKey>() {
                new() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "editor",
                    Object = "document:roadmap",
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
    /// Test BatchCheck
    /// </summary>
    [Fact]
    public async Task BatchCheckTest() {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        mockHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(Task.Run(async () => {
                await Task.Delay(500);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                };
            }))
            .Returns(Task.Run(async () => {
                await Task.Delay(500);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = false }),
                };
            }))
            .Returns(Task.Run(async () => {
                await Task.Delay(500);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                };
            }))
            .Returns(Task.Run(async () => {
                await Task.Delay(500);
                return new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = Utils.CreateJsonStringContent(new Object { }),
                };
            }));

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new List<ClientCheckRequest>(){
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "viewer",
                Object = "document:roadmap",
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:roadmap",
                    }
                },
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "admin",
                Object = "document:roadmap",
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:roadmap",
                    }
                },
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "creator",
                Object = "document:roadmap",
            },
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "deleter",
                Object = "document:roadmap",
            }
        };
        var options = new ClientBatchCheckOptions { };
        var response = await fgaClient.BatchCheck(body, options);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(4),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<BatchCheckResponse>(response);

        var allowedResponses = response.Responses.FindAll(res => res.Allowed);
        Assert.Equal(2, allowedResponses.Count);
        var notAllowedResponses = response.Responses.FindAll(res => res.Allowed == false);
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

        var jsonResponse =
            "{\"tree\":{\"root\":{\"name\":\"document:roadmap#owner\", \"union\":{\"nodes\":[{\"name\":\"document:roadmap#owner\", \"leaf\":{\"users\":{\"users\":[\"team:product#member\"]}}}, {\"name\":\"document:roadmap#owner\", \"leaf\":{\"tupleToUserset\":{\"tupleset\":\"document:roadmap#owner\", \"computed\":[{\"userset\":\"org:contoso#admin\"}]}}}]}}}}";
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
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json"),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body = new ClientExpandRequest {
            Relation = "viewer",
            Object = "document:roadmap",
        };
        var response = await fgaClient.Expand(body, new ClientWriteOptions {
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
                root: new Node(name: "document:roadmap1#owner",
                    union: new Nodes(
                        nodes: new List<Node>() {
                            new Node(name: "document:roadmap2#owner",
                                leaf: new Leaf(users: new Users(users: new List<string>() {"team:product#member"}))),
                            new Node(name: "document:roadmap3#owner",
                                leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(
                                    tupleset: "document:roadmap#owner",
                                    computed: new List<Computed>() {new Computed(userset: "org:contoso#admin")}))),
                        }),
                    difference: new UsersetTreeDifference(
                        _base: new Node(name: "document:roadmap3#owner",
                            leaf: new Leaf(users: new Users(users: new List<string>() { "team:product#member" }))),
                        subtract: new Node(name: "document:roadmap4#owner",
                            leaf: new Leaf(users: new Users(users: new List<string>() { "team:product#member" })))
                    ),
                    intersection: new Nodes(
                        nodes: new List<Node>() {
                            new Node(name: "document:roadmap5#owner",
                                leaf: new Leaf(users: new Users(users: new List<string>() {"team:product#commentor"}))),
                            new Node(name: "document:roadmap6#owner",
                                leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(
                                    tupleset: "document:roadmap#viewer",
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
            Object = "document:roadmap",
        };
        var response = await fgaClient.Expand(body, new ClientWriteOptions {
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
        var expectedResponse = new ListObjectsResponse { Objects = new List<string> { "document:roadmap" } };
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/list-objects") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
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
                    Object = "document:roadmap",
                },
            },
        };
        var response = await fgaClient.ListObjects(body, new ClientWriteOptions {
            AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
        });

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/list-objects") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

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
        mockHandler.Protected()
            .SetupSequence<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = false }),
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.OK,
                Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
            })
            .ReturnsAsync(new HttpResponseMessage() {
                StatusCode = HttpStatusCode.NotFound,
                Content = Utils.CreateJsonStringContent(new Object { }),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        var fgaClient = new OpenFgaClient(_config, httpClient);

        var body =
            new ClientListRelationsRequest() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Object = "document:roadmap",
                Relations = new List<string> { "can_view", "can_edit", "can_delete", "can_rename" },
                ContextualTuples = new List<ClientTupleKey>() {
                    new() {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "editor",
                        Object = "document:roadmap",
                    }
                }
            };
        var response = await fgaClient.ListRelations(body);

        mockHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(4),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );

        Assert.IsType<ListRelationsResponse>(response);
        Assert.Equal(2, response.Relations.Count);
        // TODO: Ensure the relations are correct, currently because the mocks are generic and we process in parallel,
        // we do not know what order they will be processed in
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
                Object = "document:roadmap",
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
            Object = "document:roadmap",
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
}