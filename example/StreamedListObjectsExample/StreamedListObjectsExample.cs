using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace StreamedListObjectsExample;

public class StreamedListObjectsExample {
    public static async Task Main() {
        try {
            var apiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080";

            var client = new OpenFgaClient(new ClientConfiguration { ApiUrl = apiUrl });

            Console.WriteLine("Creating temporary store");
            var store = await client.CreateStore(new ClientCreateStoreRequest { Name = "streamed-list-objects" });

            var clientWithStore = new OpenFgaClient(new ClientConfiguration { 
                ApiUrl = apiUrl, 
                StoreId = store.Id 
            });

            Console.WriteLine("Writing authorization model");
            var authModel = await clientWithStore.WriteAuthorizationModel(new ClientWriteAuthorizationModelRequest {
                SchemaVersion = "1.1",
                TypeDefinitions = new List<TypeDefinition> {
                    new() {
                        Type = "user",
                        Relations = new Dictionary<string, Userset>()
                    },
                    new() {
                        Type = "document",
                        Relations = new Dictionary<string, Userset> {
                            {
                                "can_read", new Userset {
                                    This = new object()
                                }
                            }
                        },
                        Metadata = new Metadata {
                            Relations = new Dictionary<string, RelationMetadata> {
                                {
                                    "can_read", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference> {
                                            new() { Type = "user" }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });

            var fga = new OpenFgaClient(new ClientConfiguration { 
                ApiUrl = apiUrl, 
                StoreId = store.Id,
                AuthorizationModelId = authModel.AuthorizationModelId
            });

            Console.WriteLine("Writing tuples");
            var tuples = new List<ClientTupleKey>();
            for (int i = 1; i <= 2000; i++) {
                tuples.Add(new ClientTupleKey {
                    User = "user:anne",
                    Relation = "can_read",
                    Object = $"document:{i}"
                });
            }
            await fga.WriteTuples(tuples);
            Console.WriteLine($"Wrote {tuples.Count} tuples");

            Console.WriteLine("Streaming objects...");
            var count = 0;
            await foreach (var response in fga.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                },
                new ClientListObjectsOptions {
                    Consistency = ConsistencyPreference.HIGHERCONSISTENCY
                })) {
                count++;
                if (count <= 3 || count % 500 == 0) {
                    Console.WriteLine($"- {response.Object}");
                }
            }
            Console.WriteLine($"✓ Streamed {count} objects");

            Console.WriteLine("Cleaning up...");
            await fga.DeleteStore();
            Console.WriteLine("Done");
        }
        catch (Exception ex) {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
