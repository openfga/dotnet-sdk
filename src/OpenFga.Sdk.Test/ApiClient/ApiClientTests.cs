//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//


using Moq;
using Moq.Protected;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
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
    /// Unit tests for ApiClient authentication token handling
    /// </summary>
    public class ApiClientTests {
        private const string TestHost = "api.fga.example";
        private const string TestStoreId = "01H0H015178Y2V4CX10C2KGHF4";
        private const string TestApiToken = "test-api-token-12345";
        private const string TestOAuthToken = "test-oauth-token-67890";
        private const string TestTokenIssuer = "oauth.example.com";
        private const string TestClientId = "test-client-id";
        private const string TestClientSecret = "test-client-secret";
        private const string TestAudience = "test-audience";

        #region Test Helpers

        /// <summary>
        /// Creates a Configuration with ApiToken credentials
        /// </summary>
        private static SdkConfiguration CreateApiTokenConfiguration(string apiToken = TestApiToken) {
            var credentialsConfig = new CredentialsConfig {
                ApiToken = apiToken
            };

            return new SdkConfiguration {
                ApiHost = TestHost,
                Credentials = new Credentials {
                    Method = CredentialsMethod.ApiToken,
                    Config = credentialsConfig
                }
            };
        }

        /// <summary>
        /// Creates a Configuration with OAuth ClientCredentials
        /// </summary>
        private static SdkConfiguration CreateOAuthConfiguration() {
            var credentialsConfig = new CredentialsConfig {
                ClientId = TestClientId,
                ClientSecret = TestClientSecret,
                ApiTokenIssuer = TestTokenIssuer,
                ApiAudience = TestAudience
            };

            return new SdkConfiguration {
                ApiHost = TestHost,
                Credentials = new Credentials {
                    Method = CredentialsMethod.ClientCredentials,
                    Config = credentialsConfig
                }
            };
        }

        /// <summary>
        /// Creates a Configuration with no credentials
        /// </summary>
        private static SdkConfiguration CreateNoCredentialsConfiguration() {
            return new SdkConfiguration {
                ApiHost = TestHost
            };
        }

        /// <summary>
        /// Creates a mock HTTP response for a successful API call
        /// </summary>
        private static HttpResponseMessage CreateSuccessResponse<T>(T content) {
            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonSerializer.Serialize(content),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
        }

        /// <summary>
        /// Creates a mock HTTP response for OAuth token exchange
        /// </summary>
        private static HttpResponseMessage CreateOAuthTokenResponse(string token = TestOAuthToken) {
            var tokenResponse = new {
                access_token = token,
                token_type = "Bearer",
                expires_in = 3600
            };

            return new HttpResponseMessage {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    JsonSerializer.Serialize(tokenResponse),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
        }

        #endregion

        #region ApiToken Authentication Tests

        /// <summary>
        /// Test that ApiToken credentials result in Authorization header being sent
        /// </summary>
        [Fact]
        public async Task ApiClient_WithApiToken_SendsAuthorizationHeader() {
            var config = CreateApiTokenConfiguration();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Verify the Authorization header is sent with the correct ApiToken
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Scheme == "Bearer" &&
                        req.Headers.Authorization.Parameter == TestApiToken),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(CreateSuccessResponse(new ReadAuthorizationModelsResponse {
                    AuthorizationModels = new System.Collections.Generic.List<AuthorizationModel>()
                }));

            var httpClient = new HttpClient(mockHandler.Object);
            var apiClient = new Sdk.ApiClient.ApiClient(config, httpClient);

            // Make a request through the API client
            var requestBuilder = new RequestBuilder<object> {
                Method = HttpMethod.Get,
                BasePath = config.BasePath,
                PathTemplate = $"/stores/{TestStoreId}/authorization-models",
                Body = null
            };

            await apiClient.SendRequestAsync<object, ReadAuthorizationModelsResponse>(
                requestBuilder,
                "ReadAuthorizationModels"
            );

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        /// <summary>
        /// Test that ApiToken does NOT pollute DefaultHeaders
        /// Regression test for issue #146
        /// </summary>
        [Fact]
        public void ApiClient_WithApiToken_DoesNotModifyDefaultHeaders() {
            var config = CreateApiTokenConfiguration();
            
            // Capture the original headers
            var originalHeadersCount = config.DefaultHeaders.Count;
            var containedAuthBefore = config.DefaultHeaders.ContainsKey("Authorization");

            // Create the ApiClient (should not throw or modify DefaultHeaders)
            _ = new Sdk.ApiClient.ApiClient(config);

            // Verify DefaultHeaders were not modified
            Assert.Equal(originalHeadersCount, config.DefaultHeaders.Count);
            Assert.Equal(containedAuthBefore, config.DefaultHeaders.ContainsKey("Authorization"));
            Assert.False(config.DefaultHeaders.ContainsKey("Authorization"));
        }

        #endregion

        #region OAuth Authentication Tests

        /// <summary>
        /// Test that OAuth credentials result in Authorization header being sent
        /// </summary>
        [Fact]
        public async Task ApiClient_WithOAuth_SendsAuthorizationHeader() {
            var config = CreateOAuthConfiguration();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Mock the OAuth token exchange
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri.ToString().Contains("/oauth/token")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(CreateOAuthTokenResponse());

            // Mock the actual API request and verify OAuth token is used
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Scheme == "Bearer" &&
                        req.Headers.Authorization.Parameter == TestOAuthToken &&
                        !req.RequestUri.ToString().Contains("/oauth/token")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(CreateSuccessResponse(new ReadAuthorizationModelsResponse {
                    AuthorizationModels = new System.Collections.Generic.List<AuthorizationModel>()
                }));

            var httpClient = new HttpClient(mockHandler.Object);
            var apiClient = new Sdk.ApiClient.ApiClient(config, httpClient);

            var requestBuilder = new RequestBuilder<object> {
                Method = HttpMethod.Get,
                BasePath = config.BasePath,
                PathTemplate = $"/stores/{TestStoreId}/authorization-models",
                Body = null
            };

            await apiClient.SendRequestAsync<object, ReadAuthorizationModelsResponse>(
                requestBuilder,
                "ReadAuthorizationModels"
            );

            // Verify both OAuth exchange and API call were made
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2), // One for OAuth token, one for API call
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        #endregion

        #region No Credentials Tests

        /// <summary>
        /// Test that requests work without credentials (no Authorization header)
        /// </summary>
        [Fact]
        public async Task ApiClient_WithNoCredentials_SendsNoAuthorizationHeader() {
            var config = CreateNoCredentialsConfiguration();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Verify NO Authorization header is sent
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Authorization == null),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(CreateSuccessResponse(new ReadAuthorizationModelsResponse {
                    AuthorizationModels = new System.Collections.Generic.List<AuthorizationModel>()
                }));

            var httpClient = new HttpClient(mockHandler.Object);
            var apiClient = new Sdk.ApiClient.ApiClient(config, httpClient);

            var requestBuilder = new RequestBuilder<object> {
                Method = HttpMethod.Get,
                BasePath = config.BasePath,
                PathTemplate = $"/stores/{TestStoreId}/authorization-models",
                Body = null
            };

            await apiClient.SendRequestAsync<object, ReadAuthorizationModelsResponse>(
                requestBuilder,
                "ReadAuthorizationModels"
            );

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        #endregion

        #region Configuration Validation Tests

        /// <summary>
        /// Test that Configuration.EnsureValid() passes with ApiToken credentials
        /// </summary>
        [Fact]
        public void Configuration_WithApiToken_PassesValidation() {
            var config = CreateApiTokenConfiguration();
            
            // Should not throw
            config.EnsureValid();
        }

        /// <summary>
        /// Test that ApiClient can be created successfully with ApiToken credentials
        /// </summary>
        [Fact]
        public void ApiClient_WithApiToken_CreatesSuccessfully() {
            var config = CreateApiTokenConfiguration();
            
            // Should not throw
            var apiClient = new Sdk.ApiClient.ApiClient(config);
            
            Assert.NotNull(apiClient);
        }

        /// <summary>
        /// Test that custom headers in options override default behavior correctly
        /// </summary>
        [Fact]
        public async Task ApiClient_WithApiToken_CustomHeadersInOptions() {
            var config = CreateApiTokenConfiguration();
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            // Verify custom header is present along with Authorization
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Parameter == TestApiToken &&
                        req.Headers.Contains("X-Custom-Header") &&
                        req.Headers.GetValues("X-Custom-Header").Contains("custom-value")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(CreateSuccessResponse(new ReadAuthorizationModelsResponse {
                    AuthorizationModels = new System.Collections.Generic.List<AuthorizationModel>()
                }));

            var httpClient = new HttpClient(mockHandler.Object);
            var apiClient = new Sdk.ApiClient.ApiClient(config, httpClient);

            var requestBuilder = new RequestBuilder<object> {
                Method = HttpMethod.Get,
                BasePath = config.BasePath,
                PathTemplate = $"/stores/{TestStoreId}/authorization-models",
                Body = null
            };

            var options = new ClientRequestOptions {
                Headers = new System.Collections.Generic.Dictionary<string, string> {
                    { "X-Custom-Header", "custom-value" }
                }
            };

            await apiClient.SendRequestAsync<object, ReadAuthorizationModelsResponse>(
                requestBuilder,
                "ReadAuthorizationModels",
                options
            );

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        #endregion
    }
}

