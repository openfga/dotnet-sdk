using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;
using System;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Utils;

/// <summary>
/// Utility methods for the OpenFGA client
/// </summary>
public static class ClientUtils {
    /// <summary>
    /// Generates a correlation ID using UUID format
    /// </summary>
    /// <returns>A UUID string with hyphens (e.g., "3f2504e0-4f89-11d3-9a0c-0305e82c3301")</returns>
    public static string GenerateCorrelationId() {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Chunks a list into smaller batches of a specified size
    /// </summary>
    /// <typeparam name="T">Type of items in the list</typeparam>
    /// <param name="source">Source list to chunk</param>
    /// <param name="chunkSize">Maximum size of each chunk</param>
    /// <returns>Enumerable of lists, each containing up to chunkSize elements</returns>
    public static IEnumerable<List<T>> ChunkList<T>(List<T> source, int chunkSize) {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (chunkSize <= 0) throw new ArgumentException("Chunk size must be greater than 0", nameof(chunkSize));

#if NET6_0_OR_GREATER
        // Use built-in Enumerable.Chunk for .NET 6+
        return source.Chunk(chunkSize).Select(chunk => chunk.ToList());
#else
        // Manual implementation for older frameworks
        return ChunkListManual(source, chunkSize);
    }

    private static IEnumerable<List<T>> ChunkListManual<T>(List<T> source, int chunkSize) {
        for (int i = 0; i < source.Count; i += chunkSize) {
            yield return source.GetRange(i, Math.Min(chunkSize, source.Count - i));
        }
#endif
    }

    /// <summary>
    /// Transforms a ClientBatchCheckItem into a BatchCheckItem (API model)
    /// </summary>
    /// <param name="item">Client batch check item</param>
    /// <param name="correlationId">Correlation ID to use (should already be generated if needed)</param>
    /// <returns>BatchCheckItem for API call</returns>
    public static BatchCheckItem TransformToBatchCheckItem(ClientBatchCheckItem item, string correlationId) {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new ArgumentException("Correlation ID cannot be null or empty", nameof(correlationId));

        return new BatchCheckItem(
            tupleKey: new CheckRequestTupleKey(
                user: item.User,
                relation: item.Relation,
                varObject: item.Object
            ),
            correlationId: correlationId,
            contextualTuples: item.ContextualTuples,
            context: item.Context
        );
    }
}