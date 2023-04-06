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
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Exceptions.Parsers;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// Test that a storeId is not required in the configuration
        /// </summary>
        [Fact]
        public void StoreIdNotRequired() {
            var storeIdRequiredConfig = new Configuration.Configuration() { ApiHost = _host };
            storeIdRequiredConfig.IsValid();
        }

        /// <summary>
        /// Test that a storeId is required when calling methods that need it
        /// </summary>
        [Fact]
        public async void StoreIdRequiredWhenNeeded() {
            var config = new Configuration.Configuration() { ApiHost = _host };
            var openFgaApi = new OpenFgaApi(config);

            async Task<ReadAuthorizationModelsResponse> ActionMissingStoreId() => await openFgaApi.ReadAuthorizationModels(null, null);
            var exception = await Assert.ThrowsAsync<FgaRequiredParamError>(ActionMissingStoreId);
            Assert.Equal("Required parameter StoreId was not defined when calling Configuration.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is required in the configuration
        // /// </summary>
        [Fact]
        public void ValidHostRequired() {
            var config = new Configuration.Configuration() { StoreId = _storeId };
            void ActionMissingHost() => config.IsValid();
            var exception = Assert.Throws<FgaRequiredParamError>(ActionMissingHost);
            Assert.Equal("Required parameter ApiHost was not defined when calling Configuration.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is well-formed
        // /// </summary>
        [Fact]
        public void ValidHostWellFormed() {
            var config = new Configuration.Configuration() { StoreId = _storeId, ApiHost = "https://api.fga.example" };
            void ActionMalformedHost() => config.IsValid();
            var exception = Assert.Throws<FgaValidationError>(ActionMalformedHost);
            Assert.Equal("Configuration.ApiScheme (https) and Configuration.ApiHost (https://api.fga.example) do not form a valid URI (https://https://api.fga.example)", exception.Message);
        }

        /// <summary>
        /// Test that providing no api token when it is required should error
        /// </summary>
        [Fact]
        public void ApiTokenRequired() {
            var missingApiTokenConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ApiToken,
                }
            };
            void ActionMissingApiToken() =>
                missingApiTokenConfig.IsValid();
            var exceptionMissingApiToken =
                Assert.Throws<FgaRequiredParamError>(ActionMissingApiToken);
            Assert.Equal("Required parameter ApiToken was not defined when calling Configuration.",
                exceptionMissingApiToken.Message);
        }

        // /// <summary>
        // /// Test that the provided api token issuer is well-formed
        // /// </summary>
        [Fact]
        public void ValidApiTokenIssuerWellFormed() {
            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientId = "some-id",
                        ClientSecret = "some-secret",
                        ApiTokenIssuer = "https://tokenissuer.fga.example",
                        ApiAudience = "some-audience",
                    }
                }
            };
            void ActionMalformedApiTokenIssuer() => config.IsValid();
            var exception = Assert.Throws<FgaValidationError>(ActionMalformedApiTokenIssuer);
            Assert.Equal("Configuration.ApiTokenIssuer does not form a valid URI (https://https://tokenissuer.fga.example)", exception.Message);
        }

        /// <summary>
        /// Test that the authorization header is being sent
        /// </summary>
        [Fact]
        public async Task ApiTokenSentInHeader() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ApiToken,
                    Config = new CredentialsConfig() {
                        ApiToken = "some-token"
                    }
                }
            };

            var readAuthorizationModelsMockExpression = ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{config.BasePath}/stores/{config.StoreId}/authorization-models") &&
                req.Method == HttpMethod.Get &&
                req.Headers.Contains("Authorization") &&
                req.Headers.Authorization.Equals(new AuthenticationHeaderValue("Bearer", "some-token")));

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    readAuthorizationModelsMockExpression,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFga = new OpenFgaApi(config, httpClient);

            var response = await openFga.ReadAuthorizationModels(null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                readAuthorizationModelsMockExpression,
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that providing no client id, secret, api token issuer or api audience when they are required should error
        /// </summary>
        [Fact]
        public void ClientIdClientSecretRequired() {
            var missingClientIdConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientSecret = "some-secret",
                        ApiTokenIssuer = "tokenissuer.fga.example",
                        ApiAudience = "some-audience",
                    }
                }
            };
            void ActionMissingClientId() =>
                missingClientIdConfig.IsValid();
            var exceptionMissingClientId =
                Assert.Throws<FgaRequiredParamError>(ActionMissingClientId);
            Assert.Equal("Required parameter ClientId was not defined when calling Configuration.",
                exceptionMissingClientId.Message);

            var missingClientSecretConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientId = "some-id",
                        ApiTokenIssuer = "tokenissuer.fga.example",
                        ApiAudience = "some-audience",
                    }
                }
            };
            void ActionMissingClientSecret() =>
                missingClientSecretConfig.IsValid();
            var exceptionMissingClientSecret =
                Assert.Throws<FgaRequiredParamError>(ActionMissingClientSecret);
            Assert.Equal("Required parameter ClientSecret was not defined when calling Configuration.",
                exceptionMissingClientSecret.Message);

            var missingApiTokenIssuerConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientId = "some-id",
                        ClientSecret = "some-secret",
                        ApiAudience = "some-audience",
                    }
                }
            };
            void ActionMissingApiTokenIssuer() =>
                missingApiTokenIssuerConfig.IsValid();
            var exceptionMissingApiTokenIssuer =
                Assert.Throws<FgaRequiredParamError>(ActionMissingApiTokenIssuer);
            Assert.Equal("Required parameter ApiTokenIssuer was not defined when calling Configuration.",
                exceptionMissingApiTokenIssuer.Message);

            var missingApiAudienceConfig = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientId = "some-id",
                        ClientSecret = "some-secret",
                        ApiTokenIssuer = "tokenissuer.fga.example",
                    }
                }
            };

            void ActionMissingApiAudience() =>
                missingApiAudienceConfig.IsValid();
            var exceptionMissingApiAudience =
                Assert.Throws<FgaRequiredParamError>(ActionMissingApiAudience);
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
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = new CredentialsConfig() {
                        ClientId = "some-id",
                        ClientSecret = "some-secret",
                        ApiTokenIssuer = "tokenissuer.fga.example",
                        ApiAudience = "some-audience",
                    }
                }
            };

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
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

            var readAuthorizationModelsMockExpression = ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{config.BasePath}/stores/{config.StoreId}/authorization-models") &&
                req.Method == HttpMethod.Get &&
                req.Headers.Contains("Authorization") &&
                req.Headers.Authorization.Equals(new AuthenticationHeaderValue("Bearer", "some-token")));
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    readAuthorizationModelsMockExpression,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                readAuthorizationModelsMockExpression,
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

        /// <summary>
        /// Test that updating StoreId after initialization works
        /// </summary>
        [Fact]
        public void UpdateStoreIdTest() {
            var config = new Configuration.Configuration() { ApiHost = _host };
            var openFgaApi = new OpenFgaApi(config);
            Assert.Null(openFgaApi.StoreId);
            var storeId = "some-id";
            openFgaApi.StoreId = storeId;
            Assert.Equal(storeId, openFgaApi.StoreId);
        }

        /**
         * Errors
         */
        /// <summary>
        /// Test 400s return FgaApiValidationError
        /// </summary>
        [Fact]
        public async Task FgaApiValidationErrorTest() {
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

            var body = new CheckRequest { TupleKey = new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"), AuthorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw" };

            Task<CheckResponse> BadRequestError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<FgaApiValidationError>(BadRequestError);
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
        /// Test 500s return FgaApiInternalError
        /// </summary>
        [Fact]
        public async Task FgaApiInternalErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            Task<CheckResponse> InternalApiError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<FgaApiInternalError>(InternalApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_config.StoreId}/check", error.RequestUrl);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }


        /// <summary>
        /// Test 500s return FgaApiInternalError
        /// </summary>
        [Fact]
        public async Task FgaApiInternalErrorRetrySuccessTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                })
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, false));

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            var response = await openFgaApi.Check(body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
        }

        /// <summary>
        /// Test 404s return FgaApiNotFoundError
        /// </summary>
        [Fact]
        public async Task FgaNotFoundErrorTest() {
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

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            Task<CheckResponse> ApiError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<FgaApiNotFoundError>(ApiError);
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
        /// Test unknown errors return FgaApiError
        /// </summary>
        [Fact]
        public async Task FgaApiErrorTest() {
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
                    StatusCode = HttpStatusCode.NotImplemented,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            Task<CheckResponse> ApiError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<FgaApiError>(ApiError);
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
        /// Test 429s return FgaApiRateLimitExceededError
        /// </summary>
        [Fact]
        public async Task FgaApiRateLimitExceededErrorTest() {
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
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true));

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                StoreId = _storeId,
                ApiHost = _host,
                MaxRetry = 5,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            Task<CheckResponse> RateLimitExceededError() => openFgaApi.Check(body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitExceededError);
            Assert.Equal("Rate Limit Error for POST Check with API limit of 2 requests per Second.", error.Message);
            Assert.Equal(100, error.ResetInMs);
            Assert.Equal(2, error.Limit);
            Assert.Equal("Second", error.LimitUnit.ToString());
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal(_config.StoreId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(6),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }
        /// <summary>
        /// Test 429s return FgaApiRateLimitExceededError
        /// </summary>
        [Fact]
        public async Task FgaApiRateLimitExceededErrorRetrySuccessTest() {
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

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

            var response = await openFgaApi.Check(body);
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_config.StoreId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
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

            var body = new CheckRequest(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"));

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
                        AuthorizationModels = new List<AuthorizationModel>() { new() { Id = authorizationModelId, TypeDefinitions = { } } }
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
            Assert.Equal(authorizationModelId, response.AuthorizationModels[0].Id);
        }

        /// <summary>
        /// Test WriteAuthorizationModel
        /// </summary>
        [Fact]
        public async Task WriteAuthorizationModelTest() {
            const string authorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw";
            var body = new WriteAuthorizationModelRequest {
                SchemaVersion = "1.1",
                TypeDefinitions = new List<TypeDefinition>() {
                    new() {
                        Type = "user",
                        Relations = new Dictionary<string, Userset>() {}
                    },
                    new() {
                        Type = "document",
                        Relations = new Dictionary<string, Userset>()
                            {
                                {"writer", new Userset(_this: new object())},
                                {
                                    "viewer",
                                    new Userset(union: new Usersets(new List<Userset>()
                                        {new(new object(), new ObjectRelation("", "writer"))}))
                                }
                            },
                        Metadata = new Metadata() {
                            Relations = new Dictionary<string, RelationMetadata>() { {
                                "viewer", new RelationMetadata() { DirectlyRelatedUserTypes = new List<RelationReference> {
                                    new () { Type = "user"}
                                } }
                            } }
                        },
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
                                typeDefinitions: new List<TypeDefinition>(), schemaVersion: "1.1")
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

            var body = new CheckRequest { TupleKey = new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"), AuthorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw" };
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

            var body = new WriteRequest {
                Writes = new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b") }),
                AuthorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw"
            };
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

            var body = new WriteRequest {
                Deletes = new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b") }),
                AuthorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw"
            };
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

            var body = new WriteRequest(
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "writer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b") }),
                new TupleKeys(new List<TupleKey> { new("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b") }),
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

            var body = new ExpandRequest { TupleKey = new TupleKey(_object: "document:roadmap", relation: "viewer"), AuthorizationModelId = "1uHxCSuTP0VKPYSnkq1pbb1jeZw" };
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ExpandRequest(new TupleKey(_object: "document:roadmap", relation: "viewer"));
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ListObjectsRequest {
                AuthorizationModelId = "01GAHCE4YVKPQEKZQHT2R89MQV",
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "can_read",
                Type = "document",
                ContextualTuples = new ContextualTupleKeys() {
                    TupleKeys = new List<TupleKey> {
                        new("folder:product", "editor", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"),
                        new("document:roadmap", "parent", "folder:product")
                    }
                }
            };
            var response = await openFgaApi.ListObjects(body);

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
        /// Test Read
        /// </summary>
        [Fact]
        public async Task ReadTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedResponse = new ReadResponse() {
                Tuples = new List<Model.Tuple>() {
                                new(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"), DateTime.Now)
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

            var body = new ReadRequest { TupleKey = new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b") };
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
                            new(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"), TupleOperation.WRITE, DateTime.Now),
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
                    StatusCode = HttpStatusCode.NoContent,
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteAssertionsRequest(assertions: new List<Assertion>() {
                new(new TupleKey("document:roadmap", "viewer", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"), true)
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

        /// <summary>
        /// Test ListStores
        /// </summary>
        [Fact]
        public async Task ListStoresTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedResponse = new ListStoresResponse() {
                Stores = new List<Store>() {
                            new() { Id = "45678", Name = "TestStore", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now}
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ListStores();
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
            var content = "{ \"stores\": [{\"id\": \"xyz123\", \"name\": \"abcdefg\", \"created_at\": \"2022-10-07T14:00:40.205Z\", \"updated_at\": \"2022-10-07T14:00:40.205Z\", \"deleted_at\": null}], \"continuation_token\": \"eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==\"}";
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ListStores();
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
            var expectedResponse = new ListStoresResponse() {
                Stores = new List<Store>() {
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var response = await openFgaApi.ListStores();
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
    }
}