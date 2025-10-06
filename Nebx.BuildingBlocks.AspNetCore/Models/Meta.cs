namespace Nebx.BuildingBlocks.AspNetCore.Models;

using System;

/// <summary>
/// Represents pagination metadata for paginated API responses.
/// </summary>
/// <remarks>
/// The <see cref="Meta"/> class contains information about the current page, 
/// total pages, page size, and total item count in a paginated result set.
/// It also provides computed properties to indicate whether there are 
/// previous or next pages available.
/// </remarks>
public class Meta
{
    /// <summary>
    /// Gets the current page number in the pagination context.
    /// </summary>
    /// <remarks>
    /// This value is determined based on the <see cref="Pagination"/> object 
    /// passed to <see cref="AddPagination(Pagination, int)"/>.
    /// </remarks>
    public int? Page { get; private set; }

    /// <summary>
    /// Gets the total number of pages available in the dataset.
    /// </summary>
    public int? TotalPages { get; private set; }

    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public int? TotalCount { get; private set; }

    /// <summary>
    /// Gets the number of items contained in each page.
    /// </summary>
    public int? PageSize { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there is another page of results available after the current one.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if there is a subsequent page; otherwise, <c>false</c>.
    /// Returns <c>null</c> if pagination information is incomplete.
    /// </remarks>
    public bool? HasNextPage => Page.HasValue && PageSize.HasValue
        ? Page * PageSize < TotalCount
        : null;

    /// <summary>
    /// Gets a value indicating whether there is a previous page of results before the current one.
    /// </summary>
    /// <remarks>
    /// Returns <c>true</c> if the current page number is greater than 1; otherwise, <c>false</c>.
    /// Returns <c>null</c> if pagination information is incomplete.
    /// </remarks>
    public bool? HasPreviousPage => Page.HasValue && PageSize.HasValue
        ? Page > 1
        : null;

    /// <summary>
    /// Populates pagination metadata using the provided <see cref="Pagination"/> parameters and total item count.
    /// </summary>
    /// <param name="pagination">The pagination parameters that include the current page and page size.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <remarks>
    /// This method calculates the total number of pages and ensures that the page number 
    /// does not exceed the valid range. If an invalid page number is provided, 
    /// the <see cref="Page"/> value defaults to 1.
    /// </remarks>
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
