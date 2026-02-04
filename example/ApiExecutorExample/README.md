# ApiExecutor Example

This example demonstrates how to use the `ApiExecutor` to make raw HTTP requests to the OpenFGA API without using the SDK's typed methods. This is useful when you need more control over the HTTP requests or want to call endpoints that aren't yet supported by the SDK's typed API.

## What This Example Demonstrates

1. **List Stores** - Making a GET request to list all stores
2. **Create Store** - Making a POST request with a typed response
3. **Get Store** - Using path parameters to get store details
4. **Create Authorization Model** - Creating an authorization model with custom request body
5. **Write Tuples** - Writing relationship tuples
6. **Read Tuples** - Reading relationship tuples with filters
7. **Check Permission** - Checking if a user has permission
8. **Raw JSON Response** - Getting responses as raw JSON strings
9. **Custom Headers** - Adding custom headers to requests

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
=== OpenFGA ApiExecutor Example ===

📋 Example 1: List Stores
Making GET request to /stores
✅ Status: OK
   Found 0 store(s)

🏪 Example 2: Create Store
Making POST request to /stores
✅ Status: Created
   Store ID: 01JQWXYZ123ABC456DEF789GHJ
   Store Name: ApiExecutor-Example-1738713600000

🔍 Example 3: Get Store Details
Making GET request to /stores/{store_id}
✅ Status: OK
   Store Name: ApiExecutor-Example-1738713600000
   Created At: 2025-02-04T10:00:00Z

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
   Raw JSON: {"stores":[],"continuation_token":""}...

📨 Example 9: Custom Headers
Making request with custom headers
✅ Status: OK
   Custom headers sent successfully

🗑️  Cleanup: Delete Store
Making DELETE request to /stores/{store_id}
✅ Status: NoContent
   Store deleted successfully

=== All examples completed successfully! ===
```

## Key Concepts

### 1. Creating Requests

Use `ApiExecutorRequestBuilder` to construct requests:

```csharp
var request = ApiExecutorRequestBuilder
    .Of(HttpMethod.Post, "/stores/{store_id}/write")
    .PathParam("store_id", storeId)
    .QueryParam("page_size", "10")
    .Header("X-Custom-Header", "value")
    .Body(requestBody)
    .Build();
```

### 2. Sending Requests

Send requests using the ApiExecutor:

```csharp
// With typed response
var response = await client.GetApiExecutor().SendAsync<CreateStoreResponse>(request);

// With raw JSON response
var response = await client.GetApiExecutor().SendAsync(request);
```

### 3. Working with Responses

Access response data through the `ApiResponse` object:

```csharp
Console.WriteLine($"Status: {response.StatusCode}");
Console.WriteLine($"Success: {response.IsSuccessful}");
Console.WriteLine($"Data: {response.Data}");
Console.WriteLine($"Raw Response: {response.RawResponse}");
Console.WriteLine($"Headers: {string.Join(", ", response.Headers.Keys)}");
```

## When to Use ApiExecutor

The ApiExecutor is useful when you need to:

- Call OpenFGA API endpoints not yet available in the SDK's typed API
- Have fine-grained control over HTTP requests
- Work with custom or experimental API features
- Debug API interactions at the HTTP level
- Build custom abstractions on top of the OpenFGA API

## Troubleshooting

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
