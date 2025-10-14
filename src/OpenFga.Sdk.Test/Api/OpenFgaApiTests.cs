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
            _storeId = "01H0H015178Y2V4CX10C2KGHF4";
            _config = new Configuration.Configuration() { ApiHost = _host };
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

        public void Dispose() {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Test that a storeId is not required in the configuration
        /// </summary>
        [Fact]
        public void StoreIdNotRequired() {
            var storeIdRequiredConfig = new Configuration.Configuration() { ApiHost = _host };
            storeIdRequiredConfig.EnsureValid();
        }

        /// <summary>
        /// Test that a storeId is required when calling methods that need it
        /// </summary>
        [Fact]
        public async Task StoreIdRequiredWhenNeeded() {
            var config = new Configuration.Configuration() { ApiHost = _host };
            var openFgaApi = new OpenFgaApi(config);

            async Task<ReadAuthorizationModelsResponse> ActionMissingStoreId() => await openFgaApi.ReadAuthorizationModels(null, null);
            var exception = await Assert.ThrowsAsync<FgaRequiredParamError>(ActionMissingStoreId);
            Assert.Equal("Required parameter StoreId was not defined when calling ReadAuthorizationModels.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is required in the configuration
        // /// </summary>
        [Fact]
        public void ValidHostRequired() {
            var config = new Configuration.Configuration() { };
            void ActionMissingHost() => config.EnsureValid();
            var exception = Assert.Throws<FgaRequiredParamError>(ActionMissingHost);
            Assert.Equal("Required parameter ApiUrl was not defined when calling Configuration.", exception.Message);
        }

        // /// <summary>
        // /// Test that a host is well-formed
        // /// </summary>
        [Fact]
        public void ValidHostWellFormed() {
            var config = new Configuration.Configuration() { ApiHost = "https://api.fga.example" };
            void ActionMalformedHost() => config.EnsureValid();
            var exception = Assert.Throws<FgaValidationError>(ActionMalformedHost);
            Assert.Equal("Configuration.ApiUrl (https://https://api.fga.example) does not form a valid URI (https://https://api.fga.example)", exception.Message);
        }

        /// <summary>
        /// Test that providing no api token when it is required should error
        /// </summary>
        [Fact]
        public void ApiTokenRequired() {
            var missingApiTokenConfig = new Configuration.Configuration() {
                ApiHost = _host,
                Credentials = new Credentials() {
                    Method = CredentialsMethod.ApiToken,
                }
            };
            void ActionMissingApiToken() =>
                missingApiTokenConfig.EnsureValid();
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
            void ActionMalformedApiTokenIssuer() => config.EnsureValid();
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
                    .StartsWith($"{config.BasePath}/stores/{_storeId}/authorization-models") &&
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

            var response = await openFga.ReadAuthorizationModels(_storeId, null, null);

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
                missingClientIdConfig.EnsureValid();
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
                missingClientSecretConfig.EnsureValid();
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
                missingApiTokenIssuerConfig.EnsureValid();
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
                missingApiAudienceConfig.EnsureValid();
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
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                        req.Method == HttpMethod.Post &&
                        req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 86400,
                        TokenType = "Bearer"
                    }),
                })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 86400,
                        TokenType = "Bearer"
                    }),
                });

            var readAuthorizationModelsMockExpression = ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{config.BasePath}/stores/{_storeId}/authorization-models") &&
                req.Method == HttpMethod.Get &&
                req.Headers.Contains("Authorization") &&
                req.Headers.Authorization.Equals(new AuthenticationHeaderValue("Bearer", "some-token")));
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    readAuthorizationModelsMockExpression,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);
            var response2 = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                    req.Method == HttpMethod.Post &&
                    req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                readAuthorizationModelsMockExpression,
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(0),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that a network call is issued to get the token at the first request if client id is provided, and then again before the next call if expired
        /// </summary>
        [Fact]
        public async Task ExchangeCredentialsAfterExpiryTest() {
            var config = new Configuration.Configuration() {
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
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                        req.Method == HttpMethod.Post &&
                        req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 1,
                        TokenType = "Bearer"
                    }),
                })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 86400,
                        TokenType = "Bearer"
                    }),
                });

            var readAuthorizationModelsMockExpression = ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{config.BasePath}/stores/{_storeId}/authorization-models") &&
                req.Method == HttpMethod.Get &&
                req.Headers.Contains("Authorization") &&
                req.Headers.Authorization.Equals(new AuthenticationHeaderValue("Bearer", "some-token")));
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    readAuthorizationModelsMockExpression,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);
            var response2 = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                    req.Method == HttpMethod.Post &&
                    req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                readAuthorizationModelsMockExpression,
                ItExpr.IsAny<CancellationToken>()
            );
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(0),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that a network calls to get credentials are retried
        /// </summary>
        [Fact]
        public async Task ExchangeCredentialsRetriesTest() {
            var config = new Configuration.Configuration() {
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
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                        req.Method == HttpMethod.Post &&
                        req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = (HttpStatusCode)429
                })
                    .ReturnsAsync(new HttpResponseMessage() {
                        StatusCode = HttpStatusCode.InternalServerError
                    })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new OAuth2Client.AccessTokenResponse() {
                        AccessToken = "some-token",
                        ExpiresIn = 86400,
                        TokenType = "Bearer"
                    }),
                });

            var readAuthorizationModelsMockExpression = ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri.ToString()
                    .StartsWith($"{config.BasePath}/stores/{_storeId}/authorization-models") &&
                req.Method == HttpMethod.Get &&
                req.Headers.Contains("Authorization") &&
                req.Headers.Authorization.Equals(new AuthenticationHeaderValue("Bearer", "some-token")));
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    readAuthorizationModelsMockExpression,
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                })
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(
                            new ReadAuthorizationModelsResponse() { AuthorizationModels = { } }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var response = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"https://{config.Credentials.Config.ApiTokenIssuer}/oauth/token") &&
                    req.Method == HttpMethod.Post &&
                    req.Content.Headers.ContentType.ToString().Equals("application/x-www-form-urlencoded")),
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
                    req.RequestUri == new Uri($"{config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("\"message\":\"FAILED_PRECONDITION\",\"code\":9", Encoding.UTF8, "application/json"),
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                },
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };

            Task<CheckResponse> BadRequestError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiValidationError>(BadRequestError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal(_storeId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                },
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };

            Task<CheckResponse> InternalApiError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiInternalError>(InternalApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_storeId}/check", error.RequestUrl);
            Assert.Equal(_storeId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.NotFound,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> ApiError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiNotFoundError>(ApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_storeId}/check", error.RequestUrl);
            Assert.Equal(_storeId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.NotImplemented,
                });

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> ApiError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiError>(ApiError);
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores/{_storeId}/check", error.RequestUrl);
            Assert.Equal(_storeId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitExceededError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitExceededError);
            Assert.Equal("Rate Limit Error for POST Check with API limit of 2 requests per Second.", error.Message);
            Assert.Equal(100, error.ResetInMs);
            Assert.Equal(2, error.Limit);
            Assert.Equal("Second", error.LimitUnit.ToString());
            Assert.Equal(error.Method, HttpMethod.Post);
            Assert.Equal("Check", error.ApiName);
            Assert.Equal(_storeId, error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(6),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, true))
                .ReturnsAsync(GetCheckResponse(new CheckResponse { Allowed = true }, false));

            var httpClient = new HttpClient(mockHandler.Object);

            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            await openFgaApi.Check(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
            const string authorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1";

            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString()
                            .StartsWith($"{_config.BasePath}/stores/{_storeId}/authorization-models") &&
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

            var response = await openFgaApi.ReadAuthorizationModels(_storeId, null, null);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString()
                        .StartsWith($"{_config.BasePath}/stores/{_storeId}/authorization-models") &&
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
            const string authorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1";

            var body = new WriteAuthorizationModelRequest {
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/authorization-models") &&
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

            var response = await openFgaApi.WriteAuthorizationModel(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/authorization-models") &&
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
                        new Uri($"{_config.BasePath}/stores/{_storeId}/authorization-models/{authorizationModelId}") &&
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

            var response = await openFgaApi.ReadAuthorizationModel(_storeId, authorizationModelId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_storeId}/authorization-models/{authorizationModelId}") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                },
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };

            var response = await openFgaApi.Check(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                Writes = new WriteRequestWrites(new List<TupleKey> { new () {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                } }),
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };
            var response = await openFgaApi.Write(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                Deletes = new WriteRequestDeletes(new List<TupleKeyWithoutCondition> { new () {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                } }),
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };
            var response = await openFgaApi.Write(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                Writes = new WriteRequestWrites(new List<TupleKey> { new () {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "writer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }, new () {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                } }),
                AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
            };
            var response = await openFgaApi.Write(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/write") &&
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
                "{\"tree\":{\"root\":{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"union\":{\"nodes\":[{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"users\":{\"users\":[\"team:product#member\"]}}}, {\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"tupleToUserset\":{\"tupleset\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"computed\":[{\"userset\":\"org:contoso#admin\"}]}}}]}}}}";
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/expand") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json"),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ExpandRequest { TupleKey = new ExpandRequestTupleKey(varObject: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a", relation: "viewer"), AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1" };
            var response = await openFgaApi.Expand(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/expand") &&
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
                root: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a1#owner",
                union: new Nodes(
                    varNodes: new List<Node>() {
                        new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a2#owner", leaf: new Leaf(users: new Users(varUsers: new List<string>(){"team:product#member"}))),
                        new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a3#owner", leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(tupleset: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner", computed: new List<Computed>(){
                        new Computed(userset: "org:contoso#admin")
                    }))),
                }),
                difference: new UsersetTreeDifference(
                    varBase: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a3#owner", leaf: new Leaf(users: new Users(varUsers: new List<string>() { "team:product#member" }))),
                    subtract: new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a4#owner", leaf: new Leaf(users: new Users(varUsers: new List<string>() { "team:product#member" })))
                ),
                intersection: new Nodes(
                    varNodes: new List<Node>() {
                        new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a5#owner", leaf: new Leaf(users: new Users(varUsers: new List<string>(){"team:product#commentor"}))),
                        new Node(name: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a6#owner", leaf: new Leaf(tupleToUserset: new UsersetTreeTupleToUserset(tupleset: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#viewer", computed: new List<Computed>(){
                        new Computed(userset: "org:contoso#owner")
                    }))),
                }))
            ));

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/expand") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(mockResponse),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ExpandRequest(new ExpandRequestTupleKey(varObject: "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a", relation: "viewer"));
            var response = await openFgaApi.Expand(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/expand") &&
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
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-objects") &&
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
                        new("document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a", "parent", "folder:product")
                    }
                }
            };
            var response = await openFgaApi.ListObjects(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/list-objects") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<ListObjectsResponse>(response);
            Assert.Single(response.Objects);
            Assert.Equal(response, expectedResponse);
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
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ListUsersRequest {
                AuthorizationModelId = "01GAHCE4YVKPQEKZQHT2R89MQV",
                Relation = "can_read",
                Object = new FgaObject {
                    Type = "document",
                    Id = "roadmap"
                },
                UserFilters = new List<UserTypeFilter> {
                    // API does not allow sending multiple filters, done here for testing purposes
                    new("user"),
                    new("team", "member")
                },
                ContextualTuples = new List<TupleKey> {
                    new("folder:product", "editor", "user:81684243-9356-4421-8fbf-a4f8d36aa31b"),
                    new("document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a", "parent", "folder:product")
                },
                Context = new { ViewCount = 100 }
            };
            var response = await openFgaApi.ListUsers(_storeId, body);

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
                                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                                }, DateTime.Now)
                            }
            };
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/read") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(expectedResponse),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ReadRequest {
                TupleKey = new ReadRequestTupleKey {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                },
            };
            var response = await openFgaApi.Read(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/read") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.IsType<ReadResponse>(response);
            Assert.Single(response.Tuples);
            Assert.Equal(response, expectedResponse);
        }

        /// <summary>
        /// Test Read With Empty Parameters
        /// </summary>
        [Fact]
        public async Task ReadEmptyTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var expectedResponse = new ReadResponse() {
                Tuples = new List<Model.Tuple>() {
                                new(new TupleKey {
                                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                                    Relation = "viewer",
                                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                                }, DateTime.Now)
                            }
            };
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/read") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(expectedResponse),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new ReadRequest { };
            var response = await openFgaApi.Read(_storeId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/read") &&
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
                            new(new TupleKey {
                                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                                Relation = "viewer",
                                Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                            }, TupleOperation.TUPLEOPERATIONWRITE, DateTime.Now),
                        },
                ContinuationToken = "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ=="
            };
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString()
                            .StartsWith($"{_config.BasePath}/stores/{_storeId}/changes") &&
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
            var startTime = DateTime.Parse("2022-01-01T00:00:00Z");
            var continuationToken = "eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==";
            var response = await openFgaApi.ReadChanges(_storeId, type, pageSize, continuationToken, startTime);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri.ToString()
                        .StartsWith($"{_config.BasePath}/stores/{_storeId}/changes") &&
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
            const string authorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1";
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri ==
                        new Uri($"{_config.BasePath}/stores/{_storeId}/assertions/{authorizationModelId}") &&
                        req.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage() {
                    StatusCode = HttpStatusCode.NoContent,
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new WriteAssertionsRequest {
                Assertions = new List<Assertion> {
                    new (new AssertionTupleKey {
                        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                        Relation = "viewer",
                        Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                    })
                },
            };
            await openFgaApi.WriteAssertions(_storeId, authorizationModelId, body);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_storeId}/assertions/{authorizationModelId}") &&
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
                        new Uri($"{_config.BasePath}/stores/{_storeId}/assertions/{authorizationModelId}") &&
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

            var response = await openFgaApi.ReadAssertions(_storeId, authorizationModelId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri ==
                    new Uri($"{_config.BasePath}/stores/{_storeId}/assertions/{authorizationModelId}") &&
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
        /// Test ListStores correctly returns FgaApiNotFoundError
        /// </summary>
        [Fact]
        public async Task ListStoresResponseErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
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
                    StatusCode = HttpStatusCode.NotFound,
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            Task<ListStoresResponse> ApiError() => openFgaApi.ListStores();
            var error = await Assert.ThrowsAsync<FgaApiNotFoundError>(ApiError);

            Assert.Equal(error.Method, HttpMethod.Get);
            Assert.Equal("ListStores", error.ApiName);
            Assert.Equal($"{_config.BasePath}/stores", error.RequestUrl);
            Assert.Null(error.StoreId);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores") &&
                    req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
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

        /**
         * Retry-After Header Tests
         */

        /// <summary>
        /// Test that Retry-After header with integer format (delay-seconds) is parsed correctly in error
        /// </summary>
        [Fact]
        public async Task RetryAfterIntegerFormatInErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "5" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "10" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "15" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 2,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Verify Retry-After header was parsed and stored
            Assert.NotNull(error.RetryAfter);
            Assert.Equal(15, error.RetryAfter);
            Assert.Equal("15", error.RetryAfterRaw);

            // Verify retry attempt tracking
            Assert.Equal(2, error.RetryAttempt);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that Retry-After header with HTTP-date format captures raw value for debugging
        /// Note: HTTP-date parsing verification is limited in mock scenarios due to header handling.
        /// The production implementation supports both integer and HTTP-date formats per RFC 7231.
        /// </summary>
        [Fact]
        public async Task RetryAfterHttpDateFormatInErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            // Use a valid RFC1123 date safely in the past to avoid long sleeps
            var httpDateFormat = DateTimeOffset.UtcNow.AddMinutes(-60).ToString("r");

            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    response.Headers.TryAddWithoutValidation("Retry-After", httpDateFormat);
                    return response;
                })
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    response.Headers.TryAddWithoutValidation("Retry-After", httpDateFormat);
                    return response;
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Verify raw header value was captured for debugging purposes
            // This is critical for troubleshooting retry issues
            Assert.Equal(httpDateFormat, error.RetryAfterRaw);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test successful retry after respecting Retry-After header
        /// </summary>
        [Fact]
        public async Task RetryAfterSuccessfulRetryTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that out-of-bounds Retry-After values (too large) are handled correctly
        /// </summary>
        [Fact]
        public async Task RetryAfterOutOfBoundsTooLargeTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            // Value > 1800 seconds (30 minutes) should be ignored
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "3600" } }, // 1 hour - too large
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "3600" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Out of bounds value should result in null RetryAfter (falls back to exponential backoff)
            Assert.Null(error.RetryAfter);
            // But raw value should still be captured
            Assert.Equal("3600", error.RetryAfterRaw);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that out-of-bounds Retry-After values (too small) are handled correctly
        /// </summary>
        [Fact]
        public async Task RetryAfterOutOfBoundsTooSmallTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            // Value < 1 second should be ignored
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "0" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "0" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Out of bounds value should result in null RetryAfter
            Assert.Null(error.RetryAfter);
            Assert.Equal("0", error.RetryAfterRaw);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that invalid Retry-After format is handled gracefully
        /// </summary>
        [Fact]
        public async Task RetryAfterInvalidFormatTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    response.Headers.TryAddWithoutValidation("Retry-After", "invalid-value");
                    return response;
                })
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    response.Headers.TryAddWithoutValidation("Retry-After", "invalid-value");
                    return response;
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Invalid format should result in null RetryAfter (falls back to exponential backoff)
            Assert.Null(error.RetryAfter);
            // Raw value should still be captured for debugging
            Assert.Equal("invalid-value", error.RetryAfterRaw);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that RetryAttempt field is correctly tracked across retries
        /// </summary>
        [Fact]
        public async Task RetryAttemptTrackingTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 2,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Should show attempt 2 (0 = initial, 1 = first retry, 2 = second retry)
            Assert.Equal(2, error.RetryAttempt);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(3),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test Retry-After with 500 Internal Server Error
        /// </summary>
        [Fact]
        public async Task RetryAfterWith500ErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Headers = { { "Retry-After", "3" } },
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Headers = { { "Retry-After", "5" } },
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> InternalError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiInternalError>(InternalError);

            // Verify Retry-After header was captured for 500 errors
            Assert.NotNull(error.RetryAfter);
            Assert.Equal(5, error.RetryAfter);
            Assert.Equal("5", error.RetryAfterRaw);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test Retry-After with 503 Service Unavailable
        /// </summary>
        [Fact]
        public async Task RetryAfterWith503ErrorTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Headers = { { "Retry-After", "2" } },
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that missing Retry-After header doesn't break retry logic
        /// </summary>
        [Fact]
        public async Task RetryWithoutRetryAfterHeaderTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    // No Retry-After header
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that x-ratelimit-reset legacy header is captured for logging/debugging only.
        /// Verifies that Retry-After (not x-ratelimit-reset) is used for actual retry delay calculation.
        /// The x-ratelimit-reset header is deprecated for retry logic but still exposed for logging.
        /// </summary>
        [Fact]
        public async Task XRateLimitResetLegacyHeaderTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    // Add Retry-After for retry delay calculation
                    response.Headers.TryAddWithoutValidation("Retry-After", "5");
                    // Add legacy x-ratelimit-reset header (milliseconds) for logging only
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitResetIn, "3000");
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitRemaining, "0");
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitTotalInPeriod, "10");
                    return response;
                })
                .ReturnsAsync(() => {
                    var response = new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                    };
                    response.Headers.TryAddWithoutValidation("Retry-After", "5");
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitResetIn, "3000");
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitRemaining, "0");
                    response.Headers.Add(RateLimitParser.RateLimitHeader.LimitTotalInPeriod, "10");
                    return response;
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var config = new Configuration.Configuration() {
                ApiHost = _host,
                MaxRetry = 1,
            };
            var openFgaApi = new OpenFgaApi(config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            Task<CheckResponse> RateLimitError() => openFgaApi.Check(_storeId, body);
            var error = await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(RateLimitError);

            // Verify Retry-After is used for retry delay calculation (new standard)
            Assert.NotNull(error.RetryAfter);
            Assert.Equal(5, error.RetryAfter);
            Assert.Equal("5", error.RetryAfterRaw);

            // Verify x-ratelimit-reset legacy header is captured for logging/debugging
            // (deprecated for retry logic, but still exposed)
            Assert.NotNull(error.XRateLimitReset);
            Assert.Equal(3000, error.XRateLimitReset); // Value in milliseconds
            Assert.NotNull(error.ResetInMs);
            Assert.Equal(3000, error.ResetInMs); // Same value from rate limit parser

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that different Retry-After values are respected across multiple retries
        /// </summary>
        [Fact]
        public async Task RetryAfterMultipleRetriesWithDifferentValuesTest() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Headers = { { "Retry-After", "2" } },
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Headers = { { "Retry-After", "3" } },
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);

            // Verify all attempts were made
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(4),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                    req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        #region End-to-End Integration Improvements

        [Fact]
        public async Task RetryAfterWithTimingVerification() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Headers = { { "Retry-After", "1" } },
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var response = await openFgaApi.Check(_storeId, body);
            sw.Stop();

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
            Assert.True(sw.ElapsedMilliseconds >= 1000, $"Expected delay >= 1000ms, but only took {sw.ElapsedMilliseconds}ms");
            Assert.True(sw.ElapsedMilliseconds < 1500, $"Expected delay < 1500ms, but took {sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task RetryWithExponentialBackoffTimingVerification() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = (HttpStatusCode)429,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                })
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var response = await openFgaApi.Check(_storeId, body);
            sw.Stop();

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
            // With MinWaitInMs=100 (default), first retry delay should be [100ms, 200ms]
            // Allow up to 500ms to account for CI environment overhead (virtualization, CPU contention, etc.)
            Assert.True(sw.ElapsedMilliseconds >= 100, $"Expected delay >= 100ms with exponential backoff, but only took {sw.ElapsedMilliseconds}ms");
            Assert.True(sw.ElapsedMilliseconds < 500, $"Expected delay < 500ms with exponential backoff, but took {sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task RetryMultipleRetriesWithDifferentDelaysTimingVerification() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var attemptTimes = new List<long>();
            var sw = System.Diagnostics.Stopwatch.StartNew();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                    attemptTimes.Add(sw.ElapsedMilliseconds);
                    if (attemptTimes.Count == 1) {
                        return new HttpResponseMessage {
                            StatusCode = (HttpStatusCode)429,
                            Headers = { { "Retry-After", "1" } },
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                        };
                    }
                    else if (attemptTimes.Count == 2) {
                        return new HttpResponseMessage {
                            StatusCode = (HttpStatusCode)429,
                            Headers = { { "Retry-After", "2" } },
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                        };
                    }
                    else {
                        return new HttpResponseMessage {
                            StatusCode = HttpStatusCode.OK,
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                        };
                    }
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);
            sw.Stop();

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
            Assert.Equal(3, attemptTimes.Count);

            var delay1 = attemptTimes[1] - attemptTimes[0];
            var delay2 = attemptTimes[2] - attemptTimes[1];

            Assert.True(delay1 >= 1000, $"First retry delay should be ~1000ms, got {delay1}ms");
            Assert.True(delay1 < 1300, $"First retry delay should be ~1000ms, got {delay1}ms");
            Assert.True(delay2 >= 2000, $"Second retry delay should be ~2000ms, got {delay2}ms");
            Assert.True(delay2 < 2300, $"Second retry delay should be ~2000ms, got {delay2}ms");
        }

        [Fact]
        public async Task RetryMetadataVerification() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var requestsReceived = 0;

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage req, CancellationToken ct) => {
                    requestsReceived++;

                    return requestsReceived == 1
                        ? new HttpResponseMessage { StatusCode = (HttpStatusCode)429, Headers = { { "Retry-After", "1" } }, Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }) }
                        : new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }) };
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
            Assert.Equal(2, requestsReceived);
        }

        [Fact]
        public async Task ExponentialBackoffWithNetworkErrors() {
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var attemptTimes = new List<long>();
            var sw = System.Diagnostics.Stopwatch.StartNew();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri == new Uri($"{_config.BasePath}/stores/{_storeId}/check") &&
                        req.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Returns((HttpRequestMessage req, CancellationToken ct) => {
                    attemptTimes.Add(sw.ElapsedMilliseconds);
                    if (attemptTimes.Count == 1) {
                        throw new HttpRequestException("Network error");
                    }
                    else {
                        return Task.FromResult(new HttpResponseMessage {
                            StatusCode = HttpStatusCode.OK,
                            Content = Utils.CreateJsonStringContent(new CheckResponse { Allowed = true }),
                        });
                    }
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var openFgaApi = new OpenFgaApi(_config, httpClient);

            var body = new CheckRequest {
                TupleKey = new CheckRequestTupleKey() {
                    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                    Relation = "viewer",
                    Object = "document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"
                }
            };

            var response = await openFgaApi.Check(_storeId, body);
            sw.Stop();

            Assert.IsType<CheckResponse>(response);
            Assert.True(response.Allowed);
            Assert.Equal(2, attemptTimes.Count);

            var delay = attemptTimes[1] - attemptTimes[0];
            // First retry delay with MinWaitInMs=100 (default) should be [100ms, 200ms]
            // Allow up to 500ms to account for CI environment overhead (virtualization, CPU contention, etc.)
            Assert.True(delay >= 100, $"Expected exponential backoff delay >= 100ms after network error, got {delay}ms");
            Assert.True(delay < 500, $"Expected exponential backoff delay < 500ms after network error, got {delay}ms");
        }

        #endregion
    }
}