using OpenFga.Sdk.Client.ApiExecutor;
using OpenFga.Sdk.Exceptions;
using System;
using System.Net.Http;
using Xunit;

namespace OpenFga.Sdk.Test.Client.ApiExecutor;

public class ApiExecutorRequestBuilderTests {
    [Fact]
    public void Of_ValidMethodAndPath_ReturnsBuilder() {
        // Act
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Get, "/stores");

        // Assert
        Assert.NotNull(builder);
        Assert.Equal(HttpMethod.Get, builder.Method);
        Assert.Equal("/stores", builder.PathTemplate);
    }

    [Fact]
    public void Of_NullMethod_ThrowsArgumentNullException() {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ApiExecutorRequestBuilder.Of(null, "/stores"));
        Assert.Contains("method", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Of_NullPath_ThrowsArgumentNullException() {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ApiExecutorRequestBuilder.Of(HttpMethod.Get, null));
        Assert.Contains("path", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Of_EmptyPath_ThrowsArgumentException() {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ApiExecutorRequestBuilder.Of(HttpMethod.Get, ""));
        Assert.Contains("path", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Of_WhitespacePath_ThrowsArgumentException() {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ApiExecutorRequestBuilder.Of(HttpMethod.Get, "   "));
        Assert.Contains("path", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Of_PathWithoutLeadingSlash_ThrowsArgumentException() {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            ApiExecutorRequestBuilder.Of(HttpMethod.Get, "stores"));
        Assert.Contains("must start with '/'", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PathParam_ValidKeyValue_StoresParameter() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores/{store_id}/check");

        // Act
        var result = builder.PathParam("store_id", "store123");

        // Assert
        Assert.Same(builder, result); // Verify fluent chaining
    }

    [Fact]
    public void PathParam_NullKey_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores/{store_id}/check");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.PathParam(null, "value"));
        Assert.Contains("key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PathParam_EmptyKey_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores/{store_id}/check");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.PathParam("", "value"));
        Assert.Contains("key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void PathParam_NullValue_ThrowsArgumentNullException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores/{store_id}/check");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            builder.PathParam("store_id", null));
        Assert.Contains("value", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void QueryParam_ValidKeyValue_StoresParameter() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Get, "/stores");

        // Act
        var result = builder.QueryParam("page_size", "20");

        // Assert
        Assert.Same(builder, result); // Verify fluent chaining
    }

    [Fact]
    public void QueryParam_NullKey_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Get, "/stores");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.QueryParam(null, "value"));
        Assert.Contains("key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void QueryParam_NullValue_ThrowsArgumentNullException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Get, "/stores");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            builder.QueryParam("page_size", null));
        Assert.Contains("value", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Header_ValidKeyValue_StoresHeader() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act
        var result = builder.Header("X-Custom-Header", "custom-value");

        // Assert
        Assert.Same(builder, result); // Verify fluent chaining
    }

    [Fact]
    public void Header_ReservedHeader_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act & Assert - Authorization is a reserved header
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.Header("Authorization", "Bearer token"));
        Assert.Contains("reserved", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Header_NullKey_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            builder.Header(null, "value"));
        Assert.Contains("key", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Header_NullValue_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.Header("X-Custom-Header", null));
        Assert.Contains("value", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Header_InvalidCharacters_ThrowsArgumentException() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act & Assert - CR/LF characters are not allowed
        var exception = Assert.Throws<ArgumentException>(() =>
            builder.Header("X-Custom-Header", "value\r\nwith newline"));
        Assert.Contains("invalid characters", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Body_ValidObject_StoresBody() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");
        var body = new { name = "test-store" };

        // Act
        var result = builder.Body(body);

        // Assert
        Assert.Same(builder, result); // Verify fluent chaining
    }

    [Fact]
    public void Body_NullObject_StoresNull() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores");

        // Act
        var result = builder.Body(null);

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void Build_WithValidRequest_ReturnsBuilder() {
        // Arrange
        var builder = ApiExecutorRequestBuilder.Of(HttpMethod.Post, "/stores/{store_id}/check")
            .PathParam("store_id", "store123")
            .QueryParam("page_size", "20")
            .Header("X-Custom-Header", "value")
            .Body(new { user = "user:anne" });

        // Act
        var result = builder.Build();

        // Assert
        Assert.Same(builder, result);
    }

    [Fact]
    public void FluentChaining_AllMethods_ReturnsBuilder() {
        // Act
        var builder = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/check")
            .PathParam("store_id", "store123")
            .QueryParam("page_size", "20")
            .Header("X-Custom-Header", "value")
            .Body(new { user = "user:anne" })
            .Build();

        // Assert
        Assert.NotNull(builder);
        Assert.Equal(HttpMethod.Post, builder.Method);
        Assert.Equal("/stores/{store_id}/check", builder.PathTemplate);
    }

    [Fact]
    public void ToRequestBuilder_ConvertsCorrectly() {
        // Arrange
        var builder = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores/{store_id}/check")
            .PathParam("store_id", "store123")
            .QueryParam("page_size", "20")
            .Body(new { user = "user:anne" })
            .Build();

        // Act
        var requestBuilder = builder.ToRequestBuilder("https://api.fga.example");

        // Assert
        Assert.NotNull(requestBuilder);
        Assert.Equal(HttpMethod.Post, requestBuilder.Method);
        Assert.Equal("https://api.fga.example", requestBuilder.BasePath);
        Assert.Equal("/stores/{store_id}/check", requestBuilder.PathTemplate);
        Assert.True(requestBuilder.PathParameters.TryGetValue("store_id", out var storeIdValue));
        Assert.Equal("store123", storeIdValue);
        Assert.True(requestBuilder.QueryParameters.TryGetValue("page_size", out var pageSizeValue));
        Assert.Equal("20", pageSizeValue);
        Assert.NotNull(requestBuilder.Body);
    }

    [Fact]
    public void GetHeaders_ReturnsCustomHeaders() {
        // Arrange
        var builder = ApiExecutorRequestBuilder
            .Of(HttpMethod.Post, "/stores")
            .Header("X-Custom-Header", "value1")
            .Header("X-Another-Header", "value2")
            .Build();

        // Act
        var headers = builder.GetHeaders();

        // Assert
        Assert.NotNull(headers);
        Assert.Equal(2, headers.Count);
        Assert.Equal("value1", headers["X-Custom-Header"]);
        Assert.Equal("value2", headers["X-Another-Header"]);
    }
}