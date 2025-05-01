//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 1.x
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://openfga.dev/community
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//

namespace OpenFga.Sdk.Client {
    /// <summary>
    /// Extensions methods for collections
    /// </summary>
    internal static class Extensions {
        /// <summary>
        /// Splits a list into chunks of a specified size.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <param name="source">The source list to chunk.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>An enumerable of chunks.</returns>
        public static IEnumerable<List<T>> Chunk<T>(this List<T> source, int chunkSize) {
            for (int i = 0; i < source.Count; i += chunkSize) {
                yield return source.GetRange(i, Math.Min(chunkSize, source.Count - i));
            }
        }

        /// <summary>
        /// Asynchronously executes an action for each element in a collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="action">The action to execute for each element.</param>
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism.</param>
        /// <returns>A task that represents the completion of all parallel operations.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> action, int maxDegreeOfParallelism = 5) {
            var tasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: maxDegreeOfParallelism);

            foreach (var item in source) {
                await throttler.WaitAsync();

                tasks.Add(Task.Run(async () => {
                    try {
                        await action(item);
                    }
                    finally {
                        throttler.Release();
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }
    }
}