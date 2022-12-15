# Changelog

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
