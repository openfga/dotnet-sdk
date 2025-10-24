namespace OpenFga.Sdk.Client.Model;

/// <inheritdoc />
public interface IClientRequestOptionsWithAuthZModelId : IClientRequestOptionsWithStoreId, AuthorizationModelIdOptions {
}

public interface IClientRequestOptionsWithAuthZModelIdAndConsistency : IClientRequestOptionsWithAuthZModelId, IClientConsistencyOptions {
}