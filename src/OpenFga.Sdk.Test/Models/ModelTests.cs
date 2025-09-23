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


using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace OpenFga.Sdk.Test.Models {
    public class OpenFgaModelTests : IDisposable {

        public void Dispose() {
            // Cleanup when everything is done.
        }

        /// <summary>
        /// Deserialize ReadAuthorizationModelsResponse
        /// </summary>
        [Fact]
        public void DeserializeReadAuthorizationModelsResponse() {
            var jsonResponse =
                "{\"authorization_models\":[{\"id\":\"01FQHMTEX3ASF7TAGZZ828KSQ2\",\"type_definitions\":[{\"type\":\"group\",\"relations\":{\"member\":{\"this\":{}}}},{\"type\":\"folder\",\"relations\":{\"create_file\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"owner\":{\"this\":{}},\"parent\":{\"this\":{}},\"viewer\":{\"union\":{\"child\":[{\"this\":{}},{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}}}]}}}},{\"type\":\"doc\",\"relations\":{\"change_owner\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"owner\":{\"this\":{}},\"parent\":{\"this\":{}},\"read\":{\"union\":{\"child\":[{\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}},{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}}}]}},\"share\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"viewer\":{\"this\":{}},\"write\":{\"union\":{\"child\":[{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}}}]}}}}]}],\"continuation_token\":\"\"}";

            JsonSerializer.Deserialize<ReadAuthorizationModelsResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ReadAuthorizationModelResponse
        /// </summary>
        [Fact]
        public void DeserializeReadAuthorizationModelResponse() {
            var jsonResponse =
                "{\"authorization_model\":{\"id\":\"01FQHMTEX3ASF7TAGZZ828KSQ2\",\"type_definitions\":[{\"type\":\"group\",\"relations\":{\"member\":{\"this\":{}}}},{\"type\":\"folder\",\"relations\":{\"create_file\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"owner\":{\"this\":{}},\"parent\":{\"this\":{}},\"viewer\":{\"union\":{\"child\":[{\"this\":{}},{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}}}]}}}},{\"type\":\"doc\",\"relations\":{\"change_owner\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"owner\":{\"this\":{}},\"parent\":{\"this\":{}},\"read\":{\"union\":{\"child\":[{\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}},{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"viewer\"}}}]}},\"share\":{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},\"viewer\":{\"this\":{}},\"write\":{\"union\":{\"child\":[{\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}},{\"tupleToUserset\":{\"tupleset\":{\"object\":\"\",\"relation\":\"parent\"},\"computedUserset\":{\"object\":\"\",\"relation\":\"owner\"}}}]}}}}]}}";

            JsonSerializer.Deserialize<ReadAuthorizationModelResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize WriteAuthorizationModelResponse
        /// </summary>
        [Fact]
        public void DeserializeWriteAuthorizationModelResponse() {
            var jsonResponse =
                "{\"authorization_model_id\":\"01G56QKZCF23KQ1WPQPAK3WXKB\"}";

            JsonSerializer.Deserialize<WriteAuthorizationModelResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ReadResponse
        /// </summary>
        [Fact]
        public void DeserializeReadResponse() {
            var jsonResponse =
                "{\"tuples\":[{\"tuple_key\":{\"object\":\"document:planning\",\"relation\":\"viewer\",\"user\":\"user:jane\"},\"timestamp\":\"2022-01-01T00:00:00.000000000Z\"}]}";

            JsonSerializer.Deserialize<ReadResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ReadChangesResponse
        /// </summary>
        [Fact]
        public void DeserializeReadChangesResponse() {
            var jsonResponse =
                "{\"changes\":[{\"tuple_key\":{\"object\":\"document:planning\",\"relation\":\"viewer\",\"user\":\"user:jane\"},\"operation\":\"TUPLE_OPERATION_WRITE\",\"timestamp\":\"2022-01-01T00:00:00.000000000Z\"},{\"tuple_key\":{\"object\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a\",\"relation\":\"owner\",\"user\":\"user:anna\"},\"operation\":\"TUPLE_OPERATION_DELETE\",\"timestamp\":\"2022-01-01T00:00:00.000000000Z\"}],\"continuation_token\":\"abcxyz==\"}";

            JsonSerializer.Deserialize<ReadChangesResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize CheckResponse
        /// </summary>
        [Fact]
        public void DeserializeCheckResponse() {
            var jsonResponse =
                "{\"allowed\":true,\"resolution\":\"\"}";

            JsonSerializer.Deserialize<CheckResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ReadAssertionsResponse
        /// </summary>
        [Fact]
        public void DeserializeReadAssertionsResponse() {
            var jsonResponse =
                "{\"authorization_model_id\":\"01FQHMTEX3ASF7TAGZZ828KSQ2\",\"assertions\":[{\"tuple_key\":{\"object\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a\",\"relation\":\"viewer\",\"user\":\"carlos\"},\"expectation\":true}]}";

            JsonSerializer.Deserialize<ReadAssertionsResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ExpandResponse
        /// </summary>
        [Fact]
        public void DeserializeExpandResponse() {
            var jsonResponse =
                "{\"tree\":{\"root\":{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"union\":{\"nodes\":[{\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"users\":{\"users\":[\"team:product#member\"]}}}, {\"name\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"leaf\":{\"tupleToUserset\":{\"tupleset\":\"document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a#owner\", \"computed\":[{\"userset\":\"org:contoso#admin\"}]}}}]}}}}";

            JsonSerializer.Deserialize<ExpandResponse>(jsonResponse);
        }

        /// <summary>
        /// Deserialize ListObjectsResponse
        /// </summary>
        [Fact]
        public void DeserializeListObjectsResponse() {
            var jsonResponse =
                "{\"object_ids\":[\"roadmap\"]}";

            JsonSerializer.Deserialize<ListObjectsResponse>(jsonResponse);
        }
    }
}