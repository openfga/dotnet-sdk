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

namespace OpenFga.Sdk.Framework.Tests
{
    public class FrameworkCompatibilityTests
    {
        [Fact]
        public void CanCreateClientConfiguration()
        {
            // This test verifies that the SDK can be properly referenced and used in .NET Framework 4.8
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
        public void CanUseNetStandard20Collections()
        {
            // Create a dictionary and use extension methods
            var dict = new Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            };
            
            // Access using extensions
            var hasKey = dict.ContainsKey("key1");
            Assert.True(hasKey);
            
            // Create list and use its methods
            var list = new List<string> { "item1", "item2" };
            Assert.Equal(2, list.Count);
        }
    }
}