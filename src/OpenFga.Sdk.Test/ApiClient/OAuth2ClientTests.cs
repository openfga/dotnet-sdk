using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SdkConfiguration = OpenFga.Sdk.Configuration.Configuration;

namespace OpenFga.Sdk.Test.ApiClient {
    /// <summary>
    /// Unit tests for OAuth2Client retry logic
    /// </summary>
    public class OAuth2ClientTests {
        private const string TestTokenIssuer = "issuer.fga.example";
        private const string TestClientId = "test-client-id";
        private const string TestClientSecret = "test-client-secret";
        private const string TestAudience = "test-audience";

        #region Test Helpers

        /// <summary>
        /// Creates a Credentials configuration for testing
        /// </summary>
        private static Credentials CreateTestCredentials(
            string clientId = TestClientId,
            string clientSecret = TestClientSecret,
            string tokenIssuer = TestTokenIssuer,
            string audience = TestAudience) {
            return new Credentials {
                Method = CredentialsMethod.ClientCredentials,
                Config = new CredentialsConfig {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    ApiTokenIssuer = tokenIssuer,
                    ApiAudience = audience
                }
            };
        }

        /// <summary>
        /// Creates a mock token response
        /// </summary>
        private static HttpResponseMessage CreateTokenResponse(
            string accessToken = "test-access-token",
            long expiresIn = 3600) {
            var tokenResponse = new {
                access_token = accessToken,
                token_type = "Bearer",
                expires_in = expiresIn
            };

            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonSerializer.Serialize(tokenResponse),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
        }

        /// <summary>
        /// Creates a RetryParams instance for testing
        /// </summary>
        private static RetryParams CreateTestRetryParams(int maxRetry = 3, int minWaitInMs = 100) {
            return new RetryParams {
                MaxRetry = maxRetry,
                MinWaitInMs = minWaitInMs
            };
        }

        #endregion

        #region OAuth2Client Retry Integration

        [Fact]
        public async Task OAuth2_ExchangeToken_RetriesOn429RateLimit() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 3, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    callCount++;
                    if (callCount < 3) {
                        return new HttpResponseMessage {
                            StatusCode = (HttpStatusCode)429,
                            Content = new StringContent("{\"error\":\"rate_limit_exceeded\"}")
                        };
                    }
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(token);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_RetriesOn500ServerError() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 3, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    callCount++;
                    if (callCount == 1) {
                        return new HttpResponseMessage {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Content = new StringContent("{\"error\":\"internal_server_error\"}")
                        };
                    }
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(token);
            Assert.Equal(2, callCount);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_RespectsRetryAfterHeader() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 3, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    callCount++;
                    if (callCount == 1) {
                        var response = new HttpResponseMessage {
                            StatusCode = (HttpStatusCode)429,
                            Content = new StringContent("{\"error\":\"rate_limit_exceeded\"}")
                        };
                        response.Headers.TryAddWithoutValidation("Retry-After", "1");
                        return response;
                    }
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var token = await oauth2Client.GetAccessTokenAsync();
            sw.Stop();

