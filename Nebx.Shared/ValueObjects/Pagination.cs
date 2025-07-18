namespace Nebx.Shared.ValueObjects;

public record Pagination
{
    public int Page { get; }
    public int PageSize { get; }
    public int Offset => (Page - 1) * PageSize;
    public const int DefaultSize = 10;

    public Pagination(int page, int pageSize = DefaultSize)
    {
        if (page < 1) throw new ArgumentOutOfRangeException(nameof(page));
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));


        Page = page;
        PageSize = pageSize;
    }
}