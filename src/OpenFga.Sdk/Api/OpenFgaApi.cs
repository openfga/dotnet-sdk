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


using OpenFga.Sdk.Client;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Api;

public class OpenFgaApi : IDisposable {
    private Configuration.Configuration _configuration;
    private ApiClient _apiClient;

    /// <summary>
    /// Store ID
    /// </summary>
    public string? StoreId {
        get => _configuration.StoreId;
        set => _configuration.StoreId = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenFgaApi"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpClient"></param>
    public OpenFgaApi(
        Configuration.Configuration configuration,
        HttpClient? httpClient = null
    ) {
        this._configuration = configuration;
        this._apiClient = new ApiClient(_configuration, httpClient);
    }

    /// <summary>
    /// Check whether a user is authorized to access an object The Check API queries to check if the user has a certain relationship with an object in a certain store. Path parameter &#x60;store_id&#x60; as well as the body parameter &#x60;tuple_key&#x60; with specified &#x60;object&#x60;, &#x60;relation&#x60; and &#x60;user&#x60; subfields are all required. Optionally, a &#x60;contextual_tuples&#x60; object may also be included in the body of the request. This object contains one field &#x60;tuple_keys&#x60;, which is an array of tuple keys. The response will return whether the relationship exists in the field &#x60;allowed&#x60;.  ## Example In order to check if user &#x60;anne&#x60; has a &#x60;can_read&#x60; relationship with object &#x60;document:2021-budget&#x60; given the following contextual tuple &#x60;&#x60;&#x60;json {   \&quot;user\&quot;: \&quot;anne\&quot;,   \&quot;relation\&quot;: \&quot;member\&quot;,   \&quot;object\&quot;: \&quot;time_slot:office_hours\&quot; } &#x60;&#x60;&#x60; a check API call should be fired with the following body: &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;user\&quot;: \&quot;anne\&quot;,     \&quot;relation\&quot;: \&quot;can_read\&quot;,     \&quot;object\&quot;: \&quot;document:2021-budget\&quot;   },   \&quot;contextual_tuples\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;anne\&quot;,         \&quot;relation\&quot;: \&quot;member\&quot;,         \&quot;object\&quot;: \&quot;time_slot:office_hours\&quot;       }     ]   } } &#x60;&#x60;&#x60; OpenFGA&#39;s response will include &#x60;{ \&quot;allowed\&quot;: true }&#x60; if there is a relationship and &#x60;{ \&quot;allowed\&quot;: false }&#x60; if there isn&#39;t.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CheckResponse</returns>
    public async Task<CheckResponse> Check(CheckRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<CheckResponse>(requestBuilder,
            "Check", cancellationToken);
    }

    /// <summary>
    /// Create a store Create a unique OpenFGA store which will be used to store authorization models and relationship tuples.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateStoreResponse</returns>
    public async Task<CreateStoreResponse> CreateStore(CreateStoreRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };

        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<CreateStoreResponse>(requestBuilder,
            "CreateStore", cancellationToken);
    }

