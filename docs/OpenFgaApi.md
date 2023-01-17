# OpenFga.Sdk.Api.OpenFgaApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**Check**](OpenFgaApi.md#check) | **POST** /stores/{store_id}/check | Check whether a user is authorized to access an object
[**CreateStore**](OpenFgaApi.md#createstore) | **POST** /stores | Create a store
[**DeleteStore**](OpenFgaApi.md#deletestore) | **DELETE** /stores/{store_id} | Delete a store
[**Expand**](OpenFgaApi.md#expand) | **POST** /stores/{store_id}/expand | Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship
[**GetStore**](OpenFgaApi.md#getstore) | **GET** /stores/{store_id} | Get a store
[**ListObjects**](OpenFgaApi.md#listobjects) | **POST** /stores/{store_id}/list-objects | [EXPERIMENTAL] Get all objects of the given type that the user has a relation with
[**ListStores**](OpenFgaApi.md#liststores) | **GET** /stores | List all stores
[**Read**](OpenFgaApi.md#read) | **POST** /stores/{store_id}/read | Get tuples from the store that matches a query, without following userset rewrite rules
[**ReadAssertions**](OpenFgaApi.md#readassertions) | **GET** /stores/{store_id}/assertions/{authorization_model_id} | Read assertions for an authorization model ID
[**ReadAuthorizationModel**](OpenFgaApi.md#readauthorizationmodel) | **GET** /stores/{store_id}/authorization-models/{id} | Return a particular version of an authorization model
[**ReadAuthorizationModels**](OpenFgaApi.md#readauthorizationmodels) | **GET** /stores/{store_id}/authorization-models | Return all the authorization models for a particular store
[**ReadChanges**](OpenFgaApi.md#readchanges) | **GET** /stores/{store_id}/changes | Return a list of all the tuple changes
[**Write**](OpenFgaApi.md#write) | **POST** /stores/{store_id}/write | Add or delete tuples from the store
[**WriteAssertions**](OpenFgaApi.md#writeassertions) | **PUT** /stores/{store_id}/assertions/{authorization_model_id} | Upsert assertions for an authorization model ID
[**WriteAuthorizationModel**](OpenFgaApi.md#writeauthorizationmodel) | **POST** /stores/{store_id}/authorization-models | Create a new authorization model


<a name="check"></a>
# **Check**
> CheckResponse Check (CheckRequest body)

Check whether a user is authorized to access an object

The Check API queries to check if the user has a certain relationship with an object in a certain store. A `contextual_tuples` object may also be included in the body of the request. This object contains one field `tuple_keys`, which is an array of tuple keys. You may also provide an `authorization_model_id` in the body. This will be used to assert that the input `tuple_key` is valid for the model specified. If not specified, the assertion will be made against the latest authorization model ID. The response will return whether the relationship exists in the field `allowed`.  ## Example In order to check if user `user:anne` of type `user` has a `reader` relationship with object `document:2021-budget` given the following contextual tuple ```json {   \"user\": \"user:anne\",   \"relation\": \"member\",   \"object\": \"time_slot:office_hours\" } ``` the Check API can be used with the following request body: ```json {   \"tuple_key\": {     \"user\": \"user:anne\",     \"relation\": \"reader\",     \"object\": \"document:2021-budget\"   },   \"contextual_tuples\": {     \"tuple_keys\": [       {         \"user\": \"user:anne\",         \"relation\": \"member\",         \"object\": \"time_slot:office_hours\"       }     ]   } } ``` OpenFGA's response will include `{ \"allowed\": true }` if there is a relationship and `{ \"allowed\": false }` if there isn't.

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new CheckRequest(); // CheckRequest | 

            try
            {
                // Check whether a user is authorized to access an object
                CheckResponse response = await openFgaApi.Check(body);
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

 **body** | [**CheckRequest**](CheckRequest.md)|  | 

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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="createstore"></a>
# **CreateStore**
> CreateStoreResponse CreateStore ()

Create a store

Create a unique OpenFGA store which will be used to store authorization models and relationship tuples.

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
    public class CreateStoreExample
    {
        public static void Main()
        {
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);

            try
            {
                // Create a store
                CreateStoreResponse response = await openFgaApi.CreateStore();
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.CreateStore: " + e.Message );
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


### Return type

[**CreateStoreResponse**](CreateStoreResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="deletestore"></a>
# **DeleteStore**
> void DeleteStore ()

Delete a store

Delete an OpenFGA store. This does not delete the data associated with the store, like tuples or authorization models.

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
    public class DeleteStoreExample
    {
        public static void Main()
        {
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);

            try
            {
                // Delete a store
                openFgaApi.DeleteStore();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.DeleteStore: " + e.Message );
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


### Return type

void (empty response body)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="expand"></a>
# **Expand**
> ExpandResponse Expand (ExpandRequest body)

Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship

The Expand API will return all users and usersets that have certain relationship with an object in a certain store. This is different from the `/stores/{store_id}/read` API in that both users and computed usersets are returned. Body parameters `tuple_key.object` and `tuple_key.relation` are all required. The response will return a tree whose leaves are the specific users and usersets. Union, intersection and difference operator are located in the intermediate nodes.  ## Example To expand all users that have the `reader` relationship with object `document:2021-budget`, use the Expand API with the following request body ```json {   \"tuple_key\": {     \"object\": \"document:2021-budget\",     \"relation\": \"reader\"   } } ``` OpenFGA's response will be a userset tree of the users and usersets that have read access to the document. ```json {   \"tree\":{     \"root\":{       \"type\":\"document:2021-budget#reader\",       \"union\":{         \"nodes\":[           {             \"type\":\"document:2021-budget#reader\",             \"leaf\":{               \"users\":{                 \"users\":[                   \"user:bob\"                 ]               }             }           },           {             \"type\":\"document:2021-budget#reader\",             \"leaf\":{               \"computed\":{                 \"userset\":\"document:2021-budget#writer\"               }             }           }         ]       }     }   } } ``` The caller can then call expand API for the `writer` relationship for the `document:2021-budget`.

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new ExpandRequest(); // ExpandRequest | 

            try
            {
                // Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship
                ExpandResponse response = await openFgaApi.Expand(body);
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

 **body** | [**ExpandRequest**](ExpandRequest.md)|  | 

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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="getstore"></a>
# **GetStore**
> GetStoreResponse GetStore ()

Get a store

Returns an OpenFGA store by its identifier

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
    public class GetStoreExample
    {
        public static void Main()
        {
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);

            try
            {
                // Get a store
                GetStoreResponse response = await openFgaApi.GetStore();
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.GetStore: " + e.Message );
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


### Return type

[**GetStoreResponse**](GetStoreResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="listobjects"></a>
# **ListObjects**
> ListObjectsResponse ListObjects (ListObjectsRequest body)

[EXPERIMENTAL] Get all objects of the given type that the user has a relation with

The ListObjects API returns a list of all the objects of the given type that the user has a relation with. To achieve this, both the store tuples and the authorization model are used. An `authorization_model_id` may be specified in the body. If it is, it will be used to decide the underlying implementation used. If it is not specified, the latest authorization model ID will be used. You may also specify `contextual_tuples` that will be treated as regular tuples. The response will contain the related objects in an array in the \"objects\" field of the response and they will be strings in the object format `<type>:<id>` (e.g. \"document:roadmap\")  

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
    public class ListObjectsExample
    {
        public static void Main()
        {
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new ListObjectsRequest(); // ListObjectsRequest | 

            try
            {
                // [EXPERIMENTAL] Get all objects of the given type that the user has a relation with
                ListObjectsResponse response = await openFgaApi.ListObjects(body);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ListObjects: " + e.Message );
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

 **body** | [**ListObjectsRequest**](ListObjectsRequest.md)|  | 

### Return type

[**ListObjectsResponse**](ListObjectsResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="liststores"></a>
# **ListStores**
> ListStoresResponse ListStores (string? continuationToken = null)

List all stores

Returns a paginated list of OpenFGA stores.

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
    public class ListStoresExample
    {
        public static void Main()
        {
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var continuationToken = "continuationToken_example";  // string? |  (optional) 

            try
            {
                // List all stores
                ListStoresResponse response = await openFgaApi.ListStores(continuationToken);
                Debug.WriteLine(response);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling OpenFgaApi.ListStores: " + e.Message );
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

 **continuationToken** | **string?**|  | [optional] 

### Return type

[**ListStoresResponse**](ListStoresResponse.md)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="read"></a>
# **Read**
> ReadResponse Read (ReadRequest body)

Get tuples from the store that matches a query, without following userset rewrite rules

The Read API will return the tuples for a certain store that match a query filter specified in the body of the request. It is different from the `/stores/{store_id}/expand` API in that it only returns relationship tuples that are stored in the system and satisfy the query.  In the body: 1. tuple_key is optional.  If tuple_key is not specified, it will return all tuples in the store.2. `tuple_key.object` is mandatory if tuple_key is specified. It can be a full object (e.g., `type:object_id`) or type only (e.g., `type:`). 3. `tuple_key.user` is mandatory if tuple_key is specified in the case the `tuple_key.object` is a type only. ## Examples ### Query for all objects in a type definition To query for all objects that `user:bob` has `reader` relationship in the document type definition, call read API with body of ```json {  \"tuple_key\": {      \"user\": \"user:bob\",      \"relation\": \"reader\",      \"object\": \"document:\"   } } ``` The API will return tuples and an optional continuation token, something like ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"user:bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `user:bob` has a `reader` relationship with 1 document `document:2021-budget`. ### Query for all stored relationship tuples that have a particular relation and object To query for all users that have `reader` relationship with `document:2021-budget`, call read API with body of  ```json {   \"tuple_key\": {      \"object\": \"document:2021-budget\",      \"relation\": \"reader\"    } } ``` The API will return something like  ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"user:bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `document:2021-budget` has 1 `reader` (`user:bob`).  Note that the API will not return writers such as `user:anne` even when all writers are readers.  This is because only direct relationship are returned for the READ API. ### Query for all users with all relationships for a particular document To query for all users that have any relationship with `document:2021-budget`, call read API with body of  ```json {   \"tuple_key\": {       \"object\": \"document:2021-budget\"    } } ``` The API will return something like  ```json {   \"tuples\": [     {       \"key\": {         \"user\": \"user:anne\",         \"relation\": \"writer\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-05T13:42:12.356Z\"     },     {       \"key\": {         \"user\": \"user:bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       },       \"timestamp\": \"2021-10-06T15:32:11.128Z\"     }   ] } ``` This means that `document:2021-budget` has 1 `reader` (`user:bob`) and 1 `writer` (`user:anne`). 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new ReadRequest(); // ReadRequest | 

            try
            {
                // Get tuples from the store that matches a query, without following userset rewrite rules
                ReadResponse response = await openFgaApi.Read(body);
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

 **body** | [**ReadRequest**](ReadRequest.md)|  | 

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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readassertions"></a>
# **ReadAssertions**
> ReadAssertionsResponse ReadAssertions (string authorizationModelId)

Read assertions for an authorization model ID

The ReadAssertions API will return, for a given authorization model id, all the assertions stored for it. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readauthorizationmodel"></a>
# **ReadAuthorizationModel**
> ReadAuthorizationModelResponse ReadAuthorizationModel (string id)

Return a particular version of an authorization model

The ReadAuthorizationModel API returns an authorization model by its identifier. The response will return the authorization model for the particular version.  ## Example To retrieve the authorization model with ID `01G5JAVJ41T49E9TT3SKVS7X1J` for the store, call the GET authorization-models by ID API with `01G5JAVJ41T49E9TT3SKVS7X1J` as the `id` path parameter.  The API will return: ```json {   \"authorization_model\":{     \"id\":\"01G5JAVJ41T49E9TT3SKVS7X1J\",     \"type_definitions\":[       {         \"type\":\"user\"       },       {         \"type\":\"document\",         \"relations\":{           \"reader\":{             \"union\":{               \"child\":[                 {                   \"this\":{}                 },                 {                   \"computedUserset\":{                     \"object\":\"\",                     \"relation\":\"writer\"                   }                 }               ]             }           },           \"writer\":{             \"this\":{}           }         }       }     ]   } } ``` In the above example, there are 2 types (`user` and `document`). The `document` type has 2 relations (`writer` and `reader`).

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readauthorizationmodels"></a>
# **ReadAuthorizationModels**
> ReadAuthorizationModelsResponse ReadAuthorizationModels (int? pageSize = null, string? continuationToken = null)

Return all the authorization models for a particular store

The ReadAuthorizationModels API will return all the authorization models for a certain store. OpenFGA's response will contain an array of all authorization models, sorted in descending order of creation.  ## Example Assume that a store's authorization model has been configured twice. To get all the authorization models that have been created in this store, call GET authorization-models. The API will return a response that looks like: ```json {   \"authorization_models\": [     {       \"id\": \"01G50QVV17PECNVAHX1GG4Y5NC\",       \"type_definitions\": [...]     },     {       \"id\": \"01G4ZW8F4A07AKQ8RHSVG9RW04\",       \"type_definitions\": [...]     },   ] } ``` If there are more authorization models available, the response will contain an extra field `continuation_token`: ```json {   \"authorization_models\": [     {       \"id\": \"01G50QVV17PECNVAHX1GG4Y5NC\",       \"type_definitions\": [...]     },     {       \"id\": \"01G4ZW8F4A07AKQ8RHSVG9RW04\",       \"type_definitions\": [...]     },   ],   \"continuation_token\": \"eyJwayI6IkxBVEVTVF9OU0NPTkZJR19hdXRoMHN0b3JlIiwic2siOiIxem1qbXF3MWZLZExTcUoyN01MdTdqTjh0cWgifQ==\" } ``` 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var pageSize = 56;  // int? |  (optional) 
            var continuationToken = "continuationToken_example";  // string? |  (optional) 

            try
            {
                // Return all the authorization models for a particular store
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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="readchanges"></a>
# **ReadChanges**
> ReadChangesResponse ReadChanges (string? type = null, int? pageSize = null, string? continuationToken = null)

Return a list of all the tuple changes

The ReadChanges API will return a paginated list of tuple changes (additions and deletions) that occurred in a given store, sorted by ascending time. The response will include a continuation token that is used to get the next set of changes. If there are no changes after the provided continuation token, the same token will be returned in order for it to be used when new changes are recorded. If the store never had any tuples added or removed, this token will be empty. You can use the `type` parameter to only get the list of tuple changes that affect objects of that type. 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="write"></a>
# **Write**
> Object Write (WriteRequest body)

Add or delete tuples from the store

The Write API will update the tuples for a certain store. Tuples and type definitions allow OpenFGA to determine whether a relationship exists between an object and an user. In the body, `writes` adds new tuples while `deletes` removes existing tuples. The API is not idempotent: if, later on, you try to add the same tuple, or if you try to delete a non-existing tuple, it will throw an error. An `authorization_model_id` may be specified in the body. If it is, it will be used to assert that each written tuple (not deleted) is valid for the model specified. If it is not specified, the latest authorization model ID will be used. ## Example ### Adding relationships To add `user:anne` as a `writer` for `document:2021-budget`, call write API with the following  ```json {   \"writes\": {     \"tuple_keys\": [       {         \"user\": \"user:anne\",         \"relation\": \"writer\",         \"object\": \"document:2021-budget\"       }     ]   } } ``` ### Removing relationships To remove `user:bob` as a `reader` for `document:2021-budget`, call write API with the following  ```json {   \"deletes\": {     \"tuple_keys\": [       {         \"user\": \"user:bob\",         \"relation\": \"reader\",         \"object\": \"document:2021-budget\"       }     ]   } } ``` 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new WriteRequest(); // WriteRequest | 

            try
            {
                // Add or delete tuples from the store
                Object response = await openFgaApi.Write(body);
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

 **body** | [**WriteRequest**](WriteRequest.md)|  | 

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
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="writeassertions"></a>
# **WriteAssertions**
> void WriteAssertions (string authorizationModelId, WriteAssertionsRequest body)

Upsert assertions for an authorization model ID

The WriteAssertions API will upsert new assertions for an authorization model id, or overwrite the existing ones. An assertion is an object that contains a tuple key, and the expectation of whether a call to the Check API of that tuple key will return true or false. 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var authorizationModelId = "authorizationModelId_example";  // string | 
            var body = new WriteAssertionsRequest(); // WriteAssertionsRequest | 

            try
            {
                // Upsert assertions for an authorization model ID
                openFgaApi.WriteAssertions(authorizationModelId, body);
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
 **body** | [**WriteAssertionsRequest**](WriteAssertionsRequest.md)|  | 

### Return type

void (empty response body)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

<a name="writeauthorizationmodel"></a>
# **WriteAuthorizationModel**
> WriteAuthorizationModelResponse WriteAuthorizationModel (WriteAuthorizationModelRequest body)

Create a new authorization model

The WriteAuthorizationModel API will add a new authorization model to a store. Each item in the `type_definitions` array is a type definition as specified in the field `type_definition`. The response will return the authorization model's ID in the `id` field.  ## Example To add an authorization model with `user` and `document` type definitions, call POST authorization-models API with the body:  ```json {   \"type_definitions\":[     {       \"type\":\"user\"     },     {       \"type\":\"document\",       \"relations\":{         \"reader\":{           \"union\":{             \"child\":[               {                 \"this\":{}               },               {                 \"computedUserset\":{                   \"object\":\"\",                   \"relation\":\"writer\"                 }               }             ]           }         },         \"writer\":{           \"this\":{}         }       }     }   ] } ``` OpenFGA's response will include the version id for this authorization model, which will look like  ``` {\"authorization_model_id\": \"01G50QVV17PECNVAHX1GG4Y5NC\"} ``` 

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
            var configuration = new Configuration() {
                ApiScheme = Environment.GetEnvironmentVariable("OPENFGA_API_SCHEME"), // optional, defaults to "https"
                ApiHost = Environment.GetEnvironmentVariable("OPENFGA_API_HOST"), // required, define without the scheme (e.g. api.fga.example instead of https://api.fga.example)
                StoreId = Environment.GetEnvironmentVariable("OPENFGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
            };
            HttpClient httpClient = new HttpClient();
            var openFgaApi = new OpenFgaApi(config, httpClient);
            var body = new WriteAuthorizationModelRequest(); // WriteAuthorizationModelRequest | 

            try
            {
                // Create a new authorization model
                WriteAuthorizationModelResponse response = await openFgaApi.WriteAuthorizationModel(body);
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

 **body** | [**WriteAuthorizationModelRequest**](WriteAuthorizationModelRequest.md)|  | 

### Return type

[**WriteAuthorizationModelResponse**](WriteAuthorizationModelResponse.md)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | A successful response. |  -  |
| **400** | Request failed due to invalid input. |  -  |
| **404** | Request failed due to incorrect path. |  -  |
| **500** | Request failed due to internal server error. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

