# OpenFga.Sdk.Model.User
User.  Represents any possible value for a user (subject or principal). Can be a: - Specific user object e.g.: 'user:will', 'folder:marketing', 'org:contoso', ...) - Specific userset (e.g. 'group:engineering#member') - Public-typed wildcard (e.g. 'user:*')  See https://openfga.dev/docs/concepts#what-is-a-user

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Object** | [**FgaObject**](FgaObject.md) |  | [optional] 
**Userset** | [**UsersetUser**](UsersetUser.md) |  | [optional] 
**Wildcard** | [**TypedWildcard**](TypedWildcard.md) |  | [optional] 

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

