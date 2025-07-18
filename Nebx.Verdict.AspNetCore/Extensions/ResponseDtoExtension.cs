using Microsoft.AspNetCore.Http;
using Nebx.Verdict.AspNetCore.Dtos;

namespace Nebx.Verdict.AspNetCore.Extensions;

public static class ResponseDtoExtension
{
    public static IResult ToMinimalApiResult<T>(this SuccessDto<T> dto)
    {
        return Results.Ok(dto);
    }
}