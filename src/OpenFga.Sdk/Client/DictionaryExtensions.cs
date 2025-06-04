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
    /// Extensions for Dictionary
    /// </summary>
    public static class DictionaryExtensions {
        /// <summary>
        /// Gets the value associated with the specified key if the key exists in the dictionary, or returns the default value.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search in.</param>
        /// <param name="key">The key to find.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The value associated with the key, or the default value if the key is not found.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default) {
            if (dictionary == null) {
                return defaultValue;
            }

            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}