using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents an HTTP 501 Not Implemented failure.
/// </summary>
public sealed record NotImplementedFailure()
    : HttpFailure(
        StatusCodes.Status501NotImplemented,
        "Not Implemented",
        "The requested functionality is not implemented.");