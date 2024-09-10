using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using OpenFga.Sdk.Telemetry;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using System.Diagnostics;

namespace OpenTelemetryExample;

public class OpenTelemetryExample {
    public static async Task Main() {
        try {
            var credentials = new Credentials();
            if (Environment.GetEnvironmentVariable("FGA_CLIENT_ID") != null) {
                credentials.Method = CredentialsMethod.ClientCredentials;
                credentials.Config = new CredentialsConfig {
                    ApiAudience = Environment.GetEnvironmentVariable("FGA_API_AUDIENCE"),
                    ApiTokenIssuer = Environment.GetEnvironmentVariable("FGA_API_TOKEN_ISSUER"),
                    ClientId = Environment.GetEnvironmentVariable("FGA_CLIENT_ID"),
                    ClientSecret = Environment.GetEnvironmentVariable("FGA_CLIENT_SECRET")
                };
            }
            else if (Environment.GetEnvironmentVariable("FGA_API_TOKEN") != null) {
                credentials.Method = CredentialsMethod.ApiToken;
                credentials.Config = new CredentialsConfig {
                    ApiToken = Environment.GetEnvironmentVariable("FGA_API_TOKEN")
                };
            }

            // Customize the metrics and the attributes on each metric
            TelemetryConfig telemetryConfig = new TelemetryConfig() {
                Metrics = new Dictionary<string, MetricConfig> {
                    [TelemetryMeter.TokenExchangeCount] = new() {
                        Attributes = new HashSet<string> {
                            TelemetryAttribute.HttpHost,
                            TelemetryAttribute.HttpStatus,
                            TelemetryAttribute.HttpUserAgent,
                            TelemetryAttribute.RequestClientId,
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
                }
            };

            var configuration = new ClientConfiguration {
                ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080",
                StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"),
                AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"),
                Credentials = credentials,
                Telemetry = telemetryConfig
            };
            var fgaClient = new OpenFgaClient(configuration);

            // Setup OpenTelemetry
            // See: https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/docs/metrics/customizing-the-sdk/README.md
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .AddHttpClientInstrumentation()
                .AddMeter(Metrics.Name)
                .ConfigureResource(resourceBuilder =>
                    resourceBuilder.AddService(Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ??
                                               "openfga-otel-dotnet-example"))
                .AddOtlpExporter() // Required to export to an OTLP compatible endpoint
                .AddConsoleExporter() // Only needed to export the metrics to the console (e.g. when debugging)
                .Build();

            var performStoreActions = configuration.StoreId == null;
            GetStoreResponse? currentStore = null;
            if (performStoreActions) {
                // ListStores
                Console.WriteLine("Listing Stores");
                var stores1 = await fgaClient.ListStores();
                Console.WriteLine("Stores Count: " + stores1.Stores?.Count());

                // CreateStore
                Console.WriteLine("Creating Test Store");
                var store = await fgaClient.CreateStore(new ClientCreateStoreRequest { Name = "Test Store" });
                Console.WriteLine("Test Store ID: " + store.Id);

                // Set the store id
                fgaClient.StoreId = store.Id;

                // ListStores after Create
                Console.WriteLine("Listing Stores");
                var stores = await fgaClient.ListStores();
                Console.WriteLine("Stores Count: " + stores.Stores?.Count());

                // GetStore
                Console.WriteLine("Getting Current Store");
                currentStore = await fgaClient.GetStore();
                Console.WriteLine("Current Store Name: " + currentStore.Name);
            }

            // ReadAuthorizationModels
            Console.WriteLine("Reading Authorization Models");
            var models = await fgaClient.ReadAuthorizationModels();
            Console.WriteLine("Models Count: " + models.AuthorizationModels?.Count());

            // ReadLatestAuthorizationModel
            var latestAuthorizationModel = await fgaClient.ReadLatestAuthorizationModel();
            if (latestAuthorizationModel != null) {
                Console.WriteLine("Latest Authorization Model ID " + latestAuthorizationModel.AuthorizationModel?.Id);
            }
            else {
                Console.WriteLine("Latest Authorization Model not found");
            }

            // WriteAuthorizationModel
            Console.WriteLine("Writing an Authorization Model");
            var body = new ClientWriteAuthorizationModelRequest {
                SchemaVersion = "1.1",
                TypeDefinitions =
                    new List<TypeDefinition> {
                        new() { Type = "user", Relations = new Dictionary<string, Userset>() },
                        new() {
                            Type = "document",
                            Relations =
                                new Dictionary<string, Userset> {
                                    { "writer", new Userset { This = new object() } }, {
                                        "viewer",
                                        new Userset {
                                            Union = new Usersets {
                                                Child = new List<Userset> {
                                                    new() { This = new object() },
                                                    new() {
                                                        ComputedUserset = new ObjectRelation { Relation = "writer" }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                },
                            Metadata = new Metadata {
                                Relations = new Dictionary<string, RelationMetadata> {
                                    {
                                        "writer",
                                        new RelationMetadata {
                                            DirectlyRelatedUserTypes = new List<RelationReference> {
                                                new() { Type = "user" },
                                                new() { Type = "user", Condition = "ViewCountLessThan200" }
                                            }
                                        }
                                    }, {
                                        "viewer",
                                        new RelationMetadata {
                                            DirectlyRelatedUserTypes = new List<RelationReference> {
                                                new() { Type = "user" }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                Conditions = new Dictionary<string, Condition> {
                    ["ViewCountLessThan200"] = new() {
                        Name = "ViewCountLessThan200",
                        Expression = "ViewCount < 200",
                        Parameters = new Dictionary<string, ConditionParamTypeRef> {
                            ["ViewCount"] = new() { TypeName = TypeName.INT },
                            ["Type"] = new() { TypeName = TypeName.STRING },
                            ["Name"] = new() { TypeName = TypeName.STRING }
                        }
                    }
                }
            };
            var authorizationModel = await fgaClient.WriteAuthorizationModel(body);
            Thread.Sleep(1000);
            Console.WriteLine("Authorization Model ID " + authorizationModel.AuthorizationModelId);

            // ReadAuthorizationModels - after Write
            Console.WriteLine("Reading Authorization Models");
            models = await fgaClient.ReadAuthorizationModels();
            Console.WriteLine("Models Count: " + models.AuthorizationModels?.Count());

            // ReadLatestAuthorizationModel - after Write
            latestAuthorizationModel = await fgaClient.ReadLatestAuthorizationModel();
            Console.WriteLine("Latest Authorization Model ID " + latestAuthorizationModel?.AuthorizationModel?.Id);

            // Set the model ID
            fgaClient.AuthorizationModelId = latestAuthorizationModel?.AuthorizationModel?.Id;

            var contToken = "";
            do {
                // Read All Tuples
                Console.WriteLine("Reading All Tuples (paginated)");
                var existingTuples =
                    await fgaClient.Read(null, new ClientReadOptions { ContinuationToken = contToken });
                contToken = existingTuples.ContinuationToken;

                // Deleting All Tuples
                Console.WriteLine("Deleting All Tuples (paginated)");
                var tuplesToDelete = new List<ClientTupleKeyWithoutCondition>();
                existingTuples.Tuples.ForEach(tuple => tuplesToDelete.Add(new ClientTupleKeyWithoutCondition {
                    User = tuple.Key.User, Relation = tuple.Key.Relation, Object = tuple.Key.Object
                }));
                if (tuplesToDelete.Count > 0) {
                    await fgaClient.DeleteTuples(tuplesToDelete);
                }
            } while (contToken != "");

            // Write
            Console.WriteLine("Writing Tuples");
            await fgaClient.Write(
                new ClientWriteRequest {
                    Writes = new List<ClientTupleKey> {
                        new() {
                            User = "user:anne",
                            Relation = "writer",
                            Object = "document:roadmap",
                            Condition = new RelationshipCondition {
                                Name = "ViewCountLessThan200", Context = new { Name = "Roadmap", Type = "document" }
                            }
                        }
                    }
                }, new ClientWriteOptions { AuthorizationModelId = authorizationModel.AuthorizationModelId });
            Console.WriteLine("Done Writing Tuples");

            // Read
            Console.WriteLine("Reading Tuples");
            var readTuples = await fgaClient.Read();
            Console.WriteLine("Read Tuples" + readTuples.ToJson());

            // ReadChanges
            Console.WriteLine("Reading Tuple Changess");
            var readChangesTuples = await fgaClient.ReadChanges();
            Console.WriteLine("Read Changes Tuples" + readChangesTuples.ToJson());

            // Check
            Console.WriteLine("Checking for access");
            try {
                var failingCheckResponse = await fgaClient.Check(new ClientCheckRequest {
                    User = "user:anne", Relation = "viewer", Object = "document:roadmap"
                });
                Console.WriteLine("Allowed: " + failingCheckResponse.Allowed);
            }
            catch (Exception e) {
                Console.WriteLine("Failed due to: " + e.Message);
            }

            // Checking for access with context
            Console.WriteLine("Checking for access with context");
            var checkResponse = await fgaClient.Check(new ClientCheckRequest {
                User = "user:anne", Relation = "viewer", Object = "document:roadmap", Context = new { ViewCount = 100 }
            });
            Console.WriteLine("Allowed: " + checkResponse.Allowed);

            // WriteAssertions
            await fgaClient.WriteAssertions(new List<ClientAssertion> {
                new() { User = "user:carl", Relation = "writer", Object = "document:budget", Expectation = true },
                new() { User = "user:anne", Relation = "viewer", Object = "document:roadmap", Expectation = false }
            });
            Console.WriteLine("Assertions updated");

            // ReadAssertions
            Console.WriteLine("Reading Assertions");
            var assertions = await fgaClient.ReadAssertions();
            Console.WriteLine("Assertions " + assertions.ToJson());

            // Checking for access w/ context in a loop
            var rnd = new Random();
            var randomNumber = rnd.Next(1, 1000);
            Console.WriteLine($"Checking for access with context in a loop ({randomNumber} times)");
            for (var index = 0; index < randomNumber; index++) {
                checkResponse = await fgaClient.Check(new ClientCheckRequest {
                    User = "user:anne",
                    Relation = "viewer",
                    Object = "document:roadmap",
                    Context = new { ViewCount = 100 }
                });
                Console.WriteLine("Allowed: " + checkResponse.Allowed);
            }

            if (performStoreActions) {
                // DeleteStore
                Console.WriteLine("Deleting Current Store");
                await fgaClient.DeleteStore();
                Console.WriteLine("Deleted Store: " + currentStore?.Name);
            }
        }
        catch (ApiException e) {
            Console.WriteLine("Error: " + e);
            Debug.Print("Error: " + e);
        }
    }
}