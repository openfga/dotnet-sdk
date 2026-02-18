# Custom API Requests Example

This example demonstrates how to use `ApiExecutor` to make custom HTTP requests to the OpenFGA API with full response details (status code, headers, raw response, typed data). This is useful when you need more control over HTTP requests or want to call endpoints that aren't yet supported by the SDK's typed API.

## What This Example Demonstrates

1. **List Stores** - Making a GET request to list all stores
2. **Create Store** - Making a POST request with a typed response
3. **Get Store** - Using path parameters to get store details
4. **Create Authorization Model** - Creating an authorization model with custom request body
5. **Write Tuples** - Writing relationship tuples
6. **Read Tuples** - Reading relationship tuples with filters
7. **Check Permission** - Checking if a user has permission
8. **Raw JSON Response** - Getting responses as raw JSON strings
9. **Custom Headers** - Adding custom headers to requests via ClientRequestOptions
10. **Fluent API** - Using the enhanced RequestBuilder with fluent methods

## Key Concepts

### ApiClient.ExecuteAsync vs Standard SDK Methods

The SDK provides two ways to interact with the OpenFGA API:

1. **Standard SDK Methods** (recommended for most use cases):
   ```csharp
   var response = await client.Check(checkRequest);
   ```

2. **Custom API Requests** (for advanced scenarios):
   ```csharp
   var executor = client.ApiExecutor;
   var request = RequestBuilder<object>.Create(...);
   var response = await executor.ExecuteAsync<object, CheckResponse>(request, "Check");
   // Now you have: response.StatusCode, response.Headers, response.RawResponse, response.Data
   ```

### RequestBuilder: Two Styles

You can build requests using either style:

**Object Initializer** (compatible with existing patterns):
```csharp
var request = new RequestBuilder<object> {
    Method = HttpMethod.Post,
    BasePath = config.ApiUrl,
    PathTemplate = "/stores/{store_id}/check",
    PathParameters = new Dictionary<string, string> { { "store_id", storeId } },
    QueryParameters = new Dictionary<string, string>(),
    Body = requestBody
};
```

**Fluent API** (enhanced developer experience):
```csharp
var request = RequestBuilder<object>
    .Create(HttpMethod.Post, config.ApiUrl, "/stores/{store_id}/check")
    .WithPathParameter("store_id", storeId)
    .WithQueryParameter("timeout", "30s")
    .WithBody(requestBody);
```

## Prerequisites

- .NET 8.0 SDK or later
- Docker (for running OpenFGA server)

## Quick Start

### Option 1: Using Make (Recommended)

The easiest way to run this example:

```bash
# Show available commands
make help

# Run everything (start OpenFGA, run example, stop OpenFGA)
make run-all

# Or run step by step:
make start-openfga  # Start OpenFGA in Docker
make run            # Run the example
make stop-openfga   # Stop OpenFGA when done
```

### Option 2: Manual Setup

1. **Start OpenFGA Server:**
   ```bash
   docker run -d --name openfga-example -p 8080:8080 openfga/openfga:latest run
   ```

2. **Verify OpenFGA is running:**
   ```bash
   curl http://localhost:8080/healthz
   # Should return: {"status":"SERVING"}
   ```

3. **Run the example:**
   ```bash
   dotnet run
   ```

4. **Stop OpenFGA when done:**
   ```bash
   docker stop openfga-example && docker rm openfga-example
   ```

## Example Output

```text
=== OpenFGA Custom API Requests Example ===

📋 Example 1: List Stores
Making GET request to /stores
✅ Status: OK
   Is Successful: True
   Found 0 store(s)

🏪 Example 2: Create Store
Making POST request to /stores
✅ Status: Created
   Store ID: 01JQWXYZ123ABC456DEF789GHJ
   Store Name: ApiExecutor-Example-1738713600000
   Raw Response Length: 245 chars

🔍 Example 3: Get Store Details
Making GET request to /stores/{store_id}
✅ Status: OK
   Store Name: ApiExecutor-Example-1738713600000
   Created At: 2025-02-04T10:00:00Z
   Response Headers: 8

📝 Example 4: Create Authorization Model
Making POST request to /stores/{store_id}/authorization-models
✅ Status: Created
   Model ID: 01JQWXYZ789DEF123ABC456GHJ

✍️  Example 5: Write Relationship Tuples
Making POST request to /stores/{store_id}/write
✅ Status: OK
   Tuples written successfully

📖 Example 6: Read Relationship Tuples
Making POST request to /stores/{store_id}/read
✅ Status: OK
   Found 2 tuple(s):
     - user:alice is writer of document:roadmap
     - user:bob is reader of document:roadmap

🔐 Example 7: Check Permission
Making POST request to /stores/{store_id}/check
✅ Status: OK
   Allowed: True

📄 Example 8: Raw JSON Response
Getting response as raw JSON string instead of typed object
✅ Status: OK
   Raw JSON (first 100 chars): {"stores":[],"continuation_token":""}...
   RawResponse and Data are the same: True

📨 Example 9: Custom Headers
Making request with custom headers
✅ Status: OK
   Custom headers sent successfully
   Response has 8 headers

🎯 Example 10: Fluent API for Request Building
Using the enhanced RequestBuilder with fluent methods
✅ Status: OK
   Found 0 store(s) using fluent API
   Note: Fluent API provides better validation and cleaner syntax!

🗑️  Cleanup: Delete Store
Making DELETE request to /stores/{store_id}
✅ Status: NoContent
   Store deleted successfully

=== All examples completed successfully! ===
```

