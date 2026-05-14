# Changelog

## [0.11.0](https://github.com/openfga/dotnet-sdk/compare/v0.10.1...v0.11.0) (2026-05-14)


### ⚠ BREAKING CHANGES

* 
* remove excluded users from ListUsers response ([#64](https://github.com/openfga/dotnet-sdk/issues/64))
* remove excluded users from ListUsers response
* **v0.3.0:** support for conditions and some breaking changes ([#36](https://github.com/openfga/dotnet-sdk/issues/36))
* **v0.3.0:** support for conditions and some breaking changes
* allow overriding store id per request
* support conditions
* regenerate based on api@061d8d8d395fd4791215d1fb4886bc3fcdc2d637
* rename `TypeDefinitions` interface to `WriteAuthorizationModelRequest`

### Added

* add batch check telemetry attribute ([#181](https://github.com/openfga/dotnet-sdk/issues/181)) ([a7d2019](https://github.com/openfga/dotnet-sdk/commit/a7d201902c24c645b9965d943012633a4dc92716))
* add OpenFgaClient wrapping OpenFgaApi ([a9086b8](https://github.com/openfga/dotnet-sdk/commit/a9086b8b11a4252501c8b3d45ad2fa75902f3b17))
* add retries to client credential requests ([c16208a](https://github.com/openfga/dotnet-sdk/commit/c16208a711db6c4118fcd4c0ffb44083e3e323e9))
* add retries to client credential requests ([#51](https://github.com/openfga/dotnet-sdk/issues/51)) ([3205d9c](https://github.com/openfga/dotnet-sdk/commit/3205d9c9e77b3515818fed608a490298358d97af))
* add StreamedListObjects support ([#156](https://github.com/openfga/dotnet-sdk/issues/156)) ([cf92422](https://github.com/openfga/dotnet-sdk/commit/cf92422baf5b2a6184237f78abe739169d62d816))
* add support for consistency param ([b8d1210](https://github.com/openfga/dotnet-sdk/commit/b8d12104b2cf3a1d2d248949d89e87ce7166e303))
* add support for exporting metrics ([ecb0914](https://github.com/openfga/dotnet-sdk/commit/ecb09146f0ecf47b2bfc7bae1c0304611d807bda))
* add support for exporting metrics ([#69](https://github.com/openfga/dotnet-sdk/issues/69)) ([ba3c108](https://github.com/openfga/dotnet-sdk/commit/ba3c108127800d9c53bebbdfbdc1082bea21cc55))
* add support for ListObjects API endpoint ([1318771](https://github.com/openfga/dotnet-sdk/commit/1318771d97f65c25d613ea7fa4a2cdaa9f0d17f8))
* add support for modular models metadata ([450b688](https://github.com/openfga/dotnet-sdk/commit/450b68899ca7c31a72ca18c29505dd104d0d1530))
* add support for modular models metadata ([#53](https://github.com/openfga/dotnet-sdk/issues/53)) ([5509927](https://github.com/openfga/dotnet-sdk/commit/55099271ee97e7e206076ba601948563722715dc))
* add support for specifying consistency parameter ([#70](https://github.com/openfga/dotnet-sdk/issues/70)) ([f020bc6](https://github.com/openfga/dotnet-sdk/commit/f020bc67747323a78b3c8d04708617ac6a52007f))
* add write conflict resolution options ([#136](https://github.com/openfga/dotnet-sdk/issues/136)) ([774dda0](https://github.com/openfga/dotnet-sdk/commit/774dda05e77070f05090690e2161ad08ae62b563))
* allow overriding store id per request ([2de496f](https://github.com/openfga/dotnet-sdk/commit/2de496f6005dd4aef06af961e5f254d8a7fbf49d))
* Allow the api token issuer to be a fully qualified url with a path component ([#151](https://github.com/openfga/dotnet-sdk/issues/151)) ([e7a67f3](https://github.com/openfga/dotnet-sdk/commit/e7a67f34c6822065ba3f7d444a1fbd4aaf764447))
* ApiExecutor for raw requests ([#176](https://github.com/openfga/dotnet-sdk/issues/176)) ([1099b78](https://github.com/openfga/dotnet-sdk/commit/1099b782b0acd1a3f0754d7bbe96fee30e8feb94))
* batch check fallback to client level authmodel ID ([#199](https://github.com/openfga/dotnet-sdk/issues/199)) ([733c4c3](https://github.com/openfga/dotnet-sdk/commit/733c4c3438bc899643c8755fb4ed3e5085b86a5a))
* **config:** update auth opts & allow no storeId for certain requests ([ced4960](https://github.com/openfga/dotnet-sdk/commit/ced496049d584a9eead968a9486ab42c1c060a29))
* **dotnet-sdk:** Added support for an interface on the client ([#168](https://github.com/openfga/dotnet-sdk/issues/168)) ([c119407](https://github.com/openfga/dotnet-sdk/commit/c1194070640a3f2be68d25fe7f0be272ff1f54af))
* draft releases, version verification, and reusable workflows ([#210](https://github.com/openfga/dotnet-sdk/issues/210)) ([04b5ad1](https://github.com/openfga/dotnet-sdk/commit/04b5ad1ef3ba72930722e57771f818cc7679fcdb))
* enable FromJson use in Client methods ([#180](https://github.com/openfga/dotnet-sdk/issues/180)) ([8939070](https://github.com/openfga/dotnet-sdk/commit/8939070b6dfa0a77533c4acdcfd7338ad124e21a))
* Implement server-side BatchCheck using /batch-check endpoint ([#150](https://github.com/openfga/dotnet-sdk/issues/150)) ([8483107](https://github.com/openfga/dotnet-sdk/commit/84831070f4a52fd1de62ff04e924cbf24486e9c8))
* initial commit ([ea77389](https://github.com/openfga/dotnet-sdk/commit/ea77389c8b485ede840c59a47170ef5652b6ba0f))
* per-request custom headers ([#133](https://github.com/openfga/dotnet-sdk/issues/133)) ([e5c46f5](https://github.com/openfga/dotnet-sdk/commit/e5c46f5d7d01340311ed366d073305e00a4a1868))
* release automation configs ([#193](https://github.com/openfga/dotnet-sdk/issues/193)) ([ec3acde](https://github.com/openfga/dotnet-sdk/commit/ec3acdecf78da7a5f5049b88f0e2f59e422c1e12))
* report a per http call metric ([#173](https://github.com/openfga/dotnet-sdk/issues/173)) ([66b8d2f](https://github.com/openfga/dotnet-sdk/commit/66b8d2fb143ca425ca2c609307a22efe9a65d77a))
* streaming api executor ([#189](https://github.com/openfga/dotnet-sdk/issues/189)) ([6bebdf7](https://github.com/openfga/dotnet-sdk/commit/6bebdf758f855e27f4d6b45d4c242f7ea10c6c52))
* support .NET Standard 2.0 ([#122](https://github.com/openfga/dotnet-sdk/issues/122)) ([7f6db0a](https://github.com/openfga/dotnet-sdk/commit/7f6db0a72725c2ec7efbd3561e8bad3f1114910c))
* support `Retry-After` headers ([#139](https://github.com/openfga/dotnet-sdk/issues/139)) ([874130c](https://github.com/openfga/dotnet-sdk/commit/874130caccaadc7e029191ed5463c0669271ae39))
* support conditions ([545a0e3](https://github.com/openfga/dotnet-sdk/commit/545a0e39cd0e127d84d8c555ab95415755d25d77))
* support list users ([3f3e5b8](https://github.com/openfga/dotnet-sdk/commit/3f3e5b856ac4dba1bf1bf38697d7ce6d37e5940a))
* support list users ([#57](https://github.com/openfga/dotnet-sdk/issues/57)) ([cc110af](https://github.com/openfga/dotnet-sdk/commit/cc110af2711bca023100b6b654159f2bb5040317))
* support nuget prereleases ([#109](https://github.com/openfga/dotnet-sdk/issues/109)) ([f88bbec](https://github.com/openfga/dotnet-sdk/commit/f88bbeced119811ce1f8511f54f79ce2126c691a))
* use ApiUrl and deprecate ApiSchema and ApiHost ([a0d4234](https://github.com/openfga/dotnet-sdk/commit/a0d42344d192f8475f606d9cfce52838fdabfa6b))
* validate that store id and auth model id in ulid format ([7c82d78](https://github.com/openfga/dotnet-sdk/commit/7c82d7886db2a71e95c87580a1f7f7c9a048b376))


### Fixed

* `ClientListObjects.ContextualTuples` is now `ClientTupleKey[]` ([02c4f15](https://github.com/openfga/dotnet-sdk/commit/02c4f151418fd4db744052bca71b0ed78081e92d))
* add ClientCreateStoreOptions ([1c17c24](https://github.com/openfga/dotnet-sdk/commit/1c17c243cecc09030c53a1313f8eb9d27bf6ab06))
* add contents read perm to publish workflow ([fd16e9c](https://github.com/openfga/dotnet-sdk/commit/fd16e9c67769777e6f40187e702bc562b546a6b2))
* add contents read permission to publish workflow ([#166](https://github.com/openfga/dotnet-sdk/issues/166)) ([fd16e9c](https://github.com/openfga/dotnet-sdk/commit/fd16e9c67769777e6f40187e702bc562b546a6b2))
* **client:** read all tuples ([ea615e8](https://github.com/openfga/dotnet-sdk/commit/ea615e857e3194a569cb819e9b3b82d54250d55b))
* **client:** read all tuples ([#32](https://github.com/openfga/dotnet-sdk/issues/32)) ([2b64d9e](https://github.com/openfga/dotnet-sdk/commit/2b64d9ebc78d90924b4ae9cc4f127dcc54e5d274))
* ClientWriteAssertions ([812d2f6](https://github.com/openfga/dotnet-sdk/commit/812d2f6da6cabef8c945ad0ff454195f98e9239a))
* code coverage reporting ([e7feab8](https://github.com/openfga/dotnet-sdk/commit/e7feab8d6b102127aa24549a07c71a8ecadcdfe5))
* code coverage reporting ([ce5eb04](https://github.com/openfga/dotnet-sdk/commit/ce5eb04a9de8bcc56eccb00147f381982492b304))
* code coverage reporting ([9dadff1](https://github.com/openfga/dotnet-sdk/commit/9dadff1659f5db3afbc536e50f3685806f1c03eb))
* deserializng nullable DateTime ([3f69211](https://github.com/openfga/dotnet-sdk/commit/3f69211c5a92f7a05ea92d49e081a1874e35be2c))
* do not crash in error handling when storeId missing ([2c2358f](https://github.com/openfga/dotnet-sdk/commit/2c2358f5eaa6da18e7498659875bd8e23caf4249))
* do not retry on &lt;5xx (except 429), 501 ([1147282](https://github.com/openfga/dotnet-sdk/commit/1147282084c2472262f7c9c673a955508fc903cb))
* evaluate client creds token expiry period in seconds instead of ms ([7f72628](https://github.com/openfga/dotnet-sdk/commit/7f726283bde4573e79115e2e0e290c2361e34f45))
* evaluate client creds token expiry period in seconds instead of ms ([#20](https://github.com/openfga/dotnet-sdk/issues/20)) ([bb06e2c](https://github.com/openfga/dotnet-sdk/commit/bb06e2c0c616422e762fa0a254182d50cb46a316))
* exception when using the same configuration with multiple clients ([802ce9c](https://github.com/openfga/dotnet-sdk/commit/802ce9cff2243e18234138fb738c8c7dd7a57df0))
* fix example makefile to run openfga ([2fefac1](https://github.com/openfga/dotnet-sdk/commit/2fefac1991288edd0bd18f53bb6c1db01714341e))
* fixes to Client Write and the README ([f6800b1](https://github.com/openfga/dotnet-sdk/commit/f6800b16065ce1217664efefcfff7ac6f20150e0))
* interface for ClientListObjects ([47708a6](https://github.com/openfga/dotnet-sdk/commit/47708a65b44b8d7801e865b44c29a5d483956e7c))
* lock token exchange flow so at most one can be active at the sam… ([#213](https://github.com/openfga/dotnet-sdk/issues/213)) ([35d10f4](https://github.com/openfga/dotnet-sdk/commit/35d10f40c5774bd641c0f049d49daf7c30aa642f))
* lower the min version of system.* dependencies and cleaned up unnecessary ones ([#144](https://github.com/openfga/dotnet-sdk/issues/144)) ([b1ea748](https://github.com/openfga/dotnet-sdk/commit/b1ea748f0a6743ef672038efda9dea5394336b88))
* patch consistency preference unspecified due to conflict ([013193f](https://github.com/openfga/dotnet-sdk/commit/013193f451e8625da00beceef72d5d79b59223ca))
* relax flaky retry after test ([#161](https://github.com/openfga/dotnet-sdk/issues/161)) ([7fe66c0](https://github.com/openfga/dotnet-sdk/commit/7fe66c01e146211959d0bb03ca2c7e6c2c2710a5))
* Remove dependecy to OpenTelemetry.Api ([f46064f](https://github.com/openfga/dotnet-sdk/commit/f46064f61fd647edef86186b6f1b6ce1226b8289))
* remove fromJson override for CreateStore ([#188](https://github.com/openfga/dotnet-sdk/issues/188)) ([fa436b7](https://github.com/openfga/dotnet-sdk/commit/fa436b799a72ad23fba010460850d4c13e3d2707))
* resolve ApiToken reserved header exception ([#146](https://github.com/openfga/dotnet-sdk/issues/146)) ([#152](https://github.com/openfga/dotnet-sdk/issues/152)) ([f1bc1e0](https://github.com/openfga/dotnet-sdk/commit/f1bc1e06903dbbf6162393406808a9a4f40edaf4))
* resolve issue deserializing enums / release v0.0.3 ([93ee4af](https://github.com/openfga/dotnet-sdk/commit/93ee4afb9325ed06c5fe9bdf5a58e6f34fb131bd))
* throw an error if no relations provided to ListRelations ([c530ed7](https://github.com/openfga/dotnet-sdk/commit/c530ed77f52408bbbb6b6a44aed9d493242c7176))
* update links to SECURITY.md ([6af7419](https://github.com/openfga/dotnet-sdk/commit/6af7419ed66812a8419ba9eb47e6deffc3124a74))
* update SDK contributing docs ([8c65c30](https://github.com/openfga/dotnet-sdk/commit/8c65c3056ee5d38be04025503bc2e65954dbbe56))
* update stores links ([3c2935d](https://github.com/openfga/dotnet-sdk/commit/3c2935d1e462f2ae738f5f9edf5819547c4ee7e0))
* use correct content type for token request ([003d525](https://github.com/openfga/dotnet-sdk/commit/003d525adf34c5b438d2688e90cd868c2b17a20e))
* use correct content type for token request ([#43](https://github.com/openfga/dotnet-sdk/issues/43)) ([cf1bdc6](https://github.com/openfga/dotnet-sdk/commit/cf1bdc64bc3065615af76b6fee119860458b3c62))
* use interface instead of class for ClientCreateStore ([87a08d1](https://github.com/openfga/dotnet-sdk/commit/87a08d1be209b860218baa5a9e3204023e5b4ff6))


### Documentation

* add FromJson usage examples to README ([#185](https://github.com/openfga/dotnet-sdk/issues/185)) ([95ad5ff](https://github.com/openfga/dotnet-sdk/commit/95ad5ff6a08b60dc05a555d850a769f959b13c36))
* sync docs changes ([2098f13](https://github.com/openfga/dotnet-sdk/commit/2098f13b77229b7e43b7277f65a6fdc4953fcf46))
* Use invite link for Discord Server in README ([9475442](https://github.com/openfga/dotnet-sdk/commit/9475442d7eaf01ff7aa3db6e978261c5617ed01e))
* Use invite link for Discord Server in README ([#28](https://github.com/openfga/dotnet-sdk/issues/28)) ([0f62bbb](https://github.com/openfga/dotnet-sdk/commit/0f62bbb53b2351b89733d27cb4124dbe3c7320a7))


### Miscellaneous

* regenerate based on api@061d8d8d395fd4791215d1fb4886bc3fcdc2d637 ([fa43463](https://github.com/openfga/dotnet-sdk/commit/fa43463ded102df3f660bae6d741e1a8c1dea090))
* remove excluded users from ListUsers response ([8da4653](https://github.com/openfga/dotnet-sdk/commit/8da4653a511e2c32aa130644b00fd32401a34460))
* remove excluded users from ListUsers response ([#64](https://github.com/openfga/dotnet-sdk/issues/64)) ([8ffebc0](https://github.com/openfga/dotnet-sdk/commit/8ffebc0ec1da94f02753db6aad8c062c3f7207d7))
* rename `TypeDefinitions` interface to `WriteAuthorizationModelRequest` ([7a9b236](https://github.com/openfga/dotnet-sdk/commit/7a9b2363d763fe6dfdee72ff006fb4a06eb9dc1b))
* **v0.3.0:** support for conditions and some breaking changes ([8c77f04](https://github.com/openfga/dotnet-sdk/commit/8c77f04579392ec9a76a73331d6d67fa01a6af79))
* **v0.3.0:** support for conditions and some breaking changes ([#36](https://github.com/openfga/dotnet-sdk/issues/36)) ([27514de](https://github.com/openfga/dotnet-sdk/commit/27514de97081fde08ffebfedb545414a2b4954c7))
* v0.4.0 ([#65](https://github.com/openfga/dotnet-sdk/issues/65)) ([85bcbfa](https://github.com/openfga/dotnet-sdk/commit/85bcbfa5a324db64859b3b9001ef3070702c5098))

## [0.10.1](https://github.com/openfga/dotnet-sdk/compare/v0.10.0...v0.10.1) (2026-05-14)


### Added

* batch check fallback to client level authmodel ID ([#199](https://github.com/openfga/dotnet-sdk/issues/199)) ([733c4c3](https://github.com/openfga/dotnet-sdk/commit/733c4c3438bc899643c8755fb4ed3e5085b86a5a))

### Fixed

* lock token exchange flow so at most one can be active at the same time ([#213](https://github.com/openfga/dotnet-sdk/issues/213)) ([35d10f4](https://github.com/openfga/dotnet-sdk/commit/35d10f40c5774bd641c0f049d49daf7c30aa642f))


## v0.10.0

### [0.10.0](https://github.com/openfga/dotnet-sdk/compare/v0.9.1...v0.10.0) (2026-03-24)

### Fixed
- fix: `StreamedListObjects` now correctly retries connection establishment on rate-limit (429) and transient errors; once streaming begins it is intentionally not retried

### Added
- feat: add ApiExecutor for raw requests (#176)
- feat: add streaming support via `ApiExecutor.ExecuteStreamingAsync` for streaming endpoints (e.g. `/streamed-list-objects`)
- feat: add `FromJson()` methods to `ClientWriteAuthorizationModelRequest` and `ClientCreateStoreRequest` to enable loading from JSON string (#180)
- feat: report a per call HTTP metric (#173)

### Breaking Changes

> [!WARNING]
> - **`ApiClient.SendRequestAsync` removed**: If you were calling `SendRequestAsync` directly on `ApiClient`, switch to `ApiExecutor.ExecuteAsync`:
>
>   Before:
>   ```csharp
>   var result = await apiClient.SendRequestAsync<MyRequest, MyResponse>(requestBuilder, "ApiName");
>   ```
>
>   After:
>   ```csharp
>   var result = (await apiClient.ApiExecutor.ExecuteAsync<MyRequest, MyResponse>(requestBuilder, "ApiName")).Data;
>   ```

## v0.9.1

### [0.9.1](https://github.com/openfga/dotnet-sdk/compare/v0.9.0...v0.9.1) (2026-01-26)

- feat: add support for streamed list objects (#156)
- feat: add support for an interface on the client (#168)

## v0.9.0

### [0.9.0](https://github.com/openfga/dotnet-sdk/compare/v0.8.0...v0.9.0) (2025-12-01)

### Added
- feat: add server-side `BatchCheck()` method using `/batch-check` API endpoint
  - See [Batch Check documentation](README.md#batch-check) for usage examples and configuration

### Changed
- **BREAKING**: Existing `BatchCheck()` renamed to `ClientBatchCheck()`
  - New server-side `BatchCheck()` method requires [OpenFGA server v1.8.0+](https://github.com/openfga/openfga/releases/tag/v1.8.0)
  - For configuration options and behavior details, see [README documentation](README.md#batch-check)

### Fixed
- fix: ApiToken credentials no longer cause reserved header exception (#146)

## v0.8.0

### [0.8.0](https://github.com/openfga/dotnet-sdk/compare/v0.7.0...v0.8.0) (2025-10-22)

- feat!: add per-request custom headers support
  - `DefaultHeaders` support to `ClientConfiguration` for headers sent with every request
  - per-request headers support via `Headers` property on all client options classes
  - `IRequestOptions` interface and `RequestOptions` class for API-level header support
  - `IClientRequestOptions` interface and `ClientRequestOptions` class for client-level header support
  - add header validation to prevent overriding of reserved headers
- feat: add write conflict resolution options
  - `ConflictOptions` to control behavior for duplicate writes and missing deletes
  - `OnDuplicateWrites` option: `Error` (default) or `Ignore` for handling duplicate tuple writes
  - `OnMissingDeletes` option: `Error` (default) or `Ignore` for handling missing tuple deletes
  - Available in `ClientWriteOptions.Conflict` property
- feat: add Retry-After header support for rate limiting
  - Retry logic now respects `Retry-After` header from HTTP 429 responses
  - Falls back to exponential backoff when Retry-After header is missing or invalid

[!WARNING]
BREAKING CHANGES:

- **OpenFgaApi methods**: All API methods now accept an `IRequestOptions? options` parameter. If you are using the low-level `OpenFgaApi` directly, you may need to update your calls:

  Before:

  ```csharp
  await api.Check(storeId, body, cancellationToken);
  ```

  After:

  ```csharp
  var options = new RequestOptions {
      Headers = new Dictionary<string, string> { { "X-Custom-Header", "value" } }
  };
  await api.Check(storeId, body, options, cancellationToken);
  ```

- **ClientRequestOptions renamed**: The base client request options interface has been renamed from `ClientRequestOptions` to `IClientRequestOptions` to better follow .NET naming conventions. A concrete `ClientRequestOptions` class is now also available. If you were casting to or implementing this interface, update your code:

  Before:

  ```csharp
  var options = obj as ClientRequestOptions;
  ```

  After:

  ```csharp
  var options = obj as IClientRequestOptions;
  ```

Note: If you are using the high-level `OpenFgaClient`, no changes are required to your existing code. The new headers functionality is additive via the existing options parameters.

## v0.7.0

### [0.7.0](https://github.com/openfga/dotnet-sdk/compare/v0.6.0...v0.7.0) (2025-10-01)

- feat!: add support for .NET Standard 2.0, .NET 8.0 and .NET 9.0

[!WARNING]
BREAKING CHANGES:

- While we have dropped .NET 6.0 as a target framework, the SDK now supports .NET Standard 2.0 - which means it can still be used in .NET 6.0 projects.
- We have updated the underlying OpenAPI generator to a newer version, which has caused some changes in the generated code. Below is a summary of the changes:

| Old Name    | New Name            | Affected Models                                                                                                                       |
| ----------- | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| \_Nodes     | VarNodes            | Nodes (and related constructors/references)                                                                                           |
| \_Users     | VarUsers            | Users (and related constructors/references)                                                                                           |
| \_base      | VarBase             | Difference, UsersetTreeDifference                                                                                                     |
| \_object    | VarObject           | CheckRequestTupleKey, ExpandRequestTupleKey, TupleKey, TupleKeyWithoutCondition, FgaObject, ObjectRelation, ReadRequestTupleKey, User |
| \_this      | VarThis             | Userset                                                                                                                               |
| UNSPECIFIED | TYPENAMEUNSPECIFIED | TypeName (and other members in this enum)                                                                                             |

| Old Enum Value | New Enum Value       | Affected Enum  |
| -------------- | -------------------- | -------------- |
| WRITE          | TUPLEOPERATIONWRITE  | TupleOperation |
| DELETE         | TUPLEOPERATIONDELETE | TupleOperation |

## v0.6.0

### [0.6.0](https://github.com/openfga/dotnet-sdk/compare/v0.5.1...v0.6.0) (2025-09-30)

- feat: add support for `start_time` parameter in `ReadChanges` endpoint
- feat: update API definitions
- feat: support assertions context and contextual tuples
- feat: support contextual tuples in `Expand`
- feat!: support passing in name to filter in `ListStores`
- fix: remove dependency on OpenTelemetry.Api (#100) - thanks @m4tchl0ck
- fix: limit default retries to `3` from `15` (https://github.com/openfga/sdk-generator/pull/420) - thanks @ovindu-a
- fix: `ListRelations` should not swallow errors
- chore(docs): replace readable names with uuid to discourage storing PII in OpenFGA (https://github.com/openfga/sdk-generator/pull/433) - thanks @sccalabr

[!WARNING]
BREAKING CHANGES:

- The `ListStores` method now accepts a body parameter with an optional `Name` to filter the stores. This is a breaking change as it changes the method contract to allow passing in a body with the name.

## v0.5.1

### [0.5.1](https://github.com/openfga/dotnet-sdk/compare/v0.5.0...v0.5.1) (2024-09-09)

- feat: export OpenTelemetry metrics. Refer to the [https://github.com/openfga/dotnet-sdk/blob/main/OpenTelemetry.md](documentation) for more.

## v0.5.0

### [0.5.0](https://github.com/openfga/dotnet-sdk/compare/v0.4.0...v0.5.0) (2024-08-28)

- feat: support consistency parameter (#70)
  Note: To use this feature, you need to be running OpenFGA v1.5.7+ with the experimental flag `enable-consistency-params` enabled.
  See the [v1.5.7 release notes](https://github.com/openfga/openfga/releases/tag/v1.5.7) for details.

## v0.4.0

### [0.4.0](https://github.com/openfga/dotnet-sdk/compare/v0.3.2...v0.4.0) (2024-06-14)

- chore!: remove excluded users from ListUsers response

BREAKING CHANGE:

This version removes the `ExcludedUsers` field from the `ListUsersResponse` and `ClientListUsersResponse` classes,
for more details see the [associated API change](https://github.com/openfga/api/pull/171).

## v0.3.2

### [0.3.2](https://github.com/openfga/dotnet-sdk/compare/v0.3.1...v0.3.2) (2024-04-30)

- feat: support the [ListUsers](https://github.com/openfga/rfcs/blob/main/20231214-listUsers-api.md) endpoint (#57)
- feat: add retries to client credential requests (#51)
- feat: add support for modular models metadata (#53)

## v0.3.1

### [0.3.1](https://github.com/openfga/dotnet-sdk/compare/v0.3.0...v0.3.1) (2024-02-13)

- fix: use correct content type for token request

## v0.3.0

### [0.3.0](https://github.com/openfga/dotnet-sdk/compare/v0.2.5...v0.3.0) (2023-12-20)

- feat!: initial support for [conditions](https://openfga.dev/blog/conditional-tuples-announcement)
- feat!: allow overriding storeId per request (#33)
- feat: support specifying a port and path for the API (You can now set the `ApiUrl` to something like: `https://api.fga.exampleL8080/some_path`)
- feat: validate that store id and auth model id in ulid format (#23)
- fix: exception when using the same configuration with multiple clients (#26)
- fix: `OpenFgaClient.ReadLatestAuthorizationModel` can now return a null if no model has ever been created in that store
- fix: `OpenFgaClient.Read` and `OpenFgaClient.ReadChanges` now allow a null body
- chore!: use latest API interfaces
- chore: dependency updates
- chore: add [example project](./example)

BREAKING CHANGES:
Note: This release comes with substantial breaking changes, especially to the interfaces due to the protobuf changes in the last release.

While the http interfaces did not break (you can still use `v0.2.5` SDK with a `v1.3.8+` server),
the grpc interface did and this caused a few changes in the interfaces of the SDK.

If you are using `OpenFgaClient`, the changes required should be smaller, if you are using `OpenFgaApi` a bit more changes will be needed.

You will have to modify some parts of your code, but we hope this will be to the better as a lot of the parameters are now correctly marked as required,
and so the Pointer-to-String conversion is no longer needed.

Some of the changes to expect:

- When initializing a client, please use `ApiUrl`. The separate `ApiScheme` and `ApiHost` fields have been deprecated

```csharp
var configuration = new ClientConfiguration() {
    ApiUrl = Environment.GetEnvironmentVariable("FGA_API_URL"), // required, e.g. https://api.fga.example
    StoreId = Environment.GetEnvironmentVariable("FGA_STORE_ID"), // not needed when calling `CreateStore` or `ListStores`
    AuthorizationModelId = Environment.GetEnvironmentVariable("FGA_MODEL_ID"), // Optional, can be overridden per request
};
var fgaClient = new OpenFgaClient(configuration);
```

- `OpenFgaApi` now requires `storeId` as first param when needed
- `Configuration` no longer accepts `storeId` (`ClientConfiguration` is not affected)
- The following request interfaces changed:
  - `CheckRequest`: the `TupleKey` field is now of interface `CheckRequestTupleKey`, you can also now pass in `Context`
  - `ExpandRequest`: the `TupleKey` field is now of interface `ExpandRequestTupleKey`
  - `ReadRequest`: the `TupleKey` field is now of interface `ReadRequestTupleKey`
  - `WriteRequest`: now takes `WriteRequestWrites` and `WriteRequestDeletes`, the latter of which accepts `TupleKeyWithoutCondition`
  - And more
- The following interfaces had fields that were pointers are are now the direct value:
  - `CreateStoreResponse`
  - `GetStoreResponse`
  - `ListStoresResponse`
  - `ListObjectsResponse`
  - `ReadChangesResponse`
  - `ReadResponse`
  - `AuthorizationModel`
  - And more

Take a look at https://github.com/openfga/dotnet-sdk/commit/fa43463ded102df3f660bae6d741e1a8c1dea090 for more model changes.

## v0.2.5

### [0.2.5](https://github.com/openfga/dotnet-sdk/compare/v0.2.4...v0.2.5) (2023-12-01)

- fix(client): read with no filter (read all tuples)
- chore(deps): update dependencies

## v0.2.4

### [0.2.4](https://github.com/openfga/dotnet-sdk/compare/v0.2.3...v0.2.4) (2023-05-01)

- fix: client credentials token expiry period was being evaluated as ms instead of seconds, leading to token refreshes on every call

## v0.2.3

### [0.2.3](https://github.com/openfga/dotnet-sdk/compare/v0.2.2...v0.2.3) (2023-04-13)

- fix: changed interface of contextual tuples in `ClientListObjects` to be `ClientTupleKey` instead of `TupleKey`
- fix: Client `WriteAuthorizationModel` now expects `ClientWriteAuthorizationModelRequest` instead of `WriteAuthorizationModelRequest`
- chore: changed a few interfaces to expect interfaces instead of classes

## v0.2.2

### [0.2.2](https://github.com/openfga/dotnet-sdk/compare/v0.2.1...v0.2.2) (2023-04-12)

- feat(client): add OpenFgaClient wrapper see [docs](https://github.com/openfga/dotnet-sdk/tree/main#readme), see the `v0.2.1` docs for [the OpenFgaApi docs](https://github.com/openfga/dotnet-sdk/tree/v0.2.1#readme)
- feat(client): implement `BatchCheck` to check multiple tuples in parallel
- feat(client): implement `ListRelations` to check in one call whether a user has multiple relations to an objects
- feat(client): add support for a non-transactional `Write`
- chore(config): bump default max retries to `15`
- fix: retry on 5xx errors

## v0.2.1

### [0.2.1](https://github.com/openfga/dotnet-sdk/compare/v0.2.0...v0.2.1) (2023-01-17)

- chore(deps): upgrade `Microsoft.NET.Test.Sdk` and `Moq` dev dependencies

## v0.2.0

### [0.2.0](https://github.com/openfga/dotnet-sdk/compare/v0.1.2...v0.2.0) (2022-12-14)

Updated to include support for [OpenFGA 0.3.0](https://github.com/openfga/openfga/releases/tag/v0.3.0)

Changes:

- [BREAKING] feat(list-objects)!: response has been changed to include the object type
  e.g. response that was `{"object_ids":["roadmap"]}`, will now be `{"objects":["document:0192ab2a-d83f-756d-9397-c5ed9f3cb69a"]}`

Fixes:

- fix(models): update interfaces that had incorrectly optional fields to make them required

Chore:

- chore(deps): update dev dependencies

## v0.1.2

### [0.1.2](https://github.com/openfga/dotnet-sdk/compare/v0.1.1...v0.1.2) (2022-11-15)

- feat: regenerate from latest API Document, changes include:
  - documentation fixes
  - types that represent enabling wildcards in authorization models
- fix: send authorization header to server when ApiToken used (https://github.com/openfga/sdk-generator/issues/58)
- chore: update test dependencies

## v0.1.1

### [0.1.1](https://github.com/openfga/dotnet-sdk/compare/v0.1.0...v0.1.1) (2022-10-07)

- Fix for issue in deserializing nullable DateTime (https://github.com/openfga/dotnet-sdk/issues/5)

## v0.1.0

### [0.1.0](https://github.com/openfga/dotnet-sdk/compare/v0.0.3...v0.1.0) (2022-09-29)

- BREAKING: exported interface `TypeDefinitions` is now `WriteAuthorizationModelRequest`
  This is only a breaking change on the SDK, not the API. It was changed to conform to the proto changes in [openfga/api](https://github.com/openfga/api/pull/27).
- chore(deps): upgrade dependencies

## v0.0.3

### [0.0.3](https://github.com/openfga/dotnet-sdk/compare/v0.0.2...v0.0.3) (2022-09-09)

- Fix for issue in deserializing enums (https://github.com/openfga/sdk-generator/issues/7)

## v0.0.2

### [0.0.2](https://github.com/openfga/dotnet-sdk/compare/v0.0.1...v0.0.2) (2022-08-15)

Support for [ListObjects API]](https://openfga.dev/api/service#/Relationship%20Queries/ListObjects)

You call the API and receive the list of object ids from a particular type that the user has a certain relation with.

For example, to find the list of documents that Anne can read:

```csharp
var body = new ListObjectsRequest{
    AuthorizationModelId = "01GAHCE4YVKPQEKZQHT2R89MQV",
    User = "anne",
    Relation = "can_read",
    Type = "document"
};
var response = await openFgaApi.ListObjects(body);

// response.ObjectIds = ["roadmap"]
```

## v0.0.1

### [0.0.1](https://github.com/openfga/dotnet-sdk/releases/tag/v0.0.1) (2022-06-17)

Initial OpenFGA .NET SDK release

- Support for [OpenFGA](https://github.com/openfga/openfga) API
  - CRUD stores
  - Create, read & list authorization models
  - Writing and Reading Tuples
  - Checking authorization
  - Using Expand to understand why access was granted
