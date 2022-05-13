# OpenFga.Sdk.Api.OpenFgaApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**Check**](OpenFgaApi.md#check) | **POST** /stores/{store_id}/check | Check whether a user is authorized to access an object
[**Expand**](OpenFgaApi.md#expand) | **POST** /stores/{store_id}/expand | Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship
[**Read**](OpenFgaApi.md#read) | **POST** /stores/{store_id}/read | Get tuples from the store that matches a query, without following userset rewrite rules
[**ReadAssertions**](OpenFgaApi.md#readassertions) | **GET** /stores/{store_id}/assertions/{authorization_model_id} | Read assertions for an authorization model ID
[**ReadAuthorizationModel**](OpenFgaApi.md#readauthorizationmodel) | **GET** /stores/{store_id}/authorization-models/{id} | Return a particular version of an authorization model
[**ReadAuthorizationModels**](OpenFgaApi.md#readauthorizationmodels) | **GET** /stores/{store_id}/authorization-models | Return all the authorization model IDs for a particular store
[**ReadChanges**](OpenFgaApi.md#readchanges) | **GET** /stores/{store_id}/changes | Return a list of all the tuple changes
[**Write**](OpenFgaApi.md#write) | **POST** /stores/{store_id}/write | Add or delete tuples from the store
[**WriteAssertions**](OpenFgaApi.md#writeassertions) | **PUT** /stores/{store_id}/assertions/{authorization_model_id} | Upsert assertions for an authorization model ID
[**WriteAuthorizationModel**](OpenFgaApi.md#writeauthorizationmodel) | **POST** /stores/{store_id}/authorization-models | Create a new authorization model


<a name="check"></a>
# **Check**
> CheckResponse Check (CheckRequestParams _params)

Check whether a user is authorized to access an object

The Check API queries to check if the user has a certain relationship with an object in a certain store. Path parameter `store_id` as well as the body parameter `tuple_key` with specified `object`, `relation` and `user` subfields are all required. Optionally, a `contextual_tuples` object may also be included in the body of the request. This object contains one field `tuple_keys`, which is an array of tuple keys. The response will return whether the relationship exists in the field `allowed`.  ## Example In order to check if user `anne` has a `can_read` relationship with object `document:2021-budget` given the following contextual tuple ```json {   \"user\": \"anne\",   \"relation\": \"member\",   \"object\": \"time_slot:office_hours\" } ``` a check API call should be fired with the following body: ```json {   \"tuple_key\": {     \"user\": \"anne\",     \"relation\": \"can_read\",     \"object\": \"document:2021-budget\"   },   \"contextual_tuples\": {     \"tuple_keys\": [       {         \"user\": \"anne\",         \"relation\": \"member\",         \"object\": \"time_slot:office_hours\"       }     ]   } } ``` OpenFGA's response will include `{ \"allowed\": true }` if there is a relationship and `{ \"allowed\": false }` if there isn't.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class CheckExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var _params = new CheckRequestParams(); // CheckRequestParams | 

            try
            {
                // Check whether a user is authorized to access an object
                CheckResponse response = await openFgaApi.Check(_params);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.Check: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **_params** | [**CheckRequestParams**](CheckRequestParams.md)|  | 

### Return type

[**CheckResponse**](CheckResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="expand"></a>
# **Expand**
> ExpandResponse Expand (ExpandRequestParams _params)

Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship

The Expand API will return all users (including user and userset) that have certain relationship with an object in a certain store. This is different from the `/stores/{store_id}/read` API in that both users and computed references are returned. Path parameter `store_id` as well as body parameter `object`, `relation` are all required. The response will return a userset tree whose leaves are the user id and usersets.  Union, intersection and difference operator are located in the intermediate nodes.  ## Example Assume the following type definition for document: ```yaml   type document     relations       define reader as self or writer       define writer as self ``` In order to expand all users that have `reader` relationship with object `document:2021-budget`, an expand API call should be fired with the following body ```json {   \"tuple_key\": {     \"object\": \"document:2021-budget\",     \"relation\": \"reader\"   } } ``` OpenFGA's response will be a userset tree of the users and computed usersets that have read access to the document. ```json {   \"tree\":{     \"root\":{       \"type\":\"document:2021-budget#reader\",       \"union\":{         \"nodes\":[           {             \"type\":\"document:2021-budget#reader\",             \"leaf\":{               \"users\":{                 \"users\":[                   \"bob\"                 ]               }             }           },           {             \"type\":\"document:2021-budget#reader\",             \"leaf\":{               \"computed\":{                 \"userset\":\"document:2021-budget#writer\"               }             }           }         ]       }     }   } } ``` The caller can then call expand API for the `writer` relationship for the `document:2021-budget`.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ExpandExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var _params = new ExpandRequestParams(); // ExpandRequestParams | 

            try
            {
                // Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship
                ExpandResponse response = await openFgaApi.Expand(_params);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.Expand: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **_params** | [**ExpandRequestParams**](ExpandRequestParams.md)|  | 

### Return type

[**ExpandResponse**](ExpandResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="read"></a>
# **Read**
> ReadResponse Read (ReadRequestParams _params)

Get tuples from the store that matches a query, without following userset rewrite rules

The POST read API will return the tuples for a certain store that matches a query filter specified in the body. Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. It is different from the `/stores/{store_id}/expand` API in that only read returns relationship tuples that are stored in the system and satisfy the query. It does not expand or traverse the graph by taking the authorization model into account.Path parameter `store_id` is required.  In the body: 1. Object is mandatory. An object can be a full object (e.g., `type:object_id`) or type only (e.g., `type:`). 2. User is mandatory in the case the object is type only. ## Examples ### Query for all objects in a type definition To query for all objects that `bob` has `reader` relationship in the document type definition, call read API with body of ```json {  \"tuple_key\": {      \"user\": \"bob\",      \"relation\": \"reader\",      \"object\": \"document:\"   } } ``` The API will return tuples and an optional continuation token, something like ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `bob` has a `reader` relationship with 1 document `document:2021-budget`. ### Query for all users with particular relationships for a particular document To query for all users that have `reader` relationship with `document:2021-budget`, call read API with body of  ```json {   \"tuple_key\": {      \"object\": \"document:2021-budget\",      \"relation\": \"reader\"    } } ``` The API will return something like  ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `document:2021-budget` has 1 `reader` (`bob`).  Note that the API will not return writers such as `anne` even when all writers are readers.  This is because only direct relationship are returned for the READ API. ### Query for all users with all relationships for a particular document To query for all users that have any relationship with `document:2021-budget`, call read API with body of  ```json {   \"tuple_key\": {       \"object\": \"document:2021-budget\"    } } ``` The API will return something like  ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"anne\",         \"relation\": \"writer\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-05T13:42:12.356Z\"     },     {       \"key\": {         \"user\": \"bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `document:2021-budget` has 1 `reader` (`bob`) and 1 `writer` (`anne`). 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ReadExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var _params = new ReadRequestParams(); // ReadRequestParams | 

            try
            {
                // Get tuples from the store that matches a query, without following userset rewrite rules
                ReadResponse response = await openFgaApi.Read(_params);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.Read: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **_params** | [**ReadRequestParams**](ReadRequestParams.md)|  | 

### Return type

[**ReadResponse**](ReadResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readassertions"></a>
# **ReadAssertions**
> ReadAssertionsResponse ReadAssertions (string authorizationModelId)

Read assertions for an authorization model ID

The GET assertions API will return, for a given authorization model id, all the assertions stored for it. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ReadAssertionsExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var authorizationModelId = "authorizationModelId_example";  // string | 

            try
            {
                // Read assertions for an authorization model ID
                ReadAssertionsResponse response = await openFgaApi.ReadAssertions(authorizationModelId);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ReadAssertions: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **authorizationModelId** | **string**|  | 

### Return type

[**ReadAssertionsResponse**](ReadAssertionsResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readauthorizationmodel"></a>
# **ReadAuthorizationModel**
> ReadAuthorizationModelResponse ReadAuthorizationModel (string id)

Return a particular version of an authorization model

The GET authorization-models by ID API will return a particular version of authorization model that had been configured for a certain store.   Path parameter `store_id` and `id` are required. The response will return the authorization model for the particular version.  ## Example To retrieve the authorization model with ID `1yunpF9DkzXMzm0dHrsCuWsooEV` for the store, call the GET authorization-models by ID API with `1yunpF9DkzXMzm0dHrsCuWsooEV` as the `id` path parameter.  The API will return: ```json {   \"authorization_model\":{     \"id\":\"1yunpF9DkzXMzm0dHrsCuWsooEV\",     \"type_definitions\":[       {         \"type\":\"document\",         \"relations\":{           \"reader\":{             \"union\":{               \"child\":[                 {                   \"this\":{}                 },                 {                   \"computedUserset\":{                     \"object\":\"\",                     \"relation\":\"writer\"                   }                 }               ]             }           },           \"writer\":{             \"this\":{}           }         }       }     ]   } } ``` In the above example, there is only 1 type (`document`) with 2 relations (`writer` and `reader`).

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ReadAuthorizationModelExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var id = "id_example";  // string | 

            try
            {
                // Return a particular version of an authorization model
                ReadAuthorizationModelResponse response = await openFgaApi.ReadAuthorizationModel(id);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ReadAuthorizationModel: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **id** | **string**|  | 

### Return type

[**ReadAuthorizationModelResponse**](ReadAuthorizationModelResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readauthorizationmodels"></a>
# **ReadAuthorizationModels**
> ReadAuthorizationModelsResponse ReadAuthorizationModels (int? pageSize = null, string? continuationToken = null)

Return all the authorization model IDs for a particular store

The GET authorization-models API will return all the IDs of the authorization models for a certain store. Path parameter `store_id` is required. OpenFGA's response will contain an array of all authorization model IDs, sorted in descending order of creation.  ## Example Assume that the store's authorization model has been configured twice.  To get all the IDs of the authorization models that had been created in this store, call GET authorization-models.  The API will return a response that looks like: ```json {   \"authorization_model_ids\": [       \"1yunpF9DkzXMzm0dHrsCuWsooEV\",       \"1yundoHpJHlodgn4EOVar2DhmKp\"   ] } ``` If there are more authorization model IDs available, the response will contain an extra field `continuation_token`: ```json {   \"authorization_model_ids\": [       \"1yunpF9DkzXMzm0dHrsCuWsooEV\",       \"1yundoHpJHlodgn4EOVar2DhmKp\"   ],   \"continuation_token\": \"eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==\" } ``` 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ReadAuthorizationModelsExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var pageSize = 56;  // int? |  (optional) 
            var continuationToken = "continuationToken_example";  // string? |  (optional) 

            try
            {
                // Return all the authorization model IDs for a particular store
                ReadAuthorizationModelsResponse response = await openFgaApi.ReadAuthorizationModels(pageSize, continuationToken);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ReadAuthorizationModels: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **pageSize** | **int?**|  | [optional] 
 **continuationToken** | **string?**|  | [optional] 

### Return type

[**ReadAuthorizationModelsResponse**](ReadAuthorizationModelsResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readchanges"></a>
# **ReadChanges**
> ReadChangesResponse ReadChanges (string? type = null, int? pageSize = null, string? continuationToken = null)

Return a list of all the tuple changes

The GET changes API will return a paginated list of tuple changes (additions and deletions) that occurred in a given store, sorted by ascending time. The response will include a continuation token that is used to get the next set of changes. If there are no changes after the provided continuation token, the same token will be returned in order for it to be used when new changes are recorded. If the store never had any tuples added or removed, this token will be empty. You can use the `type` parameter to only get the list of tuple changes that affect objects of that type. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class ReadChangesExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var type = "type_example";  // string? |  (optional) 
            var pageSize = 56;  // int? |  (optional) 
            var continuationToken = "continuationToken_example";  // string? |  (optional) 

            try
            {
                // Return a list of all the tuple changes
                ReadChangesResponse response = await openFgaApi.ReadChanges(type, pageSize, continuationToken);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ReadChanges: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **type** | **string?**|  | [optional] 
 **pageSize** | **int?**|  | [optional] 
 **continuationToken** | **string?**|  | [optional] 

### Return type

[**ReadChangesResponse**](ReadChangesResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="write"></a>
# **Write**
> Object Write (WriteRequestParams _params)

Add or delete tuples from the store

The POST write API will update the tuples for a certain store.  Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. Path parameter `store_id` is required.  In the body, `writes` adds new tuples while `deletes` removes existing tuples. ## Example ### Adding relationships To add `anne` as a `writer` for `document:2021-budget`, call write API with the following  ```json {   \"writes\": {     \"tuple_keys\": [       {         \"user\": \"anne\",         \"relation\": \"writer\",         \"object\": \"document:2021-budget\"       }     ]   } } ``` ### Removing relationships To remove `bob` as a `reader` for `document:2021-budget`, call write API with the following  ```json {   \"deletes\": {     \"tuple_keys\": [       {         \"user\": \"bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       }     ]   } } ``` 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class WriteExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var _params = new WriteRequestParams(); // WriteRequestParams | 

            try
            {
                // Add or delete tuples from the store
                Object response = await openFgaApi.Write(_params);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.Write: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **_params** | [**WriteRequestParams**](WriteRequestParams.md)|  | 

### Return type

**Object**

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="writeassertions"></a>
# **WriteAssertions**
> Object WriteAssertions (string authorizationModelId, WriteAssertionsRequestParams _params)

Upsert assertions for an authorization model ID

The POST assertions API will add new assertions for an authorization model id, or overwrite the existing ones. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class WriteAssertionsExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var authorizationModelId = "authorizationModelId_example";  // string | 
            var _params = new WriteAssertionsRequestParams(); // WriteAssertionsRequestParams | 

            try
            {
                // Upsert assertions for an authorization model ID
                Object response = await openFgaApi.WriteAssertions(authorizationModelId, _params);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.WriteAssertions: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **authorizationModelId** | **string**|  | 
 **_params** | [**WriteAssertionsRequestParams**](WriteAssertionsRequestParams.md)|  | 

### Return type

**Object**

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **204** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="writeauthorizationmodel"></a>
# **WriteAuthorizationModel**
> WriteAuthorizationModelResponse WriteAuthorizationModel (TypeDefinitions typeDefinitions)

Create a new authorization model

The POST authorization-model API will update the authorization model for a certain store. Path parameter `store_id` and `type_definitions` array in the body are required.  Each item in the `type_definitions` array is a type definition as specified in the field `type_definition`. The response will return the authorization model's ID in the `id` field.  ## Example To update the authorization model with a single `document` authorization model, call POST authorization-models API with the body:  ```json {   \"type_definitions\":[     {       \"type\":\"document\",       \"relations\":{         \"reader\":{           \"union\":{             \"child\":[               {                 \"this\":{                  }               },               {                 \"computedUserset\":{                   \"object\":\"\",                   \"relation\":\"writer\"                 }               }             ]           }         },         \"writer\":{           \"this\":{            }         }       }     }   ] } ``` OpenFGA's response will include the version id for this authorization model, which will look like  ``` {\"authorization_model_id\": \"1yunpF9DkzXMzm0dHrsCuWsooEV\"} ``` 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using OpenFga.Sdk.Api;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace Example
{
    public class WriteAuthorizationModelExample
    {
        public static void Main()
        {
            var configuration = new Configuration(storeId, environment) {
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"),
                Host = Environment.GetEnvironmentVariable("OPENFGA_HOST"),
                ApiTokenIssuer = Environment.GetEnvironmentVariable("OPENFGA_API_TOKEN_ISSUER"), // optional, required if client id is passed
                ApiAudience = Environment.GetEnvironmentVariable("OPENFGA_API_AUDIENCE"), // optional, required if client id is passed
                ClientId = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_ID"), // optional
                ClientSecret = Environment.GetEnvironmentVariable("OPENFGA_CLIENT_SECRET"), // optional, required if client id is passed
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var typeDefinitions = new TypeDefinitions(); // TypeDefinitions | 

            try
            {
                // Create a new authorization model
                WriteAuthorizationModelResponse response = await openFgaApi.WriteAuthorizationModel(typeDefinitions);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.WriteAuthorizationModel: " + e.Message );
                Debug.Print("Status Code: "+ e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------

 **typeDefinitions** | [**TypeDefinitions**](TypeDefinitions.md)|  | 

### Return type

[**WriteAuthorizationModelResponse**](WriteAuthorizationModelResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **201** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **401** | Request failed due to authentication errors. |  -  |
| **403** | Request failed due to forbidden permission. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **429** | Request failed due to too many requests. |  -  |
| **500** | Request failed due to internal server error. |  -  |
| **0** | An unexpected error response. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

