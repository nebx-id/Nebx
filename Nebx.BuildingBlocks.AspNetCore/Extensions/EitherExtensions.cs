using Funcky;
using Funcky.Monads;
using Microsoft.AspNetCore.Http;
using Nebx.BuildingBlocks.AspNetCore.Models;
using Nebx.BuildingBlocks.AspNetCore.Models.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions;

public static class EitherExtension
{
    public static IResult ToMinimalApiResult<T>(this Either<Failure, T> either) where T : notnull
    {
        return either.Match(
            right: value => value is Unit
                ? Results.Ok()
                : Results.Ok(value),
            left: failure =>
            {
                var errorResponse = failure.ToErrorResponse();
                return Results.Json(errorResponse, statusCode: errorResponse.StatusCode);
            }
        );
    }

    public static IResult ToMinimalApiResult(this Either<Failure, Success> either)
    {
        return either.Match(
            right: value => value.ToMinimalApiResult(), left: failure =>
            {
                var errorResponse = failure.ToErrorResponse();
                return Results.Json(errorResponse, statusCode: errorResponse.StatusCode);
            });
    }

    public static IResult ToMinimalApiResult(this Failure httpStatusFailure)
        => Results.Json(httpStatusFailure.ToErrorResponse(),
            statusCode: httpStatusFailure.StatusCode);

    public static IResult ToMinimalApiResult(this Success success)
        => success switch
        {
            NoContent => Results.NoContent(),
            Ok ok => Results.Ok(),
            Ok<object?> { Value: null } => Results.Ok(),
            Ok<object?> ok => Results.Ok(ok.Value),
            Created created => Results.Created(created.Location, null),
            Created<object?> created => Results.Created(created.Location, created.Value),
            _ => throw new NotSupportedException(
                $"Success type '{success.GetType().Name}' is not mapped to an IResult."),
        };

    private static ErrorResponse ToErrorResponse(this Failure failure)
    {
        var errorResponse = failure switch
        {
            BadRequest f => ErrorResponse.Create(f.Message, f.StatusCode),
            NotFound f => ErrorResponse.Create(f.Message, f.StatusCode),
            Conflict f => ErrorResponse.Create(f.Message, f.StatusCode),
            Unauthorized f => ErrorResponse.Create(f.Message, f.StatusCode),
            Forbidden f => ErrorResponse.Create(f.Message, f.StatusCode),
            _ => throw new NotSupportedException(
                $"Success type '{failure.GetType().Name}' is not mapped to an IResult."),
        };

        if (failure is BadRequest badRequest) errorResponse.AddErrors(badRequest.Errors);
        return errorResponse;
    }
}