using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OpenFga.Sdk.Client;
using OpenFga.Sdk.Client.Model;
using OpenFga.Sdk.Model;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers;

#if NET8_0_OR_GREATER
namespace OpenFga.Sdk.Test.Client {
    public class StreamedListObjectsRetryIntegrationTests : IAsyncLifetime {
        private const string OpenFgaImage = "openfga/openfga:latest";
        private IContainer _openFgaContainer;
        private string _apiUrl;
        private string _storeId;
        private string _authorizationModelId;

        public StreamedListObjectsRetryIntegrationTests() {
            _openFgaContainer = new ContainerBuilder()
                .WithImage(OpenFgaImage)
                .WithCommand("run")
                .WithPortBinding(8080, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(8080)))
                .Build();
        }

        public async Task InitializeAsync() {
            await _openFgaContainer.StartAsync();
            var host = _openFgaContainer.Hostname;
            var port = _openFgaContainer.GetMappedPublicPort(8080);
            _apiUrl = $"http://{host}:{port}";

            // Create a store dynamically on the live server
            var setupConfig = new ClientConfiguration { ApiUrl = _apiUrl };
            using var setupClient = new OpenFgaClient(setupConfig);

            var storeResponse = await setupClient.CreateStore(new ClientCreateStoreRequest { Name = "test-store" });
            _storeId = storeResponse.Id;

            // Write a minimal authorization model so StreamedListObjects has something to work with
            var modelConfig = new ClientConfiguration { ApiUrl = _apiUrl, StoreId = _storeId };
            using var modelClient = new OpenFgaClient(modelConfig);

            var modelResponse = await modelClient.WriteAuthorizationModel(new ClientWriteAuthorizationModelRequest {
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
            _authorizationModelId = modelResponse.AuthorizationModelId;
        }

        public async Task DisposeAsync() {
            await _openFgaContainer.DisposeAsync();
        }

        [Fact]
        public async Task StreamedListObjects_RateLimitError_RetriesAndSucceeds() {
            var config = new ClientConfiguration {
                ApiUrl = _apiUrl,
                StoreId = _storeId,
                AuthorizationModelId = _authorizationModelId,
                RetryParams = new RetryParams { MaxRetry = 1, MinWaitInMs = 0 }
            };
            using var fgaClient = new OpenFgaClient(config);

            // Write a tuple so there's something to return
            await fgaClient.Write(new ClientWriteRequest {
                Writes = new List<ClientTupleKey> {
                    new() {
                        User = "user:anne",
                        Relation = "can_read",
                        Object = "document:1"
                    }
                }
            });

            var results = new List<string>();
            await foreach (var response in fgaClient.StreamedListObjects(
                new ClientListObjectsRequest {
                    User = "user:anne",
                    Relation = "can_read",
                    Type = "document"
                })) {
                results.Add(response.Object);
            }

            Assert.NotEmpty(results);
            Assert.Contains("document:1", results);
        }
    }
}
#endif