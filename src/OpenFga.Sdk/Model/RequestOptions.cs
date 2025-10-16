using System.Collections.Generic;

namespace OpenFga.Sdk.Model;

/// <summary>
///     Request Options
/// </summary>
public interface IRequestOptions {
    /// <summary>
    ///     Custom headers to include with this specific request.
    /// </summary>
    IDictionary<string, string>? Headers { get; set; }
}

/// <summary>
///     RequestOptions
/// </summary>
public partial class RequestOptions : IRequestOptions {
    /// <summary>
    ///     Initializes a new instance of the <see cref="RequestOptions" /> class.
    /// </summary>
    public RequestOptions() {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RequestOptions" /> class.
    /// </summary>
    /// <param name="headers">Custom headers to include with this specific request.</param>
    public RequestOptions(IDictionary<string, string>? headers = default) {
        this.Headers = headers;
    }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}