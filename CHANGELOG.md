# Changelog

## [Unreleased](https://github.com/openfga/dotnet-sdk/compare/v0.7.0...HEAD)

- feat!: add per-request custom headers support
  - `DefaultHeaders` support to `ClientConfiguration` for headers sent with every request
  - per-request headers support via `Headers` property on all client options classes
  - `IRequestOptions` interface and `RequestOptions` class for API-level header support
  - `IClientRequestOptions` interface and `ClientRequestOptions` class for client-level header support
  - add header validation to prevent overriding of reserved headers
- feat: add write conflict resolution options
  - `ConflictOptions` to control behavior for duplicate writes and missing deletes
  - `OnDuplicateWrites` option: `Error` (default) or `Ignore` for handling duplicate tuple writes
  - `OnMissingDeletes` option: `Error` (default) or `Ignore` for handling missing tuple deletes
  - Available in `ClientWriteOptions.Conflict` property

[!WARNING]
BREAKING CHANGES:

- **OpenFgaApi methods**: All API methods now accept an `IRequestOptions? options` parameter. If you are using the low-level `OpenFgaApi` directly, you may need to update your calls:

  Before:

  ```csharp
  await api.Check(storeId, body, cancellationToken);
  ```

  After:

  ```csharp
  var options = new RequestOptions {
      Headers = new Dictionary<string, string> { { "X-Custom-Header", "value" } }
  };
  await api.Check(storeId, body, options, cancellationToken);
  ```

- **ClientRequestOptions renamed**: The base client request options interface has been renamed from `ClientRequestOptions` to `IClientRequestOptions` to better follow .NET naming conventions. A concrete `ClientRequestOptions` class is now also available. If you were casting to or implementing this interface, update your code:

  Before:

  ```csharp
  var options = obj as ClientRequestOptions;
  ```

  After:

  ```csharp
  var options = obj as IClientRequestOptions;
  ```

Note: If you are using the high-level `OpenFgaClient`, no changes are required to your existing code. The new headers functionality is additive via the existing options parameters.

## v0.7.0

### [0.7.0](https://github.com/openfga/dotnet-sdk/compare/v0.6.0...v0.7.0) (2025-10-01)

- feat!: add support for .NET Standard 2.0, .NET 8.0 and .NET 9.0

[!WARNING]
BREAKING CHANGES:

- While we have dropped .NET 6.0 as a target framework, the SDK now supports .NET Standard 2.0 - which means it can still be used in .NET 6.0 projects.
- We have updated the underlying OpenAPI generator to a newer version, which has caused some changes in the generated code. Below is a summary of the changes:

| Old Name    | New Name            | Affected Models                                                                                                                       |
| ----------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| \_Nodes     | VarNodes            | Nodes (and related constructors/references)                                                                                           |
| \_Users     | VarUsers            | Users (and related constructors/references)                                                                                           |
| \_base      | VarBase             | Difference, UsersetTreeDifference                                                                                                     |
| \_object    | VarObject           | CheckRequestTupleKey, ExpandRequestTupleKey, TupleKey, TupleKeyWithoutCondition, FgaObject, ObjectRelation, ReadRequestTupleKey, User |
| \_this      | VarThis             | Userset                                                                                                                               |
| UNSPECIFIED | TYPENAMEUNSPECIFIED | TypeName (and other members in this enum)                                                                                             |

| Old Enum Value | New Enum Value       | Affected Enum  |
| -------------- | -------------------- | -------------- |
| WRITE          | TUPLEOPERATIONWRITE  | TupleOperation |
| DELETE         | TUPLEOPERATIONDELETE | TupleOperation |

## v0.6.0

### [0.6.0](https://github.com/openfga/dotnet-sdk/compare/v0.5.1...v0.6.0) (2025-09-30)

