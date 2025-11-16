using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents an HTTP 404 Not Found failure.
/// </summary>
public sealed record NotFoundFailure(string? Detail = null)
    : HttpFailure(
        StatusCodes.Status404NotFound,
        "Not Found",
        Detail ?? "The requested resource could not be found.");