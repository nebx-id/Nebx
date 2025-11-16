using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents an HTTP 500 Internal Server Error failure.
/// </summary>
public sealed record InternalServerErrorFailure()
    : HttpFailure(StatusCodes.Status500InternalServerError,
        "Internal Server Error",
        "An unexpected server error occurred.");