namespace Nebx.BuildingBlocks.AspNetCore.Models.Responses;

public record ErrorResponse
{
    public int StatusCode { get; private set; }
    public string Message { get; private set; } = string.Empty;
    public string? ErrorCode { get; private set; }
    public IReadOnlyDictionary<string, string>? Errors { get; private set; }

    private ErrorResponse()
    {
    }

    public static ErrorResponse Create(string message, int statusCode, string? errorCode = null)
    {
        return new ErrorResponse()
        {
            StatusCode = statusCode,
            Message = message,
            ErrorCode = errorCode
        };
    }

    public void AddErrorCode(string code) => ErrorCode = code;

    public void AddErrors(IReadOnlyDictionary<string, string>? errors) => Errors = errors;
}