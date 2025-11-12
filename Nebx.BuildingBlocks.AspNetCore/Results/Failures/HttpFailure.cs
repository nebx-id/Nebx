using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Results.Failures;

/// <summary>
/// Represents a standardized HTTP failure with a status code, title, and detailed message.
/// </summary>
/// <param name="StatusCode">The HTTP status code describing the failure.</param>
/// <param name="Title">A short, human-readable summary of the failure.</param>
/// <param name="Detail">A detailed message explaining the failure.</param>
/// <remarks>
/// Serves as the base record for all HTTP failure types (e.g., <see cref="BadRequestFailure"/>).
/// </remarks>
public abstract record HttpFailure(int StatusCode, string Title, string Detail) : Failure(Detail);

/// <summary>
/// Represents an HTTP 400 Bad Request failure with optional validation errors.
/// </summary>
public sealed record BadRequestFailure : HttpFailure
{
    /// <summary>
    /// Gets optional validation or field-level errors associated with the failure.
    /// </summary>
    public IReadOnlyDictionary<string, string[]>? Errors { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestFailure"/> record.
    /// </summary>
    /// <param name="errors">Optional detailed validation errors.</param>
    public BadRequestFailure(IReadOnlyDictionary<string, string[]>? errors = null)
        : base(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            errors is not null && errors.Count > 0 ? "One or more request parameters are invalid." : "Invalid request."
        )
    {
        Errors = errors;
    }
}

/// <summary>
/// Represents an HTTP 401 Unauthorized failure.
/// </summary>
public sealed record UnauthorizedFailure()
    : HttpFailure(
        StatusCodes.Status401Unauthorized,
        "Unauthorized",
        "Authentication is required to access this resource or the provided credentials are invalid.");

/// <summary>
/// Represents an HTTP 403 Forbidden failure.
/// </summary>
public sealed record ForbiddenFailure()
    : HttpFailure(
        StatusCodes.Status403Forbidden,
        "Forbidden",
        "You do not have permission to perform this action.");

/// <summary>
/// Represents an HTTP 404 Not Found failure.
/// </summary>
public sealed record NotFoundFailure(string? Detail = null)
    : HttpFailure(
        StatusCodes.Status404NotFound,
        "Not Found",
        Detail ?? "The requested resource could not be found.");

/// <summary>
/// Represents an HTTP 409 Conflict failure.
/// </summary>
public sealed record ConflictFailure(string? Detail = null)
    : HttpFailure(
        StatusCodes.Status409Conflict,
        "Conflict",
        Detail ?? "A conflict occurred with the current state of the resource.");

/// <summary>
/// Represents an HTTP 415 Unsupported Media Type failure.
/// </summary>
public sealed record UnsupportedMediaTypeFailure()
    : HttpFailure(
        StatusCodes.Status415UnsupportedMediaType,
        "Unsupported Media Type",
        "The media type of the request is not supported.");

/// <summary>
/// Represents an HTTP 422 Unprocessable Entity failure.
/// </summary>
public sealed record UnprocessableEntityFailure(string? Detail = null)
    : HttpFailure(StatusCodes.Status422UnprocessableEntity,
        "Unprocessable Entity",
        Detail ?? "The request could not be processed due to validation errors.");

/// <summary>
/// Represents an HTTP 500 Internal Server Error failure.
/// </summary>
public sealed record InternalServerErrorFailure()
    : HttpFailure(StatusCodes.Status500InternalServerError,
        "Internal Server Error",
        "An unexpected server error occurred.");

/// <summary>
/// Represents an HTTP 501 Not Implemented failure.
/// </summary>
public sealed record NotImplementedFailure()
    : HttpFailure(
        StatusCodes.Status501NotImplemented,
        "Not Implemented",
        "The requested functionality is not implemented.");
