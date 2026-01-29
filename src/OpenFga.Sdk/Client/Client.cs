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

public class OpenFgaClient : IOpenFgaClient, IDisposable {
    private readonly ClientConfiguration _configuration;
    protected OpenFgaApi api;
    private readonly Lazy<ApiExecutor.ApiExecutor> _apiExecutor;

    public OpenFgaClient(
        ClientConfiguration configuration,
        HttpClient? httpClient = null
    ) {
        configuration.EnsureValid();
        _configuration = configuration;
        api = new OpenFgaApi(_configuration, httpClient);
        _apiExecutor = new Lazy<ApiExecutor.ApiExecutor>(() => new ApiExecutor.ApiExecutor(
            api.ApiClientInternal,
            _configuration));
    }

    /// <inheritdoc />
    public string? StoreId {
        get => _configuration.StoreId;
        set => _configuration.StoreId = value;
    }

    /// <inheritdoc />
    public string? AuthorizationModelId {
        get => _configuration.AuthorizationModelId;
        set => _configuration.AuthorizationModelId = value;
    }

    /// <summary>
    /// Gets the ApiExecutor for making custom API requests.
    /// The ApiExecutor allows you to call arbitrary OpenFGA API endpoints while
    /// automatically leveraging the SDK's authentication, retry logic, and error handling.
    /// </summary>
    /// <returns>An ApiExecutor instance</returns>
    public ApiExecutor.ApiExecutor ApiExecutor() {
        return _apiExecutor.Value;
    }

    public void Dispose() {
        if (_apiExecutor.IsValueCreated) {
            _apiExecutor.Value?.Dispose();
        }
        api.Dispose();
    }

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
        IClientBatchCheckClientOptions? options,
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
        IClientBatchCheckClientOptions? options,
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