    /// <summary>
    /// Delete a store Delete an OpenFGA store. This does not delete the data associated to it, like tuples or authorization models.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task DeleteStore(CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("DELETE"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        await this._apiClient.SendRequestAsync(requestBuilder,
            "DeleteStore", cancellationToken);
    }

    /// <summary>
    /// Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship The Expand API will return all users (including user and userset) that have certain relationship with an object in a certain store. This is different from the &#x60;/stores/{store_id}/read&#x60; API in that both users and computed references are returned. Path parameter &#x60;store_id&#x60; as well as body parameter &#x60;object&#x60;, &#x60;relation&#x60; are all required. The response will return a userset tree whose leaves are the user id and usersets.  Union, intersection and difference operator are located in the intermediate nodes.  ## Example Assume the following type definition for document: &#x60;&#x60;&#x60;yaml   type document     relations       define reader as self or writer       define writer as self &#x60;&#x60;&#x60; In order to expand all users that have &#x60;reader&#x60; relationship with object &#x60;document:2021-budget&#x60;, an expand API call should be fired with the following body &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;object\&quot;: \&quot;document:2021-budget\&quot;,     \&quot;relation\&quot;: \&quot;reader\&quot;   } } &#x60;&#x60;&#x60; OpenFGA&#39;s response will be a userset tree of the users and computed usersets that have read access to the document. &#x60;&#x60;&#x60;json {   \&quot;tree\&quot;:{     \&quot;root\&quot;:{       \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,       \&quot;union\&quot;:{         \&quot;nodes\&quot;:[           {             \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,             \&quot;leaf\&quot;:{               \&quot;users\&quot;:{                 \&quot;users\&quot;:[                   \&quot;bob\&quot;                 ]               }             }           },           {             \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,             \&quot;leaf\&quot;:{               \&quot;computed\&quot;:{                 \&quot;userset\&quot;:\&quot;document:2021-budget#writer\&quot;               }             }           }         ]       }     }   } } &#x60;&#x60;&#x60; The caller can then call expand API for the &#x60;writer&#x60; relationship for the &#x60;document:2021-budget&#x60;.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ExpandResponse</returns>
    public async Task<ExpandResponse> Expand(ExpandRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/expand",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ExpandResponse>(requestBuilder,
            "Expand", cancellationToken);
    }

    /// <summary>
    /// Get a store Returns an OpenFGA store.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of GetStoreResponse</returns>
    public async Task<GetStoreResponse> GetStore(CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<GetStoreResponse>(requestBuilder,
            "GetStore", cancellationToken);
    }

    /// <summary>
    /// ListObjects lists all of the object ids for objects of the provided type that the given user has a specific relation with. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ListObjectsResponse</returns>
    public async Task<ListObjectsResponse> ListObjects(ListObjectsRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/list-objects",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ListObjectsResponse>(requestBuilder,
            "ListObjects", cancellationToken);
    }

    /// <summary>
    /// Get all stores Returns a paginated list of OpenFGA stores.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ListStoresResponse</returns>
    public async Task<ListStoresResponse> ListStores(int? pageSize = default(int?), string? continuationToken = default(string?), CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };

        var queryParams = new Dictionary<string, string>();
        if (pageSize != null) {
            queryParams.Add("page_size", pageSize.ToString());
        }
        if (continuationToken != null) {
            queryParams.Add("continuation_token", continuationToken.ToString());
        }

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ListStoresResponse>(requestBuilder,
            "ListStores", cancellationToken);
    }

    /// <summary>
    /// Get tuples from the store that matches a query, without following userset rewrite rules The POST read API will return the tuples for a certain store that matches a query filter specified in the body. Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. It is different from the &#x60;/stores/{store_id}/expand&#x60; API in that only read returns relationship tuples that are stored in the system and satisfy the query. It does not expand or traverse the graph by taking the authorization model into account.Path parameter &#x60;store_id&#x60; is required.  In the body: 1. Object is mandatory. An object can be a full object (e.g., &#x60;type:object_id&#x60;) or type only (e.g., &#x60;type:&#x60;). 2. User is mandatory in the case the object is type only. ## Examples ### Query for all objects in a type definition To query for all objects that &#x60;bob&#x60; has &#x60;reader&#x60; relationship in the document type definition, call read API with body of &#x60;&#x60;&#x60;json {  \&quot;tuple_key\&quot;: {      \&quot;user\&quot;: \&quot;bob\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;,      \&quot;object\&quot;: \&quot;document:\&quot;   } } &#x60;&#x60;&#x60; The API will return tuples and an optional continuation token, something like &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ] } &#x60;&#x60;&#x60; This means that &#x60;bob&#x60; has a &#x60;reader&#x60; relationship with 1 document &#x60;document:2021-budget&#x60;. ### Query for all users with particular relationships for a particular document To query for all users that have &#x60;reader&#x60; relationship with &#x60;document:2021-budget&#x60;, call read API with body of  &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {      \&quot;object\&quot;: \&quot;document:2021-budget\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;    } } &#x60;&#x60;&#x60; The API will return something like  &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ] } &#x60;&#x60;&#x60; This means that &#x60;document:2021-budget&#x60; has 1 &#x60;reader&#x60; (&#x60;bob&#x60;).  Note that the API will not return writers such as &#x60;anne&#x60; even when all writers are readers.  This is because only direct relationship are returned for the READ API. ### Query for all users with all relationships for a particular document To query for all users that have any relationship with &#x60;document:2021-budget&#x60;, call read API with body of  &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {       \&quot;object\&quot;: \&quot;document:2021-budget\&quot;    } } &#x60;&#x60;&#x60; The API will return something like  &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;anne\&quot;,         \&quot;relation\&quot;: \&quot;writer\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-05T13:42:12.356Z\&quot;     },     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ] } &#x60;&#x60;&#x60; This means that &#x60;document:2021-budget&#x60; has 1 &#x60;reader&#x60; (&#x60;bob&#x60;) and 1 &#x60;writer&#x60; (&#x60;anne&#x60;). 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadResponse</returns>
    public async Task<ReadResponse> Read(ReadRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/read",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ReadResponse>(requestBuilder,
            "Read", cancellationToken);
    }

    /// <summary>
    /// Read assertions for an authorization model ID The GET assertions API will return, for a given authorization model id, all the assertions stored for it. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="authorizationModelId"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAssertionsResponse</returns>
    public async Task<ReadAssertionsResponse> ReadAssertions(string authorizationModelId, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        if (authorizationModelId != null) {
            pathParams.Add("authorization_model_id", authorizationModelId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/assertions/{authorization_model_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ReadAssertionsResponse>(requestBuilder,
            "ReadAssertions", cancellationToken);
    }

    /// <summary>
    /// Return a particular version of an authorization model The GET authorization-models by ID API will return a particular version of authorization model that had been configured for a certain store.   Path parameter &#x60;store_id&#x60; and &#x60;id&#x60; are required. The response will return the authorization model for the particular version.  ## Example To retrieve the authorization model with ID &#x60;01G5JAVJ41T49E9TT3SKVS7X1J&#x60; for the store, call the GET authorization-models by ID API with &#x60;01G5JAVJ41T49E9TT3SKVS7X1J&#x60; as the &#x60;id&#x60; path parameter.  The API will return: &#x60;&#x60;&#x60;json {   \&quot;authorization_model\&quot;:{     \&quot;id\&quot;:\&quot;01G5JAVJ41T49E9TT3SKVS7X1J\&quot;,     \&quot;type_definitions\&quot;:[       {         \&quot;type\&quot;:\&quot;document\&quot;,         \&quot;relations\&quot;:{           \&quot;reader\&quot;:{             \&quot;union\&quot;:{               \&quot;child\&quot;:[                 {                   \&quot;this\&quot;:{}                 },                 {                   \&quot;computedUserset\&quot;:{                     \&quot;object\&quot;:\&quot;\&quot;,                     \&quot;relation\&quot;:\&quot;writer\&quot;                   }                 }               ]             }           },           \&quot;writer\&quot;:{             \&quot;this\&quot;:{}           }         }       }     ]   } } &#x60;&#x60;&#x60; In the above example, there is only 1 type (&#x60;document&#x60;) with 2 relations (&#x60;writer&#x60; and &#x60;reader&#x60;).
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAuthorizationModelResponse</returns>
    public async Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(string id, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        if (id != null) {
            pathParams.Add("id", id.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models/{id}",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ReadAuthorizationModelResponse>(requestBuilder,
            "ReadAuthorizationModel", cancellationToken);
    }

    /// <summary>
    /// Return all the authorization models for a particular store The GET authorization-models API will return all the authorization models for a certain store. Path parameter &#x60;store_id&#x60; is required. OpenFGA&#39;s response will contain an array of all authorization models, sorted in descending order of creation.  ## Example Assume that a store&#39;s authorization model has been configured twice. To get all the authorization models that have been created in this store, call GET authorization-models. The API will return a response that looks like: &#x60;&#x60;&#x60;json {   \&quot;authorization_models\&quot;: [     {       \&quot;id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;,       \&quot;type_definitions\&quot;: [...]     },     {       \&quot;id\&quot;: \&quot;01G4ZW8F4A07AKQ8RHSVG9RW04\&quot;,       \&quot;type_definitions\&quot;: [...]     },   ] } &#x60;&#x60;&#x60; If there are more authorization models available, the response will contain an extra field &#x60;continuation_token&#x60;: &#x60;&#x60;&#x60;json {   \&quot;authorization_models\&quot;: [     {       \&quot;id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;,       \&quot;type_definitions\&quot;: [...]     },     {       \&quot;id\&quot;: \&quot;01G4ZW8F4A07AKQ8RHSVG9RW04\&quot;,       \&quot;type_definitions\&quot;: [...]     },   ],   \&quot;continuation_token\&quot;: \&quot;eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ&#x3D;&#x3D;\&quot; } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAuthorizationModelsResponse</returns>
    public async Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(int? pageSize = default(int?), string? continuationToken = default(string?), CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();
        if (pageSize != null) {
            queryParams.Add("page_size", pageSize.ToString());
        }
        if (continuationToken != null) {
            queryParams.Add("continuation_token", continuationToken.ToString());
        }

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ReadAuthorizationModelsResponse>(requestBuilder,
            "ReadAuthorizationModels", cancellationToken);
    }

    /// <summary>
    /// Return a list of all the tuple changes The GET changes API will return a paginated list of tuple changes (additions and deletions) that occurred in a given store, sorted by ascending time. The response will include a continuation token that is used to get the next set of changes. If there are no changes after the provided continuation token, the same token will be returned in order for it to be used when new changes are recorded. If the store never had any tuples added or removed, this token will be empty. You can use the &#x60;type&#x60; parameter to only get the list of tuple changes that affect objects of that type. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="type"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadChangesResponse</returns>
    public async Task<ReadChangesResponse> ReadChanges(string? type = default(string?), int? pageSize = default(int?), string? continuationToken = default(string?), CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();
        if (type != null) {
            queryParams.Add("type", type.ToString());
        }
        if (pageSize != null) {
            queryParams.Add("page_size", pageSize.ToString());
        }
        if (continuationToken != null) {
            queryParams.Add("continuation_token", continuationToken.ToString());
        }

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/changes",
            PathParameters = pathParams,
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<ReadChangesResponse>(requestBuilder,
            "ReadChanges", cancellationToken);
    }

    /// <summary>
    /// Add or delete tuples from the store The POST write API will update the tuples for a certain store.  Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. Path parameter &#x60;store_id&#x60; is required.  In the body, &#x60;writes&#x60; adds new tuples while &#x60;deletes&#x60; removes existing tuples. ## Example ### Adding relationships To add &#x60;anne&#x60; as a &#x60;writer&#x60; for &#x60;document:2021-budget&#x60;, call write API with the following  &#x60;&#x60;&#x60;json {   \&quot;writes\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;anne\&quot;,         \&quot;relation\&quot;: \&quot;writer\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       }     ]   } } &#x60;&#x60;&#x60; ### Removing relationships To remove &#x60;bob&#x60; as a &#x60;reader&#x60; for &#x60;document:2021-budget&#x60;, call write API with the following  &#x60;&#x60;&#x60;json {   \&quot;deletes\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       }     ]   } } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of Object</returns>
    public async Task<Object> Write(WriteRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/write",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<Object>(requestBuilder,
            "Write", cancellationToken);
    }

    /// <summary>
    /// Upsert assertions for an authorization model ID The Write Assertions API will add new assertions for an authorization model id, or overwrite the existing ones. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="authorizationModelId"></param>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task WriteAssertions(string authorizationModelId, WriteAssertionsRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        if (authorizationModelId != null) {
            pathParams.Add("authorization_model_id", authorizationModelId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("PUT"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/assertions/{authorization_model_id}",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        await this._apiClient.SendRequestAsync(requestBuilder,
            "WriteAssertions", cancellationToken);
    }

    /// <summary>
    /// Create a new authorization model The POST authorization-model API will update the authorization model for a certain store. Path parameter &#x60;store_id&#x60; and &#x60;type_definitions&#x60; array in the body are required.  Each item in the &#x60;type_definitions&#x60; array is a type definition as specified in the field &#x60;type_definition&#x60;. The response will return the authorization model&#39;s ID in the &#x60;id&#x60; field.  ## Example To update the authorization model with a single &#x60;document&#x60; authorization model, call POST authorization-models API with the body:  &#x60;&#x60;&#x60;json {   \&quot;type_definitions\&quot;:[     {       \&quot;type\&quot;:\&quot;document\&quot;,       \&quot;relations\&quot;:{         \&quot;reader\&quot;:{           \&quot;union\&quot;:{             \&quot;child\&quot;:[               {                 \&quot;this\&quot;:{}               },               {                 \&quot;computedUserset\&quot;:{                   \&quot;object\&quot;:\&quot;\&quot;,                   \&quot;relation\&quot;:\&quot;writer\&quot;                 }               }             ]           }         },         \&quot;writer\&quot;:{           \&quot;this\&quot;:{}         }       }     }   ] } &#x60;&#x60;&#x60; OpenFGA&#39;s response will include the version id for this authorization model, which will look like  &#x60;&#x60;&#x60; {\&quot;authorization_model_id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;} &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of WriteAuthorizationModelResponse</returns>
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(WriteAuthorizationModelRequest body, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(_configuration.StoreId)) {
            throw new FgaRequiredParamError("Configuration", nameof(_configuration.StoreId));
        }
        pathParams.Add("store_id", _configuration.StoreId);


        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models",
            PathParameters = pathParams,
            Body = Utils.CreateJsonStringContent(body),
            QueryParameters = queryParams
        };

        return await this._apiClient.SendRequestAsync<WriteAuthorizationModelResponse>(requestBuilder,
            "WriteAuthorizationModel", cancellationToken);
    }


    public void Dispose() {
        _apiClient.Dispose();
    }
}