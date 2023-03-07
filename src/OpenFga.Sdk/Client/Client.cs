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
    private int DEFAULT_MAX_RETRY_OVERRIDE = 15;
    private readonly int DEFAULT_WAIT_TIME_BETWEEN_CHUNKS_IN_MS = 100;

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
    public async Task<ListStoresResponse> ListStores(ClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListStores(options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
   * CreateStore - Initialize a store
   */
    public async Task<CreateStoreResponse> CreateStore(ClientCreateStoreRequest body,
        ClientListStoresOptions? options = default,
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
        ClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadAuthorizationModels(options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
     * WriteAuthorizationModel - Create a new version of the authorization model
     */
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(WriteAuthorizationModelRequest body,
        ClientRequestOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.WriteAuthorizationModel(body, cancellationToken);

    /**
     * ReadAuthorizationModel - Read the current authorization model
     */
    public async Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(
        ClientReadAuthorizationModelOptions? options = default,
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
    public async Task<ReadAuthorizationModelResponse> ReadLatestAuthorizationModel(
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) {
        var response =
            await ReadAuthorizationModels(new ClientReadAuthorizationModelsOptions {PageSize = 1}, cancellationToken);
        return new ReadAuthorizationModelResponse {AuthorizationModel = response.AuthorizationModels?[0]};
    }

    /***********************
     * Relationship Tuples *
     ***********************/

    /**
     * Read Changes - Read the list of historical relationship tuple writes and deletes
     */
    public async Task<ReadChangesResponse> ReadChanges(ClientReadChangesRequest body,
        ClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadChanges(body.Type, options?.PageSize, options?.ContinuationToken, cancellationToken);

    /**
     * Read - Read tuples previously written to the store (does not evaluate)
     */
    public async Task<ReadResponse> Read(ClientReadRequest body, ClientReadOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.Read(
            new ReadRequest {
                TupleKey = body, PageSize = options?.PageSize, ContinuationToken = options?.ContinuationToken
            }, cancellationToken);

    /**
     * Write - Create or delete relationship tuples
     */
    public async Task<ClientWriteResponse> Write(ClientWriteRequest body, ClientWriteOptions options = default,
        CancellationToken cancellationToken = default) {
        var maxPerChunk =
            options.Transaction?.MaxPerChunk ??
            1; // 1 has to be the default otherwise the chunks will be sent in transactions
        var waitTimeBetweenChunksInMs =
            options.Transaction?.WaitTimeBetweenChunksInMs ?? DEFAULT_WAIT_TIME_BETWEEN_CHUNKS_IN_MS;
        var authorizationModelId = GetAuthorizationModelId(options);

        if (options.Transaction?.Disable != true) {
            await api.Write(
                new WriteRequest {
                    Writes = new TupleKeys(body.Writes ?? new List<TupleKey>()),
                    Deletes = new TupleKeys(body.Deletes ?? new List<TupleKey>()),
                    AuthorizationModelId = authorizationModelId
                }, cancellationToken);
            return new ClientWriteResponse {
                Writes =
                    body.Writes?.ConvertAll(tupleKey =>
                        new ClientWriteSingleResponse {TupleKey = tupleKey, Status = ClientWriteStatus.SUCCESS}) ??
                    new List<ClientWriteSingleResponse>(),
                Deletes = body.Deletes?.ConvertAll(tupleKey => new ClientWriteSingleResponse {
                    TupleKey = tupleKey, Status = ClientWriteStatus.SUCCESS
                }) ?? new List<ClientWriteSingleResponse>()
            };
        }

        var responses = new ClientWriteResponse {
            Writes = new List<ClientWriteSingleResponse>(), Deletes = new List<ClientWriteSingleResponse>()
        };

        var writeChunks = body.Writes?.Chunk(maxPerChunk).ToList() ?? new List<TupleKey[]>();
        for (var index = 0; index < writeChunks.Count(); index++) {
            var request = writeChunks[index].ToList();
            try {
                var response = await api.Write(
                    new WriteRequest {Writes = new TupleKeys(request), AuthorizationModelId = authorizationModelId},
                    cancellationToken);

                responses.Writes.AddRange(request.ConvertAll(req => new ClientWriteSingleResponse {
                    TupleKey = req, Status = ClientWriteStatus.SUCCESS
                }));
            }
            catch (Exception e) {
                responses.Writes.AddRange(request.ConvertAll(req => new ClientWriteSingleResponse {
                    TupleKey = req, Status = ClientWriteStatus.FAILURE, Error = e
                }));
            }
        }

        var deleteChunks = body.Deletes?.Chunk(maxPerChunk).ToList() ?? new List<TupleKey[]>();
        for (var index = 0; index < deleteChunks.Count(); index++) {
            var request = deleteChunks[index].ToList();
            try {
                var response = await api.Write(
                    new WriteRequest {Deletes = new TupleKeys(request), AuthorizationModelId = authorizationModelId},
                    cancellationToken);

                responses.Deletes.AddRange(request.ConvertAll(req => new ClientWriteSingleResponse {
                    TupleKey = req, Status = ClientWriteStatus.SUCCESS
                }));
            }
            catch (Exception e) {
                responses.Deletes.AddRange(request.ConvertAll(req => new ClientWriteSingleResponse {
                    TupleKey = req, Status = ClientWriteStatus.FAILURE, Error = e
                }));
            }
        }

        return responses;
    }

    /**
     * WriteTuples - Utility method to write tuples, wraps Write
     */
    public async Task<ClientWriteResponse> WriteTuples(List<TupleKey> body, ClientWriteOptions options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest {Writes = body}, options, cancellationToken);

    /**
     * DeleteTuples - Utility method to delete tuples, wraps Write
     */
    public async Task<ClientWriteResponse> DeleteTuples(List<TupleKey> body, ClientWriteOptions options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest {Deletes = body}, options, cancellationToken);

    /************************
     * Relationship Queries *
     ************************/

    /**
   * Check - Check if a user has a particular relation with an object (evaluates)
   */
    public async Task<CheckResponse> Check(ClientCheckRequest body,
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) =>
        await api.Check(
            new CheckRequest {
                TupleKey = new TupleKey {User = body.User, Relation = body.Relation, Object = body.Object},
                ContextualTuples =
                    new ContextualTupleKeys {
                        TupleKeys = body.ContextualTuples?.ConvertAll(tupleKey => tupleKey.ToTupleKey()) ??
                                    new List<TupleKey>()
                    },
                AuthorizationModelId = GetAuthorizationModelId(options)
            }, cancellationToken);

    /**
   * BatchCheck - Run a set of checks (evaluates)
   */
    public async Task<BatchCheckResponse> BatchCheck(List<ClientCheckRequest> body,
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) {
        var responses = new ConcurrentBag<BatchCheckSingleResponse>();
        await Parallel.ForEachAsync(body,
            new ParallelOptions {MaxDegreeOfParallelism = DEFAULT_MAX_METHOD_PARALLEL_REQS}, async (request, token) => {
                try {
                    var response = await Check(request, options, cancellationToken);

                    responses.Add(new BatchCheckSingleResponse {
                        Allowed = response.Allowed ?? false, Request = request, Error = null
                    });
                }
                catch (Exception e) {
                    responses.Add(new BatchCheckSingleResponse {Allowed = false, Request = request, Error = e});
                }
            });

        return new BatchCheckResponse {Responses = responses.ToList()};    
    }

    /**
   * Expand - Expands the relationships in userset tree format (evaluates)
   */
    public async Task<ExpandResponse> Expand(ClientExpandRequest body,
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) =>
        await api.Expand(
            new ExpandRequest {
                TupleKey = new TupleKey {Relation = body.Relation, Object = body.Object},
                AuthorizationModelId = GetAuthorizationModelId(options)
            }, cancellationToken);

    /**
     * ListObjects - List the objects of a particular type that the user has a certain relation to (evaluates)
     */
    public async Task<ListObjectsResponse> ListObjects(ClientListObjectsRequest body,
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListObjects(new ListObjectsRequest {
            User = body.User,
            Relation = body.Relation,
            Type = body.Type,
            ContextualTuples = new ContextualTupleKeys {TupleKeys = body.ContextualTuples ?? new List<TupleKey>()},
            AuthorizationModelId = GetAuthorizationModelId(options)
        });

    /**
     * ListRelations - List all the relations a user has with an object (evaluates)
     */
    public async Task<ListRelationsResponse> ListRelations(ListRelationsRequest body,
        IClientRequestOptionsWithAuthZModelId options = default,
        CancellationToken cancellationToken = default) {
        var responses = new ListRelationsResponse();
        var batchCheckRequests = new List<ClientCheckRequest>();
        for (var index = 0; index < body.Relations.Count; index++) {
            var relation = body.Relations[index];
            batchCheckRequests.Add(new ClientCheckRequest {
                User = body.User, Relation = relation, Object = body.Object, ContextualTuples = body.ContextualTuples
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
    public async Task<ReadAssertionsResponse> ReadAssertions(ClientReadAssertionsOptions options = default,
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
        ClientWriteAssertionsOptions options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        var assertions = new List<Assertion>();

        for (var index = 0; index < body.Count; index++) {
            var assertion = body[index];
            assertions.Append(new Assertion {
                TupleKey = new TupleKey {
                    User = assertion.User, Relation = assertion.Relation, Object = assertion.Object
                },
                Expectation = assertion.Expectation
            });
        }

        await api.WriteAssertions(authorizationModelId,
            new WriteAssertionsRequest {Assertions = assertions}, cancellationToken);
    }
}