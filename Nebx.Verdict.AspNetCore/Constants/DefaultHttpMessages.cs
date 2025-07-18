namespace Nebx.Verdict.AspNetCore.Constants;

public static class DefaultHttpMessages
{
    public const string NotFound = "The requested resource could not be found.";
    public const string BadRequest = "Your request could not be processed due to invalid or missing data.";
    public const string Unauthorized = "You must be authenticated to access this resource.";
    public const string Forbidden = "You do not have permission to access this resource.";
    public const string UnprocessableEntity = "Your request was understood but contains data that cannot be processed.";
    public const string Conflict = "A resource with the same identifier already exists, causing a conflict.";

    public const string InternalServerError =
        "The server encountered an error while processing your request. Please contact support if the problem persists.";
}