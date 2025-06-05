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


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;

namespace OpenFga.Sdk.NetCore31.Tests
{
    public class NetCore31CompatibilityTests
    {
        [Fact]
        public void CanCreateClientConfiguration()
        {
            // This test verifies that the SDK can be properly referenced and used in .NET Core 3.1
            var config = new ClientConfiguration 
            { 
                ApiUrl = "https://api.openfga.example",
                StoreId = "01GXSA8YR785C4FYS3C0RTG7B1" 
            };
            
            Assert.NotNull(config);
            Assert.Equal("https://api.openfga.example", config.ApiUrl);
            Assert.Equal("01GXSA8YR785C4FYS3C0RTG7B1", config.StoreId);
        }
        
        [Fact]
        public void CanCreateOpenFgaClient()
        {
            var config = new ClientConfiguration 
            { 
                ApiUrl = "https://api.openfga.example",
                StoreId = "01GXSA8YR785C4FYS3C0RTG7B1" 
            };
            
            var client = new OpenFgaClient(config);
            Assert.NotNull(client);
            Assert.Equal("01GXSA8YR785C4FYS3C0RTG7B1", client.StoreId);
        }
        
        [Fact]
        public void CanSerializeObjects()
        {
            // Test using System.Text.Json which is one of the packages added for .NET Standard 2.0 compatibility
            var tuple = new Client.Model.ClientTupleKey
            {
                User = "user:123",
                Relation = "viewer",
                Object = "document:456"
            };
            
            var json = System.Text.Json.JsonSerializer.Serialize(tuple);
            Assert.Contains("user:123", json);
            Assert.Contains("viewer", json);
            Assert.Contains("document:456", json);
        }
        
        [Fact]
        public void CanUseHttpClientFeatures()
        {
            // Test that we can use HttpClient features in .NET Core 3.1
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler);
            
            // Create a request message (will be used for API calls)
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openfga.example");
            request.Headers.Add("X-Test-Header", "test-value");
            
            Assert.NotNull(client);
            Assert.NotNull(request);
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.True(request.Headers.Contains("X-Test-Header"));
        }
    }
}