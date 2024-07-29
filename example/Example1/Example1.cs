using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System.Diagnostics;

namespace Example1;

public class Example1 {
    public static async Task Main() {
        try {
            var credentials = new Credentials();
            if (Environment.GetEnvironmentVariable("FGA_CLIENT_ID") != null) {
                credentials.Method = CredentialsMethod.ClientCredentials;
                credentials.Config = new CredentialsConfig() {
                    ApiAudience = Environment.GetEnvironmentVariable("FGA_API_AUDIENCE"),
                    ApiTokenIssuer = Environment.GetEnvironmentVariable("FGA_API_TOKEN_ISSUER"),
                    ClientId = Environment.GetEnvironmentVariable("FGA_CLIENT_ID"),
                    ClientSecret = Environment.GetEnvironmentVariable("FGA_CLIENT_SECRET")
                };
            }

            var configuration = new ClientConfiguration {
                ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL") ?? "http://localhost:8080", // required, e.g. https://api.fga.example
                StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
                AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
                Credentials = credentials
            };
            var fgaClient = new OpenFgaClient(configuration);

            // ListStores
            Console.WriteLine("Listing Stores");
            var stores1 = await fgaClient.ListStores();
            Console.WriteLine("Stores Count: " + stores1.Stores?.Count());

            // CreateStore
            Console.WriteLine("Creating Test Store");
            var store = await fgaClient.CreateStore(new ClientCreateStoreRequest {Name = "Test Store"});
            Console.WriteLine("Test Store ID: " + store.Id);

            // Set the store id
            fgaClient.StoreId = store.Id;

            // ListStores after Create
            Console.WriteLine("Listing Stores");
            var stores = await fgaClient.ListStores();
            Console.WriteLine("Stores Count: " + stores.Stores?.Count());

            // GetStore
            Console.WriteLine("Getting Current Store");
            var currentStore = await fgaClient.GetStore();
            Console.WriteLine("Current Store Name: " + currentStore.Name);

            // ReadAuthorizationModels
            Console.WriteLine("Reading Authorization Models");
            var models = await fgaClient.ReadAuthorizationModels();
            Console.WriteLine("Models Count: " + models.AuthorizationModels?.Count());

            // ReadLatestAuthorizationModel
            var latestAauthorizationModel = await fgaClient.ReadLatestAuthorizationModel();
            if (latestAauthorizationModel != null) {
                Console.WriteLine("Latest Authorization Model ID " + latestAauthorizationModel.AuthorizationModel.Id);
            }
            else {
                Console.WriteLine("Latest Authorization Model not found");
            }

            // WriteAuthorizationModel
            Console.WriteLine("Writing an Authorization Model");
            var body = new ClientWriteAuthorizationModelRequest {
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
                                "writer", new Userset {
                                    This = new object()
                                }
                            }, {
                                "viewer",
                                new Userset {
                                    Union = new Usersets {
                                        Child = new List<Userset> {
                                            new() {
                                                This = new object()
                                            },
                                            new() {
                                                ComputedUserset = new ObjectRelation {
                                                    Relation = "writer"
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
                                    "writer", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference> {
                                            new() {Type = "user"},
                                            new() {Type = "user", Condition = "ViewCountLessThan200"}
                                        }
                                    }
                                }, {
                                    "viewer", new RelationMetadata {
                                        DirectlyRelatedUserTypes = new List<RelationReference> {
                                            new() {Type = "user"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Conditions = new Dictionary<string, Condition> {
                    ["ViewCountLessThan200"] = new Condition() {
                        Name = "ViewCountLessThan200",
                        Expression = "ViewCount < 200",
                        Parameters = new Dictionary<string, ConditionParamTypeRef> {
                            ["ViewCount"] = new ConditionParamTypeRef {
                                TypeName = TypeName.INT
                            },
                            ["Type"] = new ConditionParamTypeRef {
                                TypeName = TypeName.STRING
                            },
                            ["Name"] = new ConditionParamTypeRef {
                                TypeName = TypeName.STRING
                            }
                        }
                    }
                }
            };
            var authorizationModel = await fgaClient.WriteAuthorizationModel(body);
            Console.WriteLine("Authorization Model ID " + authorizationModel.AuthorizationModelId);

            // ReadAuthorizationModels - after Write
            Console.WriteLine("Reading Authorization Models");
            models = await fgaClient.ReadAuthorizationModels();
            Console.WriteLine("Models Count: " + models.AuthorizationModels?.Count());

            // ReadLatestAuthorizationModel - after Write
            latestAauthorizationModel = await fgaClient.ReadLatestAuthorizationModel();
            Console.WriteLine("Latest Authorization Model ID " + latestAauthorizationModel.AuthorizationModel.Id);

            // Set the model ID
            fgaClient.AuthorizationModelId = latestAauthorizationModel.AuthorizationModel.Id;

            // Write
            Console.WriteLine("Writing Tuples");
            await fgaClient.Write(new ClientWriteRequest {
                Writes = new List<ClientTupleKey> {
                    new() {
                        User = "user:anne",
                        Relation = "writer",
                        Object = "document:roadmap",
                        Condition = new RelationshipCondition() {
                            Name = "ViewCountLessThan200",
                            Context = new { Name = "Roadmap", Type = "document" }
                        }
                    }
                }
            }, new ClientWriteOptions {
                AuthorizationModelId = authorizationModel.AuthorizationModelId
            });
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
                    User = "user:anne",
                    Relation = "viewer",
                    Object = "document:roadmap"
                });
                Console.WriteLine("Allowed: " + failingCheckResponse.Allowed);
            }
            catch (Exception e) {
                Console.WriteLine("Failed due to: " + e.Message);
            }

            // Checking for access with context
            Console.WriteLine("Checking for access with context");
            var checkResponse = await fgaClient.Check(new ClientCheckRequest {
                User = "user:anne",
                Relation = "viewer",
                Object = "document:roadmap",
                Context = new { ViewCount = 100 }
            });
            Console.WriteLine("Allowed: " + checkResponse.Allowed);

            // WriteAssertions
            await fgaClient.WriteAssertions(new List<ClientAssertion>() {
                new ClientAssertion() {
                    User = "user:carl",
                    Relation = "writer",
                    Object = "document:budget",
                    Expectation = true,
                },
                new ClientAssertion() {
                    User = "user:anne",
                    Relation = "viewer",
                    Object = "document:roadmap",
                    Expectation = false,
                }
            });
            Console.WriteLine("Assertions updated");

            // ReadAssertions
            Console.WriteLine("Reading Assertions");
            var assertions = await fgaClient.ReadAssertions();
            Console.WriteLine("Assertions " + assertions.ToJson());

            // DeleteStore
            Console.WriteLine("Deleting Current Store");
            await fgaClient.DeleteStore();
            Console.WriteLine("Deleted Store: " + currentStore.Name);
        }
        catch (ApiException e) {
            Console.WriteLine("Error: " + e);
            Debug.Print("Error: " + e);
        }
    }
}