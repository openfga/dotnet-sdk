# OpenFga.Sdk.Model.ListUsersRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**AuthorizationModelId** | **string** |  | [optional] 
**Object** | [**FgaObject**](FgaObject.md) |  | 
**Relation** | **string** |  | 
**UserFilters** | [**List&lt;UserTypeFilter&gt;**](UserTypeFilter.md) | The type of results returned. Only accepts exactly one value. | 
**ContextualTuples** | [**List&lt;TupleKey&gt;**](TupleKey.md) |  | [optional] 
**Context** | **Object** | Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation. | [optional] 
**Consistency** | **ConsistencyPreference** |  | [optional] 

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

