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


using OpenFga.Sdk.Test.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace OpenFga.Sdk.Test.FrameworkCompatibility {
    public class FrameworkTests {
        [Fact]
        public void VerifyFrameworkDetection() {
            string frameworkName = FrameworkCompat.GetFrameworkName();
            Console.WriteLine($"Running tests on {frameworkName}");

            // This test just verifies that our multi-targeting configuration works
            Assert.NotNull(frameworkName);
        }

        [Fact]
        public void TestStandardLibraryFeatures() {
            // Test .NET Standard 2.0 compatible Dictionary features
            var dictionary = new Dictionary<string, string>();
            dictionary["key1"] = "value1";
            dictionary["key2"] = "value2";

            // Verify we can use basic LINQ features (.NET Standard 2.0 compatible)
            var keys = new List<string>();
            foreach (var kvp in dictionary) {
                keys.Add(kvp.Key);
            }

            Assert.Equal(2, keys.Count);
            Assert.Contains("key1", keys);
            Assert.Contains("key2", keys);
        }

#if !NET48
        // Skip on .NET Framework 4.8 as it might not have some of these features
        [Fact]
        public void TestHttpClientFunctionality() {
            // Create an HTTP client (should work across all frameworks)
            var client = new System.Net.Http.HttpClient();
            Assert.NotNull(client);

            // Test that we can create basic request messages
            var request = new System.Net.Http.HttpRequestMessage(
                System.Net.Http.HttpMethod.Get,
                "https://example.com");

            Assert.NotNull(request);
            Assert.Equal(System.Net.Http.HttpMethod.Get, request.Method);
        }
#endif
    }
}