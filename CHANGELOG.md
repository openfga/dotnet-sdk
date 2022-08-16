# Changelog

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
