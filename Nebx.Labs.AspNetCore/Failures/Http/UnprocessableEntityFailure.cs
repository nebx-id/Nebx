using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents an HTTP 422 Unprocessable Entity failure.
/// </summary>
public sealed record UnprocessableEntityFailure(string? Detail = null)
    : HttpFailure(StatusCodes.Status422UnprocessableEntity,
        "Unprocessable Entity",
        Detail ?? "The request could not be processed due to validation errors.");