            Assert.NotNull(token);
            Assert.Equal(2, callCount);
            Assert.True(sw.ElapsedMilliseconds >= 1000, $"Should have delayed at least 1 second, but only took {sw.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_SucceedsAfterRetries() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 5, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    callCount++;
                    if (callCount <= 2) {
                        return new HttpResponseMessage {
                            StatusCode = (HttpStatusCode)429,
                            Content = new StringContent("{\"error\":\"rate_limit_exceeded\"}")
                        };
                    }
                    return CreateTokenResponse("success-token-after-retries", 7200);
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(token);
            Assert.Equal("success-token-after-retries", token);
            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_FailsAfterMaxRetries() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 2, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    callCount++;
                    return new HttpResponseMessage {
                        StatusCode = (HttpStatusCode)429,
                        Content = new StringContent("{\"error\":\"rate_limit_exceeded\"}")
                    };
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            await Assert.ThrowsAsync<FgaApiRateLimitExceededError>(async () => {
                await oauth2Client.GetAccessTokenAsync();
            });

            Assert.Equal(3, callCount);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_RetriesOnNetworkError() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 3, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>((req, ct) => {
                    callCount++;
                    if (callCount == 1) {
                        throw new HttpRequestException("Network error");
                    }
                    return Task.FromResult(CreateTokenResponse());
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(token);
            Assert.Equal(2, callCount);
        }

        #endregion

        #region OAuth2 Scopes

        [Fact]
        public async Task OAuth2_ExchangeToken_IncludesScopesInRequest() {
            var credentials = new Credentials {
                Method = CredentialsMethod.ClientCredentials,
                Config = new CredentialsConfig {
                    ClientId = TestClientId,
                    ClientSecret = TestClientSecret,
                    ApiTokenIssuer = TestTokenIssuer,
                    ApiAudience = TestAudience,
                    Scopes = "scope1 scope2"
                }
            };
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            string? requestBody = null;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken ct) => {
                    requestBody = request.Content is null
                        ? null
                        : await request.Content.ReadAsStringAsync();
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);
            await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(requestBody);
            Assert.Contains("scope=scope1+scope2", requestBody);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_OmitsScopeWhenNotConfigured() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            string? requestBody = null;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken ct) => {
                    requestBody = request.Content is null
                        ? null
                        : await request.Content.ReadAsStringAsync();
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);
            await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(requestBody);
            Assert.DoesNotContain("scope=", requestBody);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_WorksWithoutAudience() {
            var credentials = new Credentials {
                Method = CredentialsMethod.ClientCredentials,
                Config = new CredentialsConfig {
                    ClientId = TestClientId,
                    ClientSecret = TestClientSecret,
                    ApiTokenIssuer = TestTokenIssuer,
                    Scopes = "read write"
                }
            };
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            string? requestBody = null;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken ct) => {
                    requestBody = request.Content is null
                        ? null
                        : await request.Content.ReadAsStringAsync();
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);
            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.Equal("test-access-token", token);
            Assert.NotNull(requestBody);
            Assert.DoesNotContain("audience=", requestBody);
            Assert.Contains("scope=read+write", requestBody);
        }

        [Fact]
        public async Task OAuth2_ExchangeToken_IncludesBothAudienceAndScopes() {
            var credentials = new Credentials {
                Method = CredentialsMethod.ClientCredentials,
                Config = new CredentialsConfig {
                    ClientId = TestClientId,
                    ClientSecret = TestClientSecret,
                    ApiTokenIssuer = TestTokenIssuer,
                    ApiAudience = TestAudience,
                    Scopes = "openid profile"
                }
            };
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            string? requestBody = null;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken ct) => {
                    requestBody = request.Content is null
                        ? null
                        : await request.Content.ReadAsStringAsync();
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);
            await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(requestBody);
            Assert.Contains("audience=test-audience", requestBody);
            Assert.Contains("scope=openid+profile", requestBody);
        }

        #endregion

        #region ApiTokenIssuer Path Handling

        [Theory]
        // No scheme should be normalized to https and use default path
        [InlineData("issuer.fga.example", "https://issuer.fga.example/oauth/token")]
        [InlineData("issuer.fga.example:8080", "https://issuer.fga.example:8080/oauth/token")]
        // No scheme with custom path should be normalized to https and keep custom path
        [InlineData("issuer.fga.example/custom/token", "https://issuer.fga.example/custom/token")]
        [InlineData("issuer.fga.example:8080/custom/token", "https://issuer.fga.example:8080/custom/token")]
        // Domain without path should use default /oauth/token
        [InlineData("https://issuer.fga.example", "https://issuer.fga.example/oauth/token")]
        [InlineData("http://issuer.fga.example", "http://issuer.fga.example/oauth/token")]
        // Domain with trailing slash should use default /oauth/token
        [InlineData("https://issuer.fga.example/", "https://issuer.fga.example/oauth/token")]
        // Domain with port should use default /oauth/token
        [InlineData("https://issuer.fga.example:8080", "https://issuer.fga.example:8080/oauth/token")]
        [InlineData("https://issuer.fga.example:8080/", "https://issuer.fga.example:8080/oauth/token")]
        // Domain with custom path should keep the custom path
        [InlineData("https://issuer.fga.example/custom/token", "https://issuer.fga.example/custom/token")]
        [InlineData("https://issuer.fga.example:8080/custom/token", "https://issuer.fga.example:8080/custom/token")]
        public async Task OAuth2_Constructor_SetsCorrectTokenIssuerPath(string tokenIssuer, string expectedPath) {
            // Arrange
            var credentials = CreateTestCredentials(tokenIssuer: tokenIssuer);

            string? actualRequestUri = null;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken ct) => {
                    actualRequestUri = request.RequestUri?.ToString();
                    return CreateTokenResponse();
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);
            var retryParams = CreateTestRetryParams();

