using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

/// <summary>
/// Represents a standardized error response that can be returned from an API or service.
/// </summary>
/// <remarks>
/// The <see cref="ErrorResponse"/> record encapsulates key information about an error, 
/// including its HTTP status code, descriptive message, and optional error details.
/// It supports attaching an error code and a collection of validation or field-specific errors.
/// </remarks>
public record ErrorResponse
{
    /// <summary>
    /// Gets the HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    /// Gets the human-readable message describing the error.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets an optional application-specific error code for identifying the type of error.
    /// </summary>
    public string? ErrorCode { get; private set; }

    /// <summary>
    /// Gets a collection of key-value pairs containing specific error details.
    /// Typically used for validation errors where each key is a field name and 
    /// each value describes the validation issue.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Errors { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// This constructor is private to enforce controlled creation via the <see cref="Create"/> method.
    /// </summary>
    private ErrorResponse()
    {
    }

    /// <summary>
    /// Creates a new <see cref="ErrorResponse"/> instance with the specified message and status code.
    /// </summary>
    /// <param name="message">The descriptive message associated with the error.</param>
    /// <param name="statusCode">The HTTP status code representing the error type.</param>
    /// <returns>A configured <see cref="ErrorResponse"/> instance.</returns>
    public static ErrorResponse Create(string message, int statusCode)
    {
        return new ErrorResponse()
        {
            StatusCode = statusCode,
            Message = message,
        };
    }

    /// <summary>
    /// Adds an application-specific error code to the response.
    /// </summary>
    /// <param name="code">The unique error code identifying the error type or category.</param>
    public void AddErrorCode(string code) => ErrorCode = code;

    /// <summary>
    /// Adds a collection of key-value error details to the response.
    /// </summary>
    /// <param name="errors">
    /// A dictionary of error details where each key represents the name of a field or parameter, 
    /// and each value contains the corresponding validation or error message.
    /// </param>
    public void AddErrors(IReadOnlyDictionary<string, string>? errors) => Errors = errors;
}
