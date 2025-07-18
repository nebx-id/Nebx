namespace Nebx.Shared.Dtos;

public record ErrorDto
{
    public int StatusCode { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public string? Path { get; private set; }
    public string? RequestId { get; private set; }
    public IReadOnlyDictionary<string, string>? Errors { get; private set; }

    private ErrorDto()
    {
    }

    public static ErrorDto Create(string message, int statusCode, string path, string requestId)
    {
        return new ErrorDto()
        {
            StatusCode = statusCode,
            Message = message,
            Path = path,
            RequestId = requestId,
        };
    }

    public static ErrorDto Create(string message, int statusCode)
    {
        return new ErrorDto()
        {
            StatusCode = statusCode,
            Message = message,
        };
    }

    public ErrorDto SetPath(string path)
    {
        Path = path;
        return this;
    }

    public ErrorDto SetRequestId(string requestId)
    {
        RequestId = requestId;
        return this;
    }

    public void AddErrors(IReadOnlyDictionary<string, string> errors)
    {
        if (errors.Count == 0) return;

        Errors = errors;
    }
}