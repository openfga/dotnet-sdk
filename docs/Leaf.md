# OpenFga.Sdk.Model.Leaf
A leaf node contains either - a set of users (which may be individual users, or usersets   referencing other relations) - a computed node, which is the result of a computed userset   value in the authorization model - a tupleToUserset nodes, containing the result of expanding   a tupleToUserset value in a authorization model.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Users** | [**Users**](Users.md) |  | [optional] 
**Computed** | [**Computed**](Computed.md) |  | [optional] 
**TupleToUserset** | [**UsersetTreeTupleToUserset**](UsersetTreeTupleToUserset.md) |  | [optional] 

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

