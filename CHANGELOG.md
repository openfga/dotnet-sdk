# Changelog

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
    e.g. response that was `{"object_ids":["roadmap"]}`, will now be `{"objects":["document:roadmap"]}`

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
