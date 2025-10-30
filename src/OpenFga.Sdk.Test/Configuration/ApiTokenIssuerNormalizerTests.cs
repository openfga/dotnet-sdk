using OpenFga.Sdk.Configuration;
using Xunit;

namespace OpenFga.Sdk.Test.Configuration {
    public class ApiTokenIssuerNormalizerTests {

        [Theory]
        // Null and empty input tests
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        // No scheme tests
        [InlineData("issuer.fga.example", "https://issuer.fga.example")]
        [InlineData("issuer.fga.example:8080", "https://issuer.fga.example:8080")]
        [InlineData("issuer.fga.example/some_endpoint", "https://issuer.fga.example/some_endpoint")]
        [InlineData("issuer.fga.example:8080/some_endpoint", "https://issuer.fga.example:8080/some_endpoint")]
        // HTTPS scheme tests
        [InlineData("https://issuer.fga.example", "https://issuer.fga.example")]
        // HTTP scheme tests
        [InlineData("http://issuer.fga.example", "http://issuer.fga.example")]
        public void Normalize_ReturnsExpectedResult(string input, string expected) {
            // Act
            var result = ApiTokenIssuerNormalizer.Normalize(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}

