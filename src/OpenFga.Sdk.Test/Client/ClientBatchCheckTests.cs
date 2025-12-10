using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Client.Utils;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OpenFga.Sdk.Test.Client;

public class ClientBatchCheckTests {
    [Fact]
    public void ClientBatchCheckItem_Constructor_SetsProperties() {
        var item = new ClientBatchCheckItem(
            user: "user:anne",
            relation: "viewer",
            @object: "document:roadmap",
            correlationId: "test-id",
            contextualTuples: new ContextualTupleKeys(),
            context: new { ViewCount = 100 }
        );

        Assert.Equal("user:anne", item.User);
        Assert.Equal("viewer", item.Relation);
        Assert.Equal("document:roadmap", item.Object);
        Assert.Equal("test-id", item.CorrelationId);
        Assert.NotNull(item.ContextualTuples);
        Assert.NotNull(item.Context);
    }

    [Fact]
    public void ClientBatchCheckItem_Constructor_ThrowsOnNullUser() {
        Assert.Throws<ArgumentNullException>(() => new ClientBatchCheckItem(
            user: null!,
            relation: "viewer",
            @object: "document:roadmap"
        ));
    }

    [Fact]
    public void ClientBatchCheckItem_Constructor_ThrowsOnNullRelation() {
        Assert.Throws<ArgumentNullException>(() => new ClientBatchCheckItem(
            user: "user:anne",
            relation: null!,
            @object: "document:roadmap"
        ));
    }

    [Fact]
    public void ClientBatchCheckItem_Constructor_ThrowsOnNullObject() {
        Assert.Throws<ArgumentNullException>(() => new ClientBatchCheckItem(
            user: "user:anne",
            relation: "viewer",
            @object: null!
        ));
    }

    [Fact]
    public void ClientBatchCheckRequest_Constructor_SetsChecks() {
        var checks = new List<ClientBatchCheckItem> {
            new ClientBatchCheckItem("user:anne", "viewer", "document:roadmap")
        };
        var request = new ClientBatchCheckRequest(checks);

        Assert.Single(request.Checks);
        Assert.Equal("user:anne", request.Checks[0].User);
    }

    [Fact]
    public void ClientBatchCheckSingleResponse_Constructor_SetsProperties() {
        var item = new ClientBatchCheckItem("user:anne", "viewer", "document:roadmap");
        var response = new ClientBatchCheckSingleResponse(
            allowed: true,
            request: item,
            correlationId: "test-id"
        );

        Assert.True(response.Allowed);
        Assert.Equal("test-id", response.CorrelationId);
        Assert.Equal(item, response.Request);
        Assert.Null(response.Error);
    }

    [Fact]
    public void ClientBatchCheckResponse_Constructor_SetsResults() {
        var item = new ClientBatchCheckItem("user:anne", "viewer", "document:roadmap");
        var singleResponse = new ClientBatchCheckSingleResponse(true, item, "test-id");
        var response = new ClientBatchCheckResponse(new List<ClientBatchCheckSingleResponse> { singleResponse });

        Assert.Single(response.Result);
        Assert.Equal("test-id", response.Result[0].CorrelationId);
    }

    [Fact]
    public void ClientUtils_GenerateCorrelationId_ReturnsValidGuid() {
        var id = ClientUtils.GenerateCorrelationId();

        Assert.NotNull(id);
        Assert.NotEmpty(id);
        // Should be parseable as a GUID
        Assert.True(Guid.TryParse(id, out _));
    }

    [Fact]
    public void ClientUtils_GenerateCorrelationId_ReturnsUniqueValues() {
        var id1 = ClientUtils.GenerateCorrelationId();
        var id2 = ClientUtils.GenerateCorrelationId();

        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void ClientUtils_ChunkList_SplitsListCorrectly() {
        var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var chunks = ClientUtils.ChunkList(list, 3).ToList();

        Assert.Equal(4, chunks.Count);
        Assert.Equal(3, chunks[0].Count);
        Assert.Equal(3, chunks[1].Count);
        Assert.Equal(3, chunks[2].Count);
        Assert.Single(chunks[3]);
    }

    [Fact]
    public void ClientUtils_ChunkList_HandlesExactDivision() {
        var list = new List<int> { 1, 2, 3, 4, 5, 6 };
        var chunks = ClientUtils.ChunkList(list, 2).ToList();

        Assert.Equal(3, chunks.Count);
        Assert.All(chunks, chunk => Assert.Equal(2, chunk.Count));
    }

    [Fact]
    public void ClientUtils_ChunkList_ThrowsOnNullList() {
        Assert.Throws<ArgumentNullException>(() =>
            ClientUtils.ChunkList<int>(null!, 5).ToList()
        );
    }

    [Fact]
    public void ClientUtils_ChunkList_ThrowsOnInvalidChunkSize() {
        var list = new List<int> { 1, 2, 3 };
        Assert.Throws<ArgumentException>(() =>
            ClientUtils.ChunkList(list, 0).ToList()
        );
    }

    [Fact]
    public void ClientUtils_TransformToBatchCheckItem_CreatesCorrectModel() {
        var clientItem = new ClientBatchCheckItem(
            user: "user:anne",
            relation: "viewer",
            @object: "document:roadmap",
            correlationId: "test-id",
            contextualTuples: new ContextualTupleKeys { TupleKeys = new List<TupleKey>() },
            context: new { ViewCount = 100 }
        );

        var batchItem = ClientUtils.TransformToBatchCheckItem(clientItem, "test-id");

        Assert.NotNull(batchItem);
        Assert.Equal("test-id", batchItem.CorrelationId);
        Assert.NotNull(batchItem.TupleKey);
        Assert.Equal("user:anne", batchItem.TupleKey.User);
        Assert.Equal("viewer", batchItem.TupleKey.Relation);
        Assert.Equal("document:roadmap", batchItem.TupleKey.Object);
    }

    [Fact]
    public void ClientUtils_TransformToBatchCheckItem_ThrowsOnNullItem() {
        Assert.Throws<ArgumentNullException>(() =>
            ClientUtils.TransformToBatchCheckItem(null!, "test-id")
        );
    }

    [Fact]
    public void ClientUtils_TransformToBatchCheckItem_ThrowsOnNullCorrelationId() {
        var clientItem = new ClientBatchCheckItem("user:anne", "viewer", "document:roadmap");
        Assert.Throws<ArgumentException>(() =>
            ClientUtils.TransformToBatchCheckItem(clientItem, null!)
        );
    }

    [Fact]
    public void ClientBatchCheckOptions_MaxBatchSizeProperty_CanBeSet() {
        var options = new ClientBatchCheckOptions {
            MaxBatchSize = 100
        };

        Assert.Equal(100, options.MaxBatchSize);
    }

    [Fact]
    public void ClientBatchCheckOptions_MaxParallelRequestsProperty_CanBeSet() {
        var options = new ClientBatchCheckOptions {
            MaxParallelRequests = 5
        };

        Assert.Equal(5, options.MaxParallelRequests);
    }

    [Fact]
    public void ClientBatchCheckClientOptions_MaxParallelRequestsProperty_CanBeSet() {
        var options = new ClientBatchCheckClientOptions {
            MaxParallelRequests = 5
        };

        Assert.Equal(5, options.MaxParallelRequests);
    }
}