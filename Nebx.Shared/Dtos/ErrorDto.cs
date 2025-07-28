using Ardalis.GuardClauses;

namespace Nebx.Shared.Dtos;

public record ErrorDto
{
    public int StatusCode { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public string RequestId { get; private set; } = string.Empty;
    public string? ErrorCode { get; private set; }
    public IReadOnlyDictionary<string, string>? Errors { get; private set; }

    private ErrorDto()
    {
    }

    public static ErrorDto Create(string message, int statusCode, string path, string requestId)
    {
        return new ErrorDto()
        {
            StatusCode = Guard.Against.NegativeOrZero(statusCode, nameof(statusCode)),
            Message = Guard.Against.NullOrWhiteSpace(message, nameof(message)),
            Path = Guard.Against.NullOrWhiteSpace(path, nameof(path)),
            RequestId = Guard.Against.NullOrWhiteSpace(requestId, nameof(requestId)),
        };
    }

    public void AddErrors(IReadOnlyDictionary<string, string> errors)
    {
        if (errors.Count == 0) return;

        Errors = errors;
    }

    public void AddErrorCode(string code)
    {
        ErrorCode = Guard.Against.NullOrWhiteSpace(code, nameof(code));
    }
}