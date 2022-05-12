# OpenFga.Sdk.Model.AuthErrorCode
- no_auth_error: no error  - auth_failure: generic authentication error. Details available via message field.  - auth_failed_invalid_subject: authentication failure due to invalid subject.  - auth_failed_invalid_audience: authentication failure due to invalid audience.  - auth_failed_invalid_issuer: authentication failure due to invalid issuer.  - invalid_claims: authentication failure due to invalid claims.  - auth_failed_invalid_bearer_token: authentication failure due to invalid bearer token.  - missing_customer_in_bearer_token: authentication failure with customer id missing in bearer token.  - missing_store_in_bearer_token: authentication failure with store id missing in bearer token.  - store_mismatch_in_bearer_token: authentication failure due to store mismatch from bearer token.  - customer_mismatch_in_bearer_token: authentication failure due to customer id in the request being different from the customer id in the bearer token.  - bearer_token_missing: bearer token missing in request.  - unauthenticated: unauthenticated.  - insufficient_permissions: insufficient permissions.  - unauthorized_principal: authentication denial due to unauthorized principal.

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------

[[Back to Model list]](../README.md#models) [[Back to API list]](../README.md#api-endpoints) [[Back to README]](../README.md)

