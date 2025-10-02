namespace Nebx.BuildingBlocks.AspNetCore.Models;

public record Pagination
{
    public int Page { get; }
    public int PageSize { get; }
    public int Offset => (Page - 1) * PageSize;

    public Pagination(int page, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        Page = page;
        PageSize = pageSize;
    }
}