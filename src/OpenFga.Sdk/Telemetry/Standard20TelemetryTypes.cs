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
    /// Meter implementation for telemetry that works with OpenTelemetry if available
    /// </summary>
    public class Meter {
        public string Name { get; }
        public string Version { get; }

        private readonly object? _realMeter;

        public Meter(string name) {
            Name = name;
            Version = string.Empty;
            _realMeter = null;

            // Try to create a real OpenTelemetry meter if the package is available
            try {
                var meterType = Type.GetType("System.Diagnostics.Metrics.Meter, System.Diagnostics.DiagnosticSource");
                if (meterType != null) {
                    _realMeter = Activator.CreateInstance(meterType, new object[] { name });
                }
            }
            catch {
                // Silently continue with the no-op implementation if OpenTelemetry isn't available
            }
        }

        public Meter(string name, string version) {
            Name = name;
            Version = version;
            _realMeter = null;

            // Try to create a real OpenTelemetry meter if the package is available
            try {
                var meterType = Type.GetType("System.Diagnostics.Metrics.Meter, System.Diagnostics.DiagnosticSource");
                if (meterType != null) {
                    _realMeter = Activator.CreateInstance(meterType, new object[] { name, version });
                }
            }
            catch {
                // Silently continue with the no-op implementation if OpenTelemetry isn't available
            }
        }

        /// <summary>
        /// Creates a counter for the meter
        /// </summary>
        public Counter<T> CreateCounter<T>(string name, string description = null, string unit = null) {
            return new Counter<T>(name, _realMeter);
        }

        /// <summary>
        /// Creates a histogram for the meter
        /// </summary>
        public Histogram<T> CreateHistogram<T>(string name, string description = null, string unit = null) {
            return new Histogram<T>(name, _realMeter);
        }
    }

    /// <summary>
    /// Counter implementation for telemetry
    /// </summary>
    /// <typeparam name="T">Type of the counter</typeparam>
    public class Counter<T> {
        public string Name { get; }
        private readonly object? _realCounter;

        public Counter(string name, object? meterInstance = null) {
            Name = name;
            _realCounter = null;

            // Try to create a real counter if the OpenTelemetry meter is available
            try {
                if (meterInstance != null) {
                    var createCounterMethod = meterInstance.GetType().GetMethod("CreateCounter",
                        new[] { typeof(string), typeof(string), typeof(string) });

                    if (createCounterMethod != null) {
                        _realCounter = createCounterMethod.MakeGenericMethod(typeof(T))
                            .Invoke(meterInstance, new object[] { name, null, null });
                    }
                }
            }
            catch {
                // Silently continue with the no-op implementation
            }
        }

        public void Add(T value, TagList tags = null) {
            // Try to call the real Add method if the counter is available
            try {
                if (_realCounter != null) {
                    var addMethod = _realCounter.GetType().GetMethod("Add",
                        new[] { typeof(T), typeof(TagList) });

                    if (addMethod != null) {
                        addMethod.Invoke(_realCounter, new object[] { value, tags });
                    }
                }
            }
            catch {
                // Silently continue as no-op if the method call fails
            }
        }
    }

    /// <summary>
    /// Histogram implementation for telemetry
    /// </summary>
    /// <typeparam name="T">Type of the histogram</typeparam>
    public class Histogram<T> {
        public string Name { get; }
        private readonly object? _realHistogram;

        public Histogram(string name, object? meterInstance = null) {
            Name = name;
            _realHistogram = null;

            // Try to create a real histogram if the OpenTelemetry meter is available
            try {
                if (meterInstance != null) {
                    var createHistogramMethod = meterInstance.GetType().GetMethod("CreateHistogram",
                        new[] { typeof(string), typeof(string), typeof(string) });

                    if (createHistogramMethod != null) {
                        _realHistogram = createHistogramMethod.MakeGenericMethod(typeof(T))
                            .Invoke(meterInstance, new object[] { name, null, null });
                    }
                }
            }
            catch {
                // Silently continue with the no-op implementation
            }
        }

        public void Record(T value, TagList tags = null) {
            // Try to call the real Record method if the histogram is available
            try {
                if (_realHistogram != null) {
                    var recordMethod = _realHistogram.GetType().GetMethod("Record",
                        new[] { typeof(T), typeof(TagList) });

                    if (recordMethod != null) {
                        recordMethod.Invoke(_realHistogram, new object[] { value, tags });
                    }
                }
            }
            catch {
                // Silently continue as no-op if the method call fails
            }
        }
    }
}