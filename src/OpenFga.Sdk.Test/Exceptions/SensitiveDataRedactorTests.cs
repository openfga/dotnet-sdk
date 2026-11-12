using OpenFga.Sdk.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Exceptions {
    /// <summary>
    /// Unit tests for <see cref="SensitiveDataRedactor"/>, which keeps credentials out of exception objects.
    /// </summary>
    public class SensitiveDataRedactorTests {
        private const string Secret = "super-secret-value";
        private static readonly string[] MultiHeaderValues = { "one", "two" };

        /// <summary>
        /// The redactor may drop a form-encoded body (return <see langword="null"/>) when it cannot be safely re-read on
        /// a given runtime (e.g. .NET Framework). That is acceptable; what must never happen is the raw secret being retained.
        /// </summary>
        private static async Task AssertRedactedOrDropped(HttpContent? redacted, string expectedBody) {
            if (redacted == null) {
                return;
            }

            Assert.Equal("application/x-www-form-urlencoded", redacted.Headers.ContentType?.MediaType);
            var body = await redacted.ReadAsStringAsync();
            Assert.DoesNotContain(Secret, body);
            Assert.Equal(expectedBody, body);
        }

        #region Form-encoded bodies

        [Fact]
        public void RedactFormUrlEncodedBody_RedactsClientSecret_PreservesAllowedFieldsVerbatim() {
            var body = $"client_id=my-client&client_secret={Secret}&grant_type=client_credentials&audience=https%3A%2F%2Fapi.fga.example&scope=read+write";

            var redacted = SensitiveDataRedactor.RedactFormUrlEncodedBody(body);

            Assert.Equal(
                "client_id=my-client&client_secret=[REDACTED]&grant_type=client_credentials&audience=https%3A%2F%2Fapi.fga.example&scope=read+write",
                redacted);
            Assert.DoesNotContain(Secret, redacted);
        }

        [Theory]
        [InlineData("client_secret")]
        [InlineData("CLIENT_SECRET")]
        [InlineData("Client_Secret")]
        [InlineData("client%5Fsecret")] // '_' percent-encoded
        [InlineData("client_assertion")]
        [InlineData("assertion")]
        [InlineData("refresh_token")]
        [InlineData("password")]
        [InlineData("some_future_credential_field")]
        public void RedactFormUrlEncodedBody_RedactsEveryFieldThatIsNotAllowListed(string key) {
            var redacted = SensitiveDataRedactor.RedactFormUrlEncodedBody($"grant_type=client_credentials&{key}={Secret}");

            Assert.Equal($"grant_type=client_credentials&{key}=[REDACTED]", redacted);
            Assert.DoesNotContain(Secret, redacted);
        }

        [Theory]
        [InlineData("grant_type")]
        [InlineData("client_id")]
        [InlineData("audience")]
        [InlineData("scope")]
        [InlineData("Client_ID")]
        public void RedactFormUrlEncodedBody_PreservesAllowListedFields(string key) {
            var body = $"{key}=some-value";

            Assert.Equal(body, SensitiveDataRedactor.RedactFormUrlEncodedBody(body));
        }

        [Fact]
        public void RedactFormUrlEncodedBody_HandlesEdgeCases() {
            Assert.Equal("", SensitiveDataRedactor.RedactFormUrlEncodedBody(""));

            // A key without a value, repeated separators and a value containing '=' must not break parsing or leak
            Assert.Equal("flag=[REDACTED]", SensitiveDataRedactor.RedactFormUrlEncodedBody("flag"));
            Assert.Equal("client_id=a&&client_secret=[REDACTED]&", SensitiveDataRedactor.RedactFormUrlEncodedBody($"client_id=a&&client_secret={Secret}&"));
            Assert.Equal("client_secret=[REDACTED]", SensitiveDataRedactor.RedactFormUrlEncodedBody("client_secret=a=b=c"));
            Assert.Equal("client_secret=[REDACTED]", SensitiveDataRedactor.RedactFormUrlEncodedBody("client_secret="));
        }

        #endregion

        #region HttpContent

        [Fact]
        public async Task RedactRequestContent_FormUrlEncodedContent_IsRedacted() {
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "client_id", "my-client" },
                { "client_secret", Secret },
                { "grant_type", "client_credentials" },
            });

            var redacted = SensitiveDataRedactor.RedactRequestContent(content);

            Assert.NotSame(content, redacted);
            await AssertRedactedOrDropped(redacted, "client_id=my-client&client_secret=[REDACTED]&grant_type=client_credentials");
        }

        [Fact]
        public async Task RedactRequestContent_StringContentWithFormMediaType_IsRedacted() {
            var content = new StringContent($"client_id=my-client&client_secret={Secret}", Encoding.UTF8, "application/x-www-form-urlencoded");

            var redacted = SensitiveDataRedactor.RedactRequestContent(content);

            Assert.NotSame(content, redacted);
            await AssertRedactedOrDropped(redacted, "client_id=my-client&client_secret=[REDACTED]");
        }

        [Fact]
        public void RedactRequestContent_JsonContent_IsReturnedUnchanged() {
            var content = new StringContent("{\"tuple_key\":{\"user\":\"user:anne\"}}", Encoding.UTF8, "application/json");

            Assert.Same(content, SensitiveDataRedactor.RedactRequestContent(content));
        }

        [Fact]
        public void RedactRequestContent_Null_ReturnsNull() {
            Assert.Null(SensitiveDataRedactor.RedactRequestContent(null));
        }

        [Fact]
        public void RedactRequestContent_FormEncodedStreamContent_IsDroppedRatherThanRead() {
            // Non in-memory content cannot be safely re-read, so it must be dropped (never retained raw)
            var content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes($"client_secret={Secret}")));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            Assert.Null(SensitiveDataRedactor.RedactRequestContent(content));
        }

        [Fact]
        public void RedactRequestContent_DisposedFormEncodedContent_IsDroppedRatherThanThrowing() {
            // .NET Framework's HttpClient disposes the request content once the request has been sent
            var content = new FormUrlEncodedContent(new Dictionary<string, string> { { "client_secret", Secret } });
            content.Dispose();

            Assert.Null(SensitiveDataRedactor.RedactRequestContent(content));
        }

        #endregion

        #region Headers

        [Fact]
        public void RedactRequestHeaders_RedactsCredentialHeaders_KeepsSchemeAndOtherHeaders() {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.fga.example/stores");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Secret);
            request.Headers.TryAddWithoutValidation("Proxy-Authorization", $"Basic {Secret}");
            request.Headers.TryAddWithoutValidation("Cookie", $"session={Secret}");
            request.Headers.TryAddWithoutValidation("X-Custom-Header", "custom-value");
            request.Headers.TryAddWithoutValidation("X-Multi", MultiHeaderValues);

            var redacted = SensitiveDataRedactor.RedactRequestHeaders(request.Headers);

            Assert.NotSame(request.Headers, redacted);
            Assert.Equal("Bearer [REDACTED]", Assert.Single(redacted.GetValues("Authorization")));
            Assert.Equal("Basic [REDACTED]", Assert.Single(redacted.GetValues("Proxy-Authorization")));
            Assert.Equal("[REDACTED]", Assert.Single(redacted.GetValues("Cookie")));
            Assert.Equal("custom-value", Assert.Single(redacted.GetValues("X-Custom-Header")));
            Assert.Equal(MultiHeaderValues, redacted.GetValues("X-Multi").ToArray());
            Assert.DoesNotContain(Secret, redacted.ToString());

            // The original request headers are left untouched
            Assert.Equal(Secret, request.Headers.Authorization?.Parameter);
        }

        [Fact]
        public void RedactRequestHeaders_AuthorizationWithoutScheme_IsFullyRedacted() {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.fga.example/stores");
            request.Headers.TryAddWithoutValidation("Authorization", Secret);

            var redacted = SensitiveDataRedactor.RedactRequestHeaders(request.Headers);

            Assert.Equal("[REDACTED]", Assert.Single(redacted.GetValues("Authorization")));
        }

        [Fact]
        public void RedactRequestHeaders_HeaderNameMatchingIsCaseInsensitive() {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.fga.example/stores");
            request.Headers.TryAddWithoutValidation("authorization", $"Bearer {Secret}");

            var redacted = SensitiveDataRedactor.RedactRequestHeaders(request.Headers);

            Assert.DoesNotContain(Secret, redacted.ToString());
            Assert.Equal("Bearer [REDACTED]", Assert.Single(redacted.GetValues("Authorization")));
        }

        [Fact]
        public void RedactRequestHeaders_Null_ReturnsEmptyHeaders() {
            var redacted = SensitiveDataRedactor.RedactRequestHeaders(null);

            Assert.NotNull(redacted);
            Assert.Empty(redacted);
        }

        #endregion

        #region FgaApiError integration

        [Fact]
        public async Task FgaApiError_ConstructedFromRequest_DoesNotRetainCredentials() {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://issuer.fga.example/oauth/token") {
                Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                    { "client_id", "my-client" },
                    { "client_secret", Secret },
                    { "grant_type", "client_credentials" },
                })
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Secret);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized) {
                Content = new StringContent("{\"error\":\"invalid_client\"}", Encoding.UTF8, "application/json")
            };

            var error = new FgaApiError(response, request, "ExchangeTokenAsync");

            Assert.NotSame(request.Content, error.RequestData);
            await AssertRedactedOrDropped(error.RequestData, "client_id=my-client&client_secret=[REDACTED]&grant_type=client_credentials");
            Assert.Equal("Bearer [REDACTED]", Assert.Single(error.RequestHeaders.GetValues("Authorization")));
            Assert.DoesNotContain(Secret, error.ToString());

            // Non-sensitive metadata is still available
            Assert.Equal(HttpMethod.Post, error.Method);
            Assert.Equal("https://issuer.fga.example/oauth/token", error.RequestUrl);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, error.StatusCode);
            Assert.Same(response.Content, error.ResponseData);
        }

        #endregion
    }
}