namespace Nebx.Shared.Dtos;

public static class SuccessDto
{
    public static SuccessDto<T> Create<T>(T? data)
    {
        return new SuccessDto<T>(data);
    }

    public static SuccessDto<T> Create<T>(T? data, MetaDto meta)
    {
        return new SuccessDto<T>(data, meta);
    }
}

public record SuccessDto<T>
{
    public T? Data { get; internal init; }
    public MetaDto? Meta { get; private init; }

    internal SuccessDto(T? data)
    {
        Data = data;
    }

    internal SuccessDto(T? data, MetaDto? meta)
    {
        Data = data;
        Meta = meta;
    }
}