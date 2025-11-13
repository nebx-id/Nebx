using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures;

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