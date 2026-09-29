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
        [Fact]
        public void WriteRequestWritesEqualsNullSafety() {
            var writesWithNull = new WriteRequestWrites(new List<TupleKey>(), onDuplicate: null);
            var writesWithError = new WriteRequestWrites(new List<TupleKey>(), onDuplicate: WriteRequestWrites.OnDuplicateEnum.Error);
            var writesWithNull2 = new WriteRequestWrites(new List<TupleKey>(), onDuplicate: null);

            Assert.False(writesWithNull.Equals(writesWithError));
            Assert.False(writesWithError.Equals(writesWithNull));
            Assert.True(writesWithNull.Equals(writesWithNull2));

            var writesNullProps1 = new WriteRequestWrites(new List<TupleKey>(), onDuplicate: null) { AdditionalProperties = null };
            var writesNullProps2 = new WriteRequestWrites(new List<TupleKey>(), onDuplicate: null) { AdditionalProperties = null };
            Assert.True(writesNullProps1.Equals(writesNullProps2));
            Assert.False(writesNullProps1.Equals(writesWithNull));
            Assert.False(writesWithNull.Equals(writesNullProps1));
        }
        [Fact]
        public void WriteRequestDeletesEqualsNullSafety() {
            var deletesWithNull = new WriteRequestDeletes(new List<TupleKeyWithoutCondition>(), onMissing: null);
            var deletesWithError = new WriteRequestDeletes(new List<TupleKeyWithoutCondition>(), onMissing: WriteRequestDeletes.OnMissingEnum.Error);
            var deletesWithNull2 = new WriteRequestDeletes(new List<TupleKeyWithoutCondition>(), onMissing: null);

            Assert.False(deletesWithNull.Equals(deletesWithError));
            Assert.False(deletesWithError.Equals(deletesWithNull));
            Assert.True(deletesWithNull.Equals(deletesWithNull2));

            var deletesNullProps1 = new WriteRequestDeletes(new List<TupleKeyWithoutCondition>(), onMissing: null) { AdditionalProperties = null };
            var deletesNullProps2 = new WriteRequestDeletes(new List<TupleKeyWithoutCondition>(), onMissing: null) { AdditionalProperties = null };
            Assert.True(deletesNullProps1.Equals(deletesNullProps2));
            Assert.False(deletesNullProps1.Equals(deletesWithNull));
            Assert.False(deletesWithNull.Equals(deletesNullProps1));
        }
        [Fact]
        public void CheckRequestEqualsNullSafety() {
            var checkWithDefault = new CheckRequest(new CheckRequestTupleKey("doc:1", "viewer", "user:anne"));
            var checkWithNullProps1 = new CheckRequest(new CheckRequestTupleKey("doc:1", "viewer", "user:anne")) { AdditionalProperties = null };
            var checkWithNullProps2 = new CheckRequest(new CheckRequestTupleKey("doc:1", "viewer", "user:anne")) { AdditionalProperties = null };

            Assert.True(checkWithNullProps1.Equals(checkWithNullProps2));
            Assert.False(checkWithNullProps1.Equals(checkWithDefault));
            Assert.False(checkWithDefault.Equals(checkWithNullProps1));
        }
    }
}
