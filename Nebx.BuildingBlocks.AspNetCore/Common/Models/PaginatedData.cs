namespace Nebx.BuildingBlocks.AspNetCore.Common.Models;

public static class PaginatedData
{
    public static PaginatedData<T> Create<T>(IEnumerable<T> items, int totalCount)
    {
        return new PaginatedData<T>(items, totalCount);
    }
}

public record PaginatedData<T>(IEnumerable<T> Items, int TotalCount);