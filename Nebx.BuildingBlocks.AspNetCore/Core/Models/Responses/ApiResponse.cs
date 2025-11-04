using System.Text.Json.Serialization;

namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.Responses;

/// <summary>
/// Represents a standardized API response wrapper containing data, metadata, and strongly typed error information.
/// </summary>
/// <typeparam name="TData">The type of the response data payload.</typeparam>
/// <typeparam name="TError">
/// The type of the error details returned when the request fails. Must inherit from <see cref="ErrorDetail"/>.
/// </typeparam>
public record ApiResponse<TData, TError> where TError : ErrorDetail
{
    [JsonConstructor]
    private ApiResponse()
    {
    }

    /// <summary>The main data payload returned by the API.</summary>
    public TData? Data { get; init; }

    /// <summary>Metadata providing additional context about the response.</summary>
    public Meta? Meta { get; init; }

    /// <summary>Error details describing why the request failed, if applicable.</summary>
    public TError? Error { get; init; }

    /// <summary>
    /// Creates a successful API response containing the specified data and optional metadata.
    /// </summary>
    /// <param name="data">The data payload returned by the API.</param>
    /// <param name="meta">Optional metadata providing additional response context.</param>
    /// <returns>
    /// A new <see cref="ApiResponse{TData, TError}"/> instance representing a successful result.
    /// </returns>
    public static ApiResponse<TData, TError> Success(TData? data, Meta? meta = null)
        => new() { Data = data, Meta = meta };

    /// <summary>
    /// Creates a failed API response containing strongly typed error details.
    /// </summary>
    /// <param name="error">An instance of <typeparamref name="TError"/> describing the failure.</param>
    /// <returns>
    /// A new <see cref="ApiResponse{TData, TError}"/> instance representing a failed result.
    /// </returns>
    public static ApiResponse<TData, TError> Fail(TError error)
        => new() { Error = error };
}