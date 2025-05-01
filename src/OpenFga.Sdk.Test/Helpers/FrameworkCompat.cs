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
using System.Threading.Tasks;

namespace OpenFga.Sdk.Test.Helpers {
    /// <summary>
    /// Helper utilities for cross-framework testing
    /// </summary>
    public static class FrameworkCompat {
        /// <summary>
        /// Gets a string representation of the current framework
        /// </summary>
        public static string GetFrameworkName() {
#if NET48
            return ".NET Framework 4.8";
#elif NETCOREAPP3_1
            return ".NET Core 3.1";
#elif NET6_0
            return ".NET 6.0";
#else
            return "Unknown Framework";
#endif
        }

        /// <summary>
        /// Run a test with framework-specific code as needed
        /// </summary>
        public static void RunFrameworkSpecificTest(
            Action defaultAction,
            Action netFrameworkAction = null,
            Action netCoreAction = null,
            Action net6Action = null) {
#if NET48
            if (netFrameworkAction != null)
                netFrameworkAction();
            else
                defaultAction();
#elif NETCOREAPP3_1
            if (netCoreAction != null)
                netCoreAction();
            else
                defaultAction();
#elif NET6_0
            if (net6Action != null)
                net6Action();
            else
                defaultAction();
#else
            defaultAction();
#endif
        }

        /// <summary>
        /// Run an async test with framework-specific code as needed
        /// </summary>
        public static async Task RunFrameworkSpecificTestAsync(
            Func<Task> defaultAction,
            Func<Task> netFrameworkAction = null,
            Func<Task> netCoreAction = null,
            Func<Task> net6Action = null) {
#if NET48
            if (netFrameworkAction != null)
                await netFrameworkAction();
            else
                await defaultAction();
#elif NETCOREAPP3_1
            if (netCoreAction != null)
                await netCoreAction();
            else
                await defaultAction();
#elif NET6_0
            if (net6Action != null)
                await net6Action();
            else
                await defaultAction();
#else
            await defaultAction();
#endif
        }
    }
}