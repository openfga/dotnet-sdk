# Streamed List Objects Example

Demonstrates using `StreamedListObjects` to retrieve objects via the streaming API in the .NET SDK.

## What is StreamedListObjects?

The Streamed ListObjects API is very similar to the ListObjects API, with two key differences:

1. **Streaming Results**: Instead of collecting all objects before returning a response, it streams them to the client as they are collected.
2. **No Pagination Limit**: Returns all results without the 1000-object limit of the standard ListObjects API.

This makes it ideal for scenarios where you need to retrieve large numbers of objects, especially when querying computed relations.

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
- Writes an authorization model with **computed relations**
- Adds 2000 tuples (1000 owners + 1000 viewers)
- Queries the **computed `can_read` relation** via `StreamedListObjects`
- Shows all 2000 results (demonstrating computed relations)
- Shows progress (first 3 objects and every 500th)
- Cleans up the store

## Authorization Model

The example demonstrates OpenFGA's **computed relations**:

```
type user

type document
  relations
    define owner: [user]
    define viewer: [user]
    define can_read: owner or viewer
```

**Why this matters:**
- We write tuples to `owner` and `viewer` (base permissions)
- We query `can_read` (computed from owner OR viewer)

**Example flow:**
1. Write: `user:anne owner document:1-1000`
2. Write: `user:anne viewer document:1001-2000`
3. Query: `StreamedListObjects(user:anne, relation:can_read, type:document)`
4. Result: All 2000 documents (because `can_read = owner OR viewer`)

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
