using Nebx.Labs.Core.Domain.Models;

namespace Nebx.Labs.AspNetCore.Responses;

/// <summary>
/// Represents metadata associated with paginated API responses.
/// </summary>
public sealed class Meta
{
    /// <summary>
    /// The current page number in the pagination context.
    /// </summary>
    /// <remarks>
    /// This value is derived from the <see cref="Pagination"/> object
    /// supplied to <see cref="AddPagination(Pagination, int)"/>.
    /// </remarks>
    public int? Page { get; private set; }

    /// <summary>
    /// The total number of pages available in the dataset.
    /// </summary>
    public int? TotalPages { get; private set; }

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public int? TotalCount { get; private set; }

    /// <summary>
    /// The number of items contained in each page.
    /// </summary>
    public int? PageSize { get; private set; }

    /// <summary>
    /// Indicates whether there is a subsequent page of results available.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if there is another page after the current one; otherwise, <c>false</c>.
    /// Returns <c>null</c> if pagination information is incomplete.
    /// </remarks>
    public bool? HasNextPage => Page.HasValue && PageSize.HasValue
        ? Page * PageSize < TotalCount
        : null;

    /// <summary>
    /// Indicates whether there is a preceding page of results available.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if the current page number is greater than 1; otherwise, <c>false</c>.
    /// Returns <c>null</c> if pagination information is incomplete.
    /// </remarks>
    public bool? HasPreviousPage => Page.HasValue && PageSize.HasValue
        ? Page > 1
        : null;

    /// <summary>
    /// Populates pagination metadata using the provided page parameters and total item count.
    /// </summary>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <remarks>
    /// Calculates the total number of pages and ensures the page number
    /// remains within a valid range.
    /// If the specified page exceeds the total pages, the <see cref="Page"/> value defaults to 1.
    /// </remarks>
    public void AddPagination(int page, int pageSize, int totalCount)
    {
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var isPageInvalid = page > 1 && page > totalPages;

        Page = isPageInvalid ? 1 : page;
        TotalPages = totalPages == 0 ? 1 : totalPages;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Populates pagination metadata using a <see cref="Pagination"/> object.
    /// </summary>
    /// <param name="pagination">The pagination parameters containing the current page and page size.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    public void AddPagination(Pagination pagination, int totalCount)
        => AddPagination(pagination.Page, pagination.PageSize, totalCount);
}
