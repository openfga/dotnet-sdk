# OpenFga.Sdk.Model.CheckRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**TupleKey** | [**CheckRequestTupleKey**](CheckRequestTupleKey.md) |  | 
**ContextualTuples** | [**ContextualTupleKeys**](ContextualTupleKeys.md) |  | [optional] 
**AuthorizationModelId** | **string** |  | [optional] 
**Trace** | **bool** | Defaults to false. Making it true has performance implications. | [optional] [readonly] 
**Context** | **Object** | Additional request context that will be used to evaluate any ABAC conditions encountered in the query evaluation. | [optional] 

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

