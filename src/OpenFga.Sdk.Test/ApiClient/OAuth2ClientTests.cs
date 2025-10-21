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
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Telemetry;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.ApiClient {
    /// <summary>
    /// Unit tests for OAuth2Client retry logic
    /// </summary>
    public class OAuth2ClientTests {
        private const string TestTokenIssuer = "oauth.example.com";
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
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
            var configuration = new Configuration.Configuration { ApiUrl = "https://api.example.com" };
            var baseClient = new BaseClient(configuration, httpClient);
            var metrics = new Metrics(configuration);

            var oauth2Client = new OAuth2Client(credentials, baseClient, retryParams, metrics);

            var token = await oauth2Client.GetAccessTokenAsync();

            Assert.NotNull(token);
            Assert.Equal(2, callCount);
        }

        #endregion
    }
}