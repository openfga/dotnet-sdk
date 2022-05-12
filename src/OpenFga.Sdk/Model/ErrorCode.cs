//
// OpenFGA/.NET SDK for OpenFGA
//
// API version: 0.1
// Website: https://openfga.dev
// Documentation: https://openfga.dev/docs
// Support: https://discord.gg/8naAwJfWN6
// License: [Apache-2.0](https://github.com/openfga/dotnet-sdk/blob/main/LICENSE)
//
// NOTE: This file was auto generated. DO NOT EDIT.
//


using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace OpenFga.Sdk.Model {
    /// <summary>
    /// Defines ErrorCode
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorCode {
        /// <summary>
        /// Enum NoError for value: no_error
        /// </summary>
        [EnumMember(Value = "no_error")]
        NoError = 1,

        /// <summary>
        /// Enum ValidationError for value: validation_error
        /// </summary>
        [EnumMember(Value = "validation_error")]
        ValidationError = 2,

        /// <summary>
        /// Enum AuthorizationModelNotFound for value: authorization_model_not_found
        /// </summary>
        [EnumMember(Value = "authorization_model_not_found")]
        AuthorizationModelNotFound = 3,

        /// <summary>
        /// Enum AuthorizationModelResolutionTooComplex for value: authorization_model_resolution_too_complex
        /// </summary>
        [EnumMember(Value = "authorization_model_resolution_too_complex")]
        AuthorizationModelResolutionTooComplex = 4,

        /// <summary>
        /// Enum InvalidWriteInput for value: invalid_write_input
        /// </summary>
        [EnumMember(Value = "invalid_write_input")]
        InvalidWriteInput = 5,

        /// <summary>
        /// Enum CannotAllowDuplicateTuplesInOneRequest for value: cannot_allow_duplicate_tuples_in_one_request
        /// </summary>
        [EnumMember(Value = "cannot_allow_duplicate_tuples_in_one_request")]
        CannotAllowDuplicateTuplesInOneRequest = 6,

        /// <summary>
        /// Enum CannotAllowDuplicateTypesInOneRequest for value: cannot_allow_duplicate_types_in_one_request
        /// </summary>
        [EnumMember(Value = "cannot_allow_duplicate_types_in_one_request")]
        CannotAllowDuplicateTypesInOneRequest = 7,

        /// <summary>
        /// Enum CannotAllowMultipleReferencesToOneRelation for value: cannot_allow_multiple_references_to_one_relation
        /// </summary>
        [EnumMember(Value = "cannot_allow_multiple_references_to_one_relation")]
        CannotAllowMultipleReferencesToOneRelation = 8,

        /// <summary>
        /// Enum InvalidContinuationToken for value: invalid_continuation_token
        /// </summary>
        [EnumMember(Value = "invalid_continuation_token")]
        InvalidContinuationToken = 9,

        /// <summary>
        /// Enum InvalidTupleSet for value: invalid_tuple_set
        /// </summary>
        [EnumMember(Value = "invalid_tuple_set")]
        InvalidTupleSet = 10,

        /// <summary>
        /// Enum InvalidCheckInput for value: invalid_check_input
        /// </summary>
        [EnumMember(Value = "invalid_check_input")]
        InvalidCheckInput = 11,

        /// <summary>
        /// Enum InvalidExpandInput for value: invalid_expand_input
        /// </summary>
        [EnumMember(Value = "invalid_expand_input")]
        InvalidExpandInput = 12,

        /// <summary>
        /// Enum UnsupportedUserSet for value: unsupported_user_set
        /// </summary>
        [EnumMember(Value = "unsupported_user_set")]
        UnsupportedUserSet = 13,

        /// <summary>
        /// Enum InvalidObjectFormat for value: invalid_object_format
        /// </summary>
        [EnumMember(Value = "invalid_object_format")]
        InvalidObjectFormat = 14,

        /// <summary>
        /// Enum WriteFailedDueToInvalidInput for value: write_failed_due_to_invalid_input
        /// </summary>
        [EnumMember(Value = "write_failed_due_to_invalid_input")]
        WriteFailedDueToInvalidInput = 15,

        /// <summary>
        /// Enum AuthorizationModelAssertionsNotFound for value: authorization_model_assertions_not_found
        /// </summary>
        [EnumMember(Value = "authorization_model_assertions_not_found")]
        AuthorizationModelAssertionsNotFound = 16,

        /// <summary>
        /// Enum LatestAuthorizationModelNotFound for value: latest_authorization_model_not_found
        /// </summary>
        [EnumMember(Value = "latest_authorization_model_not_found")]
        LatestAuthorizationModelNotFound = 17,

        /// <summary>
        /// Enum TypeNotFound for value: type_not_found
        /// </summary>
        [EnumMember(Value = "type_not_found")]
        TypeNotFound = 18,

        /// <summary>
        /// Enum RelationNotFound for value: relation_not_found
        /// </summary>
        [EnumMember(Value = "relation_not_found")]
        RelationNotFound = 19,

        /// <summary>
        /// Enum EmptyRelationDefinition for value: empty_relation_definition
        /// </summary>
        [EnumMember(Value = "empty_relation_definition")]
        EmptyRelationDefinition = 20,

        /// <summary>
        /// Enum InvalidUser for value: invalid_user
        /// </summary>
        [EnumMember(Value = "invalid_user")]
        InvalidUser = 21,

        /// <summary>
        /// Enum InvalidTuple for value: invalid_tuple
        /// </summary>
        [EnumMember(Value = "invalid_tuple")]
        InvalidTuple = 22,

        /// <summary>
        /// Enum UnknownRelation for value: unknown_relation
        /// </summary>
        [EnumMember(Value = "unknown_relation")]
        UnknownRelation = 23,

        /// <summary>
        /// Enum StoreIdInvalidLength for value: store_id_invalid_length
        /// </summary>
        [EnumMember(Value = "store_id_invalid_length")]
        StoreIdInvalidLength = 24,

        /// <summary>
        /// Enum AssertionsTooManyItems for value: assertions_too_many_items
        /// </summary>
        [EnumMember(Value = "assertions_too_many_items")]
        AssertionsTooManyItems = 25,

        /// <summary>
        /// Enum IdTooLong for value: id_too_long
        /// </summary>
        [EnumMember(Value = "id_too_long")]
        IdTooLong = 26,

        /// <summary>
        /// Enum AuthorizationModelIdTooLong for value: authorization_model_id_too_long
        /// </summary>
        [EnumMember(Value = "authorization_model_id_too_long")]
        AuthorizationModelIdTooLong = 27,

        /// <summary>
        /// Enum TupleKeyValueNotSpecified for value: tuple_key_value_not_specified
        /// </summary>
        [EnumMember(Value = "tuple_key_value_not_specified")]
        TupleKeyValueNotSpecified = 28,

        /// <summary>
        /// Enum TupleKeysTooManyOrTooFewItems for value: tuple_keys_too_many_or_too_few_items
        /// </summary>
        [EnumMember(Value = "tuple_keys_too_many_or_too_few_items")]
        TupleKeysTooManyOrTooFewItems = 29,

        /// <summary>
        /// Enum PageSizeInvalid for value: page_size_invalid
        /// </summary>
        [EnumMember(Value = "page_size_invalid")]
        PageSizeInvalid = 30,

        /// <summary>
        /// Enum ParamMissingValue for value: param_missing_value
        /// </summary>
        [EnumMember(Value = "param_missing_value")]
        ParamMissingValue = 31,

        /// <summary>
        /// Enum DifferenceBaseMissingValue for value: difference_base_missing_value
        /// </summary>
        [EnumMember(Value = "difference_base_missing_value")]
        DifferenceBaseMissingValue = 32,

        /// <summary>
        /// Enum SubtractBaseMissingValue for value: subtract_base_missing_value
        /// </summary>
        [EnumMember(Value = "subtract_base_missing_value")]
        SubtractBaseMissingValue = 33,

        /// <summary>
        /// Enum ObjectTooLong for value: object_too_long
        /// </summary>
        [EnumMember(Value = "object_too_long")]
        ObjectTooLong = 34,

        /// <summary>
        /// Enum RelationTooLong for value: relation_too_long
        /// </summary>
        [EnumMember(Value = "relation_too_long")]
        RelationTooLong = 35,

        /// <summary>
        /// Enum TypeDefinitionsTooFewItems for value: type_definitions_too_few_items
        /// </summary>
        [EnumMember(Value = "type_definitions_too_few_items")]
        TypeDefinitionsTooFewItems = 36,

        /// <summary>
        /// Enum TypeInvalidLength for value: type_invalid_length
        /// </summary>
        [EnumMember(Value = "type_invalid_length")]
        TypeInvalidLength = 37,

        /// <summary>
        /// Enum TypeInvalidPattern for value: type_invalid_pattern
        /// </summary>
        [EnumMember(Value = "type_invalid_pattern")]
        TypeInvalidPattern = 38,

        /// <summary>
        /// Enum RelationsTooFewItems for value: relations_too_few_items
        /// </summary>
        [EnumMember(Value = "relations_too_few_items")]
        RelationsTooFewItems = 39,

        /// <summary>
        /// Enum RelationsTooLong for value: relations_too_long
        /// </summary>
        [EnumMember(Value = "relations_too_long")]
        RelationsTooLong = 40,

        /// <summary>
        /// Enum RelationsInvalidPattern for value: relations_invalid_pattern
        /// </summary>
        [EnumMember(Value = "relations_invalid_pattern")]
        RelationsInvalidPattern = 41,

        /// <summary>
        /// Enum ObjectInvalidPattern for value: object_invalid_pattern
        /// </summary>
        [EnumMember(Value = "object_invalid_pattern")]
        ObjectInvalidPattern = 42,

        /// <summary>
        /// Enum QueryStringTypeContinuationTokenMismatch for value: query_string_type_continuation_token_mismatch
        /// </summary>
        [EnumMember(Value = "query_string_type_continuation_token_mismatch")]
        QueryStringTypeContinuationTokenMismatch = 43,

        /// <summary>
        /// Enum ExceededEntityLimit for value: exceeded_entity_limit
        /// </summary>
        [EnumMember(Value = "exceeded_entity_limit")]
        ExceededEntityLimit = 44,

        /// <summary>
        /// Enum InvalidContextualTuple for value: invalid_contextual_tuple
        /// </summary>
        [EnumMember(Value = "invalid_contextual_tuple")]
        InvalidContextualTuple = 45,

        /// <summary>
        /// Enum DuplicateContextualTuple for value: duplicate_contextual_tuple
        /// </summary>
        [EnumMember(Value = "duplicate_contextual_tuple")]
        DuplicateContextualTuple = 46

    }

}