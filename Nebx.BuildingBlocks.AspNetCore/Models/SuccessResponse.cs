using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

/// <summary>
/// Represents a non-generic success response for API operations.
/// </summary>
/// <remarks>
/// The <see cref="SuccessResponse"/> record acts as a base type for successful API responses.  
/// It provides a static factory method that creates a typed <see cref="SuccessResponse{T}"/> instance 
/// containing the response data and optional pagination metadata.
/// </remarks>
public record SuccessResponse
{
    /// <summary>
    /// Creates a new <see cref="SuccessResponse{T}"/> containing the specified data and optional metadata.
    /// </summary>
    /// <typeparam name="T">The type of the data returned in the success response.</typeparam>
    /// <param name="data">The main content or result data of the response.</param>
    /// <param name="meta">
    /// Optional pagination or contextual metadata (for example, pagination information from a <see cref="Meta"/> object).
    /// </param>
    /// <returns>
    /// A new <see cref="SuccessResponse{T}"/> instance containing the provided data and optional metadata.
    /// </returns>
    public static SuccessResponse<T> Create<T>(T data, Meta? meta = null)
    {
        return SuccessResponse<T>.Create(data, meta);
    }
}

/// <summary>
/// Represents a strongly typed success response containing data and optional metadata.
/// </summary>
/// <typeparam name="T">The type of the data included in the response.</typeparam>
/// <remarks>
/// The <see cref="SuccessResponse{T}"/> record extends <see cref="SuccessResponse"/> to support
/// returning structured response data along with additional metadata.  
/// It is commonly used in RESTful APIs to wrap returned data and contextual information
/// (for example, pagination details or response summaries).
/// </remarks>
public record SuccessResponse<T> : SuccessResponse
{
    /// <summary>
    /// Gets the main data payload of the success response.
    /// </summary>
    public T Data { get; init; }

    /// <summary>
    /// Gets optional metadata associated with the response.
    /// </summary>
    /// <remarks>
    /// Typically contains pagination information (see <see cref="Meta"/>) 
    /// or other contextual data relevant to the result.
    /// </remarks>
    public Meta? Meta { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SuccessResponse{T}"/> class.
    /// </summary>
    /// <param name="Data">The main data payload of the response.</param>
    /// <param name="Meta">Optional metadata associated with the response.</param>
    private SuccessResponse(T Data, Meta? Meta)
    {
        this.Data = Data;
        this.Meta = Meta;
    }

    /// <summary>
    /// Creates a new <see cref="SuccessResponse{T}"/> instance with the specified data and optional metadata.
    /// </summary>
    /// <param name="data">The response data to include.</param>
    /// <param name="meta">Optional metadata such as pagination details.</param>
    /// <returns>
    /// A new <see cref="SuccessResponse{T}"/> object initialized with the provided values.
    /// </returns>
    public static SuccessResponse<T> Create(T data, Meta? meta = null) => new(data, meta);

    /// <summary>
    /// Converts the success response into an <see cref="IResult"/> suitable for minimal API endpoints.
    /// </summary>
    /// <remarks>
    /// This method wraps the response in a standard HTTP 200 OK result 
    /// using <see cref="Results.Ok(object?)"/> for direct use in ASP.NET Core Minimal APIs.
    /// </remarks>
    /// <returns>An <see cref="IResult"/> representing a 200 OK response containing this object.</returns>
    public IResult ToMinimalApiResult() => Results.Ok(this);
}