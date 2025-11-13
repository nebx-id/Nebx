using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures;

/// <summary>
/// Represents an HTTP 403 Forbidden failure.
/// </summary>
public sealed record ForbiddenFailure()
    : HttpFailure(
        StatusCodes.Status403Forbidden,
        "Forbidden",
        "You do not have permission to perform this action.");