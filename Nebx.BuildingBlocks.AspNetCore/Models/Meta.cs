namespace Nebx.BuildingBlocks.AspNetCore.Models;

public class Meta
{
    public int? Page { get; private set; }
    public int? TotalPages { get; private set; }
    public int? TotalCount { get; private set; }
    public int? PageSize { get; private set; }
    public bool? HasNextPage => Page.HasValue && PageSize.HasValue ? Page * PageSize < TotalCount : null;
    public bool? HasPreviousPage => Page.HasValue && PageSize.HasValue ? Page > 1 : null;

    public void AddPagination(Pagination pagination, int totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize);
        var isPageInvalid = pagination.Page > 1 && pagination.Page > totalPages;

        Page = isPageInvalid ? 1 : pagination.Page;
        TotalPages = totalPages == 0 ? 1 : totalPages;
        PageSize = pagination.PageSize;
        TotalCount = totalCount;
    }
}