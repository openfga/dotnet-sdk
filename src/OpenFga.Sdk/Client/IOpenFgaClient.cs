using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Client;

/// <summary>
/// Interface for the OpenFGA client
/// </summary>
/// <remarks>
/// This interface is provisioned to support unit testing scenarios by making mocking feasible.
/// </remarks>
public interface IOpenFgaClient
{
    /// <summary>
    /// StoreId - The ID of the FGA store.
    /// </summary>
    string? StoreId { get; set; }

    /// <summary>
    /// AuthorizationModelId - The ID of the authorization model in the FGA store.
    /// </summary>
    string? AuthorizationModelId { get; set; }

    /// <summary>
    /// ListStores - Lists all stores.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of stores.</returns>
    Task<ListStoresResponse> ListStores(
        IClientListStoresRequest? body,
        IClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// CreateStore - Initialize a store.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response which contains the results of the create operation.</returns>
    Task<CreateStoreResponse> CreateStore(
        ClientCreateStoreRequest body,
        IClientRequestOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// GetStore - Get information about the current store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the current store information.</returns>
    Task<GetStoreResponse> GetStore(
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// DeleteStore - Delete a store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the delete operation.</returns>
    Task DeleteStore(
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ReadAuthorizationModels - Read all authorization models.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of authorization models.</returns>
    Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(
        IClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// WriteAuthorizationModel - Create a new version of the authorization model.
    /// </summary>
    /// <param name="body">The request body containing the authorization model details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the create operation.</returns>
    Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(
        ClientWriteAuthorizationModelRequest body,
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ReadAuthorizationModel - Read the current authorization model.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the current authorization model.</returns>
    Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(
        IClientReadAuthorizationModelOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ReadLatestAuthorizationModel - Read the latest authorization model for the current store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the latest authorization model.</returns>
    Task<ReadAuthorizationModelResponse?> ReadLatestAuthorizationModel(
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ReadChanges - Read the list of historical relationship tuple writes and deletes
    /// </summary>
    /// <param name="body">The request body containing the change details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of changes.</returns>
    Task<ReadChangesResponse> ReadChanges(
        ClientReadChangesRequest? body = default,
        ClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Read - Read tuples previously written to the store (does not evaluate).
    /// </summary>
    /// <param name="body">The request body containing the read details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of tuples.</returns>
    Task<ReadResponse> Read(
        ClientReadRequest? body = default,
        IClientReadOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Write - Create or delete relationship tuples.
    /// </summary>
    /// <param name="body">The request body containing the write details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the write operation.</returns>
    Task<ClientWriteResponse> Write(
        ClientWriteRequest body,
        IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// WriteTuples - Utility method to write tuples, wraps Write.
    /// </summary>
    /// <param name="body">The request body containing the write details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the write operation.</returns>
    Task<ClientWriteResponse> WriteTuples(
        List<ClientTupleKey> body,
        IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// DeleteTuples - Utility method to delete tuples, wraps Write.
    /// </summary>
    /// <param name="body">The request body containing the delete details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the delete operation.</returns>
    Task<ClientWriteResponse> DeleteTuples(
        List<ClientTupleKeyWithoutCondition> body,
        IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Check - Check if a user has a particular relation with an object (evaluates).
    /// </summary>
    /// <param name="body">The request body containing the check details.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the check operation.</returns>
    Task<CheckResponse> Check(
        IClientCheckRequest body,
        IClientCheckOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// BatchCheck - Run a set of checks using the server-side /batch-check endpoint (evaluates)
    /// </summary>
    /// <remarks>
    /// This method uses the server-side batch check API endpoint. It automatically:
    /// - Generates correlation IDs for checks that don't have one
    /// - Validates that correlation IDs are unique
    /// - Chunks requests based on maxBatchSize (default: 50)
    /// - Executes batches in parallel based on maxParallelRequests (default: 10)
    ///
    /// Best for large batches (≥ 10 checks) as it leverages server-side optimizations.
    /// For small batches with specific control needs, use ClientBatchCheck instead.
    /// </remarks>
    /// <param name="body">The batch check request with a list of checks</param>
    /// <param name="options">Optional configuration including MaxBatchSize and MaxParallelRequests</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ClientBatchCheckResponse with correlation ID mapping to results</returns>
    Task<ClientBatchCheckResponse> BatchCheck(
        ClientBatchCheckRequest body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ClientBatchCheck - Run a set of checks by executing individual /check calls in parallel (evaluates)
    /// </summary>
    /// <remarks>
    /// This method makes individual check API calls in parallel on the client side.
    /// Best for small batches (&lt; 10 checks) when you need specific control over individual requests.
    ///
    /// For larger batches or to use the server-side /batch-check endpoint, use the BatchCheck method instead.
    /// </remarks>
    /// <param name="body">List of check requests to execute</param>
    /// <param name="options">Optional configuration including MaxParallelRequests</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ClientBatchCheckClientResponse with list of individual check responses</returns>
    Task<ClientBatchCheckClientResponse> ClientBatchCheck(
        List<ClientCheckRequest> body,
        IClientBatchCheckClientOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Expand - Expands the relationships in userset tree format (evaluates).
    /// </summary>
    /// <param name="body">The request body containing the expand details.</param>
    /// <param name="options">Optional configuration for the expand request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the expand operation.</returns>
    Task<ExpandResponse> Expand(
        IClientExpandRequest body,
        IClientExpandOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ListObjects - List the objects of a particular type that the user has a certain relation to (evaluates)
    /// </summary>
    /// <param name="body">The request body containing the list objects details.</param>
    /// <param name="options">Optional configuration for the list objects request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the list objects operation.</returns>
    Task<ListObjectsResponse> ListObjects(
        IClientListObjectsRequest body,
        IClientListObjectsOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ListRelations - List all the relations a user has with an object (evaluates)
    /// </summary>
    /// <param name="body">The request body containing the list relations details.</param>
    /// <param name="options">Optional configuration for the list relations request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the list relations operation.</returns>
    Task<ListRelationsResponse> ListRelations(
        IClientListRelationsRequest body,
        IClientListRelationsOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ListUsers - List all users of the given type that the object has a relation with (evaluates)
    /// </summary>
    /// <param name="body">The request body containing the list users details.</param>
    /// <param name="options">Optional configuration for the list users request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the list users operation.</returns>
    Task<ListUsersResponse> ListUsers(
        IClientListUsersRequest body,
        IClientListUsersOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// ReadAssertions - Read assertions for a particular authorization model
    /// </summary>
    /// <param name="options">Optional configuration for the read assertions request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the read assertions operation.</returns>
    Task<ReadAssertionsResponse> ReadAssertions(
        IClientReadAssertionsOptions? options = default,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// WriteAssertions - Updates assertions for a particular authorization model
    /// </summary>
    /// <param name="body">The request body containing the write assertions details.</param>
    /// <param name="options">Optional configuration for the write assertions request.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating the result of the write assertions operation.</returns>
    Task WriteAssertions(
        List<ClientAssertion> body,
        IClientWriteAssertionsOptions? options = default,
        CancellationToken cancellationToken = default
    );
}