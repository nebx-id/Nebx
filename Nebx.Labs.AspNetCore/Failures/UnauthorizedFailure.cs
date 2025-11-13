using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures;

/// <summary>
/// Represents an HTTP 401 Unauthorized failure.
/// </summary>
public sealed record UnauthorizedFailure()
    : HttpFailure(
        StatusCodes.Status401Unauthorized,
        "Unauthorized",
        "Authentication is required to access this resource or the provided credentials are invalid.");