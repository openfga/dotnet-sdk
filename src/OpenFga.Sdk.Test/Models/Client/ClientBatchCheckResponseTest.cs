using OpenFga.Sdk.Client.Model;
using System.Collections.Generic;
using Xunit;

public class ClientBatchCheckClientResponseTests {
    [Fact]
    public void Equals_ReturnsTrue_WhenResponsesAreEqual() {
        var response1 = new BatchCheckSingleResponse(true, new ClientCheckRequest(), null);
        var response2 = new BatchCheckSingleResponse(true, new ClientCheckRequest(), null);
        var clientResponse1 = new ClientBatchCheckClientResponse(new List<BatchCheckSingleResponse> { response1 });
        var clientResponse2 = new ClientBatchCheckClientResponse(new List<BatchCheckSingleResponse> { response2 });

        Assert.True(clientResponse1.Equals(clientResponse2));
    }

    [Fact]
    public void Equals_ReturnsFalse_WhenResponsesAreNotEqual() {
        var response1 = new BatchCheckSingleResponse(true, new ClientCheckRequest(), null);
        var response2 = new BatchCheckSingleResponse(false, new ClientCheckRequest(), null);
        var clientResponse1 = new ClientBatchCheckClientResponse(new List<BatchCheckSingleResponse> { response1 });
        var clientResponse2 = new ClientBatchCheckClientResponse(new List<BatchCheckSingleResponse> { response2 });

        Assert.False(clientResponse1.Equals(clientResponse2));
    }

    [Fact]
    public void AppendResponse_AddsResponseToList() {
        var clientResponse = new ClientBatchCheckClientResponse();
        var response = new BatchCheckSingleResponse(true, new ClientCheckRequest(), null);

        clientResponse.AppendResponse(response);

        Assert.Contains(response, clientResponse.Responses);
    }
}