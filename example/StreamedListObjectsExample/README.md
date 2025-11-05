# Streamed List Objects Example

Demonstrates using `StreamedListObjects` to retrieve objects via the streaming API in the .NET SDK.

## What is StreamedListObjects?

The Streamed ListObjects API is very similar to the ListObjects API, with two key differences:

1. **Streaming Results**: Instead of collecting all objects before returning a response, it streams them to the client as they are collected.
2. **No Pagination Limit**: The number of results returned is only limited by the execution timeout specified in the flag `OPENFGA_LIST_OBJECTS_DEADLINE`.

This makes it ideal for scenarios where you need to retrieve large numbers of objects without being constrained by pagination limits.

## Prerequisites

- .NET 6.0 or higher
- OpenFGA server running on `http://localhost:8080` (or set `FGA_API_URL`)

## Running

### Using the local build

```bash
# From the SDK root directory, build the SDK first
cd /Users/danieljonathan/Workspaces/openfga/dotnet-sdk
dotnet build src/OpenFga.Sdk/OpenFga.Sdk.csproj

# Then run the example
cd example/StreamedListObjectsExample
dotnet run
```

### Using environment variables

You can configure the example using environment variables:

```bash
export FGA_API_URL="http://localhost:8080"
# Optional OAuth credentials
export FGA_CLIENT_ID="your-client-id"
export FGA_CLIENT_SECRET="your-client-secret"
export FGA_API_AUDIENCE="your-api-audience"
export FGA_API_TOKEN_ISSUER="your-token-issuer"

dotnet run
```

## What it does

1. Creates a temporary store
2. Writes a simple authorization model with `user` and `document` types
3. Writes 10 sample tuples (user:anne can_read document:1-10)
4. Demonstrates the difference between `ListObjects` and `StreamedListObjects`
5. Shows how to handle early cancellation and cleanup
6. Demonstrates using `CancellationToken` for timeout control
7. Cleans up by deleting the temporary store

## Key Features Demonstrated

### IAsyncEnumerable Pattern

The `StreamedListObjects` method returns `IAsyncEnumerable<StreamedListObjectsResponse>`, which is the idiomatic .NET way to handle streaming data:

```csharp
await foreach (var response in fgaClient.StreamedListObjects(request)) {
    Console.WriteLine($"Received: {response.Object}");
}
```

### Early Break and Cleanup

The streaming implementation properly handles early termination:

```csharp
await foreach (var response in fgaClient.StreamedListObjects(request)) {
    Console.WriteLine(response.Object);
    if (someCondition) {
        break; // Stream is automatically cleaned up
    }
}
```

### Cancellation Support

Full support for `CancellationToken`:

```csharp
using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(5));

try {
    await foreach (var response in fgaClient.StreamedListObjects(request, options, cts.Token)) {
        Console.WriteLine(response.Object);
    }
}
catch (OperationCanceledException) {
    Console.WriteLine("Operation cancelled");
}
```

## Benefits Over ListObjects

- **No Pagination**: Retrieve all objects in a single streaming request
- **Lower Memory**: Objects are processed as they arrive, not held in memory
- **Early Termination**: Can stop streaming at any point without wasting resources
- **Better for Large Results**: Ideal when expecting hundreds or thousands of objects

## Performance Considerations

- Streaming starts immediately - no need to wait for all results
- HTTP connection remains open during streaming
- Properly handles cleanup if consumer stops early
- Supports all the same options as `ListObjects` (consistency, contextual tuples, etc.)

