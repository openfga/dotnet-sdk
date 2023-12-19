//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System.Collections.Concurrent;

namespace OpenFga.Sdk.Client;

public class OpenFgaClient : IDisposable {
    private readonly ClientConfiguration _configuration;
    protected OpenFgaApi api;
    private string CLIENT_BULK_REQUEST_ID_HEADER = "X-OpenFGA-Client-Bulk-Request-Id";
    private string CLIENT_METHOD_HEADER = "X-OpenFGA-Client-Method";

    private readonly int DEFAULT_MAX_METHOD_PARALLEL_REQS = 10;

    public OpenFgaClient(
        ClientConfiguration configuration,
        HttpClient? httpClient = null
    ) {
        _configuration = configuration;
        api = new OpenFgaApi(_configuration, httpClient);
    }

    /// <summary>
    ///     Store ID
    /// </summary>
    public string? StoreId {
        get => _configuration.StoreId;
        set => _configuration.StoreId = value;
    }

    /// <summary>
    ///     Authorization Model ID
    /// </summary>
    public string? AuthorizationModelId {
        get => _configuration.AuthorizationModelId;
        set => _configuration.AuthorizationModelId = value;
    }

    public void Dispose() => api.Dispose();

    private string? GetAuthorizationModelId(AuthorizationModelIdOptions? options) =>
        options?.AuthorizationModelId ?? AuthorizationModelId;

    /**********
     * Stores *
     **********/

