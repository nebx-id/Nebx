using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Core.Failures;

/// <summary>
/// Represents a standardized HTTP failure with a specific status code and message.
/// </summary>
/// <param name="StatusCode">The HTTP status code associated with the failure.</param>
/// <param name="Message">A descriptive message explaining the failure.</param>
public abstract record HttpFailure(int StatusCode, string Message) : Failure(Message);

/// <summary>
/// Represents an HTTP 400 Bad Request failure with optional validation errors.
/// </summary>
public sealed record BadRequestFailure : HttpFailure
{
    /// <summary>
    /// Gets optional validation or field-level errors associated with the failure.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Errors { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BadRequestFailure"/> record.
    /// </summary>
    /// <param name="errors">Optional detailed validation errors.</param>
    public BadRequestFailure(IReadOnlyDictionary<string, string>? errors = null)
        : base(
            StatusCodes.Status400BadRequest,
            "The request could not be understood or was missing required parameters."
        )
    {
        Errors = errors;
    }
}

/// <summary>
/// Represents an HTTP 401 Unauthorized failure.
/// </summary>
public sealed record UnauthorizedFailure()
    : HttpFailure(StatusCodes.Status401Unauthorized,
        "Authentication is required or has failed.");

/// <summary>
/// Represents an HTTP 403 Forbidden failure.
/// </summary>
public sealed record ForbiddenFailure()
    : HttpFailure(StatusCodes.Status403Forbidden,
        "You do not have permission to perform this action.");

/// <summary>
/// Represents an HTTP 404 Not Found failure.
/// </summary>
public sealed record NotFoundFailure()
    : HttpFailure(StatusCodes.Status404NotFound,
        "The requested resource could not be found.");

/// <summary>
/// Represents an HTTP 409 Conflict failure.
/// </summary>
public sealed record ConflictFailure()
    : HttpFailure(StatusCodes.Status409Conflict,
        "A conflict occurred with the current state of the resource.");

/// <summary>
/// Represents an HTTP 415 Unsupported Media Type failure.
/// </summary>
public sealed record UnsupportedMediaTypeFailure()
    : HttpFailure(StatusCodes.Status415UnsupportedMediaType,
        "The media type of the request is not supported.");

/// <summary>
/// Represents an HTTP 422 Unprocessable Entity failure.
/// </summary>
public sealed record UnprocessableEntityFailure()
    : HttpFailure(StatusCodes.Status422UnprocessableEntity,
        "The request could not be processed due to validation errors.");

/// <summary>
/// Represents an HTTP 500 Internal Server Error failure.
/// </summary>
public sealed record InternalServerErrorFailure()
    : HttpFailure(StatusCodes.Status500InternalServerError,
        "An unexpected server error occurred.");

/// <summary>
/// Represents an HTTP 501 Not Implemented failure.
/// </summary>
public sealed record NotImplementedFailure()
    : HttpFailure(StatusCodes.Status501NotImplemented,
        "The requested functionality is not implemented.");