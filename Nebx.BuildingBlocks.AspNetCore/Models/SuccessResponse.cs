using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

public record SuccessResponse
{
    public static SuccessResponse<T> Create<T>(T data, Meta? meta = null)
    {
        return SuccessResponse<T>.Create(data, meta);
    }
}

public record SuccessResponse<T> : SuccessResponse
{
    public T Data { get; init; }
    public Meta? Meta { get; init; }

    private SuccessResponse(T Data, Meta? Meta)
    {
        this.Data = Data;
        this.Meta = Meta;
    }

    public static SuccessResponse<T> Create(T data, Meta? meta = null) => new(data, meta);

    public IResult ToMinimalApiResult() => Results.Ok(this);
}