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


namespace OpenFga.Sdk.Telemetry {
    /// <summary>
    /// TagList implementation for telemetry attributes
    /// </summary>
    public class TagList : Dictionary<string, object> {
        // Helper methods to replace typical Add operations

        public void Add(string key) {
            base.Add(key, true);
        }

        public void AddRange(Dictionary<string, object> keyValues) {
            foreach (var item in keyValues) {
                base.Add(item.Key, item.Value);
            }
        }

        public new void Add(string key, object value) {
            base.Add(key, value);
        }
    }

    /// <summary>
    /// Meter implementation for telemetry
    /// </summary>
    public class Meter {
        public string Name { get; }
        public string Version { get; }

        public Meter(string name) {
            Name = name;
            Version = string.Empty;
        }

        public Meter(string name, string version) {
            Name = name;
            Version = version;
        }

        /// <summary>
        /// Creates a counter for the meter
        /// </summary>
        public Counter<T> CreateCounter<T>(string name, string description = null, string unit = null) {
            return new Counter<T>(name);
        }

        /// <summary>
        /// Creates a histogram for the meter
        /// </summary>
        public Histogram<T> CreateHistogram<T>(string name, string description = null, string unit = null) {
            return new Histogram<T>(name);
        }
    }

    /// <summary>
    /// Counter implementation for telemetry
    /// </summary>
    /// <typeparam name="T">Type of the counter</typeparam>
    public class Counter<T> {
        public string Name { get; }

        public Counter(string name) {
            Name = name;
        }

        public void Add(T value, TagList tags = null) {
            // No-op implementation
        }
    }

    /// <summary>
    /// Histogram implementation for telemetry
    /// </summary>
    /// <typeparam name="T">Type of the histogram</typeparam>
    public class Histogram<T> {
        public string Name { get; }

        public Histogram(string name) {
            Name = name;
        }

        public void Record(T value, TagList tags = null) {
            // No-op implementation
        }
    }
}