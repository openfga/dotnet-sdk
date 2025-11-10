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

```bash
# From the SDK root directory, build the SDK first
dotnet build src/OpenFga.Sdk/OpenFga.Sdk.csproj

# Then run the example
cd example/StreamedListObjectsExample
dotnet run
```

## What it does

- Creates a temporary store
- Writes a simple authorization model
- Adds 2000 tuples
- Streams results via `StreamedListObjects`
- Shows progress (first 3 objects and every 500th)
- Cleans up the store

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
