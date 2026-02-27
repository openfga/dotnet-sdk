# OpenTelemetry

This SDK produces [metrics](https://opentelemetry.io/docs/concepts/signals/metrics/) using [OpenTelemetry](https://opentelemetry.io/) that allow you to view data such as request timings. These metrics also include attributes for the model and store ID, as well as the API called to allow you to build reporting.

When an OpenTelemetry SDK instance is configured, the metrics will be exported and sent to the collector configured as part of your applications configuration. If you are not using OpenTelemetry, the metric functionality is a no-op and the events are never sent.

In cases when metrics events are sent, they will not be viewable outside of infrastructure configured in your application, and are never available to the OpenFGA team or contributors.

## Metrics

### Supported Metrics

| Metric Name                          | Type      | Enabled by Default | Description                                                                          |
|--------------------------------------|-----------|--------------------|--------------------------------------------------------------------------------------|
| `fga-client.request.duration`        | Histogram | Yes                | The total request time for FGA requests                                              |
| `fga-client.query.duration`          | Histogram | Yes                | The amount of time the FGA server took to internally process and evaluate the request |
| `fga-client.http_request.duration`   | Histogram | No                 | The duration of individual HTTP requests sent by the SDK                             |
| `fga-client.credentials.request`     | Counter   | Yes                | The total number of times a new token was requested when using ClientCredentials     |
| `fga-client.request.count`           | Counter   | No                 | The total number of requests made to the FGA server                                  |

### Supported attributes

| Attribute Name                 | Type     | Enabled by Default | Description                                                                                                                                                 |
|--------------------------------|----------|--------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `fga-client.response.model_id` | `string` | Yes                | The authorization model ID that the FGA server used                                                                                                         |
| `fga-client.request.method`    | `string` | Yes                | The FGA method/action that was performed (e.g. `Check`, `ListObjects`, ...) in TitleCase                                                                    |
| `fga-client.request.store_id`  | `string` | Yes                | The store ID that was sent as part of the request                                                                                                           |
| `fga-client.request.model_id`  | `string` | Yes                | The authorization model ID that was sent as part of the request, if any                                                                                     |
| `fga-client.request.client_id`        | `string` | Yes                | The client ID associated with the request, if any                                                                                                           |
| `fga-client.request.batch_check_size` | `int`    | No                 | The number of checks in a batch check request                                                                                                               |
| `fga-client.user`                     | `string` | No                 | The user that is associated with the action of the request for check and list objects                                                                       |
| `http.request.resend_count`    | `int`    | Yes                | The number of retries attempted (Only sent if the request was retried. Count of `1` means the request was retried once in addition to the original request) |
| `http.response.status_code`    | `int`    | Yes                | The status code of the response                                                                                                                             |
| `http.request.method`          | `string` | No                 | The HTTP method for the request                                                                                                                             |
| `http.host`                    | `string` | Yes                | Host identifier of the origin the request was sent to                                                                                                       |
| `url.scheme`                   | `string` | No                 | HTTP Scheme of the request (`http`/`https`)                                                                                                                 |
| `url.full`                     | `string` | No                 | Full URL of the request                                                                                                                                     |
| `user_agent.original`          | `string` | Yes                | User Agent used in the query                                                                                                                                |

### Default Metrics

Not all metrics and attributes are enabled by default.

Some attributes, like `fga-client.user` have been disabled by default due to their high cardinality, which may result for very high costs when using some SaaS metric collectors.
If you expect to have a high cardinality for a specific attribute, you can disable it by updating the `TelemetryConfig` accordingly.
## Configuration

See the OpenTelemetry docs on [Customizing the SDK](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/customizing-the-sdk/README.md).

```csharp
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;
using OpenFga.Sdk.Telemetry;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics;

namespace Example {
    public class Example {
        public static async Task Main() {
            try {
                // Setup OpenTelemetry Metrics
                using var meterProvider = Sdk.CreateMeterProviderBuilder()
                    .AddHttpClientInstrumentation() // To instrument the default http client
                    .AddMeter(Metrics.Name) // .AddMeter("OpenFga.Sdk") also works
                    .ConfigureResource(resourceBuilder => resourceBuilder.AddService("openfga-dotnet-example"))
                    .AddOtlpExporter() // Required to export to an OTLP compatible endpoint
                    .AddConsoleExporter() // Only needed to export the metrics to the console (e.g. when debugging)
                    .Build();

                // Configure the OpenFGA SDK with default configuration (default metrics and attributes will be enabled)
                var configuration = new ClientConfiguration() {
                    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL"),
                    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"),
                    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"),
                    // Credentials = ... // If needed
                };
                var fgaClient = new OpenFgaClient(configuration);

                // Call the SDK normally
                var response = await fgaClient.ReadAuthorizationModels();
            } catch (ApiException e) {
                 Debug.Print("Error: "+ e);
            }
        }
    }
}
```

#### Customize metrics
You can can customize the metrics that are enabled and the attributes that are included in the metrics by setting the `TelemetryConfig` property on the `ClientConfiguration` object.
If you do set the `Telemetry` property to anything other than `null`, the default configuration will be overridden.

```csharp
TelemetryConfig telemetryConfig = new () {
    Metrics = new Dictionary<string, MetricConfig> {
        [TelemetryMeter.TokenExchangeCount] = new () {
            Attributes = new HashSet<string> {
                TelemetryAttribute.HttpScheme,
                TelemetryAttribute.HttpMethod,
                TelemetryAttribute.HttpHost,
                TelemetryAttribute.HttpStatus,
                TelemetryAttribute.HttpUserAgent,
                TelemetryAttribute.RequestMethod,
                TelemetryAttribute.RequestClientId,
                TelemetryAttribute.RequestStoreId,
                TelemetryAttribute.RequestModelId,
                TelemetryAttribute.RequestRetryCount,
                TelemetryAttribute.ResponseModelId
            }
        },
        [TelemetryMeter.RequestDuration] = new () {
            Attributes = new HashSet<string> {
                TelemetryAttribute.HttpStatus,
                TelemetryAttribute.HttpUserAgent,
                TelemetryAttribute.RequestMethod,
                TelemetryAttribute.RequestClientId,
                TelemetryAttribute.RequestStoreId,
                TelemetryAttribute.RequestModelId,
                TelemetryAttribute.RequestRetryCount,
            }
        },
        [TelemetryMeter.QueryDuration] = new () {
            Attributes = new HashSet<string> {
                TelemetryAttribute.HttpStatus,
                TelemetryAttribute.HttpUserAgent,
                TelemetryAttribute.RequestMethod,
                TelemetryAttribute.RequestClientId,
                TelemetryAttribute.RequestStoreId,
                TelemetryAttribute.RequestModelId,
                TelemetryAttribute.RequestRetryCount,
            }
        },
        [TelemetryMeter.HttpRequestDuration] = new () {
            Attributes = new HashSet<string> {
                TelemetryAttribute.HttpStatus,
                TelemetryAttribute.HttpUserAgent,
                TelemetryAttribute.RequestMethod,
                TelemetryAttribute.RequestClientId,
                TelemetryAttribute.RequestStoreId,
                TelemetryAttribute.RequestModelId,
                TelemetryAttribute.RequestRetryCount,
            }
        },
    }
};

var configuration = new ClientConfiguration() {
    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL"),
    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"),
    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"),
    // Credentials = ... // If needed
    Telemetry = telemetryConfig
};
```

### More Resources
* [OpenTelemetry.Instrumentation.Http](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Instrumentation.Http/README.md) for instrumenting the HttpClient.
* If you are using .NET 8+, checkout the built-in metrics.

A number of these metrics are baked into .NET 8+ as well:

## Example

There is an [example project](https://github.com/openfga/dotnet-sdk/blob/main/example/OpenTelemetryExample) that provides some guidance on how to configure OpenTelemetry available in the examples directory.
