using Microsoft.AspNetCore.Http;
using Nebx.Verdict.AspNetCore.Constants;
using Nebx.Verdict.AspNetCore.Dtos;

namespace Nebx.Verdict.AspNetCore.Extensions;

public static class VerdictToResultExtension
{
    public static IResult ToMinimalApiResult(this IVerdict verdict, IHttpContextAccessor accessor)
    {
        return verdict.IsSuccess
            ? CreateSuccessResponse(verdict)
            : CreateErrorResponse(verdict, accessor);
    }

    private static IResult CreateSuccessResponse(IVerdict verdict)
    {
        var metadata = verdict.GetMetadata() ?? throw new InvalidOperationException("Metadata is null");
        var statusCode = (HttpStatusCodes)metadata[HttpKeys.StatusCode];

        return statusCode switch
        {
            HttpStatusCodes.Success => verdict is Verdict ? Results.Ok() : Results.Ok(verdict.GetValue()),
            HttpStatusCodes.NoContent => Results.NoContent(),
            HttpStatusCodes.Created => Results.Created("", verdict.GetValue()),
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null)
        };
    }

    private static IResult CreateErrorResponse(IVerdict verdict, IHttpContextAccessor accessor)
    {
        var metadata = verdict.GetMetadata() ?? throw new InvalidOperationException("Metadata is null");
        var context = accessor.HttpContext ?? throw new InvalidOperationException("Http context is null");
        var statusCode = (HttpStatusCodes)metadata[HttpKeys.StatusCode];

        var path = context.Request.Path;
        var requestId = context.TraceIdentifier;
        var response = ErrorDto.Create(verdict.Message, (int)statusCode, path, requestId);

        var errors = verdict.GetErrors();
        if (errors is not null) response.AddErrors(errors);

        return statusCode switch
        {
            HttpStatusCodes.BadRequest => Results.BadRequest(response),
            HttpStatusCodes.UnprocessableEntity => Results.UnprocessableEntity(response),
            HttpStatusCodes.NotFound => Results.NotFound(response),
            HttpStatusCodes.Conflict => Results.Conflict(response),
            HttpStatusCodes.Forbidden => Results.Json(
                response,
                contentType: ContentTypes.Json,
                statusCode: response.StatusCode),
            HttpStatusCodes.Unauthorized => Results.Json(
                response,
                contentType: ContentTypes.Json,
                statusCode: response.StatusCode),
            HttpStatusCodes.InternalServerError => throw new Exception(response.Message),
            _ => throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null)
        };
    }
}