- feat: add support for `start_time` parameter in `ReadChanges` endpoint
- feat: update API definitions
- feat: support assertions context and contextual tuples
- feat: support contextual tuples in `Expand`
- feat!: support passing in name to filter in `ListStores`
- fix: remove dependency on OpenTelemetry.Api (#100) - thanks @m4tchl0ck
- fix: limit default retries to `3` from `15` (https://github.com/openfga/sdk-generator/pull/420) - thanks @ovindu-a
- fix: `ListRelations` should not swallow errors
- chore(docs): replace readable names with uuid to discourage storing PII in OpenFGA (https://github.com/openfga/sdk-generator/pull/433) - thanks @sccalabr

[!WARNING]
BREAKING CHANGES:

- The `ListStores` method now accepts a body parameter with an optional `Name` to filter the stores. This is a breaking change as it changes the method contract to allow passing in a body with the name.

## v0.5.1

### [0.5.1](https://github.com/openfga/dotnet-sdk/compare/v0.5.0...v0.5.1) (2024-09-09)

- feat: export OpenTelemetry metrics. Refer to the [https://github.com/openfga/dotnet-sdk/blob/main/OpenTelemetry.md](documentation) for more.

## v0.5.0

### [0.5.0](https://github.com/openfga/dotnet-sdk/compare/v0.4.0...v0.5.0) (2024-08-28)

- feat: support consistency parameter (#70)
  Note: To use this feature, you need to be running OpenFGA v1.5.7+ with the experimental flag `enable-consistency-params` enabled.
  See the [v1.5.7 release notes](https://github.com/openfga/openfga/releases/tag/v1.5.7) for details.

## v0.4.0

### [0.4.0](https://github.com/openfga/dotnet-sdk/compare/v0.3.2...v0.4.0) (2024-06-14)

- chore!: remove excluded users from ListUsers response

BREAKING CHANGE:

This version removes the `ExcludedUsers` field from the `ListUsersResponse` and `ClientListUsersResponse` classes,
for more details see the [associated API change](https://github.com/openfga/api/pull/171).

## v0.3.2

### [0.3.2](https://github.com/openfga/dotnet-sdk/compare/v0.3.1...v0.3.2) (2024-04-30)

- feat: support the [ListUsers](https://github.com/openfga/rfcs/blob/main/20231214-listUsers-api.md) endpoint (#57)
- feat: add retries to client credential requests (#51)
- feat: add support for modular models metadata (#53)

## v0.3.1

### [0.3.1](https://github.com/openfga/dotnet-sdk/compare/v0.3.0...v0.3.1) (2024-02-13)

- fix: use correct content type for token request

## v0.3.0

### [0.3.0](https://github.com/openfga/dotnet-sdk/compare/v0.2.5...v0.3.0) (2023-12-20)

- feat!: initial support for [conditions](https://openfga.dev/blog/conditional-tuples-announcement)
- feat!: allow overriding storeId per request (#33)
- feat: support specifying a port and path for the API (You can now set the `ApiUrl` to something like: `https://api.fga.exampleL8080/some_path`)
- feat: validate that store id and auth model id in ulid format (#23)
- fix: exception when using the same configuration with multiple clients (#26)
- fix: `OpenFgaClient.ReadLatestAuthorizationModel` can now return a null if no model has ever been created in that store
- fix: `OpenFgaClient.Read` and `OpenFgaClient.ReadChanges` now allow a null body
- chore!: use latest API interfaces
- chore: dependency updates
- chore: add [example project](./example)

BREAKING CHANGES:
Note: This release comes with substantial breaking changes, especially to the interfaces due to the protobuf changes in the last release.

While the http interfaces did not break (you can still use `v0.2.5` SDK with a `v1.3.8+` server),
the grpc interface did and this caused a few changes in the interfaces of the SDK.

If you are using `OpenFgaClient`, the changes required should be smaller, if you are using `OpenFgaApi` a bit more changes will be needed.

You will have to modify some parts of your code, but we hope this will be to the better as a lot of the parameters are now correctly marked as required,
and so the Pointer-to-String conversion is no longer needed.

Some of the changes to expect:

- When initializing a client, please use `ApiUrl`. The separate `ApiScheme` and `ApiHost` fields have been deprecated

```csharp
var configuration = new ClientConfiguration() {
    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL"), // required, e.g. https://api.fga.example
    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
};
var fgaClient = new OpenFgaClient(configuration);
```

- `OpenFgaApi` now requires `storeId` as first param when needed
- `Configuration` no longer accepts `storeId` (`ClientConfiguration` is not affected)
- The following request interfaces changed:
  - `CheckRequest`: the `TupleKey` field is now of interface `CheckRequestTupleKey`, you can also now pass in `Context`
  - `ExpandRequest`: the `TupleKey` field is now of interface `ExpandRequestTupleKey`
  - `ReadRequest`: the `TupleKey` field is now of interface `ReadRequestTupleKey`
  - `WriteRequest`: now takes `WriteRequestWrites` and `WriteRequestDeletes`, the latter of which accepts `TupleKeyWithoutCondition`
  - And more
- The following interfaces had fields that were pointers are are now the direct value:
  - `CreateStoreResponse`
  - `GetStoreResponse`
  - `ListStoresResponse`
  - `ListObjectsResponse`
  - `ReadChangesResponse`
  - `ReadResponse`
  - `AuthorizationModel`
  - And more

Take a look at https://github.com/openfga/dotnet-sdk/commit/fa43463ded102df3f660bae6d741e1a8c1dea090 for more model changes.

## v0.2.5

### [0.2.5](https://github.com/openfga/dotnet-sdk/compare/v0.2.4...v0.2.5) (2023-12-01)

- fix(client): read with no filter (read all tuples)
- chore(deps): update dependencies

## v0.2.4

### [0.2.4](https://github.com/openfga/dotnet-sdk/compare/v0.2.3...v0.2.4) (2023-05-01)

- fix: client credentials token expiry period was being evaluated as ms instead of seconds, leading to token refreshes on every call

## v0.2.3

### [0.2.3](https://github.com/openfga/dotnet-sdk/compare/v0.2.2...v0.2.3) (2023-04-13)

- fix: changed interface of contextual tuples in `ClientListObjects` to be `ClientTupleKey` instead of `TupleKey`
- fix: Client `WriteAuthorizationModel` now expects `ClientWriteAuthorizationModelRequest` instead of `WriteAuthorizationModelRequest`
- chore: changed a few interfaces to expect interfaces instead of classes

## v0.2.2

### [0.2.2](https://github.com/openfga/dotnet-sdk/compare/v0.2.1...v0.2.2) (2023-04-12)

- feat(client): add OpenFgaClient wrapper see [docs](https://github.com/openfga/dotnet-sdk/tree/main#readme), see the `v0.2.1` docs for [the OpenFgaApi docs](https://github.com/openfga/dotnet-sdk/tree/v0.2.1#readme)
- feat(client): implement `BatchCheck` to check multiple tuples in parallel
- feat(client): implement `ListRelations` to check in one call whether a user has multiple relations to an objects
- feat(client): add support for a non-transactional `Write`
- chore(config): bump default max retries to `15`
- fix: retry on 5xx errors

## v0.2.1

### [0.2.1](https://github.com/openfga/dotnet-sdk/compare/v0.2.0...v0.2.1) (2023-01-17)

- chore(deps): upgrade `Microsoft.NET.Test.Sdk` and `Moq` dev dependencies

## v0.2.0

### [0.2.0](https://github.com/openfga/dotnet-sdk/compare/v0.1.2...v0.2.0) (2022-12-14)

Updated to include support for [OpenFGA 0.3.0](https://github.com/openfga/openfga/releases/tag/v0.3.0)

Changes:

- [BREAKING] feat(list-objects)!: response has been changed to include the object type
  e.g. response that was `{"object_ids":["roadmap"]}`, will now be `{"objects":["document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"]}`

Fixes:

- fix(models): update interfaces that had incorrectly optional fields to make them required

Chore:

- chore(deps): update dev dependencies

## v0.1.2

### [0.1.2](https://github.com/openfga/dotnet-sdk/compare/v0.1.1...v0.1.2) (2022-11-15)

- feat: regenerate from latest API Document, changes include:
  - documentation fixes
  - types that represent enabling wildcards in authorization models
- fix: send authorization header to server when ApiToken used (https://github.com/openfga/sdk-generator/issues/58)
- chore: update test dependencies

## v0.1.1

### [0.1.1](https://github.com/openfga/dotnet-sdk/compare/v0.1.0...v0.1.1) (2022-10-07)

- Fix for issue in deserializing nullable DateTime (https://github.com/openfga/dotnet-sdk/issues/5)

## v0.1.0

### [0.1.0](https://github.com/openfga/dotnet-sdk/compare/v0.0.3...v0.1.0) (2022-09-29)

- BREAKING: exported interface `TypeDefinitions` is now `WriteAuthorizationModelRequest`
  This is only a breaking change on the SDK, not the API. It was changed to conform to the proto changes in [openfga/api](https://github.com/openfga/api/pull/27).
- chore(deps): upgrade dependencies

## v0.0.3

### [0.0.3](https://github.com/openfga/dotnet-sdk/compare/v0.0.2...v0.0.3) (2022-09-09)

- Fix for issue in deserializing enums (https://github.com/openfga/sdk-generator/issues/7)

## v0.0.2

### [0.0.2](https://github.com/openfga/dotnet-sdk/compare/v0.0.1...v0.0.2) (2022-08-15)

Support for [ListObjects API]](https://openfga.dev/api/service#/Relationship%20Queries/ListObjects)

You call the API and receive the list of object ids from a particular type that the user has a certain relation with.

For example, to find the list of documents that Anne can read:

```csharp
var body = new ListObjectsRequest{
    AuthorizationModelId = "01GAHCE4YVKPQEKZQHT2R89MQV",
    User = "anne",
    Relation = "can_read",
    Type = "document"
};
var response = await openFgaApi.ListObjects(body);

// response.ObjectIds = ["roadmap"]
```

## v0.0.1

### [0.0.1](https://github.com/openfga/dotnet-sdk/releases/tag/v0.0.1) (2022-06-17)

Initial OpenFGA .NET SDK release

- Support for [OpenFGA](https://github.com/openfga/openfga) API
  - CRUD stores
  - Create, read & list authorization models
  - Writing and Reading Tuples
  - Checking authorization
  - Using Expand to understand why access was granted
