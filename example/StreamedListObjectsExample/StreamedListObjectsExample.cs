using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Model;

namespace StreamedListObjectsExample;

/// <summary>
/// Example demonstrating the StreamedListObjects API.
/// 
/// The Streamed ListObjects API is very similar to the ListObjects API, with two key differences:
/// 1. Instead of collecting all objects before returning a response, it streams them to the client as they are collected.
/// 2. The number of results returned is only limited by the execution timeout (OPENFGA_LIST_OBJECTS_DEADLINE).
/// 
/// This makes it ideal for scenarios where you need to retrieve large numbers of objects without pagination limits.
/// </summary>
public class StreamedListObjectsExample {
    public static async Task Main() {
        try {
            // Configure credentials (if needed)
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

            // Initialize the OpenFGA client
            var configuration = new ClientConfiguration {
                ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080",
                Credentials = credentials
            };
            var fgaClient = new OpenFgaClient(configuration);

            // Create a temporary store
            Console.WriteLine("Creating temporary store...");
            var store = await fgaClient.CreateStore(new ClientCreateStoreRequest { 
                Name = "Streamed List Objects Demo" 
            });
            fgaClient.StoreId = store.Id;
            Console.WriteLine($"Created store with ID: {store.Id}");

            // Write a simple authorization model
            Console.WriteLine("\nWriting authorization model...");
            var authModel = await fgaClient.WriteAuthorizationModel(new ClientWriteAuthorizationModelRequest {
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
                                            new() {
                                                Type = "user"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            fgaClient.AuthorizationModelId = authModel.AuthorizationModelId;
            Console.WriteLine($"Created authorization model with ID: {authModel.AuthorizationModelId}");

            // Write sample tuples
            Console.WriteLine("\nWriting sample tuples...");
            var tuples = new List<ClientTupleKey>();
            for (int i = 1; i <= 10; i++) {
                tuples.Add(new ClientTupleKey {
                    User = "user:anne",
                    Relation = "can_read",
                    Object = $"document:{i}"
                });
            }
            
            await fgaClient.WriteTuples(tuples);
            Console.WriteLine($"Wrote {tuples.Count} tuples to the store");

            // Compare StreamedListObjects vs ListObjects
            Console.WriteLine("\n--- Comparing StreamedListObjects vs ListObjects ---\n");

            // Example 1: Using ListObjects (standard paginated API)
            Console.WriteLine("Using ListObjects (standard API):");
            var standardResponse = await fgaClient.ListObjects(new ClientListObjectsRequest {
                User = "user:anne",
                Relation = "can_read",
                Type = "document"
            }, new ClientListObjectsOptions {
                Consistency = ConsistencyPreference.HIGHERCONSISTENCY
            });
            
            Console.WriteLine($"ListObjects returned {standardResponse.Objects.Count} objects:");
            foreach (var obj in standardResponse.Objects) {
                Console.WriteLine($"  - {obj}");
            }

            // Example 2: Using StreamedListObjects (streaming API)
            Console.WriteLine("\nUsing StreamedListObjects (streaming API):");
            var streamedCount = 0;
            var streamedObjects = new List<string>();
            
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                },
                new ClientListObjectsOptions {
                    Consistency = ConsistencyPreference.HIGHERCONSISTENCY
                })) {
                
                Console.WriteLine($"  - {response.Object}");
                streamedObjects.Add(response.Object);
                streamedCount++;
            }
            
            Console.WriteLine($"StreamedListObjects streamed {streamedCount} objects");

            // Example 3: Demonstrating early cancellation
            Console.WriteLine("\n--- Demonstrating Early Cancellation ---\n");
            Console.WriteLine("Streaming with early break (stopping after 5 objects):");
            
            var cancelCount = 0;
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                })) {
                
                Console.WriteLine($"  - {response.Object}");
                cancelCount++;
                
                if (cancelCount >= 5) {
                    Console.WriteLine("Breaking early - stream automatically cleaned up");
                    break; // Stream is automatically disposed
                }
            }

            // Example 4: Using CancellationToken
            Console.WriteLine("\n--- Demonstrating CancellationToken ---\n");
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(5)); // Cancel after 5 seconds
            
            Console.WriteLine("Streaming with 5-second timeout:");
            try {
                var tokenCount = 0;
                await foreach (var response in fgaClient.StreamedListObjects(
                    new ClientListObjectsRequest {
                        User = "user:anne",
                        Relation = "can_read",
                        Type = "document"
                    },
                    new ClientListObjectsOptions(),
                    cts.Token)) {
                    
                    Console.WriteLine($"  - {response.Object}");
                    tokenCount++;
                }
                Console.WriteLine($"Completed streaming {tokenCount} objects");
            }
            catch (OperationCanceledException) {
                Console.WriteLine("Streaming cancelled via CancellationToken");
            }

            // Clean up
            Console.WriteLine("\nCleaning up...");
            await fgaClient.DeleteStore();
            Console.WriteLine("Deleted temporary store");
            Console.WriteLine("\n✓ Example completed successfully!");
        }
        catch (Exception ex) {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine(ex.StackTrace);
            Environment.Exit(1);
        }
    }
}

