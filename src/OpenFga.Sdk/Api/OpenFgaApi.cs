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


using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OpenFga.Sdk.Api;

public class OpenFgaApi : IDisposable {
    private readonly ApiClient.ApiClient _apiClient;
    private readonly Configuration.Configuration _configuration;

    /// <summary>
    ///     Initializes a new instance of the <see cref="OpenFgaApi"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="httpClient"></param>
    public OpenFgaApi(
        Configuration.Configuration configuration,
        HttpClient? httpClient = null
    ) {
        configuration.EnsureValid();
        _configuration = configuration;
        _apiClient = new ApiClient.ApiClient(_configuration, httpClient);
    }

    /// <summary>
    /// Send a list of &#x60;check&#x60; operations in a single request The &#x60;BatchCheck&#x60; API functions nearly identically to &#x60;Check&#x60;, but instead of checking a single user-object relationship BatchCheck accepts a list of relationships to check and returns a map containing &#x60;BatchCheckItem&#x60; response for each check it received.  An associated &#x60;correlation_id&#x60; is required for each check in the batch. This ID is used to correlate a check to the appropriate response. It is a string consisting of only alphanumeric characters or hyphens with a maximum length of 36 characters. This &#x60;correlation_id&#x60; is used to map the result of each check to the item which was checked, so it must be unique for each item in the batch. We recommend using a UUID or ULID as the &#x60;correlation_id&#x60;, but you can use whatever unique identifier you need as long  as it matches this regex pattern: &#x60;^[\\w\\d-]{1,36}$&#x60;  NOTE: The maximum number of checks that can be passed in the &#x60;BatchCheck&#x60; API is configurable via the [OPENFGA_MAX_CHECKS_PER_BATCH_CHECK](https://openfga.dev/docs/getting-started/setup-openfga/configuration#OPENFGA_MAX_CHECKS_PER_BATCH_CHECK) environment variable. If &#x60;BatchCheck&#x60; is called using the SDK, the SDK can split the batch check requests for you.  For more details on how &#x60;Check&#x60; functions, see the docs for &#x60;/check&#x60;.  ### Examples #### A BatchCheckRequest &#x60;&#x60;&#x60;json {   \&quot;checks\&quot;: [      {        \&quot;tuple_key\&quot;: {          \&quot;object\&quot;: \&quot;document:2021-budget\&quot;          \&quot;relation\&quot;: \&quot;reader\&quot;,          \&quot;user\&quot;: \&quot;user:anne\&quot;,        },        \&quot;contextual_tuples\&quot;: {...}        \&quot;context\&quot;: {}        \&quot;correlation_id\&quot;: \&quot;01JA8PM3QM7VBPGB8KMPK8SBD5\&quot;      },      {        \&quot;tuple_key\&quot;: {          \&quot;object\&quot;: \&quot;document:2021-budget\&quot;          \&quot;relation\&quot;: \&quot;reader\&quot;,          \&quot;user\&quot;: \&quot;user:bob\&quot;,        },        \&quot;contextual_tuples\&quot;: {...}        \&quot;context\&quot;: {}        \&quot;correlation_id\&quot;: \&quot;01JA8PMM6A90NV5ET0F28CYSZQ\&quot;      }    ] } &#x60;&#x60;&#x60;  Below is a possible response to the above request. Note that the result map&#39;s keys are the &#x60;correlation_id&#x60; values from the checked items in the request: &#x60;&#x60;&#x60;json {    \&quot;result\&quot;: {      \&quot;01JA8PMM6A90NV5ET0F28CYSZQ\&quot;: {        \&quot;allowed\&quot;: false,         \&quot;error\&quot;: {\&quot;message\&quot;: \&quot;\&quot;}      },      \&quot;01JA8PM3QM7VBPGB8KMPK8SBD5\&quot;: {        \&quot;allowed\&quot;: true,         \&quot;error\&quot;: {\&quot;message\&quot;: \&quot;\&quot;}      } } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of BatchCheckResponse</returns>
    public async Task<BatchCheckResponse> BatchCheck(string storeId, BatchCheckRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("BatchCheck", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<BatchCheckRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/batch-check",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<BatchCheckRequest, BatchCheckResponse>(requestBuilder,
            "BatchCheck", options, cancellationToken);
    }