## Architecture

### How It Works

```
OpenFgaClient
    └── GetApiClient() → ApiClient (core building block)
            └── ExecuteAsync() → Full response details
                    ├── Authentication (OAuth/ApiToken)
                    ├── Retry logic with exponential backoff  
                    ├── Error handling
                    └── Metrics & telemetry

RequestBuilder
    ├── Object initializer (existing style)
    └── Fluent API (enhanced style)
```

### Benefits of ExecuteAsync

1. **Full Response Access**
   - `response.StatusCode` - HTTP status code
   - `response.Headers` - All response headers
   - `response.RawResponse` - Raw JSON string
   - `response.Data` - Strongly-typed response object
   - `response.IsSuccessful` - Quick success check

2. **Shared Infrastructure**
   - Same authentication as standard SDK methods
   - Same retry logic with exponential backoff
   - Same error handling and exceptions
   - Same metrics and telemetry

3. **Flexibility**
   - Call any OpenFGA endpoint
   - Add custom headers via `ClientRequestOptions`
   - Get raw JSON or strongly-typed responses
   - Use path and query parameters easily

## When to Use Custom API Requests

### Use Standard SDK Methods When:
- The operation is available in the SDK (Check, Write, Read, etc.)
- You don't need access to response headers
- You want the simplest API

### Use ApiClient.ExecuteAsync When:
- Calling endpoints not yet in the SDK
- You need response headers or status codes
- Building custom integrations
- Need fine-grained control over requests
- Working with experimental API features

## Code Examples

### Basic Request
```csharp
using OpenFga.Sdk.ApiClient;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;
// Optional: Use an alias to avoid namespace/class name conflicts
using FgaApiClient = OpenFga.Sdk.ApiClient.ApiClient;

var client = new OpenFgaClient(config);
var executor = client.ApiExecutor;

var request = new RequestBuilder<object> {
    Method = HttpMethod.Get,
    BasePath = config.ApiUrl,
    PathTemplate = "/stores",
    PathParameters = new Dictionary<string, string>(),
    QueryParameters = new Dictionary<string, string>()
};

var response = await executor.ExecuteAsync<object, ListStoresResponse>(
    request, 
    "ListStores"
);

Console.WriteLine($"Status: {response.StatusCode}");
Console.WriteLine($"Stores: {response.Data.Stores.Count}");
```

> **Note:** If you get namespace conflicts with `ApiClient`, you can use a type alias:
> ```csharp
> using FgaApiClient = OpenFga.Sdk.ApiClient.ApiClient;
> ```
> Then use `FgaApiClient` as the type in your method signatures.

### Fluent API Style
```csharp
var request = RequestBuilder<object>
    .Create(HttpMethod.Post, config.ApiUrl, "/stores/{store_id}/check")
    .WithPathParameter("store_id", storeId)
    .WithQueryParameter("timeout", "30s")
    .WithBody(new { 
        tuple_key = new { user = "user:anne", relation = "reader", @object = "doc:1" }
    });

var response = await executor.ExecuteAsync<object, CheckResponse>(
    request,
    "Check",
    new ClientRequestOptions {
        Headers = new Dictionary<string, string> {
            { "X-Trace-Id", traceId }
        }
    }
);
```

### Raw JSON Response
```csharp
// When you want the raw JSON without deserialization
var response = await executor.ExecuteAsync(request, "CustomEndpoint");
string json = response.Data; // Raw JSON string
```

## Resources

- [OpenFGA API Documentation](https://openfga.dev/api)

## Common Issues

### OpenFGA Connection Failed

If you see connection errors:
1. Verify OpenFGA is running: `curl http://localhost:8080/healthz`
2. Check Docker logs: `docker logs openfga-example`
3. Ensure port 8080 is not in use by another application

### Build Errors

Make sure you have .NET 8.0 SDK installed:
```bash
dotnet --version
```

## Learn More

- [OpenFGA Documentation](https://openfga.dev/docs)
- [OpenFGA API Reference](https://openfga.dev/api/service)
- [OpenFGA SDK Documentation](https://github.com/openfga/dotnet-sdk)
