# .NET SDK for OpenFGA

[![Nuget](https://img.shields.io/nuget/v/OpenFga.Sdk?label=OpenFga.Sdk&style=flat-square)](https://www.nuget.org/packages/OpenFga.Sdk)
[![Release](https://img.shields.io/github/v/release/openfga/dotnet-sdk?sort=semver&color=green)](https://github.com/openfga/dotnet-sdk/releases)
[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](./LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fopenfga%2Fdotnet-sdk.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fopenfga%2Fdotnet-sdk?ref=badge_shield)
[![Join our community](https://img.shields.io/badge/slack-cncf_%23openfga-40abb8.svg?logo=slack)](https://openfga.dev/community)
[![Twitter](https://img.shields.io/twitter/follow/openfga?color=%23179CF0&logo=twitter&style=flat-square "@openfga on Twitter")](https://twitter.com/openfga)

This is an autogenerated SDK for OpenFGA. It provides a wrapper around the [OpenFGA API definition](https://openfga.dev/api).

## Table of Contents

- [About OpenFGA](#about)
- [Resources](#resources)
- [Installation](#installation)
- [Getting Started](#getting-started)
  - [Initializing the API Client](#initializing-the-api-client)
  - [Get your Store ID](#get-your-store-id)
  - [Calling the API](#calling-the-api)
    - [Stores](#stores)
      - [List All Stores](#list-stores)
      - [Create a Store](#create-store)
      - [Get a Store](#get-store)
      - [Delete a Store](#delete-store)
    - [Authorization Models](#authorization-models)
      - [Read Authorization Models](#read-authorization-models)
      - [Write Authorization Model](#write-authorization-model)
      - [Read a Single Authorization Model](#read-a-single-authorization-model)
      - [Read the Latest Authorization Model](#read-the-latest-authorization-model)
    - [Relationship Tuples](#relationship-tuples)
      - [Read Relationship Tuple Changes (Watch)](#read-relationship-tuple-changes-watch)
      - [Read Relationship Tuples](#read-relationship-tuples)
      - [Write (Create and Delete) Relationship Tuples](#write-create-and-delete-relationship-tuples)
    - [Relationship Queries](#relationship-queries)
      - [Check](#check)
      - [Batch Check](#batch-check)
      - [Expand](#expand)
      - [List Objects](#list-objects)
      - [List Relations](#list-relations)
      - [List Users](#list-users)
    - [Assertions](#assertions)
      - [Read Assertions](#read-assertions)
      - [Write Assertions](#write-assertions)
  - [Retries](#retries)
  - [API Endpoints](#api-endpoints)
  - [Models](#models)
  - [OpenTelemetry](#opentelemetry)
- [Contributing](#contributing)
  - [Issues](#issues)
  - [Pull Requests](#pull-requests)
- [License](#license)

## About

[OpenFGA](https://openfga.dev) is an open source Fine-Grained Authorization solution inspired by [Google's Zanzibar paper](https://research.google/pubs/pub48190/). It was created by the FGA team at [Auth0](https://auth0.com) based on [Auth0 Fine-Grained Authorization (FGA)](https://fga.dev), available under [a permissive license (Apache-2)](https://github.com/openfga/rfcs/blob/main/LICENSE) and welcomes community contributions.

OpenFGA is designed to make it easy for application builders to model their permission layer, and to add and integrate fine-grained authorization into their applications. OpenFGA’s design is optimized for reliability and low latency at a high scale.


## Resources

- [OpenFGA Documentation](https://openfga.dev/docs)
- [OpenFGA API Documentation](https://openfga.dev/api/service)
- [Twitter](https://twitter.com/openfga)
- [OpenFGA Community](https://openfga.dev/community)
- [Zanzibar Academy](https://zanzibar.academy)
- [Google's Zanzibar Paper (2019)](https://research.google/pubs/pub48190/)

## Installation

The OpenFGA .NET SDK is available on [NuGet](https://www.nuget.org/).

You can install it using:

* The [dotnet CLI](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli)
```powershell
dotnet add package OpenFga.Sdk
```

* The [Package Manager Console](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-powershell) inside Visual Studio:

```powershell
Install-Package OpenFga.Sdk
```

* [Visual Studio](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio), [Visual Studio for Mac](https://docs.microsoft.com/en-us/visualstudio/mac/nuget-walkthrough) and [IntelliJ Rider](https://www.jetbrains.com/help/rider/Using_NuGet.html)

Search for and install `OpenFga.Sdk` in each of their respective package manager UIs.


## Getting Started

### Initializing the API Client

[Learn how to initialize your SDK](https://openfga.dev/docs/getting-started/setup-sdk-client)

We strongly recommend you initialize the `OpenFgaClient` only once and then re-use it throughout your app, otherwise you will incur the cost of having to re-initialize multiple times or at every request, the cost of reduced connection pooling and re-use, and would be particularly costly in the client credentials flow, as that flow will be preformed on every request.

> The `OpenFga.SdkClient` will by default retry API requests up to 15 times on 429 and 5xx errors.

#### No Credentials

```csharp
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;

namespace Example {
    public class Example {
        public static async Task Main() {
            try {
                var configuration = new ClientConfiguration() {
                    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080", // required, e.g. https://api.fga.example
                    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
                    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
                };
                var fgaClient = new OpenFgaClient(configuration);
                var response = await fgaClient.ReadAuthorizationModels();
            } catch (ApiException e) {
                 Debug.Print("Error: "+ e);
            }
        }
    }
}
```

#### API Token

```csharp
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;

namespace Example {
    public class Example {
        public static async Task Main() {
            try {
                var configuration = new ClientConfiguration() {
                    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080", // required, e.g. https://api.fga.example
                    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
                    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
                    Credentials = new Credentials() {
                        Method = CredentialsMethod.ApiToken,
                        Config = new CredentialsConfig() {
                            ApiToken = Environment.GetEnvironmentVariable("FGA_API_TOKEN"),  // will be passed as the "Authorization: Bearer ${ApiToken}" request header
                        }
                    }
                };
                var fgaClient = new OpenFgaClient(configuration);
                var response = await fgaClient.ReadAuthorizationModels();
            } catch (ApiException e) {
                 Debug.Print("Error: "+ e);
            }
        }
    }
}
```

#### Client Credentials

```csharp
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;

namespace Example {
    public class Example {
        public static async Task Main() {
            try {
                var configuration = new ClientConfiguration() {
                    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080", // required, e.g. https://api.fga.example
                    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
                    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
                    Credentials = new Credentials() {
                        Method = CredentialsMethod.ClientCredentials,
                        Config = new CredentialsConfig() {
                            ApiTokenIssuer = Environment.GetEnvironmentVariable("FGA_API_TOKEN_ISSUER"),
                            ApiAudience = Environment.GetEnvironmentVariable("FGA_API_AUDIENCE"),
                            ClientId = Environment.GetEnvironmentVariable("FGA_CLIENT_ID"),
                            ClientSecret = Environment.GetEnvironmentVariable("FGA_CLIENT_SECRET"),
                        }
                    }
                };
                var fgaClient = new OpenFgaClient(configuration);
                var response = await fgaClient.ReadAuthorizationModels();
            } catch (ApiException e) {
                 Debug.Print("Error: "+ e);
            }
        }
    }
}
```


### Get your Store ID

You need your store id to call the OpenFGA API (unless it is to call the [CreateStore](#create-store) or [ListStores](#list-stores) methods).

If your server is configured with [authentication enabled](https://openfga.dev/docs/getting-started/setup-openfga#configuring-authentication), you also need to have your credentials ready.

### Calling the API

#### Stores

##### List Stores

Get a paginated list of stores.

[API Documentation](https://openfga.dev/api/service/docs/api#/Stores/ListStores)

```csharp
var options = new ClientListStoresOptions {
    PageSize = 10,
    ContinuationToken = "...",
};
var response = await fgaClient.ListStores(options);

// stores = [{ "id": "01FQH7V8BEG3GPQW93KTRFR8JB", "name": "FGA Demo Store", "created_at": "2022-01-01T00:00:00.000Z", "updated_at": "2022-01-01T00:00:00.000Z" }]
```

##### Create Store

Initialize a store.

[API Documentation](https://openfga.dev/api/service/docs/api#/Stores/CreateStore)

```csharp
var store = await fgaClient.CreateStore(new ClientCreateStoreRequest(){Name = "FGA Demo"})

// store.Id = "01FQH7V8BEG3GPQW93KTRFR8JB"

// store store.Id in database

// update the storeId of the current instance
fgaClient.StoreId = storeId;

// continue calling the API normally
```

##### Get Store

Get information about the current store.

[API Documentation](https://openfga.dev/api/service/docs/api#/Stores/GetStore)

> Requires a client initialized with a storeId

```csharp
var store = await fgaClient.GetStore();

// store = { "id": "01FQH7V8BEG3GPQW93KTRFR8JB", "name": "FGA Demo Store", "created_at": "2022-01-01T00:00:00.000Z", "updated_at": "2022-01-01T00:00:00.000Z" }
```

##### Delete Store

Delete a store.

[API Documentation](https://openfga.dev/api/service/docs/api#/Stores/DeleteStore)

> Requires a client initialized with a storeId

```csharp
var store = await fgaClient.DeleteStore();
```

#### Authorization Models

##### Read Authorization Models

Read all authorization models in the store.

[API Documentation](https://openfga.dev/api/service#/Authorization%20Models/ReadAuthorizationModels)

```csharp
var options = new ClientReadAuthorizationModelsOptions {
    PageSize = 10,
    ContinuationToken = "...",
};
var response = await fgaClient.ReadAuthorizationModels(options);

// response.AuthorizationModels = [
// { Id: "01GXSA8YR785C4FYS3C0RTG7B1", SchemaVersion: "1.1", TypeDefinitions: [...] },
// { Id: "01GXSBM5PVYHCJNRNKXMB4QZTW", SchemaVersion: "1.1", TypeDefinitions: [...] }];
```

##### Write Authorization Model

Create a new authorization model.

[API Documentation](https://openfga.dev/api/service#/Authorization%20Models/WriteAuthorizationModel)

> Note: To learn how to build your authorization model, check the Docs at https://openfga.dev/docs.

> Learn more about [the OpenFGA configuration language](https://openfga.dev/docs/configuration-language).

> You can use the [OpenFGA Syntax Transformer](https://github.com/openfga/syntax-transformer) to convert between the friendly DSL and the JSON authorization model.

```csharp

var body = new ClientWriteAuthorizationModelRequest {
    SchemaVersion = "1.1",
    TypeDefinitions = new List<TypeDefinition> {
        new() {Type = "user", Relations = new Dictionary<string, Userset>()},
        new() {Type = "document",
            Relations = new Dictionary<string, Userset> {
                {"writer", new Userset {This = new object()}}, {
                    "viewer", new Userset {
                        Union = new Usersets {
                            Child = new List<Userset> {
                                new() {This = new object()},
                                new() {ComputedUserset = new ObjectRelation {Relation = "writer"}}
                            }
                        }
                    }
                }
            },
            Metadata = new Metadata {
                Relations = new Dictionary<string, RelationMetadata> {
                    {"writer", new RelationMetadata {
                        DirectlyRelatedUserTypes = new List<RelationReference> {
                            new() {Type = "user"}
                        }
                    }}, {"viewer", new RelationMetadata {
                        DirectlyRelatedUserTypes = new List<RelationReference> {
                            new() {Type = "user"}
                        }
                    }}
                }
            }
        }
    }
};

var response = await fgaClient.WriteAuthorizationModel(body);

// response.AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1"
```

#### Read a Single Authorization Model

Read a particular authorization model.

[API Documentation](https://openfga.dev/api/service#/Authorization%20Models/ReadAuthorizationModel)

```csharp
var options = new ClientReadAuthorizationModelOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};

var response = await fgaClient.ReadAuthorizationModel(options);

// response.AuthorizationModel.Id = "01GXSA8YR785C4FYS3C0RTG7B1"
// response.AuthorizationModel.SchemaVersion = "1.1"
// response.AuthorizationModel.TypeDefinitions = [{ "type": "document", "relations": { ... } }, { "type": "user", "relations": { ... }}]
```

##### Read the Latest Authorization Model

Reads the latest authorization model (note: this ignores the model id in configuration).

[API Documentation](https://openfga.dev/api/service#/Authorization%20Models/ReadAuthorizationModel)

```csharp
var response = await fgaClient.ReadLatestAuthorizationModel();

// response.AuthorizationModel.Id = "01GXSA8YR785C4FYS3C0RTG7B1"
// response.AuthorizationModel.SchemaVersion = "1.1"
// response.AuthorizationModel.TypeDefinitions = [{ "type": "document", "relations": { ... } }, { "type": "user", "relations": { ... }}]
```

#### Relationship Tuples

##### Read Relationship Tuple Changes (Watch)

Reads the list of historical relationship tuple writes and deletes.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Tuples/ReadChanges)

```csharp
var body = new ClientReadChangesRequest { Type = "document" };
var options = new ClientReadChangesOptions {
    PageSize = 10,
    ContinuationToken = "...",
};

var response = await fgaClient.ReadChanges(body, options);

// response.ContinuationToken = ...
// response.Changes = [
//   { TupleKey: { User, Relation, Object }, Operation: TupleOperation.WRITE, Timestamp: ... },
//   { TupleKey: { User, Relation, Object }, Operation: TupleOperation.DELETE, Timestamp: ... }
// ]
```

##### Read Relationship Tuples

Reads the relationship tuples stored in the database. It does not evaluate nor exclude invalid tuples according to the authorization model.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Tuples/Read)

```csharp
// Find if a relationship tuple stating that a certain user is a viewer of a certain document
var body = new ClientReadRequest() {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation = "viewer",
    Object = "document:roadmap",
};

// Find all relationship tuples where a certain user has a relationship as any relation to a certain document
var body = new ClientReadRequest() {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Object = "document:roadmap",
};

// Find all relationship tuples where a certain user is a viewer of any document
var body = new ClientReadRequest() {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation = "viewer",
    Object = "document:",
};

// Find all relationship tuples where any user has a relationship as any relation with a particular document

var body = new ClientReadRequest() {
    Object = "document:roadmap",
};

// Read all stored relationship tuples
var body = new ClientReadRequest();

var options = new ClientReadOptions {
    PageSize = 10,
    ContinuationToken = "...",
};

var response = await fgaClient.Read(body, options);

// In all the above situations, the response will be of the form:
// response = { Tuples: [{ Key: { User, Relation, Object }, Timestamp }, ...]}
```

##### Write (Create and Delete) Relationship Tuples

Create and/or delete relationship tuples to update the system state.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Tuples/Write)

###### Transaction mode (default)

By default, write runs in a transaction mode where any invalid operation (deleting a non-existing tuple, creating an existing tuple, one of the tuples was invalid) or a server error will fail the entire operation.

```csharp
var body = new ClientWriteRequest() {
    Writes = new List<ClientTupleKey> {
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:roadmap",
        },
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:budget",
        }
    },
    Deletes = new List<ClientTupleKeyWithoutCondition> {
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "writer",
            Object = "document:roadmap",
        }
    },
};
var options = new ClientWriteOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var response = await fgaClient.Write(body, options);
```

Convenience `WriteTuples` and `DeleteTuples` methods are also available.

###### Non-transaction mode

The SDK will split the writes into separate requests and send them sequentially to avoid violating rate limits.

```csharp
var body = new ClientWriteRequest() {
    Writes = new List<ClientTupleKey> {
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:roadmap",
        },
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "viewer",
            Object = "document:budget",
        }
    },
    Deletes = new List<ClientTupleKeyWithoutCondition> {
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "writer",
            Object = "document:roadmap",
        }
    },
};
var options = new ClientWriteOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
    Transaction = new TransactionOptions() {
        Disable = true,
        MaxParallelRequests = 5, // Maximum number of requests to issue in parallel
        MaxPerChunk = 1, // Maximum number of requests to be sent in a transaction in a particular chunk
    }
};
var response = await fgaClient.Write(body, options);
```

#### Relationship Queries

##### Check

Check if a user has a particular relation with an object.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Queries/Check)

```csharp
var options = new ClientCheckOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var body = new ClientCheckRequest {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation = "writer",
    Object = "document:roadmap"
};
var response = await fgaClient.Check(body, options);
// response.Allowed = true
```

##### Batch Check

Run a set of [checks](#check). Batch Check will return `allowed: false` if it encounters an error, and will return the error in the body.
If 429s or 5xxs are encountered, the underlying check will retry up to 15 times before giving up.

```csharp
var options = new ClientBatchCheckOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
    MaxParallelRequests = 5, // Max number of requests to issue in parallel, defaults to 10
};
var body = new List<ClientCheckRequest>(){
    new() {
        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
        Relation = "viewer",
        Object = "document:roadmap",
        ContextualTuples = new List<ClientTupleKey>() {
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "editor",
                Object = "document:roadmap",
            }
        },
    },
    new() {
        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
        Relation = "admin",
        Object = "document:roadmap",
        ContextualTuples = new List<ClientTupleKey>() {
            new() {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "editor",
                Object = "document:roadmap",
            }
        },
    },
    new() {
        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
        Relation = "creator",
        Object = "document:roadmap",
    },
    new() {
        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
        Relation = "deleter",
        Object = "document:roadmap",
    }
};

var response = await fgaClient.BatchCheck(body, options);

/*
response.Responses = [{
  Allowed: false,
  Request: {
    User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation: "viewer",
    Object: "document:roadmap",
    ContextualTuples: [{
      User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
      Relation: "editor",
      Object: "document:roadmap"
    }]
  }
}, {
  Allowed: false,
  Request: {
    User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation: "admin",
    Object: "document:roadmap",
    ContextualTuples: [{
      User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
      Relation: "editor",
      Object: "document:roadmap"
    }]
  }
}, {
  Allowed: false,
  Request: {
    User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation: "creator",
    Object: "document:roadmap",
  },
  Error: <FgaError ...>
}, {
  Allowed: true,
  Request: {
    User: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation: "deleter",
    Object: "document:roadmap",
  }},
]
*/
```

##### Expand

Expands the relationships in userset tree format.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Queries/Expand)

```csharp
var options = new ClientCheckOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var body = new ClientExpandRequest {
    Relation = "viewer",
    Object = "document:roadmap",
};
var response = await fgaClient.Expand(body, options);

// response.Tree.Root = {"name":"document:roadmap#viewer","leaf":{"users":{"users":["user:81684243-9356-4421-8fbf-a4f8d36aa31b","user:f52a4f7a-054d-47ff-bb6e-3ac81269988f"]}}}
```

##### List Objects

List the objects of a particular type a user has access to.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Queries/ListObjects)

```csharp
var options = new ClientListObjectsOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var body = new ClientListObjectsRequest {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation = "viewer",
    Type = "document",
    ContextualTuples = new List<ClientTupleKey> {
        new() {
            User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
            Relation = "writer",
            Object = "document:budget",
        },
    },
};
var response = await fgaClient.ListObjects(body, options);

// response.Objects = ["document:roadmap"]
```

##### List Relations

List the relations a user has on an object.

```csharp
ClientListRelationsRequest body =
    new ClientListRelationsRequest() {
        User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
        Object = "document:roadmap",
        Relations = new List<string> {"can_view", "can_edit", "can_delete", "can_rename"},
        ContextualTuples = new List<ClientTupleKey>() {
            new ClientTupleKey {
                User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
                Relation = "editor",
                Object = "document:roadmap",
            }
        }
    };
var response = await fgaClient.ListRelations(body);

// response.Relations = ["can_view", "can_edit"]
```

##### List Users

List the users who have a certain relation to a particular type.

[API Documentation](https://openfga.dev/api/service#/Relationship%20Queries/ListUsers)

```csharp
const options = {};

// To override the authorization model id for this request
options.authorization_model_id = "1uHxCSuTP0VKPYSnkq1pbb1jeZw";

// Only a single filter is allowed for the time being
const userFilters = [{type: "user"}];
// user filters can also be of the form
// const userFilters = [{type: "team", relation: "member"}];

const response = await fgaClient.listUsers({
  object: {
    type: "document",
    id: "roadmap"
  },
  relation: "can_read",
  user_filters: userFilters,
  context: {
    "view_count": 100
  },
  contextualTuples:
    [{
      user: "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
      relation: "editor",
      object: "folder:product"
    }, {
      user: "folder:product",
      relation: "parent",
      object: "document:roadmap"
    }]
}, options);

// response.users = [{object: {type: "user", id: "81684243-9356-4421-8fbf-a4f8d36aa31b"}}, {userset: { type: "user" }}, ...]
```

#### Assertions

##### Read Assertions

Read assertions for a particular authorization model.

[API Documentation](https://openfga.dev/api/service#/Assertions/Read%20Assertions)

```csharp
var options = new ClientReadAssertionsOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var response = await fgaClient.ReadAssertions(options);
```

##### Write Assertions

Update the assertions for a particular authorization model.

[API Documentation](https://openfga.dev/api/service#/Assertions/Write%20Assertions)

```csharp
var options = new ClientWriteAssertionsOptions {
    // You can rely on the model id set in the configuration or override it for this specific request
    AuthorizationModelId = "01GXSA8YR785C4FYS3C0RTG7B1",
};
var body = new List<ClientAssertion>() {new ClientAssertion() {
    User = "user:81684243-9356-4421-8fbf-a4f8d36aa31b",
    Relation = "viewer",
    Object = "document:roadmap",
    Expectation = true,
}};
await fgaClient.WriteAssertions(body, options);
```


### Retries

If a network request fails with a 429 or 5xx error from the server, the SDK will automatically retry the request up to 15 times with a minimum wait time of 100 milliseconds between each attempt.

To customize this behavior, create a `RetryParams` instance and assign values to the `MaxRetry` and `MinWaitInMs` constructor parameters. `MaxRetry` determines the maximum number of retries (up to 15), while `MinWaitInMs` sets the minimum wait time between retries in milliseconds.

Apply your custom retry values by passing the object to the `ClientConfiguration` constructor's `RetryParams` parameter.

```csharp
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;

namespace Example {
    public class Example {
        public static async Task Main() {
            try {
                var configuration = new ClientConfiguration() {
                    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080", // required, e.g. https://api.fga.example
                    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
                    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
                    RetryParams = new RetryParams() {
                        MaxRetry = 3, // retry up to 3 times on API requests
                        MinWaitInMs = 250 // wait a minimum of 250 milliseconds between requests
                    }
                };
                var fgaClient = new OpenFgaClient(configuration);
                var response = await fgaClient.ReadAuthorizationModels();
            } catch (ApiException e) {
                 Debug.Print("Error: "+ e);
            }
        }
    }
}
```

### API Endpoints

| Method | HTTP request | Description |
| ------------- | ------------- | ------------- |
| [**Check**](docs/OpenFgaApi.md#check) | **POST** /stores/{store_id}/check | Check whether a user is authorized to access an object |
| [**CreateStore**](docs/OpenFgaApi.md#createstore) | **POST** /stores | Create a store |
| [**DeleteStore**](docs/OpenFgaApi.md#deletestore) | **DELETE** /stores/{store_id} | Delete a store |
| [**Expand**](docs/OpenFgaApi.md#expand) | **POST** /stores/{store_id}/expand | Expand all relationships in userset tree format, and following userset rewrite rules.  Useful to reason about and debug a certain relationship |
| [**GetStore**](docs/OpenFgaApi.md#getstore) | **GET** /stores/{store_id} | Get a store |
| [**ListObjects**](docs/OpenFgaApi.md#listobjects) | **POST** /stores/{store_id}/list-objects | List all objects of the given type that the user has a relation with |
| [**ListStores**](docs/OpenFgaApi.md#liststores) | **GET** /stores | List all stores |
| [**ListUsers**](docs/OpenFgaApi.md#listusers) | **POST** /stores/{store_id}/list-users | List the users matching the provided filter who have a certain relation to a particular type. |
| [**Read**](docs/OpenFgaApi.md#read) | **POST** /stores/{store_id}/read | Get tuples from the store that matches a query, without following userset rewrite rules |
| [**ReadAssertions**](docs/OpenFgaApi.md#readassertions) | **GET** /stores/{store_id}/assertions/{authorization_model_id} | Read assertions for an authorization model ID |
| [**ReadAuthorizationModel**](docs/OpenFgaApi.md#readauthorizationmodel) | **GET** /stores/{store_id}/authorization-models/{id} | Return a particular version of an authorization model |
| [**ReadAuthorizationModels**](docs/OpenFgaApi.md#readauthorizationmodels) | **GET** /stores/{store_id}/authorization-models | Return all the authorization models for a particular store |
| [**ReadChanges**](docs/OpenFgaApi.md#readchanges) | **GET** /stores/{store_id}/changes | Return a list of all the tuple changes |
| [**Write**](docs/OpenFgaApi.md#write) | **POST** /stores/{store_id}/write | Add or delete tuples from the store |
| [**WriteAssertions**](docs/OpenFgaApi.md#writeassertions) | **PUT** /stores/{store_id}/assertions/{authorization_model_id} | Upsert assertions for an authorization model ID |
| [**WriteAuthorizationModel**](docs/OpenFgaApi.md#writeauthorizationmodel) | **POST** /stores/{store_id}/authorization-models | Create a new authorization model |



### Models

 - [Model.AbortedMessageResponse](docs/AbortedMessageResponse.md)
 - [Model.Any](docs/Any.md)
 - [Model.Assertion](docs/Assertion.md)
 - [Model.AssertionTupleKey](docs/AssertionTupleKey.md)
 - [Model.AuthorizationModel](docs/AuthorizationModel.md)
 - [Model.CheckRequest](docs/CheckRequest.md)
 - [Model.CheckRequestTupleKey](docs/CheckRequestTupleKey.md)
 - [Model.CheckResponse](docs/CheckResponse.md)
 - [Model.Computed](docs/Computed.md)
 - [Model.Condition](docs/Condition.md)
 - [Model.ConditionMetadata](docs/ConditionMetadata.md)
 - [Model.ConditionParamTypeRef](docs/ConditionParamTypeRef.md)
 - [Model.ConsistencyPreference](docs/ConsistencyPreference.md)
 - [Model.ContextualTupleKeys](docs/ContextualTupleKeys.md)
 - [Model.CreateStoreRequest](docs/CreateStoreRequest.md)
 - [Model.CreateStoreResponse](docs/CreateStoreResponse.md)
 - [Model.Difference](docs/Difference.md)
 - [Model.ErrorCode](docs/ErrorCode.md)
 - [Model.ExpandRequest](docs/ExpandRequest.md)
 - [Model.ExpandRequestTupleKey](docs/ExpandRequestTupleKey.md)
 - [Model.ExpandResponse](docs/ExpandResponse.md)
 - [Model.FgaObject](docs/FgaObject.md)
 - [Model.GetStoreResponse](docs/GetStoreResponse.md)
 - [Model.InternalErrorCode](docs/InternalErrorCode.md)
 - [Model.InternalErrorMessageResponse](docs/InternalErrorMessageResponse.md)
 - [Model.Leaf](docs/Leaf.md)
 - [Model.ListObjectsRequest](docs/ListObjectsRequest.md)
 - [Model.ListObjectsResponse](docs/ListObjectsResponse.md)
 - [Model.ListStoresResponse](docs/ListStoresResponse.md)
 - [Model.ListUsersRequest](docs/ListUsersRequest.md)
 - [Model.ListUsersResponse](docs/ListUsersResponse.md)
 - [Model.Metadata](docs/Metadata.md)
 - [Model.Node](docs/Node.md)
 - [Model.Nodes](docs/Nodes.md)
 - [Model.NotFoundErrorCode](docs/NotFoundErrorCode.md)
 - [Model.NullValue](docs/NullValue.md)
 - [Model.ObjectRelation](docs/ObjectRelation.md)
 - [Model.PathUnknownErrorMessageResponse](docs/PathUnknownErrorMessageResponse.md)
 - [Model.ReadAssertionsResponse](docs/ReadAssertionsResponse.md)
 - [Model.ReadAuthorizationModelResponse](docs/ReadAuthorizationModelResponse.md)
 - [Model.ReadAuthorizationModelsResponse](docs/ReadAuthorizationModelsResponse.md)
 - [Model.ReadChangesResponse](docs/ReadChangesResponse.md)
 - [Model.ReadRequest](docs/ReadRequest.md)
 - [Model.ReadRequestTupleKey](docs/ReadRequestTupleKey.md)
 - [Model.ReadResponse](docs/ReadResponse.md)
 - [Model.RelationMetadata](docs/RelationMetadata.md)
 - [Model.RelationReference](docs/RelationReference.md)
 - [Model.RelationshipCondition](docs/RelationshipCondition.md)
 - [Model.SourceInfo](docs/SourceInfo.md)
 - [Model.Status](docs/Status.md)
 - [Model.Store](docs/Store.md)
 - [Model.Tuple](docs/Tuple.md)
 - [Model.TupleChange](docs/TupleChange.md)
 - [Model.TupleKey](docs/TupleKey.md)
 - [Model.TupleKeyWithoutCondition](docs/TupleKeyWithoutCondition.md)
 - [Model.TupleOperation](docs/TupleOperation.md)
 - [Model.TupleToUserset](docs/TupleToUserset.md)
 - [Model.TypeDefinition](docs/TypeDefinition.md)
 - [Model.TypeName](docs/TypeName.md)
 - [Model.TypedWildcard](docs/TypedWildcard.md)
 - [Model.UnauthenticatedResponse](docs/UnauthenticatedResponse.md)
 - [Model.UnprocessableContentErrorCode](docs/UnprocessableContentErrorCode.md)
 - [Model.UnprocessableContentMessageResponse](docs/UnprocessableContentMessageResponse.md)
 - [Model.User](docs/User.md)
 - [Model.UserTypeFilter](docs/UserTypeFilter.md)
 - [Model.Users](docs/Users.md)
 - [Model.Userset](docs/Userset.md)
 - [Model.UsersetTree](docs/UsersetTree.md)
 - [Model.UsersetTreeDifference](docs/UsersetTreeDifference.md)
 - [Model.UsersetTreeTupleToUserset](docs/UsersetTreeTupleToUserset.md)
 - [Model.UsersetUser](docs/UsersetUser.md)
 - [Model.Usersets](docs/Usersets.md)
 - [Model.ValidationErrorMessageResponse](docs/ValidationErrorMessageResponse.md)
 - [Model.WriteAssertionsRequest](docs/WriteAssertionsRequest.md)
 - [Model.WriteAuthorizationModelRequest](docs/WriteAuthorizationModelRequest.md)
 - [Model.WriteAuthorizationModelResponse](docs/WriteAuthorizationModelResponse.md)
 - [Model.WriteRequest](docs/WriteRequest.md)
 - [Model.WriteRequestDeletes](docs/WriteRequestDeletes.md)
 - [Model.WriteRequestWrites](docs/WriteRequestWrites.md)



### OpenTelemetry

This SDK supports producing metrics that can be consumed as part of an [OpenTelemetry](https://opentelemetry.io/) setup. For more information, please see [the documentation](https://github.com/openfga/dotnet-sdk/blob/main/OpenTelemetry.md)

## Contributing

### Issues

If you have found a bug or if you have a feature request, please report them on the [sdk-generator repo](https://github.com/openfga/sdk-generator/issues) issues section. Please do not report security vulnerabilities on the public GitHub issue tracker.

### Pull Requests

All changes made to this repo will be overwritten on the next generation, so we kindly ask that you send all pull requests related to the SDKs to the [sdk-generator repo](https://github.com/openfga/sdk-generator) instead.

## Author

[OpenFGA](https://github.com/openfga)

## License

This project is licensed under the Apache-2.0 license. See the [LICENSE](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE) file for more info.

The code in this repo was auto generated by [OpenAPI Generator](https://github.com/OpenAPITools/openapi-generator) from a template based on the [csharp-netcore template](https://github.com/OpenAPITools/openapi-generator/tree/master/modules/openapi-generator/src/main/resources/csharp-netcore), licensed under the [Apache License 2.0](https://github.com/OpenAPITools/openapi-generator/blob/master/LICENSE).

