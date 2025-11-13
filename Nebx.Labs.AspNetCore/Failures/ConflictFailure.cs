using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures;

/// <summary>
/// Represents an HTTP 409 Conflict failure.
/// </summary>
public sealed record ConflictFailure(string? Detail = null)
    : HttpFailure(
        StatusCodes.Status409Conflict,
        "Conflict",
        Detail ?? "A conflict occurred with the current state of the resource.");