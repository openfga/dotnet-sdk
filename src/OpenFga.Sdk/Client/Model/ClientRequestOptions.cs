using OpenFga.Sdk.Model;
using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     Client Request Options
/// </summary>
public interface IClientRequestOptions : IRequestOptions {
}

/// <summary>
///     Client Request Options
/// </summary>
public partial class ClientRequestOptions : IClientRequestOptions {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientRequestOptions" /> class.
    /// </summary>
    public ClientRequestOptions() {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientRequestOptions" /> class.
    /// </summary>
    /// <param name="headers">Custom headers to include with this specific request.</param>
    public ClientRequestOptions(IDictionary<string, string>? headers = default) {
        this.Headers = headers;
    }

    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}