            // Act
            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);
            await oauth2Client.GetAccessTokenAsync();

            // Assert
            Assert.Equal(expectedPath, actualRequestUri);
        }

        #endregion

        #region Concurrency

        [Fact]
        public async Task OAuth2_ConcurrentGetAccessToken_OnlyExchangesOnce() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            var callCount = 0;
            var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>(async (req, ct) => {
                    Interlocked.Increment(ref callCount);
                    await gate.Task;
                    return CreateTokenResponse("shared-token", 3600);
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            const int concurrency = 20;
            var tasks = Enumerable.Range(0, concurrency)
                .Select(_ => oauth2Client.GetAccessTokenAsync())
                .ToList();

            gate.SetResult(true);

            var tokens = await Task.WhenAll(tasks);

            Assert.Equal(1, callCount);
            Assert.All(tokens, t => Assert.Equal("shared-token", t));
        }

        [Fact]
        public async Task OAuth2_ConcurrentGetAccessToken_AllSeeExchangeError() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            var callCount = 0;
            var gate = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns<HttpRequestMessage, CancellationToken>(async (req, ct) => {
                    Interlocked.Increment(ref callCount);
                    await gate.Task;
                    return new HttpResponseMessage {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Content = new StringContent("{\"error\":\"invalid_client\"}")
                    };
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            const int concurrency = 10;
            var tasks = Enumerable.Range(0, concurrency)
                .Select(_ => oauth2Client.GetAccessTokenAsync())
                .ToList();

            gate.SetResult(true);

            var exceptions = new List<FgaApiError>();
            foreach (var task in tasks) {
                try {
                    await task;
                    Assert.Fail("Expected FgaApiError but call succeeded");
                }
                catch (FgaApiError ex) {
                    exceptions.Add(ex);
                }
            }

            Assert.Equal(1, callCount);
            Assert.Equal(concurrency, exceptions.Count);
            Assert.All(exceptions, ex => Assert.IsAssignableFrom<FgaApiError>(ex));
        }

        [Fact]
        public async Task OAuth2_ConcurrentGetAccessToken_SkipsExchangeWhenTokenValid() {
            var credentials = CreateTestCredentials();
            var retryParams = CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10);

            var callCount = 0;
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => {
                    Interlocked.Increment(ref callCount);
                    return CreateTokenResponse("cached-token", 3600);
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            // First call populates the token
            var firstToken = await oauth2Client.GetAccessTokenAsync();
            Assert.Equal("cached-token", firstToken);
            Assert.Equal(1, callCount);

            // Subsequent concurrent calls should all hit the fast path
            var tasks = Enumerable.Range(0, 20)
                .Select(_ => oauth2Client.GetAccessTokenAsync())
                .ToList();

            var tokens = await Task.WhenAll(tasks);

            Assert.Equal(1, callCount);
            Assert.All(tokens, t => Assert.Equal("cached-token", t));
        }

        #endregion

        #region Sensitive Data Handling

        /// <summary>
        /// Walks the full exception chain and asserts that the client secret is not reachable from any
        /// surface that is commonly logged or serialized (Message, ToString, request data/headers/url).
        /// </summary>
        private static async Task AssertClientSecretIsNotExposed(Exception? exception, string clientSecret) {
            Assert.NotNull(exception);

            var depth = 0;
            for (var current = exception; current != null; current = current.InnerException, depth++) {
                Assert.True(depth < 10, "Exception chain is unexpectedly deep");

                Assert.DoesNotContain(clientSecret, current.Message);
                Assert.DoesNotContain(clientSecret, current.ToString());

                if (current is not FgaApiError apiError) {
                    continue;
                }

                Assert.DoesNotContain(clientSecret, apiError.RequestUrl ?? string.Empty);

                if (apiError.RequestData != null) {
                    var requestBody = await apiError.RequestData.ReadAsStringAsync();
                    Assert.DoesNotContain(clientSecret, requestBody);
                }

                if (apiError.RequestHeaders != null) {
                    Assert.DoesNotContain(clientSecret, apiError.RequestHeaders.ToString());
                }
            }
        }

        private static OAuth2Client CreateOAuth2ClientReturning(HttpStatusCode statusCode, string responseBody,
            Credentials? credentials = null) {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(() => new HttpResponseMessage {
                    StatusCode = statusCode,
                    Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var configuration = new SdkConfiguration { ApiUrl = Constants.FgaConstants.TestApiUrl };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            return new OAuth2Client(credentials ?? CreateTestCredentials(), baseClient,
                CreateTestRetryParams(maxRetry: 0, minWaitInMs: 10), metrics);
        }

        [Theory]
        [InlineData(HttpStatusCode.Unauthorized, typeof(FgaApiAuthenticationError))]
        [InlineData(HttpStatusCode.Forbidden, typeof(FgaApiAuthenticationError))]
        [InlineData(HttpStatusCode.BadRequest, typeof(FgaApiValidationError))]
        [InlineData(HttpStatusCode.NotFound, typeof(FgaApiNotFoundError))]
        [InlineData((HttpStatusCode)429, typeof(FgaApiRateLimitExceededError))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(FgaApiInternalError))]
        [InlineData(HttpStatusCode.NotImplemented, typeof(FgaApiError))]
        public async Task OAuth2_ExchangeTokenFailure_DoesNotExposeClientSecret(HttpStatusCode statusCode, Type expectedExceptionType) {
            const string clientSecret = "super-secret-value-that-must-never-be-logged";
            var credentials = CreateTestCredentials(clientSecret: clientSecret);
            var oauth2Client = CreateOAuth2ClientReturning(statusCode,
                "{\"error\":\"invalid_client\",\"error_description\":\"Client authentication failed\"}", credentials);

            var exception = await Assert.ThrowsAnyAsync<FgaApiError>(() => oauth2Client.GetAccessTokenAsync());

            Assert.IsType(expectedExceptionType, exception);
            Assert.Equal(statusCode, exception.StatusCode);
            await AssertClientSecretIsNotExposed(exception, clientSecret);
        }

        [Fact]
        public async Task OAuth2_ExchangeTokenFailure_RedactsSecretButKeepsNonSensitiveRequestData() {
            const string clientSecret = "super-secret-value-that-must-never-be-logged";
            var credentials = CreateTestCredentials(clientSecret: clientSecret);
            credentials.Config!.Scopes = "read write";
            var oauth2Client = CreateOAuth2ClientReturning(HttpStatusCode.Unauthorized,
                "{\"error\":\"invalid_client\"}", credentials);

            var exception = await Assert.ThrowsAsync<FgaApiAuthenticationError>(() => oauth2Client.GetAccessTokenAsync());

            // Request metadata that is useful for debugging must still be present
            Assert.Equal(HttpMethod.Post, exception.Method);
            Assert.Equal($"https://{TestTokenIssuer}/oauth/token", exception.RequestUrl);
            Assert.Equal("ExchangeTokenAsync", exception.ApiName);

#if !NETFRAMEWORK
            // Since .NET Core 3.0, HttpClient leaves the in-memory request content intact after sending, so a redacted
            // copy is retained. (.NET Framework disposes the request content, in which case the SDK drops it - fail closed.)
            Assert.NotNull(exception.RequestData);
#endif
            if (exception.RequestData != null) {
                Assert.Equal("application/x-www-form-urlencoded", exception.RequestData.Headers.ContentType?.MediaType);

                var requestBody = await exception.RequestData.ReadAsStringAsync();
                Assert.Contains($"client_id={TestClientId}", requestBody);
                Assert.Contains("grant_type=client_credentials", requestBody);
                Assert.Contains($"audience={TestAudience}", requestBody);
                Assert.Contains("scope=read+write", requestBody);

                // ... but the secret itself must be redacted
                Assert.Contains("client_secret=[REDACTED]", requestBody);
                Assert.DoesNotContain(clientSecret, requestBody);
            }

            await AssertClientSecretIsNotExposed(exception, clientSecret);
        }

        [Fact]
        public async Task OAuth2_ExchangeTokenFailure_DoesNotExposeUrlEncodedClientSecret() {
            // Secrets containing reserved characters are percent-encoded on the wire; make sure neither the raw
            // nor the encoded representation is retained.
            const string clientSecret = "s3cr3t&with=reserved chars+/?#";
            var credentials = CreateTestCredentials(clientSecret: clientSecret);
            var oauth2Client = CreateOAuth2ClientReturning(HttpStatusCode.Unauthorized,
                "{\"error\":\"invalid_client\"}", credentials);

            var exception = await Assert.ThrowsAsync<FgaApiAuthenticationError>(() => oauth2Client.GetAccessTokenAsync());

            await AssertClientSecretIsNotExposed(exception, clientSecret);
            await AssertClientSecretIsNotExposed(exception, Uri.EscapeDataString(clientSecret));
            await AssertClientSecretIsNotExposed(exception, "s3cr3t");
        }

        #endregion
    }
}