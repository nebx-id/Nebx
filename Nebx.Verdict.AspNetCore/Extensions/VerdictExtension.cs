using Nebx.Verdict.AspNetCore.Constants;

namespace Nebx.Verdict.AspNetCore.Extensions;

public static class VerdictHttpHelper
{
    internal static Verdict<T> SetHttpError<T>(
        this Verdict<T> verdict,
        HttpStatusCodes statusCode,
        string message,
        IReadOnlyDictionary<string, string>? errors = null)
    {
        verdict.SetStatusCode(statusCode);
        verdict.SetMessage(message);
        if (errors != null) verdict.WithErrors(errors);

        return verdict;
    }

    internal static Verdict SetHttpError(
        this Verdict verdict,
        HttpStatusCodes statusCode,
        string message,
        IReadOnlyDictionary<string, string>? errors = null)
    {
        verdict.SetStatusCode(statusCode);
        verdict.SetMessage(message);
        if (errors != null) verdict.WithErrors(errors);

        return verdict;
    }

    internal static Verdict SetStatusCode(this Verdict verdict, HttpStatusCodes statusCode)
    {
        var metadata = new Dictionary<string, object> { [HttpKeys.StatusCode] = statusCode };
        return verdict.WithMetadata(metadata);
    }

    internal static Verdict<T> SetStatusCode<T>(this Verdict<T> verdict, HttpStatusCodes statusCode)
    {
        var metadata = new Dictionary<string, object> { [HttpKeys.StatusCode] = statusCode };
        return verdict.WithMetadata(metadata);
    }
}

public static class VerdictExtension
{
    public static Verdict<T> NotFound<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.NotFound, DefaultHttpMessages.NotFound);
        return verdict;
    }

    public static Verdict NotFound(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.NotFound, DefaultHttpMessages.NotFound);
        return verdict;
    }

    public static Verdict<T> BadRequest<T>(this Verdict<T> verdict, IReadOnlyDictionary<string, string>? errors = null)
    {
        verdict.SetHttpError(HttpStatusCodes.BadRequest, DefaultHttpMessages.BadRequest, errors);
        return verdict;
    }

    public static Verdict BadRequest(this Verdict verdict, IReadOnlyDictionary<string, string>? errors = null)
    {
        verdict.SetHttpError(HttpStatusCodes.BadRequest, DefaultHttpMessages.BadRequest, errors);
        return verdict;
    }

    public static Verdict<T> Forbidden<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Forbidden, DefaultHttpMessages.Forbidden);
        return verdict;
    }

    public static Verdict Forbidden(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Forbidden, DefaultHttpMessages.Forbidden);
        return verdict;
    }

    public static Verdict<T> Unauthorized<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Unauthorized, DefaultHttpMessages.Unauthorized);
        return verdict;
    }

    public static Verdict Unauthorized(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Unauthorized, DefaultHttpMessages.Unauthorized);
        return verdict;
    }

    public static Verdict<T> UnprocessableEntity<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.UnprocessableEntity, DefaultHttpMessages.UnprocessableEntity);
        return verdict;
    }

    public static Verdict UnprocessableEntity(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.UnprocessableEntity, DefaultHttpMessages.UnprocessableEntity);
        return verdict;
    }

    public static Verdict<T> Conflict<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Conflict, DefaultHttpMessages.Conflict);
        return verdict;
    }

    public static Verdict Conflict(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.Conflict, DefaultHttpMessages.Conflict);
        return verdict;
    }

    public static Verdict<T> InternalError<T>(this Verdict<T> verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.InternalServerError, DefaultHttpMessages.InternalServerError);
        return verdict;
    }

    public static Verdict InternalError(this Verdict verdict)
    {
        verdict.SetHttpError(HttpStatusCodes.InternalServerError, DefaultHttpMessages.InternalServerError);
        return verdict;
    }

    public static Verdict<T> Ok<T>(this Verdict<T> verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.Success);
        return verdict;
    }

    public static Verdict Ok(this Verdict verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.Success);
        return verdict;
    }

    public static Verdict<T> NoContent<T>(this Verdict<T> verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.NoContent);
        return verdict;
    }

    public static Verdict NoContent(this Verdict verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.NoContent);
        return verdict;
    }

    public static Verdict<T> Created<T>(this Verdict<T> verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.Created);
        return verdict;
    }

    public static Verdict Created(this Verdict verdict)
    {
        verdict.SetStatusCode(HttpStatusCodes.Created);
        return verdict;
    }
}