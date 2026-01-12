using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
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
            
            // Write in batches of 100 (OpenFGA limit)
            const int batchSize = 100;
            int totalWritten = 0;
            
            // Write 1000 documents where anne is the owner
            for (int batch = 0; batch < 10; batch++) {
                var tuples = new List<ClientTupleKey>();
                for (int i = 1; i <= batchSize; i++) {
                    tuples.Add(new ClientTupleKey {
                        User = "user:anne",
                        Relation = "owner",
                        Object = $"document:{batch * batchSize + i}"
                    });
                }
                await fga.WriteTuples(tuples);
                totalWritten += tuples.Count;
            }
            
            // Write 1000 documents where anne is a viewer
            for (int batch = 0; batch < 10; batch++) {
                var tuples = new List<ClientTupleKey>();
                for (int i = 1; i <= batchSize; i++) {
                    tuples.Add(new ClientTupleKey {
                        User = "user:anne",
                        Relation = "viewer",
                        Object = $"document:{1000 + batch * batchSize + i}"
                    });
                }
                await fga.WriteTuples(tuples);
                totalWritten += tuples.Count;
            }
            
            Console.WriteLine($"Wrote {totalWritten} tuples");

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
            // Avoid logging sensitive data; only display generic info
            if (ex is FgaValidationError) {
                Console.Error.WriteLine("Validation error in configuration. Please check your configuration for errors.");
            } else if (ex.Message?.Contains("Connection refused") == true || ex.InnerException?.Message?.Contains("Connection refused") == true) {
                Console.Error.WriteLine("Is OpenFGA server running? Check FGA_API_URL environment variable or default http://localhost:8080");
            } else {
                Console.Error.WriteLine($"An error occurred. [{ex.GetType().Name}]");
            }
            Environment.Exit(1);
        }
    }
}