    /// <inheritdoc />
    public async Task<ListStoresResponse> ListStores(IClientListStoresRequest? body, IClientListStoresOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ListStores(options?.PageSize, options?.ContinuationToken, body?.Name, options, cancellationToken);

    /// <inheritdoc />
    public async Task<CreateStoreResponse> CreateStore(ClientCreateStoreRequest body,
        IClientRequestOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.CreateStore(body, options, cancellationToken);

    /// <inheritdoc />
    public async Task<GetStoreResponse> GetStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default) =>
        await api.GetStore(GetStoreId(options), options, cancellationToken);

    /// <inheritdoc />
    public async Task DeleteStore(IClientRequestOptionsWithStoreId? options = default, CancellationToken cancellationToken = default) =>
        await api.DeleteStore(GetStoreId(options), options, cancellationToken);

    /************************
     * Authorization Models *
     ************************/

    /// <inheritdoc />
    public async Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(
        IClientReadAuthorizationModelsOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadAuthorizationModels(GetStoreId(options), options?.PageSize, options?.ContinuationToken, options, cancellationToken);

    /// <inheritdoc />
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(ClientWriteAuthorizationModelRequest body,
        IClientRequestOptionsWithStoreId? options = default,
        CancellationToken cancellationToken = default) =>
        await api.WriteAuthorizationModel(GetStoreId(options), body, options, cancellationToken);

    /// <inheritdoc />
    public async Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(
        IClientReadAuthorizationModelOptions? options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        return await api.ReadAuthorizationModel(GetStoreId(options), authorizationModelId, options, cancellationToken);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<ReadChangesResponse> ReadChanges(ClientReadChangesRequest? body = default,
        ClientReadChangesOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await api.ReadChanges(GetStoreId(options), body?.Type, options?.PageSize, options?.ContinuationToken, body?.StartTime, options, cancellationToken);

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<ClientWriteResponse> WriteTuples(List<ClientTupleKey> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest { Writes = body }, options, cancellationToken);

    /// <inheritdoc />
    public async Task<ClientWriteResponse> DeleteTuples(List<ClientTupleKeyWithoutCondition> body, IClientWriteOptions? options = default,
        CancellationToken cancellationToken = default) =>
        await Write(new ClientWriteRequest { Deletes = body }, options, cancellationToken);

    /************************
     * Relationship Queries *
     ************************/

    /// <inheritdoc />
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
    
    /// <inheritdoc />
    public async Task<ClientBatchCheckClientResponse> ClientBatchCheck(List<ClientCheckRequest> body,
        IClientBatchCheckClientOptions? options = default,
        CancellationToken cancellationToken = default) {
        var responses = new ConcurrentBag<BatchCheckSingleResponse>();

        var maxParallelReqs = options?.MaxParallelRequests ?? FgaConstants.ClientMaxMethodParallelRequests;

        await ProcessCheckRequestsAsync(body, responses, options, maxParallelReqs, cancellationToken);

        return new ClientBatchCheckClientResponse { Responses = responses.ToList() };
    }

    /// <inheritdoc />
    public async Task<ClientBatchCheckResponse> BatchCheck(ClientBatchCheckRequest body,
        IClientBatchCheckOptions? options = default,
        CancellationToken cancellationToken = default) {

        // If no checks provided, return empty result
        if (body?.Checks == null || body.Checks.Count == 0) {
            return new ClientBatchCheckResponse(new List<ClientBatchCheckSingleResponse>());
        }

        // Generate bulk request ID for tracking chunked requests
        var bulkRequestId = Utils.ClientUtils.GenerateCorrelationId();
        var headers = new Dictionary<string, string>(options?.Headers ?? new Dictionary<string, string>()) {
            [FgaConstants.ClientBulkRequestIdHeader] = bulkRequestId
        };

        var maxBatchSize = options?.MaxBatchSize ?? FgaConstants.ClientMaxBatchSize;
        var maxParallelReqs = options?.MaxParallelRequests ?? FgaConstants.ClientMaxMethodParallelRequests;

        // Validate parameters to prevent deadlock or invalid configurations
        if (maxBatchSize <= 0) {
            throw new FgaValidationError("MaxBatchSize must be greater than 0.");
        }

        if (maxParallelReqs <= 0) {
            throw new FgaValidationError("MaxParallelRequests must be greater than 0.");
        }

        // Track correlation IDs to original requests
        var correlationIdToCheck = new Dictionary<string, ClientBatchCheckItem>();
        var transformedChecks = new List<BatchCheckItem>();

        // Validate and transform checks
        foreach (var check in body.Checks) {
            // Generate correlation ID if not provided
            if (string.IsNullOrWhiteSpace(check.CorrelationId)) {
                check.CorrelationId = Utils.ClientUtils.GenerateCorrelationId();
            }

            // Ensure correlation IDs are unique
            if (correlationIdToCheck.ContainsKey(check.CorrelationId)) {
                throw new FgaValidationError($"Duplicate correlation ID found: {check.CorrelationId}. Correlation IDs must be unique.");
            }

            correlationIdToCheck[check.CorrelationId] = check;

            // Transform to API model
            transformedChecks.Add(Utils.ClientUtils.TransformToBatchCheckItem(check, check.CorrelationId));
        }

        // Split into batches
        var batches = Utils.ClientUtils.ChunkList(transformedChecks, maxBatchSize);
        var results = new List<ClientBatchCheckSingleResponse>();

#if NET6_0_OR_GREATER
        // Process batches in parallel with degree of parallelism limit
        await Parallel.ForEachAsync(batches,
            new ParallelOptions {
                MaxDegreeOfParallelism = maxParallelReqs,
                CancellationToken = cancellationToken
            },
            async (batch, token) => {
                await ProcessBatchAsync(batch, options, headers, correlationIdToCheck, results, token);
            });
#else
        // For .NET Framework 4.8 and .NET Standard 2.0, use SemaphoreSlim for parallelism control
        using (var throttler = new SemaphoreSlim(maxParallelReqs)) {
            var tasks = batches.Select(async batch => {
                await throttler.WaitAsync(cancellationToken);
                try {
                    await ProcessBatchAsync(batch, options, headers, correlationIdToCheck, results, cancellationToken);
                }
                finally {
                    throttler.Release();
                }
            }).ToList();

            await Task.WhenAll(tasks);
        }
#endif

        return new ClientBatchCheckResponse(results);
    }

    private async Task ProcessBatchAsync(
        List<BatchCheckItem> batch,
        IClientBatchCheckOptions? options,
        Dictionary<string, string> headers,
        Dictionary<string, ClientBatchCheckItem> correlationIdToCheck,
        List<ClientBatchCheckSingleResponse> results,
        CancellationToken cancellationToken) {

        var batchRequest = new BatchCheckRequest(
            checks: batch,
            authorizationModelId: GetAuthorizationModelId(options),
            consistency: options?.Consistency
        );

        // Create options with headers for this batch
        var batchOptions = new ClientBatchCheckOptions {
            StoreId = options?.StoreId,
            AuthorizationModelId = options?.AuthorizationModelId,
            Consistency = options?.Consistency,
            Headers = headers
        };

        var batchResponse = await api.BatchCheck(
            GetStoreId(options),
            batchRequest,
            batchOptions,
            cancellationToken
        );

        // Map responses back to original requests using correlation IDs
        if (batchResponse.Result != null) {
            lock (results) {
                foreach (var kvp in batchResponse.Result) {
                    var correlationId = kvp.Key;
                    var result = kvp.Value;

                    if (correlationIdToCheck.TryGetValue(correlationId, out var originalCheck)) {
                        results.Add(new ClientBatchCheckSingleResponse(
                            allowed: result.Allowed ?? false,
                            request: originalCheck,
                            correlationId: correlationId,
                            error: result.Error
                        ));
                    }
                }
            }
        }
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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
     * The Streamed ListObjects API is very similar to the ListObjects API, with two key differences:
     * 1. Streaming Results: Instead of collecting all objects before returning a response, it streams them to the client as they are collected.
     * 2. No Pagination Limit: Returns all results without the 1000-object limit of the standard ListObjects API.
     * 
     * This is particularly useful when querying computed relations that may return large result sets.
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

    /// <inheritdoc />
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

        var batchCheckResponse = await ClientBatchCheck(batchCheckRequests, options, cancellationToken);


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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<ReadAssertionsResponse> ReadAssertions(IClientReadAssertionsOptions? options = default,
        CancellationToken cancellationToken = default) {
        var authorizationModelId = GetAuthorizationModelId(options);
        if (authorizationModelId == null) {
            throw new FgaRequiredParamError("ClientConfiguration", "AuthorizationModelId");
        }

        return await api.ReadAssertions(GetStoreId(options), authorizationModelId, options, cancellationToken);
    }

    /// <inheritdoc />
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