    /// <summary>
    /// Check whether a user is authorized to access an object The Check API returns whether a given user has a relationship with a given object in a given store. The &#x60;user&#x60; field of the request can be a specific target, such as &#x60;user:anne&#x60;, or a userset (set of users) such as &#x60;group:marketing#member&#x60; or a type-bound public access &#x60;user:*&#x60;. To arrive at a result, the API uses: an authorization model, explicit tuples written through the Write API, contextual tuples present in the request, and implicit tuples that exist by virtue of applying set theory (such as &#x60;document:2021-budget#viewer@document:2021-budget#viewer&#x60;; the set of users who are viewers of &#x60;document:2021-budget&#x60; are the set of users who are the viewers of &#x60;document:2021-budget&#x60;). A &#x60;contextual_tuples&#x60; object may also be included in the body of the request. This object contains one field &#x60;tuple_keys&#x60;, which is an array of tuple keys. Each of these tuples may have an associated &#x60;condition&#x60;. You may also provide an &#x60;authorization_model_id&#x60; in the body. This will be used to assert that the input &#x60;tuple_key&#x60; is valid for the model specified. If not specified, the assertion will be made against the latest authorization model ID. It is strongly recommended to specify authorization model id for better performance. You may also provide a &#x60;context&#x60; object that will be used to evaluate the conditioned tuples in the system. It is strongly recommended to provide a value for all the input parameters of all the conditions, to ensure that all tuples be evaluated correctly. By default, the Check API caches results for a short time to optimize performance. You may specify a value of &#x60;HIGHER_CONSISTENCY&#x60; for the optional &#x60;consistency&#x60; parameter in the body to inform the server that higher conisistency is preferred at the expense of increased latency. Consideration should be given to the increased latency if requesting higher consistency. The response will return whether the relationship exists in the field &#x60;allowed&#x60;.  Some exceptions apply, but in general, if a Check API responds with &#x60;{allowed: true}&#x60;, then you can expect the equivalent ListObjects query to return the object, and viceversa.  For example, if &#x60;Check(user:anne, reader, document:2021-budget)&#x60; responds with &#x60;{allowed: true}&#x60;, then &#x60;ListObjects(user:anne, reader, document)&#x60; may include &#x60;document:2021-budget&#x60; in the response. ## Examples ### Querying with contextual tuples In order to check if user &#x60;user:anne&#x60; of type &#x60;user&#x60; has a &#x60;reader&#x60; relationship with object &#x60;document:2021-budget&#x60; given the following contextual tuple &#x60;&#x60;&#x60;json {   \&quot;user\&quot;: \&quot;user:anne\&quot;,   \&quot;relation\&quot;: \&quot;member\&quot;,   \&quot;object\&quot;: \&quot;time_slot:office_hours\&quot; } &#x60;&#x60;&#x60; the Check API can be used with the following request body: &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;user\&quot;: \&quot;user:anne\&quot;,     \&quot;relation\&quot;: \&quot;reader\&quot;,     \&quot;object\&quot;: \&quot;document:2021-budget\&quot;   },   \&quot;contextual_tuples\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;user:anne\&quot;,         \&quot;relation\&quot;: \&quot;member\&quot;,         \&quot;object\&quot;: \&quot;time_slot:office_hours\&quot;       }     ]   },   \&quot;authorization_model_id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot; } &#x60;&#x60;&#x60; ### Querying usersets Some Checks will always return &#x60;true&#x60;, even without any tuples. For example, for the following authorization model &#x60;&#x60;&#x60;python model   schema 1.1 type user type document   relations     define reader: [user] &#x60;&#x60;&#x60; the following query &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {      \&quot;user\&quot;: \&quot;document:2021-budget#reader\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;,      \&quot;object\&quot;: \&quot;document:2021-budget\&quot;   } } &#x60;&#x60;&#x60; will always return &#x60;{ \&quot;allowed\&quot;: true }&#x60;. This is because usersets are self-defining: the userset &#x60;document:2021-budget#reader&#x60; will always have the &#x60;reader&#x60; relation with &#x60;document:2021-budget&#x60;. ### Querying usersets with difference in the model A Check for a userset can yield results that must be treated carefully if the model involves difference. For example, for the following authorization model &#x60;&#x60;&#x60;python model   schema 1.1 type user type group   relations     define member: [user] type document   relations     define blocked: [user]     define reader: [group#member] but not blocked &#x60;&#x60;&#x60; the following query &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {      \&quot;user\&quot;: \&quot;group:finance#member\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;,      \&quot;object\&quot;: \&quot;document:2021-budget\&quot;   },   \&quot;contextual_tuples\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;user:anne\&quot;,         \&quot;relation\&quot;: \&quot;member\&quot;,         \&quot;object\&quot;: \&quot;group:finance\&quot;       },       {         \&quot;user\&quot;: \&quot;group:finance#member\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       {         \&quot;user\&quot;: \&quot;user:anne\&quot;,         \&quot;relation\&quot;: \&quot;blocked\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       }     ]   }, } &#x60;&#x60;&#x60; will return &#x60;{ \&quot;allowed\&quot;: true }&#x60;, even though a specific user of the userset &#x60;group:finance#member&#x60; does not have the &#x60;reader&#x60; relationship with the given object. ### Requesting higher consistency By default, the Check API caches results for a short time to optimize performance. You may request higher consistency to inform the server that higher consistency should be preferred at the expense of increased latency. Care should be taken when requesting higher consistency due to the increased latency. &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {      \&quot;user\&quot;: \&quot;group:finance#member\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;,      \&quot;object\&quot;: \&quot;document:2021-budget\&quot;   },   \&quot;consistency\&quot;: \&quot;HIGHER_CONSISTENCY\&quot; } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CheckResponse</returns>
    public async Task<CheckResponse> Check(string storeId, CheckRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("Check", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<CheckRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/check",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<CheckRequest, CheckResponse>(requestBuilder,
            "Check", options, cancellationToken);
    }

