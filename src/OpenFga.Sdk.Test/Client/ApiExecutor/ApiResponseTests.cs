using OpenFga.Sdk.Client.ApiExecutor;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace OpenFga.Sdk.Test.Client.ApiExecutor;

public class ApiResponseTests {
    [Fact]
    public void Constructor_ValidData_SetsAllProperties() {
        // Arrange
        var statusCode = HttpStatusCode.OK;
        var headers = new Dictionary<string, IEnumerable<string>> {
            { "X-Request-Id", new[] { "req-123" } },
            { "Content-Type", new[] { "application/json" } }
        };
        var rawResponse = "{\"id\":\"123\"}";
        var data = new { id = "123" };

        // Act
        var response = new ApiResponse<object>(statusCode, headers, rawResponse, data);

        // Assert
        Assert.Equal(statusCode, response.StatusCode);
        Assert.Equal(headers, response.Headers);
        Assert.Equal(rawResponse, response.RawResponse);
        Assert.Equal(data, response.Data);
    }

    [Fact]
    public void IsSuccessful_2xxStatusCode_ReturnsTrue() {
        // Arrange
        var headers = new Dictionary<string, IEnumerable<string>>();
        var testCases = new[] {
            HttpStatusCode.OK,                  // 200
            HttpStatusCode.Created,             // 201
            HttpStatusCode.Accepted,            // 202
            HttpStatusCode.NoContent            // 204
        };

        foreach (var statusCode in testCases) {
            // Act
            var response = new ApiResponse<string>(statusCode, headers, "", null);

            // Assert
            Assert.True(response.IsSuccessful, $"Status code {statusCode} should be successful");
        }
    }

    [Fact]
    public void IsSuccessful_NonSuccessStatusCode_ReturnsFalse() {
        // Arrange
        var headers = new Dictionary<string, IEnumerable<string>>();
        var testCases = new[] {
            HttpStatusCode.BadRequest,          // 400
            HttpStatusCode.Unauthorized,        // 401
            HttpStatusCode.Forbidden,           // 403
            HttpStatusCode.NotFound,            // 404
            HttpStatusCode.InternalServerError, // 500
            HttpStatusCode.ServiceUnavailable   // 503
        };

        foreach (var statusCode in testCases) {
            // Act
            var response = new ApiResponse<string>(statusCode, headers, "", null);

            // Assert
            Assert.False(response.IsSuccessful, $"Status code {statusCode} should not be successful");
        }
    }

    [Fact]
    public void FromHttpResponse_ContainsAllResponseHeaders() {
        // Arrange
        var httpResponse = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"id\":\"123\"}", Encoding.UTF8, "application/json")
        };
        httpResponse.Headers.Add("X-Request-Id", "req-123");
        httpResponse.Headers.Add("X-Trace-Id", "trace-456");

        var rawResponse = "{\"id\":\"123\"}";
        var data = new { id = "123" };

        // Act
        var response = ApiResponse<object>.FromHttpResponse(httpResponse, rawResponse, data);

        // Assert
        Assert.True(response.Headers.ContainsKey("X-Request-Id"));
        Assert.Equal("req-123", response.Headers["X-Request-Id"].First());
        Assert.True(response.Headers.ContainsKey("X-Trace-Id"));
        Assert.Equal("trace-456", response.Headers["X-Trace-Id"].First());
    }

    [Fact]
    public void FromHttpResponse_ContainsContentHeaders() {
        // Arrange
        var httpResponse = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"id\":\"123\"}", Encoding.UTF8, "application/json")
        };

        var rawResponse = "{\"id\":\"123\"}";
        var data = new { id = "123" };

        // Act
        var response = ApiResponse<object>.FromHttpResponse(httpResponse, rawResponse, data);

        // Assert
        Assert.True(response.Headers.ContainsKey("Content-Type"));
        Assert.Contains("application/json", response.Headers["Content-Type"].First());
    }

    [Fact]
    public void Headers_AreCaseInsensitive() {
        // Arrange
        var httpResponse = new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("", Encoding.UTF8, "application/json")
        };
        httpResponse.Headers.Add("X-Custom-Header", "value");

        var rawResponse = "";
        var data = (string)null;

        // Act
        var response = ApiResponse<string>.FromHttpResponse(httpResponse, rawResponse, data);

        // Assert
        Assert.True(response.Headers.ContainsKey("x-custom-header"));
        Assert.True(response.Headers.ContainsKey("X-CUSTOM-HEADER"));
        Assert.True(response.Headers.ContainsKey("X-Custom-Header"));
    }

    [Fact]
    public void FromHttpResponse_WithNullData_StoresNull() {
        // Arrange
        using var httpResponse = new HttpResponseMessage {
            StatusCode = HttpStatusCode.NoContent
        };

        var rawResponse = "";

        // Act
        var response = ApiResponse<string>.FromHttpResponse(httpResponse, rawResponse, null);

        // Assert
        Assert.Null(response.Data);
        Assert.Equal("", response.RawResponse);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public void RawResponse_PreservesOriginalContent() {
        // Arrange
        var rawJson = "{\"id\":\"123\",\"name\":\"test\",\"nested\":{\"value\":42}}";
        var headers = new Dictionary<string, IEnumerable<string>>();
        var data = new { id = "123" };

        // Act
        var response = new ApiResponse<object>(HttpStatusCode.OK, headers, rawJson, data);

        // Assert
        Assert.Equal(rawJson, response.RawResponse);
    }
}
