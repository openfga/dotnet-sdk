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


using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenFga.Sdk.Client.Extensions {
    /// <summary>
    /// Extension methods for .NET Standard 2.0 compatibility
    /// </summary>
    internal static class LinqExtensions {
#if NETSTANDARD2_0 || NET48
        /// <summary>
        /// Chunks the input sequence into arrays of at most the specified size.
        /// </summary>
        /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence to chunk.</param>
        /// <param name="size">The maximum size of each chunk.</param>
        /// <returns>A sequence of arrays, each containing at most <paramref name="size"/> elements.</returns>
        public static IEnumerable<T[]> Chunk<T>(this IEnumerable<T> source, int size) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));
            var collection = source as ICollection<T>;

            if (collection?.Count == 0) {
                return Enumerable.Empty<T[]>();
            }
            return ChunkIterator(source, size);
        }

        private static IEnumerable<T[]> ChunkIterator<T>(IEnumerable<T> source, int size) {
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext()) {
                var chunk = new List<T>(size);
                do {
                    chunk.Add(enumerator.Current);
                } while (chunk.Count < size && enumerator.MoveNext());
                    
                yield return chunk.ToArray();
            }
        }
#endif
    }
}