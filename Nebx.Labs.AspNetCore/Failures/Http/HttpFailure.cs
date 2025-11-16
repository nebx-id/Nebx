namespace Nebx.Labs.AspNetCore.Failures.Http;

/// <summary>
/// Represents a standardized HTTP failure with a status code, title, and detailed message.
/// </summary>
/// <param name="StatusCode">The HTTP status code describing the failure.</param>
/// <param name="Title">A short, human-readable summary of the failure.</param>
/// <param name="Detail">A detailed message explaining the failure.</param>
/// <remarks>
/// Serves as the base record for all HTTP failure types (e.g., <see cref="BadRequestFailure"/>).
/// </remarks>
public record HttpFailure(int StatusCode, string Title, string Detail);
