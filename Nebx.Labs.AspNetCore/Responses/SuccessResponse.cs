using System.Text.Json.Serialization;

namespace Nebx.Labs.AspNetCore.Responses;

/// <summary>
/// Represents a standardized API response wrapper containing data and metadata for successful requests.
/// </summary>
/// <typeparam name="TData">The type of the response data payload.</typeparam>
public record SuccessResponse<TData>
{
    /// <summary>
    /// JSON deserialization constructor. Used by <see cref="System.Text.Json"/> when
    /// materializing an <see cref="SuccessResponse{TData}"/> instance from JSON.
    ///
    /// Do not call this constructor directly. Instead, use <see cref="Create(TData,Responses.Meta)"/>
    /// to create responses in application code.
    /// </summary>
    /// <param name="data">The deserialized data payload.</param>
    /// <param name="meta">The deserialized response metadata, if present.</param>
    [JsonConstructor]
    public SuccessResponse(TData? data, Meta? meta)
    {
        Data = data;
        Meta = meta;
    }

    private SuccessResponse()
    {
    }

    /// <summary>The main data payload returned by the API.</summary>
    public TData? Data { get; private init; }

    /// <summary>Metadata providing additional context about the response.</summary>
    public Meta? Meta { get; private init; }

    /// <summary>
    /// Creates a successful API response containing the specified data and optional metadata.
    /// </summary>
    /// <param name="data">The data payload returned by the API.</param>
    /// <param name="meta">Optional metadata providing additional response context.</param>
    /// <returns>
    /// A new <see cref="SuccessResponse{TData}"/> instance representing a successful result.
    /// </returns>
    public static SuccessResponse<TData> Create(TData? data, Meta? meta = null)
        => new() { Data = data, Meta = meta };
}

/// <summary>
/// Provides shorthand factory methods for creating standardized <see cref="SuccessResponse{TData}"/> instances.
/// </summary>
/// <remarks>
/// This static helper simplifies the creation of typed success responses,
/// reducing boilerplate in API endpoints and application logic.
/// </remarks>
public static class SuccessResponse
{
    /// <summary>
    /// Creates a successful <see cref="SuccessResponse{TData}"/> containing the specified data payload.
    /// </summary>
    /// <typeparam name="TData">The type of the data payload returned by the API.</typeparam>
    /// <param name="data">The data to include in the successful response.</param>
    /// <returns>
    /// A new <see cref="SuccessResponse{TData}"/> instance representing a successful result.
    /// </returns>
    /// <example>
    /// <code>
    /// return Results.Ok(ApiResponse.Create(userDto));
    /// </code>
    /// </example>
    public static SuccessResponse<TData> Create<TData>(TData data)
        => SuccessResponse<TData>.Create(data);

    /// <summary>
    /// Creates a successful <see cref="SuccessResponse{TData}"/> containing the specified data payload.
    /// </summary>
    /// <typeparam name="TData">The type of the data payload returned by the API.</typeparam>
    /// <param name="data">The data to include in the successful response.</param>
    /// <param name="meta">The meta to include in the successful response.</param>
    /// <returns>
    /// A new <see cref="SuccessResponse{TData}"/> instance representing a successful result.
    /// </returns>
    /// <example>
    /// <code>
    /// return Results.Ok(ApiResponse.Create(userDto, meta));
    /// </code>
    /// </example>
    public static SuccessResponse<TData> Create<TData>(TData data, Meta meta)
        => SuccessResponse<TData>.Create(data, meta);
}
