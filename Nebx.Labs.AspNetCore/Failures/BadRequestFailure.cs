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
    /// Initializes a new instance of the <see cref="BadRequestFailure"/> record,
    /// representing a 400 Bad Request error.
    /// </summary>
    /// <param name="detail">
    /// Optional human-readable explanation of the failure. If not provided,
    /// a default message is derived from <paramref name="errors"/>.
    /// </param>
    /// <param name="errors">
    /// Optional dictionary of validation errors, where each key maps to one or more issues.
    /// </param>
    public BadRequestFailure(string? detail = null, IReadOnlyDictionary<string, string[]>? errors = null)
        : base(
            StatusCodes.Status400BadRequest,
            "Bad Request",
            detail ?? (errors is not null && errors.Count > 0
                ? "One or more request parameters are invalid."
                : "Invalid request.")
        )
    {
        Errors = errors;
    }
}
