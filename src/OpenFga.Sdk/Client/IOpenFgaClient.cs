using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenFga.Sdk.Client.Model;
#if NETSTANDARD2_0 || NET48
using OpenFga.Sdk.Client.Extensions;
#endif
using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client;

/// <summary>
/// Interface contract for the OpenFGA client.
/// </summary>
public interface IOpenFgaClient
{
    /// <summary>
    /// Gets a paginated list of stores.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing a paginated list of stores.</returns>
    Task<ListStoresResponse> ListStores(IClientListStoresRequest? body, IClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Initialize a store.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the metadata about the store.</returns>
    Task<CreateStoreResponse> CreateStore(ClientCreateStoreRequest body,
        IClientRequestOptions? options = default,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get information about a specific store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the metadata about the specified store.</returns>
    Task<GetStoreResponse> GetStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a specific store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable `Task`.</returns>
    Task DeleteStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a list of all authorization models.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of all authorization models.</returns>
    Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(
        IClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates a new version of the authorization model.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the newly created authorization model.</returns>
    Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(ClientWriteAuthorizationModelRequest body,
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reads the current authorization model.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The current authorization model.</returns>
    Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(
        IClientReadAuthorizationModelOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reads the latest authorization model for the current store.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The latest authorization model, or null if none exists.</returns>
    Task<ReadAuthorizationModelResponse?> ReadLatestAuthorizationModel(
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reads the list of historical relationship tuple writes and deletes.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the historical relationship tuple writes and deletes.</returns>
    Task<ReadChangesResponse> ReadChanges(ClientReadChangesRequest? body = default,
        ClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reads relationship tuples previously written to the store (does not evaluate).
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the relationship tuples.</returns>
    Task<ReadResponse> Read(ClientReadRequest? body = default, IClientReadOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates or deletes relationship tuples.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the result of the write operation.</returns>
    Task<ClientWriteResponse> Write(ClientWriteRequest body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Writes relationship tuples.
    /// </summary>
    /// <param name="body">The list of relationship tuples to write.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the result of the write operation.</returns>
    Task<ClientWriteResponse> WriteTuples(List<ClientTupleKey> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes relationship tuples.
    /// </summary>
    /// <param name="body">The list of relationship tuples to delete.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the result of the delete operation.</returns>
    Task<ClientWriteResponse> DeleteTuples(List<ClientTupleKeyWithoutCondition> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Checks if a user has a particular relation with an object.
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response indicating whether the user has the specified relation with the object.</returns
    Task<CheckResponse> Check(IClientCheckRequest body,
        IClientCheckOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Runs a batch of checks.
    /// </summary>
    /// <param name="body">The list of check requests.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the results of the batch checks.</returns>
    Task<ClientBatchCheckClientResponse> BatchCheck(List<ClientCheckRequest> body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Expands the relationships in userset tree format (evaluates)
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the expanded relationships in userset tree format.</returns>
    Task<ExpandResponse> Expand(IClientExpandRequest body,
        IClientExpandOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lists the objects of a particular type that the user has a certain relation to (evaluates).
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of objects.</returns>
    Task<ListObjectsResponse> ListObjects(IClientListObjectsRequest body,
        IClientListObjectsOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lists all the relations a user has with an object (evaluates).
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>>A response containing the list of relations.</returns>
    Task<ListRelationsResponse> ListRelations(IClientListRelationsRequest body,
        IClientListRelationsOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Lists all users of a given type that have a certain relation with an object (evaluates).
    /// </summary>
    /// <param name="body">The body of the request.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the list of users.</returns> 
    Task<ListUsersResponse> ListUsers(IClientListUsersRequest body,
        IClientListUsersOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Reads assertions for a particular authorization model.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A response containing the assertions for the specified authorization model.</returns>  
    Task<ReadAssertionsResponse> ReadAssertions(IClientReadAssertionsOptions? options = default,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Writes assertions for a particular authorization model.
    /// </summary>
    /// <param name="body">The list of assertions to write.</param>
    /// <param name="options">The request options.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An awaitable `Task`.</returns>
    Task WriteAssertions(List<ClientAssertion> body,
        IClientWriteAssertionsOptions? options = default,
        CancellationToken cancellationToken = default);
}