    /**
   * ListStores - Get a paginated list of stores.
   */
    public async Task<ListStoresResponse> ListStores(IClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListStores(options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
   * CreateStore - Initialize a store
   */
    public async Task<CreateStoreResponse> CreateStore(ClientCreateStoreRequest body,
        ClientRequestOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.CreateStore(body, cancellationToken);

    /**
   * GetStore - Get information about the current store
   */
    public async Task<GetStoreResponse> GetStore(ClientRequestOptions? options = default, CancellationToken cancellationToken = default) =>
        await api.GetStore(cancellationToken);

    /**
   * DeleteStore - Delete a store
   */
    public async Task DeleteStore(ClientRequestOptions? options = default, CancellationToken cancellationToken = default) =>
        await api.DeleteStore(cancellationToken);

    /************************
     * Authorization Models *
     ************************/

    /**
     * ReadAuthorizationModels - Read all authorization models
     */
    public async Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(
        IClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadAuthorizationModels(options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
     * WriteAuthorizationModel - Create a new version of the authorization model
     */
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(ClientWriteAuthorizationModelRequest body,
        ClientRequestOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.WriteAuthorizationModel(body, cancellationToken);

    /**
     * ReadAuthorizationModel - Read the current authorization model
     */
    public async Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(
        IClientReadAuthorizationModelOptions? options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        return await api.ReadAuthorizationModel(authorizationModelId, cancellationToken);
    }

    /**
     * ReadLatestAuthorizationModel - Read the latest authorization model for the current store
     */
    public async Task<ReadAuthorizationModelResponse?> ReadLatestAuthorizationModel(
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default) {
        var response =
            await ReadAuthorizationModels(new ClientReadAuthorizationModelsOptions { PageSize = 1 }, cancellationToken);

        if (response.AuthorizationModels.Count > 0) {
            return new ReadAuthorizationModelResponse { AuthorizationModel = response.AuthorizationModels?[0] };
        }

        return null;
    }

    /***********************
     * Relationship Tuples *
     ***********************/

    /**
     * Read Changes - Read the list of historical relationship tuple writes and deletes
     */
    public async Task<ReadChangesResponse> ReadChanges(ClientReadChangesRequest? body = default,
        IClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadChanges(body?.Type, options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
     * Read - Read tuples previously written to the store (does not evaluate)
     */
    public async Task<ReadResponse> Read(ClientReadRequest? body = default, IClientReadOptions? options = default,
        CancellationToken cancellationToken = default) {
        ReadRequestTupleKey tupleKey = null;
        if (body != null && (body.User != null || body.Relation != null || body.Object != null)) {
            tupleKey = body;
        }
        return await api.Read(
            new ReadRequest {
                TupleKey = tupleKey,
                PageSize = options?.PageSize,
                ContinuationToken = options?.ContinuationToken
            }, cancellationToken);
    }

    /**
     * Write - Create or delete relationship tuples
     */
    public async Task<ClientWriteResponse> Write(ClientWriteRequest body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default) {
        var maxPerChunk =
            options?.Transaction?.MaxPerChunk ??
            1; // 1 has to be the default otherwise the chunks will be sent in transactions
        var maxParallelReqs =
            options?.Transaction?.MaxParallelRequests ?? DEFAULT_MAX_METHOD_PARALLEL_REQS;
        var authorizationModelId = GetAuthorizationModelId(options);

        if (options?.Transaction?.Disable != true) {
            var requestBody = new WriteRequest() {
                AuthorizationModelId = authorizationModelId
            };
            if (body.Writes?.Count > 0) {
                requestBody.Writes = new WriteRequestWrites(body.Writes.ConvertAll(key => key.ToTupleKey()));
            }
            if (body.Deletes?.Count > 0) {
                requestBody.Deletes = new WriteRequestDeletes(body.Deletes.ConvertAll(key => key.ToTupleKeyWithoutCondition()));
            }

            await api.Write(requestBody, cancellationToken);
            return new ClientWriteResponse {
                Writes =
                    body.Writes?.ConvertAll(tupleKey =>
                        new ClientWriteSingleResponse { TupleKey = tupleKey.ToTupleKey(), Status = ClientWriteStatus.SUCCESS }) ??
                    new List<ClientWriteSingleResponse>(),
                Deletes = body.Deletes?.ConvertAll(tupleKey => new ClientWriteSingleResponse {
                    TupleKey = tupleKey.ToTupleKey(),
                    Status = ClientWriteStatus.SUCCESS
                }) ?? new List<ClientWriteSingleResponse>()
            };
        }

        var writeChunks = body.Writes?.Chunk(maxPerChunk).ToList() ?? new List<ClientTupleKey[]>();
        var deleteChunks = body.Deletes?.Chunk(maxPerChunk).ToList() ?? new List<ClientTupleKeyWithoutCondition[]>();

        var writeResponses = new ConcurrentBag<ClientWriteSingleResponse>();
        var deleteResponses = new ConcurrentBag<ClientWriteSingleResponse>();
        await Parallel.ForEachAsync(writeChunks,
            new ParallelOptions { MaxDegreeOfParallelism = maxParallelReqs }, async (request, token) => {
                var writes = request.ToList();
                try {
                    await this.Write(new ClientWriteRequest() { Writes = writes }, new ClientWriteOptions() { AuthorizationModelId = authorizationModelId }, cancellationToken);

                    foreach (var tupleKey in writes) {
                        writeResponses.Add(new ClientWriteSingleResponse {
                            TupleKey = tupleKey.ToTupleKey(),
                            Status = ClientWriteStatus.SUCCESS,
                        });
                    }
                }
                catch (Exception e) {
                    foreach (var tupleKey in writes) {
                        writeResponses.Add(new ClientWriteSingleResponse {
                            TupleKey = tupleKey.ToTupleKey(),
                            Status = ClientWriteStatus.FAILURE,
                            Error = e,
                        });
                    }
                }
            });

        await Parallel.ForEachAsync(deleteChunks,
            new ParallelOptions { MaxDegreeOfParallelism = maxParallelReqs }, async (request, token) => {
                var deletes = request.ToList();
                try {
                    await this.Write(new ClientWriteRequest() { Deletes = deletes }, new ClientWriteOptions() { AuthorizationModelId = authorizationModelId }, cancellationToken);

                    foreach (var tupleKey in deletes) {
                        deleteResponses.Add(new ClientWriteSingleResponse {
                            TupleKey = tupleKey.ToTupleKey(),
                            Status = ClientWriteStatus.SUCCESS,
                        });
                    }
                }
                catch (Exception e) {
                    foreach (var tupleKey in deletes) {
                        deleteResponses.Add(new ClientWriteSingleResponse {
                            TupleKey = tupleKey.ToTupleKey(),
                            Status = ClientWriteStatus.FAILURE,
                            Error = e,
                        });
                    }
                }
            });

        return new ClientWriteResponse { Writes = writeResponses.ToList(), Deletes = deleteResponses.ToList() };
    }

    /**
     * WriteTuples - Utility method to write tuples, wraps Write
     */
    public async Task<ClientWriteResponse> WriteTuples(List<ClientTupleKey> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest { Writes = body }, options, cancellationToken);

    /**
     * DeleteTuples - Utility method to delete tuples, wraps Write
     */
    public async Task<ClientWriteResponse> DeleteTuples(List<ClientTupleKeyWithoutCondition> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest { Deletes = body }, options, cancellationToken);

    /************************
     * Relationship Queries *
     ************************/

    /**
   * Check - Check if a user has a particular relation with an object (evaluates)
   */
    public async Task<CheckResponse> Check(IClientCheckRequest body,
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default) =>
        await api.Check(
            new CheckRequest {
                TupleKey = new CheckRequestTupleKey { User = body.User, Relation = body.Relation, Object = body.Object },
                ContextualTuples =
                    new ContextualTupleKeys {
                        TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                    new List<TupleKey>()
                    },
                Context = body.Context,
                AuthorizationModelId = GetAuthorizationModelId(options)
            }, cancellationToken);

    /**
   * BatchCheck - Run a set of checks (evaluates)
   */
    public async Task<BatchCheckResponse> BatchCheck(List<ClientCheckRequest> body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default) {
        var responses = new ConcurrentBag<BatchCheckSingleResponse>();
        await Parallel.ForEachAsync(body,
            new ParallelOptions { MaxDegreeOfParallelism = options?.MaxParallelRequests ?? DEFAULT_MAX_METHOD_PARALLEL_REQS }, async (request, token) => {
                try {
                    var response = await Check(request, options, cancellationToken);

                    responses.Add(new BatchCheckSingleResponse {
                        Allowed = response.Allowed ?? false,
                        Request = request,
                        Error = null
                    });
                }
                catch (Exception e) {
                    responses.Add(new BatchCheckSingleResponse { Allowed = false, Request = request, Error = e });
                }
            });

        return new BatchCheckResponse { Responses = responses.ToList() };
    }

    /**
   * Expand - Expands the relationships in userset tree format (evaluates)
   */
    public async Task<ExpandResponse> Expand(IClientExpandRequest body,
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default) =>
        await api.Expand(
            new ExpandRequest {
                TupleKey = new ExpandRequestTupleKey { Relation = body.Relation, Object = body.Object },
                AuthorizationModelId = GetAuthorizationModelId(options)
            }, cancellationToken);

    /**
     * ListObjects - List the objects of a particular type that the user has a certain relation to (evaluates)
     */
    public async Task<ListObjectsResponse> ListObjects(IClientListObjectsRequest body,
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListObjects(new ListObjectsRequest {
            User = body.User,
            Relation = body.Relation,
            Type = body.Type,
            ContextualTuples =
                new ContextualTupleKeys {
                    TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                new List<TupleKey>()
                },
            Context = body.Context,
            AuthorizationModelId = GetAuthorizationModelId(options)
        });

    /**
     * ListRelations - List all the relations a user has with an object (evaluates)
     */
    public async Task<ListRelationsResponse> ListRelations(IClientListRelationsRequest body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default) {
        if (body.Relations.Count == 0) {
            throw new FgaValidationError("At least 1 relation to check has to be provided when calling ListRelations");
        }

        var responses = new ListRelationsResponse();
        var batchCheckRequests = new List<ClientCheckRequest>();
        for (var index = 0; index < body.Relations.Count; index++) {
            var relation = body.Relations[index];
            batchCheckRequests.Add(new ClientCheckRequest {
                User = body.User,
                Relation = relation,
                Object = body.Object,
                ContextualTuples = body.ContextualTuples,
                Context = body.Context
            });
        }

        var batchCheckResponse = await BatchCheck(batchCheckRequests, options, cancellationToken);

        for (var index = 0; index < batchCheckResponse.Responses.Count; index++) {
            var batchCheckSingleResponse = batchCheckResponse.Responses[index];
            if (batchCheckSingleResponse.Allowed && batchCheckSingleResponse.Request?.Relation != null) {
                responses.AddRelation(batchCheckSingleResponse.Request.Relation);
            }
        }

        return responses;
    }

    /**************
     * Assertions *
     **************/

    /**
   * ReadAssertions - Read assertions for a particular authorization model
   */
    public async Task<ReadAssertionsResponse> ReadAssertions(IClientReadAssertionsOptions? options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        return await api.ReadAssertions(authorizationModelId, cancellationToken);
    }

    /**
     * WriteAssertions - Updates assertions for a particular authorization model
     */
    public async Task WriteAssertions(List<ClientAssertion> body,
        IClientWriteAssertionsOptions? options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        var assertions = new List<Assertion>();

        for (var index = 0; index < body.Count; index++) {
            var assertion = body[index];
            assertions.Add(new Assertion {
                TupleKey = new AssertionTupleKey {
                    User = assertion.User,
                    Relation = assertion.Relation,
                    Object = assertion.Object
                },
                Expectation = assertion.Expectation
            });
        }

        await api.WriteAssertions(authorizationModelId,
            new WriteAssertionsRequest { Assertions = assertions }, cancellationToken);
    }
}