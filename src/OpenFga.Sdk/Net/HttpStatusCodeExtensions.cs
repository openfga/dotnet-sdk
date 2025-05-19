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


namespace System.Net {
    /// <summary>
    /// Extensions for HttpStatusCode
    /// </summary>
    public static class HttpStatusCodeExtensions {
        /// <summary>
        /// Unprocessable Entity status code (422) as defined in RFC 4918
        /// </summary>
        public static HttpStatusCode UnprocessableEntity { get; } = (HttpStatusCode)422;

        /// <summary>
        /// Too Many Requests status code (429) as defined in RFC 6585
        /// </summary>
        public static HttpStatusCode TooManyRequests { get; } = (HttpStatusCode)429;
    }
}