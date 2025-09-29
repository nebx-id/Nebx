namespace Nebx.BuildingBlocks.AspNetCore.Models;

public static class Paginated
{
    public static Paginated<T> Create<T>(IEnumerable<T> items, int totalCount)
    {
        return new Paginated<T>(items, totalCount);
    }
}

public record Paginated<T>(IEnumerable<T> Items, int TotalCount);