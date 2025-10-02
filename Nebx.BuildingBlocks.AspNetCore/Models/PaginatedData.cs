namespace Nebx.BuildingBlocks.AspNetCore.Models;

public static class PaginatedData
{
    public static PaginatedData<T> Create<T>(IEnumerable<T> items, int totalCount)
        => PaginatedData<T>.Create(items, totalCount);
}

public record PaginatedData<T>
{
    public IEnumerable<T> Items { get; init; }
    public int TotalCount { get; init; }

    private PaginatedData(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    public static PaginatedData<T> Create(IEnumerable<T> items, int totalCount) => new(items, totalCount);
}