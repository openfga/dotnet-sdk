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
                                "owner", new Userset {
                                    This = new object()
                                }
                            },
                            {
                                "viewer", new Userset {
                                    This = new object()
                                }
                            },
                            {
                                "can_read", new Userset {
                                    Union = new Usersets {
                                        Child = new List<Userset> {
                                            new() {
                                                ComputedUserset = new ObjectRelation {
                                                    Relation = "owner"
                                                }
                                            },
                                            new() {
                                                ComputedUserset = new ObjectRelation {
                                                    Relation = "viewer"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Metadata = new Metadata {
                            Relations = new Dictionary<string, RelationMetadata> {
                                {
                                    "owner", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference> {
                                            new() { Type = "user" }
                                        }
                                    }
                                },
                                {
                                    "viewer", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference> {
                                            new() { Type = "user" }
                                        }
                                    }
                                },
                                {
                                    "can_read", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference>()
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

            Console.WriteLine("Writing tuples (1000 as owner, 1000 as viewer)");
            var tuples = new List<ClientTupleKey>();
            
            // Write 1000 documents where anne is the owner
            for (int i = 1; i <= 1000; i++) {
                tuples.Add(new ClientTupleKey {
                    User = "user:anne",
                    Relation = "owner",
                    Object = $"document:{i}"
                });
            }
            
            // Write 1000 documents where anne is a viewer
            for (int i = 1001; i <= 2000; i++) {
                tuples.Add(new ClientTupleKey {
                    User = "user:anne",
                    Relation = "viewer",
                    Object = $"document:{i}"
                });
            }
            
            await fga.WriteTuples(tuples);
            Console.WriteLine($"Wrote {tuples.Count} tuples");

            Console.WriteLine("Streaming objects via computed 'can_read' relation...");
            var count = 0;
            await foreach (var response in fga.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",  // Computed: owner OR viewer
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
