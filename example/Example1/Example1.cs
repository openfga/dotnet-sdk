using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Configuration;
using OpenFga.Sdk.Exceptions;
using OpenFga.Sdk.Model;
using System.Diagnostics;

namespace Example1;

public class Example1 {
    public static async Task Main() {
        var shouldSkipListingStores = false;
        var shouldSkipStoreMethods = false;

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

            GetStoreResponse? currentStore = null;
            if (shouldSkipStoreMethods) {
                Console.WriteLine("Skipping initial store creation");
            }
            else {
                if (shouldSkipListingStores) {
                    Console.WriteLine("Skipping Listing Stores");
                }
                else {
                    // ListStores
                    Console.WriteLine("Listing Stores");
                    var stores1 = await fgaClient.ListStores();
                    Console.WriteLine("Stores Count: " + stores1.Stores?.Count());
                }

                // CreateStore
                Console.WriteLine("Creating Test Store");
                var store = await fgaClient.CreateStore(new ClientCreateStoreRequest {Name = "Test Store"});
                Console.WriteLine("Created Test Store ID: " + store.Id);

                // Set the store id
                fgaClient.StoreId = store.Id;

                if (shouldSkipListingStores) {
                    Console.WriteLine("Skipping Listing Stores");
                }
                else {
                    // ListStores after Create
                    Console.WriteLine("Listing Stores");
                    var stores = await fgaClient.ListStores();
                    Console.WriteLine("Stores Count: " + stores.Stores?.Count());
                }

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
            Thread.Sleep(10_000);
            Console.WriteLine("Reading Authorization Models");
            models = await fgaClient.ReadAuthorizationModels();
            Console.WriteLine("Models Count: " + models.AuthorizationModels?.Count());

            // ReadLatestAuthorizationModel - after Write
            latestAauthorizationModel = await fgaClient.ReadLatestAuthorizationModel();
            if (latestAauthorizationModel == null) {
                throw new Exception("Cannot find module that was written");
            }
            else {
                var latestModelId = latestAauthorizationModel.AuthorizationModel?.Id;
                Console.WriteLine("Latest Authorization Model ID " + latestModelId);

                // Set the model ID
                fgaClient.AuthorizationModelId = latestModelId;
            }

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

            // Batch checking for access with context
            Console.WriteLine("Batch checking for access with context");
            var batchCheckResponse = await fgaClient.BatchCheck(new List<ClientCheckRequest>() {
                new () {
                    User = "user:anne",
                    Relation = "viewer",
                    Object = "document:roadmap",
                    Context = new { ViewCount = 100 }
                }
            });
            Console.WriteLine("Responses[0].Allowed: " + batchCheckResponse.Responses[0].Allowed);

            // Listing relations with context
            Console.WriteLine("Listing relations with context");
            var listRelationsResponse = await fgaClient.ListRelations(new ClientListRelationsRequest() {
                User = "user:anne",
                Relations = new List<string>() {"viewer", "writer"},
                Object = "document:roadmap",
                Context = new { ViewCount = 100 }
            });
            // var allowedRelations = new List<string>();
            // listRelationsResponse.Relations?.ForEach(r => {
            //     allowedRelations.Add(r);
            // });
            Console.WriteLine("Relations: " + string.Join(" | ", listRelationsResponse.Relations!));

            // Listing objects with context
            Console.WriteLine("Listing objects with context");
            var listObjectsResponse = await fgaClient.ListObjects(new ClientListObjectsRequest() {
                User = "user:anne",
                Relation = "viewer",
                Type = "document",
                Context = new { ViewCount = 100 }
            });
            // var allowedObjects = new List<string>();
            // listObjectsResponse.Objects?.ForEach(o => {
            //     allowedObjects.Add(o);
            // });
            Console.WriteLine("Objects: " + string.Join(" | ", listObjectsResponse.Objects!));

            // Listing users with context
            Console.WriteLine("Listing users with context");
            var listUsersResponse = await fgaClient.ListUsers(new ClientListUsersRequest() {
                UserFilters = new List<UserTypeFilter>() {
                    new () { Type = "user" },
                },
                Relation = "viewer",
                Object = new FgaObject() {
                    Type = "document",
                    Id = "roadmap"
                },
                Context = new { ViewCount = 100 }
            });
            var allowedUsers = new List<string>();
            listUsersResponse.Users?.ForEach(u => {
                allowedUsers.Add(u.ToJson());
            });
            Console.WriteLine("Users: " + string.Join(" | ", allowedUsers));

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

            if (shouldSkipStoreMethods || currentStore == null) {
                Console.WriteLine("Skipping store deletion");
            }
            else {
                // DeleteStore
                Console.WriteLine("Deleting Test Store");
                await fgaClient.DeleteStore();
                Console.WriteLine("Deleted Store: " + currentStore.Name);
            }
        }
        catch (ApiException e) {
            Console.WriteLine("Error: " + e);
            Debug.Print("Error: " + e);
        }
    }
}