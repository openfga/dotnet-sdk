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