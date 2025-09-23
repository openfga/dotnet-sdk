# OpenFga.Sdk.Model.WriteRequestWrites

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**TupleKeys** | [**List&lt;TupleKey&gt;**](TupleKey.md) |  | 
**OnDuplicate** | **string** | On &#39;error&#39; ( or unspecified ), the API returns an error if an identical tuple already exists. On &#39;ignore&#39;, identical writes are treated as no-ops (matching on user, relation, object, and RelationshipCondition). | [optional] [default to OnDuplicateEnum.Error]

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

