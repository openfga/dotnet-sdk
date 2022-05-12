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
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
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

namespace OpenFga.Sdk.Test.Api {
    /// <summary>
    ///  Class for testing OpenFgaApi
    /// </summary>
    public class OpenFgaApiTests : IDisposable {
        private readonly string _storeId;
        private readonly string _host = "api.fga.example";
        private readonly Configuration.Configuration _config;

        public OpenFgaApiTests() {
            _storeId = "6c181474-aaa1-4df7-8929-6e7b3a992754-test";
            _config = new Configuration.Configuration() { StoreId = _storeId, ApiHost = _host };
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

        public void Dispose() {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Test that a storeId is required in the configuration
        /// </summary>
        [Fact]
        public void StoreIdRequired() {
            var storeIdRequiredConfig = new Configuration.Configuration() { ApiHost = _host };
            void ActionMissingStoreId() => storeIdRequiredConfig.IsValid();
            var exception = Assert.Throws<OpenFgaRequiredParamError>(ActionMissingStoreId);
            Assert.Equal("Required parameter StoreId was not defined when calling Configuration.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is required in the configuration
        // /// </summary>
        [Fact]
        public void ValidHostRequired() {
            var config = new Configuration.Configuration() { StoreId = _storeId };
            void ActionMissingHost() => config.IsValid();
            var exception = Assert.Throws<OpenFgaRequiredParamError>(ActionMissingHost);
            Assert.Equal("Required parameter ApiHost was not defined when calling Configuration.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is well-formed
        // /// </summary>
        [Fact]
        public void ValidHostWellFormed() {
            var config = new Configuration.Configuration() { StoreId = _storeId, ApiHost = "https://api.fga.example" };
            void ActionMalformedHost() => config.IsValid();
            var exception = Assert.Throws<OpenFgaValidationError>(ActionMalformedHost);
            Assert.Equal("Configuration.ApiScheme (https) and Configuration.ApiHost (https://api.fga.example) do not form a valid URI (https://https://api.fga.example)", exception.Message);
        }

        // /// <summary>
        // /// Test that the provided api token issuer is well-formed
        // /// </summary>
        [Fact]
        public void ValidApiTokenIssuerWellFormed() {
            var config = new Configuration.Configuration() { StoreId = _storeId, ApiHost = "api.fga.example", ApiTokenIssuer = "https://tokenissuer.fga.example" };
            void ActionMalformedApiTokenIssuer() => config.IsValid();
            var exception = Assert.Throws<OpenFgaValidationError>(ActionMalformedApiTokenIssuer);
            Assert.Equal("Configuration.ApiTokenIssuer does not form a valid URI (https://https://tokenissuer.fga.example)", exception.Message);
        }

        /// <summary>
        /// Test that providing no client id, secret, api token issuer or api audience when they are required should error
        /// </summary>
        [Fact]
        public void ClientIdClientSecretRequired() {
            var missingClientIdConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                ClientSecret = "some-secret",
                ApiTokenIssuer = "tokenisssuer.fga.example",
                ApiAudience = _host,
            };
            void ActionMissingClientId() =>
                missingClientIdConfig.IsValid();
            var exceptionMissingClientId =
                Assert.Throws<OpenFgaRequiredParamError>(ActionMissingClientId);
            Assert.Equal("Required parameter ClientId was not defined when calling Configuration.",
                exceptionMissingClientId.Message);

            var missingClientSecretConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                ApiTokenIssuer = "tokenisssuer.fga.example",
                ApiAudience = _host,
                ClientId = "some-id"
            };
            void ActionMissingClientSecret() =>
                missingClientSecretConfig.IsValid();
            var exceptionMissingClientSecret =
                Assert.Throws<OpenFgaRequiredParamError>(ActionMissingClientSecret);
            Assert.Equal("Required parameter ClientSecret was not defined when calling Configuration.",
                exceptionMissingClientSecret.Message);

            var missingApiTokenIssuerConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                ApiAudience = _host,
                ClientId = "some-id",
                ClientSecret = "some-secret"
            };
            void ActionMissingApiTokenIssuer() =>
                missingApiTokenIssuerConfig.IsValid();
            var exceptionMissingApiTokenIssuer =
                Assert.Throws<OpenFgaRequiredParamError>(ActionMissingApiTokenIssuer);
            Assert.Equal("Required parameter ApiTokenIssuer was not defined when calling Configuration.",
                exceptionMissingApiTokenIssuer.Message);

            var missingApiAudienceConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                ApiTokenIssuer = "tokenisssuer.fga.example",
                ClientId = "some-id",
                ClientSecret = "some-secret"
            };
            void ActionMissingApiAudience() =>
                missingApiAudienceConfig.IsValid();
            var exceptionMissingApiAudience =
                Assert.Throws<OpenFgaRequiredParamError>(ActionMissingApiAudience);
            Assert.Equal("Required parameter ApiAudience was not defined when calling Configuration.",
                exceptionMissingApiAudience.Message);
        }

        /// <summary>
        /// Test that a network call is issued to get the token at the first request if client id is provided
        /// </summary>
        [Fact]
        public async Task ExchangeCredentialsTest() {
            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                ApiTokenIssuer = "tokenisssuer.fga.example",
                ApiAudience = _host,
                ClientId = "some-id",
                ClientSecret = "some-secret"
            };

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"https://{config.ApiTokenIssuer}/oauth/token") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 20000,
                        TokenType = "Bearer"
                    }),
                });

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString()
                            .StartsWith($"{config.BasePath}/stores/{config.StoreId}/authorization-models") &&
                        req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModelIds = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"https://{config.ApiTokenIssuer}/oauth/token") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString().StartsWith($"{config.BasePath}/stores/{config.StoreId}/authorization-models") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(0),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{config.BasePath}/stores/{config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /**
         * Errors
         */
        /// <summary>
        /// Test 400s return OpenFgaApiValidationError
        /// </summary>
        [Fact]
        public async Task OpenFgaApiValidationErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("\"message\":\"FAILED_PRECONDITION\",\"code\":9", Encoding.UTF8, "application/json"),
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));

            Task<CheckResponse> BadRequestError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<OpenFgaApiValidationError>(BadRequestError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test 500s return OpenFgaApiInternalError
        /// </summary>
        [Fact]
        public async Task OpenFgaApiInternalErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));

            Task<CheckResponse> InternalApiError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<OpenFgaApiInternalError>(InternalApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_config.StoreId}/check", error.RequestUrl);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test 500s return OpenFgaApiError
        /// </summary>
        [Fact]
        public async Task OpenFgaApiErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.NotFound,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));

            Task<CheckResponse> ApiError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<OpenFgaApiError>(ApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_config.StoreId}/check", error.RequestUrl);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test 429s return OpenFgaApiRateLimitExceededError
        /// </summary>
        [Fact]
        public async Task OpenFgaApiRateLimitExceededErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true));

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));

            Task<CheckResponse> RateLimitExceededError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<OpenFgaApiRateLimitExceededError>(RateLimitExceededError);
            Assert.Equal("Rate Limit Error for POST Check with API limit of 2 requests per Second.", error.Message);
            Assert.Equal(100, error.ResetInMs);
            Assert.Equal(2, error.Limit);
            Assert.Equal("Second", error.LimitUnit.ToString());
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test Retry on Rate Limit Exceeded Error
        /// </summary>
        [Fact]
        public async Task RetryOnRateLimitTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, false));

            var httpClient = new HttpClient(mockHandler.Object);

            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                MaxRetry = 3,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));

            await openFgaApi.Check(body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /**
          * Tests
          */
        /// <summary>
        /// Test ReadAuthorizationModels
        /// </summary>
        [Fact]
        public async Task ReadAuthorizationModelsTest() {
            const string authorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw";

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString()
                            .StartsWith($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models") &&
                        req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new ReadAuthorizationModelsResponse() {
                        AuthorizationModelIds = new List<string>() { authorizationModelId }
                    }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString()
                        .StartsWith($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<ReadAuthorizationModelsResponse>(response);
            Assert.Equal(authorizationModelId, response.AuthorizationModelIds[0]);
        }

        /// <summary>
        /// Test WriteAuthorizationModel
        /// </summary>
        [Fact]
        public async Task WriteAuthorizationModelTest() {
            const string authorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw";
            var relations = new Dictionary<string, Userset>() {
                {"writer", new Userset(_this: new object())}, {
                    "viewer",
                    new Userset(union: new Usersets(new List<Userset>() {
                        new(new object(), new ObjectRelation("", "writer"))
                    }))
                }
            };
            var body = new TypeDefinitions(new List<TypeDefinition>() { new("repo", relations) });
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.WriteAuthorizationModel(body);

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
                        new Uri($"{_config.BasePath}/stores/{_config.StoreId}/authorization-models/{authorizationModelId}") &&
                        req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new ReadAuthorizationModelResponse() {
                        AuthorizationModel = new AuthorizationModel(id: authorizationModelId,
                                typeDefinitions: new List<TypeDefinition>())
                    }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModel(authorizationModelId);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));
            var response = await openFgaApi.Check(body);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteRequestParams(
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "anne") }));
            var response = await openFgaApi.Write(body);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteRequestParams(new TupleKeys(new List<TupleKey> { }),
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "anne") }));
            var response = await openFgaApi.Write(body);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteRequestParams(
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "writer", "anne") }),
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "anne") }),
                "1uHxCSuTP0VKPYSnkq1pbb1jeZw");
            var response = await openFgaApi.Write(body);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ExpandRequestParams(new TupleKey(_object: "document:roadmap", relation: "viewer"));
            var response = await openFgaApi.Expand(body);

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
                        new Node(name: "document:roadmap2#owner", leaf: new Leaf(users: new Users(users: new List<string>(){"team:product#member"}))),
                        new Node(name: "document:roadmap3#owner", leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(tupleset: "document:roadmap#owner", computed: new List<Computed>(){
                        new Computed(userset: "org:contoso#admin")
                    }))),
                }),
                difference: new UsersetTreeDifference(
                    _base: new Node(name: "document:roadmap3#owner", leaf: new Leaf(users: new Users(users: new List<string>() { "team:product#member" }))),
                    subtract: new Node(name: "document:roadmap4#owner", leaf: new Leaf(users: new Users(users: new List<string>() { "team:product#member" })))
                ),
                intersection: new Nodes(
                    nodes: new List<Node>() {
                        new Node(name: "document:roadmap5#owner", leaf: new Leaf(users: new Users(users: new List<string>(){"team:product#commentor"}))),
                        new Node(name: "document:roadmap6#owner", leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(tupleset: "document:roadmap#viewer", computed: new List<Computed>(){
                        new Computed(userset: "org:contoso#owner")
                    }))),
                }))
            ));
            var jsonResponse = JsonSerializer.Serialize(mockResponse);
            //Console.Write(jsonResponse);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ExpandRequestParams(new TupleKey(_object: "document:roadmap", relation: "viewer"));
            var response = await openFgaApi.Expand(body);

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
        /// Test Read
        /// </summary>
        [Fact]
        public async Task ReadTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedResponse = new ReadResponse() {
                Tuples = new List<Model.Tuple>() {
                                new(new TupleKey("document:roadmap", "viewer", "anne"), DateTime.Now)
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
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(expectedResponse),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ReadRequestParams(new TupleKey("document:roadmap", "viewer", "anne"));
            var response = await openFgaApi.Read(body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/read") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<ReadResponse>(response);
            Assert.Single(response.Tuples);
            Assert.Equal(response, expectedResponse);
        }

        /// <summary>
        /// Test ReadChanges
        /// </summary>
        [Fact]
        public async Task ReadChangesTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedResponse = new ReadChangesResponse() {
                Changes = new List<TupleChange>() {
                            new(new TupleKey("document:roadmap", "viewer", "anne"), TupleOperation.Write, DateTime.Now),
                        },
                ContinuationToken = "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ=="
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var type = "repo";
            var pageSize = 25;
            var continuationToken = "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==";
            var response = await openFgaApi.ReadChanges(type, pageSize, continuationToken);

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
        /// Test WriteAssertions
        /// </summary>
        [Fact]
        public async Task WriteAssertionsTest() {
            const string authorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw";
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
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new Object()),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteAssertionsRequestParams(assertions: new List<Assertion>() {
                new(new TupleKey("document:roadmap", "viewer", "anne"), true)
            });
            await openFgaApi.WriteAssertions(authorizationModelId, body);

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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ReadAssertions(authorizationModelId);

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
    }
}