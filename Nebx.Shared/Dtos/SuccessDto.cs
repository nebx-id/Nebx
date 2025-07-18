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

public class SuccessDto<T>
{
    public T? Data { get; internal set; }
    public MetaDto? Meta { get; private set; }

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