using OpenFga.Sdk.Client.Model;

namespace OpenFga.Sdk.Client;

public class ClientConfiguration : Configuration.Configuration {
    public ClientConfiguration(Configuration.Configuration config) {
        ApiScheme = config.ApiScheme;
        ApiHost = config.ApiHost;
        UserAgent = config.UserAgent;
        Credentials = config.Credentials;
        DefaultHeaders = config.DefaultHeaders;
        StoreId = config.StoreId;
        RetryParams = new RetryParams {MaxRetry = config.MaxRetry, MinWaitInMs = config.MinWaitInMs};
    }

    public ClientConfiguration() { }

    /// <summary>
    ///     Gets or sets the Authorization Model ID.
    /// </summary>
    /// <value>Authorization Model ID.</value>
    public string? AuthorizationModelId { get; set; }

    public RetryParams? RetryParams { get; set; } = new();
}