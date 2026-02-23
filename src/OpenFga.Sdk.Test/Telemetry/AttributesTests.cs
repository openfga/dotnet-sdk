//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//


using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Model;
using OpenFga.Sdk.Telemetry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace OpenFga.Sdk.Test.Telemetry {
    /// <summary>
    /// Unit tests for Attributes telemetry attribute building.
    /// </summary>
    public class AttributesTests {
        private const string TestStoreId = "01H0H015178Y2V4CX10C2KGHF4";

        /// <summary>
        /// Creates a minimal successful HttpResponseMessage suitable for BuildAttributesForResponse.
        /// </summary>
        private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode = HttpStatusCode.OK) {
            return new HttpResponseMessage {
                StatusCode = statusCode,
                RequestMessage = new HttpRequestMessage(HttpMethod.Post,
                    $"https://api.fga.example/stores/{TestStoreId}/batch-check")
            };
        }

        #region RequestBatchCheckSize – positive cases

        /// <summary>
        /// When RequestBatchCheckSize is enabled and apiName is "BatchCheck",
        /// the resulting attributes must contain fga-client.request.batch_check_size = N.
        /// </summary>
        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        public void BuildAttributesForResponse_BatchCheck_IncludesBatchCheckSizeAttribute(int checkCount) {
            var checks = BuildChecks(checkCount);
            var requestBody = new BatchCheckRequest(checks: checks);
            var requestBuilder = new RequestBuilder<BatchCheckRequest> {
                Method = HttpMethod.Post,
                BasePath = "https://api.fga.example",
                PathTemplate = $"/stores/{TestStoreId}/batch-check",
                PathParameters = new Dictionary<string, string> { { "store_id", TestStoreId } },
                Body = requestBody
            };

            var enabledAttributes = new HashSet<string> { TelemetryAttribute.RequestBatchCheckSize };
            var response = CreateResponse();

            var tagList = Attributes.BuildAttributesForResponse(
                enabledAttributes,
                credentials: null,
                apiName: "BatchCheck",
                response: response,
                requestBuilder: requestBuilder,
                requestDuration: new Stopwatch(),
                retryCount: 0
            );

            var batchSizeTag = tagList
                .FirstOrDefault(t => t.Key == TelemetryAttribute.RequestBatchCheckSize);

            Assert.NotNull(batchSizeTag.Key);
            Assert.Equal(checkCount, (int)batchSizeTag.Value!);
        }

        /// <summary>
        /// When all attributes are enabled, BatchCheck requests still include the correct batch size.
        /// </summary>
        [Fact]
        public void BuildAttributesForResponse_BatchCheck_AllAttributesEnabled_IncludesBatchCheckSize() {
            const int checkCount = 5;
            var checks = BuildChecks(checkCount);
            var requestBody = new BatchCheckRequest(checks: checks);
            var requestBuilder = new RequestBuilder<BatchCheckRequest> {
                Method = HttpMethod.Post,
                BasePath = "https://api.fga.example",
                PathTemplate = $"/stores/{TestStoreId}/batch-check",
                PathParameters = new Dictionary<string, string> { { "store_id", TestStoreId } },
                Body = requestBody
            };

            var enabledAttributes = TelemetryAttribute.GetAllAttributes();
            var response = CreateResponse();

            var tagList = Attributes.BuildAttributesForResponse(
                enabledAttributes,
                credentials: null,
                apiName: "BatchCheck",
                response: response,
                requestBuilder: requestBuilder,
                requestDuration: new Stopwatch(),
                retryCount: 0
            );

            var batchSizeTag = tagList
                .FirstOrDefault(t => t.Key == TelemetryAttribute.RequestBatchCheckSize);

            Assert.NotNull(batchSizeTag.Key);
            Assert.Equal(checkCount, (int)batchSizeTag.Value!);
        }

        #endregion

        #region RequestBatchCheckSize – negative cases

        /// <summary>
        /// When apiName is NOT "BatchCheck", the fga-client.request.batch_check_size attribute
        /// must NOT be present even if RequestBatchCheckSize is enabled.
        /// </summary>
        [Theory]
        [InlineData("Check")]
        [InlineData("ListObjects")]
        [InlineData("Write")]
        [InlineData("Read")]
        public void BuildAttributesForResponse_NonBatchCheckApi_DoesNotIncludeBatchCheckSizeAttribute(string apiName) {
            var requestBuilder = new RequestBuilder<object> {
                Method = HttpMethod.Post,
                BasePath = "https://api.fga.example",
                PathTemplate = $"/stores/{TestStoreId}/check",
                PathParameters = new Dictionary<string, string> { { "store_id", TestStoreId } },
                Body = null
            };

            var enabledAttributes = new HashSet<string> { TelemetryAttribute.RequestBatchCheckSize };
            var response = CreateResponse();

            var tagList = Attributes.BuildAttributesForResponse(
                enabledAttributes,
                credentials: null,
                apiName: apiName,
                response: response,
                requestBuilder: requestBuilder,
                requestDuration: new Stopwatch(),
                retryCount: 0
            );

            var hasBatchSizeTag = tagList
                .Any(t => t.Key == TelemetryAttribute.RequestBatchCheckSize);

            Assert.False(hasBatchSizeTag);
        }

        /// <summary>
        /// When RequestBatchCheckSize is NOT in the enabled set, the attribute must be absent
        /// even for BatchCheck requests.
        /// </summary>
        [Fact]
        public void BuildAttributesForResponse_BatchCheck_AttributeNotEnabled_DoesNotIncludeBatchCheckSize() {
            var checks = BuildChecks(3);
            var requestBody = new BatchCheckRequest(checks: checks);
            var requestBuilder = new RequestBuilder<BatchCheckRequest> {
                Method = HttpMethod.Post,
                BasePath = "https://api.fga.example",
                PathTemplate = $"/stores/{TestStoreId}/batch-check",
                PathParameters = new Dictionary<string, string> { { "store_id", TestStoreId } },
                Body = requestBody
            };

            // Enable everything EXCEPT RequestBatchCheckSize
            var enabledAttributes = TelemetryAttribute.GetAllAttributes();
            enabledAttributes.Remove(TelemetryAttribute.RequestBatchCheckSize);

            var response = CreateResponse();

            var tagList = Attributes.BuildAttributesForResponse(
                enabledAttributes,
                credentials: null,
                apiName: "BatchCheck",
                response: response,
                requestBuilder: requestBuilder,
                requestDuration: new Stopwatch(),
                retryCount: 0
            );

            var hasBatchSizeTag = tagList
                .Any(t => t.Key == TelemetryAttribute.RequestBatchCheckSize);

            Assert.False(hasBatchSizeTag);
        }

        /// <summary>
        /// When the BatchCheck request body is null, the attribute must be absent (no exception).
        /// </summary>
        [Fact]
        public void BuildAttributesForResponse_BatchCheck_NullBody_DoesNotThrow_AndAttributeAbsent() {
            var requestBuilder = new RequestBuilder<BatchCheckRequest> {
                Method = HttpMethod.Post,
                BasePath = "https://api.fga.example",
                PathTemplate = $"/stores/{TestStoreId}/batch-check",
                PathParameters = new Dictionary<string, string> { { "store_id", TestStoreId } },
                Body = null
            };

            var enabledAttributes = new HashSet<string> { TelemetryAttribute.RequestBatchCheckSize };
            var response = CreateResponse();

            var tagList = Attributes.BuildAttributesForResponse(
                enabledAttributes,
                credentials: null,
                apiName: "BatchCheck",
                response: response,
                requestBuilder: requestBuilder,
                requestDuration: new Stopwatch(),
                retryCount: 0
            );

            var hasBatchSizeTag = tagList
                .Any(t => t.Key == TelemetryAttribute.RequestBatchCheckSize);

            Assert.False(hasBatchSizeTag);
        }

        #endregion

        #region Helpers

        private static List<BatchCheckItem> BuildChecks(int count) {
            var items = new List<BatchCheckItem>(count);
            for (var i = 0; i < count; i++) {
                items.Add(new BatchCheckItem(
                    tupleKey: new CheckRequestTupleKey(
                        user: $"user:{i}",
                        relation: "viewer",
                        varObject: $"document:{i}"
                    ),
                    correlationId: $"corr-{i}"
                ));
            }
            return items;
        }

        #endregion
    }
}


