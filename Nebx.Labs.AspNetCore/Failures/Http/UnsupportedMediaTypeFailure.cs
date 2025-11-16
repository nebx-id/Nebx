using Microsoft.AspNetCore.Http;

namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents an HTTP 415 Unsupported Media Type failure.
/// </summary>
public sealed record UnsupportedMediaTypeFailure()
    : HttpFailure(
        StatusCodes.Status415UnsupportedMediaType,
        "Unsupported Media Type",
        "The media type of the request is not supported.");