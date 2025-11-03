namespace Nebx.BuildingBlocks.AspNetCore.Contracts.Models.Responses;

/// <summary>
/// Represents detailed information about an error that occurred during an API request.
/// </summary>
public record ErrorDetail
{
    private ErrorDetail()
    {
    }

    /// <summary>
    /// The HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    /// A human-readable message describing the error.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// An optional application-specific code identifying the error type.
    /// </summary>
    public string? ErrorCode { get; private set; }

    /// <summary>
    /// A collection of key-value pairs providing additional error context.
    /// Commonly used for validation errors, where each key represents a field name
    /// and the value describes the validation issue.
    /// </summary>
    public IReadOnlyDictionary<string, string>? Errors { get; private set; }

    /// <summary>
    /// The unique trace identifier associated with the request, useful for troubleshooting.
    /// </summary>
    public string? TraceId { get; private set; }

    /// <summary>
    /// Creates a new <see cref="ErrorDetail"/> instance with the specified parameters.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the error. Must be between 100 and 599.</param>
    /// <param name="message">A human-readable message describing the error. Cannot be null, empty, or whitespace.</param>
    /// <param name="traceId">Optional trace identifier. If null, a new GUID will be generated automatically.</param>
    /// <returns>A configured <see cref="ErrorDetail"/> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="statusCode"/> is outside the valid HTTP range (100–599).</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="message"/> is null, empty, or whitespace.</exception>
    public static ErrorDetail Create(
        int statusCode,
        string message,
        string? traceId = null)
    {
        if (statusCode is < 100 or > 599)
            throw new ArgumentOutOfRangeException(nameof(statusCode),
                $"Invalid HTTP status code: {statusCode}. Must be between 100 and 599.");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Error message cannot be null, empty, or whitespace.", nameof(message));

        return new ErrorDetail()
        {
            StatusCode = statusCode,
            Message = message,
            TraceId = string.IsNullOrWhiteSpace(traceId) ? Guid.NewGuid().ToString() : traceId,
        };
    }

    /// <summary>
    /// Sets the <see cref="ErrorCode"/> to the specified application-defined value.
    /// If an error code already exists, it is replaced.
    /// </summary>
    /// <param name="errorCode">The application-specific identifier representing the error type.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="errorCode"/> is null, empty, or consists only of whitespace.
    /// </exception>
    public void AddErrorCode(string errorCode)
    {
        if (string.IsNullOrWhiteSpace(errorCode))
            throw new ArgumentException("Error code cannot be null, empty, or whitespace.", nameof(errorCode));

        ErrorCode = errorCode;
    }

    /// <summary>
    /// Replaces the current <see cref="Errors"/> dictionary with the specified collection of error messages.
    /// Typically used to associate validation messages with specific fields or properties.
    /// </summary>
    /// <param name="errors">A dictionary containing field names and corresponding error messages.</param>
    /// <exception cref="ArgumentException">Thrown when the provided dictionary is empty.</exception>
    public void AddErrors(IReadOnlyDictionary<string, string> errors)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("The provided error collection cannot be null or empty.", nameof(errors));

        Errors = errors;
    }
}