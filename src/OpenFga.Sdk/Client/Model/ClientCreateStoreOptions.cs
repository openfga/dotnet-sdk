using System.Collections.Generic;

namespace OpenFga.Sdk.Client.Model;

public interface IClientCreateStoreOptions : IClientRequestOptions { }

public class ClientCreateStoreOptions : IClientCreateStoreOptions {
    /// <inheritdoc />
    public IDictionary<string, string>? Headers { get; set; }
}