    /// <summary>
    /// Create a store Create a unique OpenFGA store which will be used to store authorization models and relationship tuples.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of CreateStoreResponse</returns>
    public async Task<CreateStoreResponse> CreateStore(CreateStoreRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };

        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<CreateStoreRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<CreateStoreRequest, CreateStoreResponse>(requestBuilder,
            "CreateStore", options, cancellationToken);
    }

    /// <summary>
    /// Delete a store Delete an OpenFGA store. This does not delete the data associated with the store, like tuples or authorization models.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task DeleteStore(string storeId, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("DeleteStore", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("DELETE"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        await _apiClient.SendRequestAsync(requestBuilder,
            "DeleteStore", options, cancellationToken);
    }

    /// <summary>
    /// Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship The Expand API will return all users and usersets that have certain relationship with an object in a certain store. This is different from the &#x60;/stores/{store_id}/read&#x60; API in that both users and computed usersets are returned. Body parameters &#x60;tuple_key.object&#x60; and &#x60;tuple_key.relation&#x60; are all required. A &#x60;contextual_tuples&#x60; object may also be included in the body of the request. This object contains one field &#x60;tuple_keys&#x60;, which is an array of tuple keys. Each of these tuples may have an associated &#x60;condition&#x60;. The response will return a tree whose leaves are the specific users and usersets. Union, intersection and difference operator are located in the intermediate nodes.  ## Example To expand all users that have the &#x60;reader&#x60; relationship with object &#x60;document:2021-budget&#x60;, use the Expand API with the following request body &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;object\&quot;: \&quot;document:2021-budget\&quot;,     \&quot;relation\&quot;: \&quot;reader\&quot;   },   \&quot;authorization_model_id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot; } &#x60;&#x60;&#x60; OpenFGA&#39;s response will be a userset tree of the users and usersets that have read access to the document. &#x60;&#x60;&#x60;json {   \&quot;tree\&quot;:{     \&quot;root\&quot;:{       \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,       \&quot;union\&quot;:{         \&quot;nodes\&quot;:[           {             \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,             \&quot;leaf\&quot;:{               \&quot;users\&quot;:{                 \&quot;users\&quot;:[                   \&quot;user:bob\&quot;                 ]               }             }           },           {             \&quot;type\&quot;:\&quot;document:2021-budget#reader\&quot;,             \&quot;leaf\&quot;:{               \&quot;computed\&quot;:{                 \&quot;userset\&quot;:\&quot;document:2021-budget#writer\&quot;               }             }           }         ]       }     }   } } &#x60;&#x60;&#x60; The caller can then call expand API for the &#x60;writer&#x60; relationship for the &#x60;document:2021-budget&#x60;. ### Expand Request with Contextual Tuples  Given the model &#x60;&#x60;&#x60;python model     schema 1.1  type user  type folder     relations         define owner: [user]  type document     relations         define parent: [folder]         define viewer: [user] or writer         define writer: [user] or owner from parent &#x60;&#x60;&#x60; and the initial tuples &#x60;&#x60;&#x60;json [{     \&quot;user\&quot;: \&quot;user:bob\&quot;,     \&quot;relation\&quot;: \&quot;owner\&quot;,     \&quot;object\&quot;: \&quot;folder:1\&quot; }] &#x60;&#x60;&#x60;  To expand all &#x60;writers&#x60; of &#x60;document:1&#x60; when &#x60;document:1&#x60; is put in &#x60;folder:1&#x60;, the first call could be  &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;object\&quot;: \&quot;document:1\&quot;,     \&quot;relation\&quot;: \&quot;writer\&quot;   },   \&quot;contextual_tuples\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;folder:1\&quot;,         \&quot;relation\&quot;: \&quot;parent\&quot;,         \&quot;object\&quot;: \&quot;document:1\&quot;       }     ]   } } &#x60;&#x60;&#x60; this returns: &#x60;&#x60;&#x60;json {   \&quot;tree\&quot;: {     \&quot;root\&quot;: {       \&quot;name\&quot;: \&quot;document:1#writer\&quot;,       \&quot;union\&quot;: {         \&quot;nodes\&quot;: [           {             \&quot;name\&quot;: \&quot;document:1#writer\&quot;,             \&quot;leaf\&quot;: {               \&quot;users\&quot;: {                 \&quot;users\&quot;: []               }             }           },           {             \&quot;name\&quot;: \&quot;document:1#writer\&quot;,             \&quot;leaf\&quot;: {               \&quot;tupleToUserset\&quot;: {                 \&quot;tupleset\&quot;: \&quot;document:1#parent\&quot;,                 \&quot;computed\&quot;: [                   {                     \&quot;userset\&quot;: \&quot;folder:1#owner\&quot;                   }                 ]               }             }           }         ]       }     }   } } &#x60;&#x60;&#x60; This tells us that the &#x60;owner&#x60; of &#x60;folder:1&#x60; may also be a writer. So our next call could be to find the &#x60;owners&#x60; of &#x60;folder:1&#x60; &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {     \&quot;object\&quot;: \&quot;folder:1\&quot;,     \&quot;relation\&quot;: \&quot;owner\&quot;   } } &#x60;&#x60;&#x60; which gives &#x60;&#x60;&#x60;json {   \&quot;tree\&quot;: {     \&quot;root\&quot;: {       \&quot;name\&quot;: \&quot;folder:1#owner\&quot;,       \&quot;leaf\&quot;: {         \&quot;users\&quot;: {           \&quot;users\&quot;: [             \&quot;user:bob\&quot;           ]         }       }     }   } } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ExpandResponse</returns>
    public async Task<ExpandResponse> Expand(string storeId, ExpandRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("Expand", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<ExpandRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/expand",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<ExpandRequest, ExpandResponse>(requestBuilder,
            "Expand", options, cancellationToken);
    }

    /// <summary>
    /// Get a store Returns an OpenFGA store by its identifier
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of GetStoreResponse</returns>
    public async Task<GetStoreResponse> GetStore(string storeId, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("GetStore", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, GetStoreResponse>(requestBuilder,
            "GetStore", options, cancellationToken);
    }

    /// <summary>
    /// List all objects of the given type that the user has a relation with The ListObjects API returns a list of all the objects of the given type that the user has a relation with.  To arrive at a result, the API uses: an authorization model, explicit tuples written through the Write API, contextual tuples present in the request, and implicit tuples that exist by virtue of applying set theory (such as &#x60;document:2021-budget#viewer@document:2021-budget#viewer&#x60;; the set of users who are viewers of &#x60;document:2021-budget&#x60; are the set of users who are the viewers of &#x60;document:2021-budget&#x60;). An &#x60;authorization_model_id&#x60; may be specified in the body. If it is not specified, the latest authorization model ID will be used. It is strongly recommended to specify authorization model id for better performance. You may also specify &#x60;contextual_tuples&#x60; that will be treated as regular tuples. Each of these tuples may have an associated &#x60;condition&#x60;. You may also provide a &#x60;context&#x60; object that will be used to evaluate the conditioned tuples in the system. It is strongly recommended to provide a value for all the input parameters of all the conditions, to ensure that all tuples be evaluated correctly. By default, the Check API caches results for a short time to optimize performance. You may specify a value of &#x60;HIGHER_CONSISTENCY&#x60; for the optional &#x60;consistency&#x60; parameter in the body to inform the server that higher conisistency is preferred at the expense of increased latency. Consideration should be given to the increased latency if requesting higher consistency. The response will contain the related objects in an array in the \&quot;objects\&quot; field of the response and they will be strings in the object format &#x60;&lt;type&gt;:&lt;id&gt;&#x60; (e.g. \&quot;document:roadmap\&quot;). The number of objects in the response array will be limited by the execution timeout specified in the flag OPENFGA_LIST_OBJECTS_DEADLINE and by the upper bound specified in the flag OPENFGA_LIST_OBJECTS_MAX_RESULTS, whichever is hit first. The objects given will not be sorted, and therefore two identical calls can give a given different set of objects.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ListObjectsResponse</returns>
    public async Task<ListObjectsResponse> ListObjects(string storeId, ListObjectsRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ListObjects", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<ListObjectsRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/list-objects",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<ListObjectsRequest, ListObjectsResponse>(requestBuilder,
            "ListObjects", options, cancellationToken);
    }

    /// <summary>
    /// List all stores Returns a paginated list of OpenFGA stores and a continuation token to get additional stores. The continuation token will be empty if there are no more stores. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="name">The name parameter instructs the API to only include results that match that name.Multiple results may be returned. Only exact matches will be returned; substring matches and regexes will not be evaluated (optional)</param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ListStoresResponse</returns>
    public async Task<ListStoresResponse> ListStores(int? pageSize = default(int?), string? continuationToken = default(string?), string? name = default(string?), IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };

        var queryParams = new Dictionary<string, string>();
        if (pageSize != null) {
            queryParams.Add("page_size", pageSize.ToString());
        }
        if (continuationToken != null) {
            queryParams.Add("continuation_token", continuationToken.ToString());
        }
        if (name != null) {
            queryParams.Add("name", name.ToString());
        }

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, ListStoresResponse>(requestBuilder,
            "ListStores", options, cancellationToken);
    }

    /// <summary>
    /// List the users matching the provided filter who have a certain relation to a particular type. The ListUsers API returns a list of all the users of a specific type that have a relation to a given object.  To arrive at a result, the API uses: an authorization model, explicit tuples written through the Write API, contextual tuples present in the request, and implicit tuples that exist by virtue of applying set theory (such as &#x60;document:2021-budget#viewer@document:2021-budget#viewer&#x60;; the set of users who are viewers of &#x60;document:2021-budget&#x60; are the set of users who are the viewers of &#x60;document:2021-budget&#x60;). An &#x60;authorization_model_id&#x60; may be specified in the body. If it is not specified, the latest authorization model ID will be used. It is strongly recommended to specify authorization model id for better performance. You may also specify &#x60;contextual_tuples&#x60; that will be treated as regular tuples. Each of these tuples may have an associated &#x60;condition&#x60;. You may also provide a &#x60;context&#x60; object that will be used to evaluate the conditioned tuples in the system. It is strongly recommended to provide a value for all the input parameters of all the conditions, to ensure that all tuples be evaluated correctly. The response will contain the related users in an array in the \&quot;users\&quot; field of the response. These results may include specific objects, usersets  or type-bound public access. Each of these types of results is encoded in its own type and not represented as a string.In cases where a type-bound public access result is returned (e.g. &#x60;user:*&#x60;), it cannot be inferred that all subjects of that type have a relation to the object; it is possible that negations exist and checks should still be queried on individual subjects to ensure access to that document.The number of users in the response array will be limited by the execution timeout specified in the flag OPENFGA_LIST_USERS_DEADLINE and by the upper bound specified in the flag OPENFGA_LIST_USERS_MAX_RESULTS, whichever is hit first. The returned users will not be sorted, and therefore two identical calls may yield different sets of users.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ListUsersResponse</returns>
    public async Task<ListUsersResponse> ListUsers(string storeId, ListUsersRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ListUsers", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<ListUsersRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/list-users",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<ListUsersRequest, ListUsersResponse>(requestBuilder,
            "ListUsers", options, cancellationToken);
    }

    /// <summary>
    /// Get tuples from the store that matches a query, without following userset rewrite rules The Read API will return the tuples for a certain store that match a query filter specified in the body of the request.  The API doesn&#39;t guarantee order by any field.  It is different from the &#x60;/stores/{store_id}/expand&#x60; API in that it only returns relationship tuples that are stored in the system and satisfy the query.  In the body: 1. &#x60;tuple_key&#x60; is optional. If not specified, it will return all tuples in the store. 2. &#x60;tuple_key.object&#x60; is mandatory if &#x60;tuple_key&#x60; is specified. It can be a full object (e.g., &#x60;type:object_id&#x60;) or type only (e.g., &#x60;type:&#x60;). 3. &#x60;tuple_key.user&#x60; is mandatory if tuple_key is specified in the case the &#x60;tuple_key.object&#x60; is a type only. If tuple_key.user is specified, it needs to be a full object (e.g., &#x60;type:user_id&#x60;). ## Examples ### Query for all objects in a type definition To query for all objects that &#x60;user:bob&#x60; has &#x60;reader&#x60; relationship in the &#x60;document&#x60; type definition, call read API with body of &#x60;&#x60;&#x60;json {  \&quot;tuple_key\&quot;: {      \&quot;user\&quot;: \&quot;user:bob\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;,      \&quot;object\&quot;: \&quot;document:\&quot;   } } &#x60;&#x60;&#x60; The API will return tuples and a continuation token, something like &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;user:bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ],   \&quot;continuation_token\&quot;: \&quot;eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ&#x3D;&#x3D;\&quot; } &#x60;&#x60;&#x60; This means that &#x60;user:bob&#x60; has a &#x60;reader&#x60; relationship with 1 document &#x60;document:2021-budget&#x60;. Note that this API, unlike the List Objects API, does not evaluate the tuples in the store. The continuation token will be empty if there are no more tuples to query. ### Query for all stored relationship tuples that have a particular relation and object To query for all users that have &#x60;reader&#x60; relationship with &#x60;document:2021-budget&#x60;, call read API with body of  &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {      \&quot;object\&quot;: \&quot;document:2021-budget\&quot;,      \&quot;relation\&quot;: \&quot;reader\&quot;    } } &#x60;&#x60;&#x60; The API will return something like  &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;user:bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ],   \&quot;continuation_token\&quot;: \&quot;eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ&#x3D;&#x3D;\&quot; } &#x60;&#x60;&#x60; This means that &#x60;document:2021-budget&#x60; has 1 &#x60;reader&#x60; (&#x60;user:bob&#x60;).  Note that, even if the model said that all &#x60;writers&#x60; are also &#x60;readers&#x60;, the API will not return writers such as &#x60;user:anne&#x60; because it only returns tuples and does not evaluate them. ### Query for all users with all relationships for a particular document To query for all users that have any relationship with &#x60;document:2021-budget&#x60;, call read API with body of  &#x60;&#x60;&#x60;json {   \&quot;tuple_key\&quot;: {       \&quot;object\&quot;: \&quot;document:2021-budget\&quot;    } } &#x60;&#x60;&#x60; The API will return something like  &#x60;&#x60;&#x60;json {   \&quot;tuples\&quot;: [     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;user:anne\&quot;,         \&quot;relation\&quot;: \&quot;writer\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-05T13:42:12.356Z\&quot;     },     {       \&quot;key\&quot;: {         \&quot;user\&quot;: \&quot;user:bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       },       \&quot;timestamp\&quot;: \&quot;2021-10-06T15:32:11.128Z\&quot;     }   ],   \&quot;continuation_token\&quot;: \&quot;eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ&#x3D;&#x3D;\&quot; } &#x60;&#x60;&#x60; This means that &#x60;document:2021-budget&#x60; has 1 &#x60;reader&#x60; (&#x60;user:bob&#x60;) and 1 &#x60;writer&#x60; (&#x60;user:anne&#x60;). 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadResponse</returns>
    public async Task<ReadResponse> Read(string storeId, ReadRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("Read", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<ReadRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/read",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<ReadRequest, ReadResponse>(requestBuilder,
            "Read", options, cancellationToken);
    }

    /// <summary>
    /// Read assertions for an authorization model ID The ReadAssertions API will return, for a given authorization model id, all the assertions stored for it. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="authorizationModelId"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAssertionsResponse</returns>
    public async Task<ReadAssertionsResponse> ReadAssertions(string storeId, string authorizationModelId, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ReadAssertions", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        if (authorizationModelId != null) {
            pathParams.Add("authorization_model_id", authorizationModelId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/assertions/{authorization_model_id}",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, ReadAssertionsResponse>(requestBuilder,
            "ReadAssertions", options, cancellationToken);
    }

    /// <summary>
    /// Return a particular version of an authorization model The ReadAuthorizationModel API returns an authorization model by its identifier. The response will return the authorization model for the particular version.  ## Example To retrieve the authorization model with ID &#x60;01G5JAVJ41T49E9TT3SKVS7X1J&#x60; for the store, call the GET authorization-models by ID API with &#x60;01G5JAVJ41T49E9TT3SKVS7X1J&#x60; as the &#x60;id&#x60; path parameter.  The API will return: &#x60;&#x60;&#x60;json {   \&quot;authorization_model\&quot;:{     \&quot;id\&quot;:\&quot;01G5JAVJ41T49E9TT3SKVS7X1J\&quot;,     \&quot;type_definitions\&quot;:[       {         \&quot;type\&quot;:\&quot;user\&quot;       },       {         \&quot;type\&quot;:\&quot;document\&quot;,         \&quot;relations\&quot;:{           \&quot;reader\&quot;:{             \&quot;union\&quot;:{               \&quot;child\&quot;:[                 {                   \&quot;this\&quot;:{}                 },                 {                   \&quot;computedUserset\&quot;:{                     \&quot;object\&quot;:\&quot;\&quot;,                     \&quot;relation\&quot;:\&quot;writer\&quot;                   }                 }               ]             }           },           \&quot;writer\&quot;:{             \&quot;this\&quot;:{}           }         }       }     ]   } } &#x60;&#x60;&#x60; In the above example, there are 2 types (&#x60;user&#x60; and &#x60;document&#x60;). The &#x60;document&#x60; type has 2 relations (&#x60;writer&#x60; and &#x60;reader&#x60;).
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="id"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAuthorizationModelResponse</returns>
    public async Task<ReadAuthorizationModelResponse> ReadAuthorizationModel(string storeId, string id, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ReadAuthorizationModel", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        if (id != null) {
            pathParams.Add("id", id.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models/{id}",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, ReadAuthorizationModelResponse>(requestBuilder,
            "ReadAuthorizationModel", options, cancellationToken);
    }

    /// <summary>
    /// Return all the authorization models for a particular store The ReadAuthorizationModels API will return all the authorization models for a certain store. OpenFGA&#39;s response will contain an array of all authorization models, sorted in descending order of creation.  ## Example Assume that a store&#39;s authorization model has been configured twice. To get all the authorization models that have been created in this store, call GET authorization-models. The API will return a response that looks like: &#x60;&#x60;&#x60;json {   \&quot;authorization_models\&quot;: [     {       \&quot;id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;,       \&quot;type_definitions\&quot;: [...]     },     {       \&quot;id\&quot;: \&quot;01G4ZW8F4A07AKQ8RHSVG9RW04\&quot;,       \&quot;type_definitions\&quot;: [...]     },   ],   \&quot;continuation_token\&quot;: \&quot;eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ&#x3D;&#x3D;\&quot; } &#x60;&#x60;&#x60; If there are no more authorization models available, the &#x60;continuation_token&#x60; field will be empty &#x60;&#x60;&#x60;json {   \&quot;authorization_models\&quot;: [     {       \&quot;id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;,       \&quot;type_definitions\&quot;: [...]     },     {       \&quot;id\&quot;: \&quot;01G4ZW8F4A07AKQ8RHSVG9RW04\&quot;,       \&quot;type_definitions\&quot;: [...]     },   ],   \&quot;continuation_token\&quot;: \&quot;\&quot; } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadAuthorizationModelsResponse</returns>
    public async Task<ReadAuthorizationModelsResponse> ReadAuthorizationModels(string storeId, int? pageSize = default(int?), string? continuationToken = default(string?), IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ReadAuthorizationModels", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();
        if (pageSize != null) {
            queryParams.Add("page_size", pageSize.ToString());
        }
        if (continuationToken != null) {
            queryParams.Add("continuation_token", continuationToken.ToString());
        }

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, ReadAuthorizationModelsResponse>(requestBuilder,
            "ReadAuthorizationModels", options, cancellationToken);
    }

    /// <summary>
    /// Return a list of all the tuple changes The ReadChanges API will return a paginated list of tuple changes (additions and deletions) that occurred in a given store, sorted by ascending time. The response will include a continuation token that is used to get the next set of changes. If there are no changes after the provided continuation token, the same token will be returned in order for it to be used when new changes are recorded. If the store never had any tuples added or removed, this token will be empty. You can use the &#x60;type&#x60; parameter to only get the list of tuple changes that affect objects of that type. When reading a write tuple change, if it was conditioned, the condition will be returned. When reading a delete tuple change, the condition will NOT be returned regardless of whether it was originally conditioned or not. 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="type"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="continuationToken"> (optional)</param>
    /// <param name="startTime">Start date and time of changes to read. Format: ISO 8601 timestamp (e.g., 2022-01-01T00:00:00Z) If a continuation_token is provided along side start_time, the continuation_token will take precedence over start_time. (optional)</param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of ReadChangesResponse</returns>
    public async Task<ReadChangesResponse> ReadChanges(string storeId, string? type = default(string?), int? pageSize = default(int?), string? continuationToken = default(string?), DateTime? startTime = default(DateTime?), IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("ReadChanges", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
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
        if (startTime != null) {
            // call the specific date time stringify
            queryParams.Add("start_time", startTime?.ToString("O"));
        }

        var requestBuilder = new RequestBuilder<Any> {
            Method = new HttpMethod("GET"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/changes",
            PathParameters = pathParams,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<Any, ReadChangesResponse>(requestBuilder,
            "ReadChanges", options, cancellationToken);
    }

    /// <summary>
    /// Add or delete tuples from the store The Write API will transactionally update the tuples for a certain store. Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. In the body, &#x60;writes&#x60; adds new tuples and &#x60;deletes&#x60; removes existing tuples. When deleting a tuple, any &#x60;condition&#x60; specified with it is ignored. The API is not idempotent by default: if, later on, you try to add the same tuple key (even if the &#x60;condition&#x60; is different), or if you try to delete a non-existing tuple, it will throw an error. To allow writes when an identical tuple already exists in the database, set &#x60;\&quot;on_duplicate\&quot;: \&quot;ignore\&quot;&#x60; on the &#x60;writes&#x60; object. To allow deletes when a tuple was already removed from the database, set &#x60;\&quot;on_missing\&quot;: \&quot;ignore\&quot;&#x60; on the &#x60;deletes&#x60; object. If a Write request contains both idempotent (ignore) and non-idempotent (error) operations, the most restrictive action (error) will take precedence. If a condition fails for a sub-request with an error flag, the entire transaction will be rolled back. This gives developers explicit control over the atomicity of the requests. The API will not allow you to write tuples such as &#x60;document:2021-budget#viewer@document:2021-budget#viewer&#x60;, because they are implicit. An &#x60;authorization_model_id&#x60; may be specified in the body. If it is, it will be used to assert that each written tuple (not deleted) is valid for the model specified. If it is not specified, the latest authorization model ID will be used. ## Example ### Adding relationships To add &#x60;user:anne&#x60; as a &#x60;writer&#x60; for &#x60;document:2021-budget&#x60;, call write API with the following  &#x60;&#x60;&#x60;json {   \&quot;writes\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;user:anne\&quot;,         \&quot;relation\&quot;: \&quot;writer\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       }     ],     \&quot;on_duplicate\&quot;: \&quot;ignore\&quot;   },   \&quot;authorization_model_id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot; } &#x60;&#x60;&#x60; ### Removing relationships To remove &#x60;user:bob&#x60; as a &#x60;reader&#x60; for &#x60;document:2021-budget&#x60;, call write API with the following  &#x60;&#x60;&#x60;json {   \&quot;deletes\&quot;: {     \&quot;tuple_keys\&quot;: [       {         \&quot;user\&quot;: \&quot;user:bob\&quot;,         \&quot;relation\&quot;: \&quot;reader\&quot;,         \&quot;object\&quot;: \&quot;document:2021-budget\&quot;       }     ],     \&quot;on_missing\&quot;: \&quot;ignore\&quot;   } } &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of Object</returns>
    public async Task<Object> Write(string storeId, WriteRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("Write", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<WriteRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/write",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<WriteRequest, Object>(requestBuilder,
            "Write", options, cancellationToken);
    }

    /// <summary>
    /// Upsert assertions for an authorization model ID The WriteAssertions API will upsert new assertions for an authorization model id, or overwrite the existing ones. An assertion is an object that contains a tuple key, the expectation of whether a call to the Check API of that tuple key will return true or false, and optionally a list of contextual tuples.
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="authorizationModelId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of void</returns>
    public async Task WriteAssertions(string storeId, string authorizationModelId, WriteAssertionsRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("WriteAssertions", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        if (authorizationModelId != null) {
            pathParams.Add("authorization_model_id", authorizationModelId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<WriteAssertionsRequest> {
            Method = new HttpMethod("PUT"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/assertions/{authorization_model_id}",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        await _apiClient.SendRequestAsync(requestBuilder,
            "WriteAssertions", options, cancellationToken);
    }

    /// <summary>
    /// Create a new authorization model The WriteAuthorizationModel API will add a new authorization model to a store. Each item in the &#x60;type_definitions&#x60; array is a type definition as specified in the field &#x60;type_definition&#x60;. The response will return the authorization model&#39;s ID in the &#x60;id&#x60; field.  ## Example To add an authorization model with &#x60;user&#x60; and &#x60;document&#x60; type definitions, call POST authorization-models API with the body:  &#x60;&#x60;&#x60;json {   \&quot;type_definitions\&quot;:[     {       \&quot;type\&quot;:\&quot;user\&quot;     },     {       \&quot;type\&quot;:\&quot;document\&quot;,       \&quot;relations\&quot;:{         \&quot;reader\&quot;:{           \&quot;union\&quot;:{             \&quot;child\&quot;:[               {                 \&quot;this\&quot;:{}               },               {                 \&quot;computedUserset\&quot;:{                   \&quot;object\&quot;:\&quot;\&quot;,                   \&quot;relation\&quot;:\&quot;writer\&quot;                 }               }             ]           }         },         \&quot;writer\&quot;:{           \&quot;this\&quot;:{}         }       }     }   ] } &#x60;&#x60;&#x60; OpenFGA&#39;s response will include the version id for this authorization model, which will look like  &#x60;&#x60;&#x60; {\&quot;authorization_model_id\&quot;: \&quot;01G50QVV17PECNVAHX1GG4Y5NC\&quot;} &#x60;&#x60;&#x60; 
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <param name="body"></param>
    /// <param name="options">Request options.</param>
    /// <param name="cancellationToken">Cancellation Token to cancel the request.</param>
    /// <returns>Task of WriteAuthorizationModelResponse</returns>
    public async Task<WriteAuthorizationModelResponse> WriteAuthorizationModel(string storeId, WriteAuthorizationModelRequest body, IRequestOptions? options = null, CancellationToken cancellationToken = default) {
        var pathParams = new Dictionary<string, string> { };
        if (string.IsNullOrWhiteSpace(storeId)) {
            throw new FgaRequiredParamError("WriteAuthorizationModel", "StoreId");
        }

        if (storeId != null) {
            pathParams.Add("store_id", storeId.ToString());
        }
        var queryParams = new Dictionary<string, string>();

        var requestBuilder = new RequestBuilder<WriteAuthorizationModelRequest> {
            Method = new HttpMethod("POST"),
            BasePath = _configuration.BasePath,
            PathTemplate = "/stores/{store_id}/authorization-models",
            PathParameters = pathParams,
            Body = body,
            QueryParameters = queryParams,
        };

        return await _apiClient.SendRequestAsync<WriteAuthorizationModelRequest, WriteAuthorizationModelResponse>(requestBuilder,
            "WriteAuthorizationModel", options, cancellationToken);
    }


    public void Dispose() {
        _apiClient.Dispose();
    }
}