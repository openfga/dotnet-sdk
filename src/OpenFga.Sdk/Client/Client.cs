using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using OpenFga.Sdk.Api;
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Constants;
#if NETSTANDARD2_0 || NET48
using OpenFga.Sdk.Client.Extensions;
#endif
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client;

public class OpenFgaClient : IDisposable {
    private readonly ClientConfiguration _configuration;
    protected OpenFgaApi api;

    public OpenFgaClient(
        ClientConfiguration configuration,
        HttpClient? httpClient = null
    ) {
        configuration.EnsureValid();
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

#if NET6_0_OR_GREATER
    private async Task ProcessWriteChunksAsync<T>(
        IEnumerable<T[]> chunks,
        Func<T[], ClientWriteRequest> createRequest,
        Func<T, TupleKey> convertToTupleKey,
        ConcurrentBag<ClientWriteSingleResponse> responses,
        ClientWriteOptions clientWriteOpts,
        int maxParallelReqs,
        CancellationToken cancellationToken) {

        await Parallel.ForEachAsync(chunks,
            new ParallelOptions { MaxDegreeOfParallelism = maxParallelReqs, CancellationToken = cancellationToken },
            async (chunk, token) => {
                var items = chunk.ToList();
                try {
                    await this.Write(createRequest(chunk), clientWriteOpts, token);

                    foreach (var item in items) {
                        responses.Add(new ClientWriteSingleResponse {
                            TupleKey = convertToTupleKey(item),
                            Status = ClientWriteStatus.SUCCESS,
                        });
                    }
                }
                catch (Exception e) {
                    foreach (var item in items) {
                        responses.Add(new ClientWriteSingleResponse {
                            TupleKey = convertToTupleKey(item),
                            Status = ClientWriteStatus.FAILURE,
                            Error = e,
                        });
                    }
                }
            });
    }

    private async Task ProcessCheckRequestsAsync(
        IEnumerable<ClientCheckRequest> requests,
        ConcurrentBag<BatchCheckSingleResponse> responses,
        IClientBatchCheckOptions? options,
        int maxParallelReqs,
        CancellationToken cancellationToken) {

        await Parallel.ForEachAsync(requests,
            new ParallelOptions { MaxDegreeOfParallelism = maxParallelReqs, CancellationToken = cancellationToken },
            async (request, token) => {
                try {
                    var response = await Check(request, options, token);

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
    }
#else
    private async Task ProcessWriteChunksAsync<T>(
        IEnumerable<T[]> chunks,
        Func<T[], ClientWriteRequest> createRequest,
        Func<T, TupleKey> convertToTupleKey,
        ConcurrentBag<ClientWriteSingleResponse> responses,
        ClientWriteOptions clientWriteOpts,
        int maxParallelReqs,
        CancellationToken cancellationToken) {

        using (var throttler = new SemaphoreSlim(maxParallelReqs)) {
            var tasks = chunks.Select(async chunk => {
                await throttler.WaitAsync(cancellationToken);
                try {
                    var items = chunk.ToList();
                    try {
                        await this.Write(createRequest(chunk), clientWriteOpts, cancellationToken);

                        foreach (var item in items) {
                            responses.Add(new ClientWriteSingleResponse {
                                TupleKey = convertToTupleKey(item),
                                Status = ClientWriteStatus.SUCCESS,
                            });
                        }
                    }
                    catch (Exception e) {
                        foreach (var item in items) {
                            responses.Add(new ClientWriteSingleResponse {
                                TupleKey = convertToTupleKey(item),
                                Status = ClientWriteStatus.FAILURE,
                                Error = e,
                            });
                        }
                    }
                }
                finally {
                    throttler.Release();
                }
            }).ToList();

            await Task.WhenAll(tasks);
        }
    }

    private async Task ProcessCheckRequestsAsync(
        IEnumerable<ClientCheckRequest> requests,
        ConcurrentBag<BatchCheckSingleResponse> responses,
        IClientBatchCheckOptions? options,
        int maxParallelReqs,
        CancellationToken cancellationToken) {

        using (var throttler = new SemaphoreSlim(maxParallelReqs)) {
            var tasks = requests.Select(async request => {
                await throttler.WaitAsync(cancellationToken);
                try {
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
                }
                finally {
                    throttler.Release();
                }
            }).ToList();

            await Task.WhenAll(tasks);
        }
    }
#endif

    private string GetStoreId(StoreIdOptions? options) {
        var storeId = options?.StoreId ?? StoreId;
        if (storeId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "StoreId");
        }
        else if (!ClientConfiguration.IsWellFormedUlidString(storeId)) {
            throw new FgaValidationError("StoreId is not in a valid ulid format");
        }

        return storeId;
    }

    private string? GetAuthorizationModelId(AuthorizationModelIdOptions? options) {
        var authorizationModelId = options?.AuthorizationModelId ?? AuthorizationModelId;
        if (authorizationModelId != null && authorizationModelId != "" && !ClientConfiguration.IsWellFormedUlidString(authorizationModelId)) {
            throw new FgaValidationError("AuthorizationModelId is not in a valid ulid format");
        }

        return authorizationModelId;
    }

    private static WriteRequestWrites.OnDuplicateEnum MapOnDuplicateWrites(OnDuplicateWrites? behavior) {
        if (behavior == null) return WriteRequestWrites.OnDuplicateEnum.Error;
        return behavior.Value == OnDuplicateWrites.Error
            ? WriteRequestWrites.OnDuplicateEnum.Error
            : WriteRequestWrites.OnDuplicateEnum.Ignore;
    }

    private static WriteRequestDeletes.OnMissingEnum MapOnMissingDeletes(OnMissingDeletes? behavior) {
        if (behavior == null) return WriteRequestDeletes.OnMissingEnum.Error;
        return behavior.Value == OnMissingDeletes.Error
            ? WriteRequestDeletes.OnMissingEnum.Error
            : WriteRequestDeletes.OnMissingEnum.Ignore;
    }

    /**********
     * Stores *
     **********/

    /**
   * ListStores - Get a paginated list of stores.
   */
    public async Task<ListStoresResponse> ListStores(IClientListStoresRequest? body, IClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListStores(options?.PageSize, options?.ContinuationToken, body?.Name, options, cancellationToken);

    /**
   * CreateStore - Initialize a store
   */
    public async Task<CreateStoreResponse> CreateStore(ClientCreateStoreRequest body,
        IClientRequestOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.CreateStore(body, options, cancellationToken);

    /**
   * GetStore - Get information about the current store
   */
    public async Task<GetStoreResponse> GetStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default) =>
        await api.GetStore(GetStoreId(options), options, cancellationToken);

    /**
   * DeleteStore - Delete a store
   */
    public async Task DeleteStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default) =>
        await api.DeleteStore(GetStoreId(options), options, cancellationToken);

    /************************
     * Authorization Models *
     ************************/

    /**
     * ReadAuthorizationModels - Read all authorization models
     */
    public async Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(
        IClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadAuthorizationModels(GetStoreId(options), options?.PageSize, options?.ContinuationToken, options, cancellationToken);

    /**
     * WriteAuthorizationModel - Create a new version of the authorization model
     */
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(ClientWriteAuthorizationModelRequest body,
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default) =>
        await api.WriteAuthorizationModel(GetStoreId(options), body, options, cancellationToken);

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

        return await api.ReadAuthorizationModel(GetStoreId(options), authorizationModelId, options, cancellationToken);
    }

    /**
     * ReadLatestAuthorizationModel - Read the latest authorization model for the current store
     */
    public async Task<ReadAuthorizationModelResponse?> ReadLatestAuthorizationModel(
        IClientRequestOptionsWithAuthZModelId? options = default,
        CancellationToken cancellationToken = default) {
        var response =
            await ReadAuthorizationModels(new ClientReadAuthorizationModelsOptions { StoreId = options?.StoreId, PageSize = 1, Headers = options?.Headers }, cancellationToken);

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
        ClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadChanges(GetStoreId(options), body?.Type, options?.PageSize, options?.ContinuationToken, body?.StartTime, options, cancellationToken);

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
            GetStoreId(options),
            new ReadRequest {
                TupleKey = tupleKey,
                PageSize = options?.PageSize,
                ContinuationToken = options?.ContinuationToken,
                Consistency = options?.Consistency,
            }, options, cancellationToken);
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
            options?.Transaction?.MaxParallelRequests ?? FgaConstants.ClientMaxMethodParallelRequests;
        var authorizationModelId = GetAuthorizationModelId(options);

        if (options?.Transaction?.Disable != true) {
            var requestBody = new WriteRequest() {
                AuthorizationModelId = authorizationModelId
            };
            if (body.Writes?.Count > 0) {
                requestBody.Writes = new WriteRequestWrites(
                    body.Writes.ConvertAll(key => key.ToTupleKey()),
                    MapOnDuplicateWrites(options?.Conflict?.OnDuplicateWrites)
                );
            }
            if (body.Deletes?.Count > 0) {
                requestBody.Deletes = new WriteRequestDeletes(
                    body.Deletes.ConvertAll(key => key.ToTupleKeyWithoutCondition()),
                    MapOnMissingDeletes(options?.Conflict?.OnMissingDeletes)
                );
            }

            await api.Write(GetStoreId(options), requestBody, options, cancellationToken);
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

        var clientWriteOpts = new ClientWriteOptions() {
            StoreId = GetStoreId(options),
            AuthorizationModelId = authorizationModelId,
            Headers = options?.Headers,
            Conflict = options?.Conflict
        };

        var writeChunks = body.Writes?.Chunk(maxPerChunk).ToList() ?? new List<ClientTupleKey[]>();
        var writeResponses = new ConcurrentBag<ClientWriteSingleResponse>();
        await ProcessWriteChunksAsync(
            writeChunks,
            writes => new ClientWriteRequest() { Writes = writes.ToList() },
            tupleKey => tupleKey.ToTupleKey(),
            writeResponses,
            clientWriteOpts,
            maxParallelReqs,
            cancellationToken);

        var deleteChunks = body.Deletes?.Chunk(maxPerChunk).ToList() ?? new List<ClientTupleKeyWithoutCondition[]>();
        var deleteResponses = new ConcurrentBag<ClientWriteSingleResponse>();
        await ProcessWriteChunksAsync(
            deleteChunks,
            deletes => new ClientWriteRequest() { Deletes = deletes.ToList() },
            tupleKey => tupleKey.ToTupleKey(),
            deleteResponses,
            clientWriteOpts,
            maxParallelReqs,
            cancellationToken);

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
        IClientCheckOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.Check(
            GetStoreId(options),
            new CheckRequest {
                TupleKey = new CheckRequestTupleKey { User = body.User, Relation = body.Relation, Object = body.Object },
                ContextualTuples =
                    new ContextualTupleKeys {
                        TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                    new List<TupleKey>()
                    },
                Context = body.Context,
                AuthorizationModelId = GetAuthorizationModelId(options),
                Consistency = options?.Consistency,
            }, options, cancellationToken);

    /**
   * BatchCheck - Run a set of checks (evaluates)
   */
    public async Task<ClientBatchCheckClientResponse> BatchCheck(List<ClientCheckRequest> body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default) {
        var responses = new ConcurrentBag<BatchCheckSingleResponse>();

        var maxParallelReqs = options?.MaxParallelRequests ?? FgaConstants.ClientMaxMethodParallelRequests;

        await ProcessCheckRequestsAsync(body, responses, options, maxParallelReqs, cancellationToken);

        return new ClientBatchCheckClientResponse { Responses = responses.ToList() };
    }

    /**
   * Expand - Expands the relationships in userset tree format (evaluates)
   */
    public async Task<ExpandResponse> Expand(IClientExpandRequest body,
        IClientExpandOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.Expand(
            GetStoreId(options),
            new ExpandRequest {
                TupleKey = new ExpandRequestTupleKey { Relation = body.Relation, Object = body.Object },
                ContextualTuples =
                    new ContextualTupleKeys {
                        TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                    new List<TupleKey>()
                    },
                AuthorizationModelId = GetAuthorizationModelId(options),
                Consistency = options?.Consistency
            }, options, cancellationToken);

    /**
     * ListObjects - List the objects of a particular type that the user has a certain relation to (evaluates)
     */
    public async Task<ListObjectsResponse> ListObjects(IClientListObjectsRequest body,
        IClientListObjectsOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListObjects(GetStoreId(options), new ListObjectsRequest {
            User = body.User,
            Relation = body.Relation,
            Type = body.Type,
            ContextualTuples =
                new ContextualTupleKeys {
                    TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                new List<TupleKey>()
                },
            Context = body.Context,
            AuthorizationModelId = GetAuthorizationModelId(options),
            Consistency = options?.Consistency,
        }, options, cancellationToken);

    /**
     * StreamedListObjects - Stream all objects of a particular type that the user has a certain relation to (evaluates)
     * 
     * The Streamed ListObjects API is very similar to the ListObjects API, with two differences:
     * 1. Instead of collecting all objects before returning a response, it streams them to the client as they are collected.
     * 2. The number of results returned is only limited by the execution timeout specified in the flag OPENFGA_LIST_OBJECTS_DEADLINE.
     * 
     * Returns an async enumerable that yields StreamedListObjectsResponse objects as they are received from the server.
     */
    public async IAsyncEnumerable<StreamedListObjectsResponse> StreamedListObjects(
        IClientListObjectsRequest body,
        IClientListObjectsOptions? options = default,
        [EnumeratorCancellation] CancellationToken cancellationToken = default) {
        
        await foreach (var response in api.StreamedListObjects(GetStoreId(options), new ListObjectsRequest {
            User = body.User,
            Relation = body.Relation,
            Type = body.Type,
            ContextualTuples =
                new ContextualTupleKeys {
                    TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                new List<TupleKey>()
                },
            Context = body.Context,
            AuthorizationModelId = GetAuthorizationModelId(options),
            Consistency = options?.Consistency,
        }, options, cancellationToken)) {
            yield return response;
        }
    }

    /**
     * ListRelations - List all the relations a user has with an object (evaluates)
     */
    public async Task<ListRelationsResponse> ListRelations(IClientListRelationsRequest body,
        IClientListRelationsOptions? options = default,
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


        foreach (var batchCheckSingleResponse in batchCheckResponse.Responses) {
            if (batchCheckSingleResponse.Error != null) {
                throw batchCheckSingleResponse.Error;
            }

            if (batchCheckSingleResponse.Allowed && batchCheckSingleResponse.Request?.Relation != null) {
                responses.AddRelation(batchCheckSingleResponse.Request.Relation);
            }
        }

        return responses;
    }

    /**
     * ListUsers - List all users of the given type that the object has a relation with (evaluates)
     */
    public async Task<ListUsersResponse> ListUsers(IClientListUsersRequest body,
        IClientListUsersOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListUsers(GetStoreId(options), new ListUsersRequest {
            Object = body.Object,
            Relation = body.Relation,
            UserFilters = body.UserFilters,
            ContextualTuples = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ?? new List<TupleKey>(),
            Context = body.Context,
            AuthorizationModelId = GetAuthorizationModelId(options),
            Consistency = options?.Consistency
        }, options, cancellationToken);

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

        return await api.ReadAssertions(GetStoreId(options), authorizationModelId, options, cancellationToken);
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

        await api.WriteAssertions(GetStoreId(options), authorizationModelId,
            new WriteAssertionsRequest { Assertions = assertions }, options, cancellationToken);
    }
}