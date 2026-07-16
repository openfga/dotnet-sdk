using OpenFga.Sdk.Client.Model;
using System.Collections.Generic;
using Xunit;

namespace OpenFga.Sdk.Test.Models.Client;

public class ClientCheckRequestTests {
    [Fact]
    public void Equals_ReturnsTrue_WhenContextualTuplesHaveEqualValues() {
        var request1 = CreateRequest("document:budget");
        var request2 = CreateRequest("document:budget");

        Assert.True(request1.Equals(request2));
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenContextualTuplesHaveDifferentValues() {
        var request1 = CreateRequest("document:budget");
        var request2 = CreateRequest("document:forecast");

        Assert.False(request1.Equals(request2));
    }

    [Fact]
    public void GetHashCode_ReturnsSameValue_WhenContextualTuplesHaveEqualValues() {
        var request1 = CreateRequest("document:budget");
        var request2 = CreateRequest("document:budget");

        Assert.Equal(request1.GetHashCode(), request2.GetHashCode());
    }

    private static ClientCheckRequest CreateRequest(string contextualObject) => new() {
        User = "user:anne",
        Relation = "viewer",
        Object = "document:roadmap",
        ContextualTuples = new List<ClientTupleKey> {
            new() {
                User = "user:anne",
                Relation = "editor",
                Object = contextualObject,
            },
        },
    };
}