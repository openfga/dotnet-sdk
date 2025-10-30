using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using OpenFga.Sdk.Model;

namespace OpenFga.Sdk.Client.Model;

/// <summary>
///     Client-friendly model for a single batch check item
/// </summary>
[DataContract(Name = "ClientBatchCheckItem")]
public class ClientBatchCheckItem : IEquatable<ClientBatchCheckItem>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckItem" /> class.
    /// </summary>
    public ClientBatchCheckItem() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckItem" /> class.
    /// </summary>
    /// <param name="user">The user (required).</param>
    /// <param name="relation">The relation (required).</param>
    /// <param name="object">The object (required).</param>
    /// <param name="correlationId">Optional correlation ID for mapping responses. Will be auto-generated if not provided.</param>
    /// <param name="contextualTuples">Optional contextual tuples.</param>
    /// <param name="context">Optional context object.</param>
    public ClientBatchCheckItem(
        string user,
        string relation,
        string @object,
        string? correlationId = default,
        ContextualTupleKeys? contextualTuples = default,
        Object? context = default) {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Relation = relation ?? throw new ArgumentNullException(nameof(relation));
        Object = @object ?? throw new ArgumentNullException(nameof(@object));
        CorrelationId = correlationId;
        ContextualTuples = contextualTuples;
        Context = context;
    }

    /// <summary>
    ///     The user (e.g., "user:anne")
    /// </summary>
    [DataMember(Name = "user", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("user")]
    public string User { get; set; }

    /// <summary>
    ///     The relation (e.g., "reader")
    /// </summary>
    [DataMember(Name = "relation", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("relation")]
    public string Relation { get; set; }

    /// <summary>
    ///     The object (e.g., "document:2021-budget")
    /// </summary>
    [DataMember(Name = "object", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("object")]
    public string Object { get; set; }

    /// <summary>
    ///     Optional correlation ID. If not provided, a UUID will be auto-generated.
    ///     Must be a string containing only letters, numbers, or hyphens, with length ≤ 36 characters.
    /// </summary>
    [DataMember(Name = "correlation_id", EmitDefaultValue = false)]
    [JsonPropertyName("correlation_id")]
    public string? CorrelationId { get; set; }

    /// <summary>
    ///     Optional contextual tuples
    /// </summary>
    [DataMember(Name = "contextual_tuples", EmitDefaultValue = false)]
    [JsonPropertyName("contextual_tuples")]
    public ContextualTupleKeys? ContextualTuples { get; set; }

    /// <summary>
    ///     Optional context object
    /// </summary>
    [DataMember(Name = "context", EmitDefaultValue = false)]
    [JsonPropertyName("context")]
    public Object? Context { get; set; }

    /// <inheritdoc />
    public bool Equals(ClientBatchCheckItem? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return User == other.User &&
               Relation == other.Relation &&
               Object == other.Object &&
               CorrelationId == other.CorrelationId &&
               EqualityComparer<ContextualTupleKeys?>.Default.Equals(ContextualTuples, other.ContextualTuples) &&
               Equals(Context, other.Context);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj != null && obj.GetType() == this.GetType()) {
            return Equals((ClientBatchCheckItem)obj);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            hashCode = (hashCode * 9923) + (User?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (Relation?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (Object?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (CorrelationId?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (ContextualTuples?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (Context?.GetHashCode() ?? 0);
            return hashCode;
        }
    }
}

/// <summary>
///     Request for server-side batch check
/// </summary>
[DataContract(Name = "ClientBatchCheckRequest")]
public class ClientBatchCheckRequest : IEquatable<ClientBatchCheckRequest>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckRequest" /> class.
    /// </summary>
    public ClientBatchCheckRequest() {
        Checks = new List<ClientBatchCheckItem>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckRequest" /> class.
    /// </summary>
    /// <param name="checks">List of checks to perform.</param>
    public ClientBatchCheckRequest(List<ClientBatchCheckItem> checks) {
        Checks = checks ?? throw new ArgumentNullException(nameof(checks));
    }

    /// <summary>
    ///     List of checks to perform
    /// </summary>
    [DataMember(Name = "checks", IsRequired = true, EmitDefaultValue = false)]
    [JsonPropertyName("checks")]
    public List<ClientBatchCheckItem> Checks { get; set; }

    /// <inheritdoc />
    public bool Equals(ClientBatchCheckRequest? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Checks.SequenceEqual(other.Checks);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj != null && obj.GetType() == this.GetType()) {
            return Equals((ClientBatchCheckRequest)obj);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            if (Checks != null) {
                foreach (var check in Checks) {
                    hashCode = (hashCode * 9923) + (check?.GetHashCode() ?? 0);
                }
            }
            return hashCode;
        }
    }
}

/// <summary>
///     Single response from server-side batch check
/// </summary>
[DataContract(Name = "ClientBatchCheckSingleResponse")]
public class ClientBatchCheckSingleResponse : IEquatable<ClientBatchCheckSingleResponse>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckSingleResponse" /> class.
    /// </summary>
    public ClientBatchCheckSingleResponse() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckSingleResponse" /> class.
    /// </summary>
    /// <param name="allowed">Whether the check was allowed.</param>
    /// <param name="request">The original request.</param>
    /// <param name="correlationId">The correlation ID from the request.</param>
    /// <param name="error">Error if the check failed.</param>
    public ClientBatchCheckSingleResponse(
        bool allowed,
        ClientBatchCheckItem request,
        string correlationId,
        CheckError? error = default) {
        Allowed = allowed;
        Request = request ?? throw new ArgumentNullException(nameof(request));
        CorrelationId = correlationId ?? throw new ArgumentNullException(nameof(correlationId));
        Error = error;
    }

    /// <summary>
    ///     Whether the check was allowed
    /// </summary>
    [DataMember(Name = "allowed", EmitDefaultValue = true)]
    [JsonPropertyName("allowed")]
    public bool Allowed { get; set; }

    /// <summary>
    ///     The original request
    /// </summary>
    [DataMember(Name = "request", EmitDefaultValue = true)]
    [JsonPropertyName("request")]
    public ClientBatchCheckItem Request { get; set; }

    /// <summary>
    ///     The correlation ID
    /// </summary>
    [DataMember(Name = "correlation_id", EmitDefaultValue = true)]
    [JsonPropertyName("correlation_id")]
    public string CorrelationId { get; set; }

    /// <summary>
    ///     Error if the check failed
    /// </summary>
    [DataMember(Name = "error", EmitDefaultValue = false)]
    [JsonPropertyName("error")]
    public CheckError? Error { get; set; }

    /// <inheritdoc />
    public bool Equals(ClientBatchCheckSingleResponse? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Allowed == other.Allowed &&
               EqualityComparer<ClientBatchCheckItem>.Default.Equals(Request, other.Request) &&
               CorrelationId == other.CorrelationId &&
               EqualityComparer<CheckError?>.Default.Equals(Error, other.Error);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj != null && obj.GetType() == this.GetType()) {
            return Equals((ClientBatchCheckSingleResponse)obj);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            hashCode = (hashCode * 9923) + Allowed.GetHashCode();
            hashCode = (hashCode * 9923) + (Request?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (CorrelationId?.GetHashCode() ?? 0);
            hashCode = (hashCode * 9923) + (Error?.GetHashCode() ?? 0);
            return hashCode;
        }
    }
}

/// <summary>
///     Response from server-side batch check
/// </summary>
[DataContract(Name = "ClientBatchCheckResponse")]
public class ClientBatchCheckResponse : IEquatable<ClientBatchCheckResponse>, IValidatableObject {
    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckResponse" /> class.
    /// </summary>
    public ClientBatchCheckResponse() {
        Result = new List<ClientBatchCheckSingleResponse>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ClientBatchCheckResponse" /> class.
    /// </summary>
    /// <param name="result">List of check results.</param>
    public ClientBatchCheckResponse(List<ClientBatchCheckSingleResponse> result) {
        Result = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>
    ///     List of check results
    /// </summary>
    [DataMember(Name = "result", EmitDefaultValue = true)]
    [JsonPropertyName("result")]
    public List<ClientBatchCheckSingleResponse> Result { get; set; }

    /// <inheritdoc />
    public bool Equals(ClientBatchCheckResponse? other) {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Result.SequenceEqual(other.Result);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) {
        if (obj != null && obj.GetType() == this.GetType()) {
            return Equals((ClientBatchCheckResponse)obj);
        }
        return false;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
        yield break;
    }

    public override int GetHashCode() {
        unchecked {
            int hashCode = 9661;
            if (Result != null) {
                foreach (var item in Result) {
                    hashCode = (hashCode * 9923) + (item?.GetHashCode() ?? 0);
                }
            }
            return hashCode;
        }
    }
}


