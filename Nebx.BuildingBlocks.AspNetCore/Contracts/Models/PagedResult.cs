namespace Nebx.BuildingBlocks.AspNetCore.Contracts.Models;

/// <summary>
/// Represents a paginated result set containing a collection of items and the total count.
/// </summary>
/// <typeparam name="T">
/// The type of items contained within the paginated result.
/// </typeparam>
/// <param name="Items">
/// The read-only collection of items in the current page.
/// </param>
/// <param name="TotalCount">
/// The total number of items available across all pages.
/// </param>
/// <remarks>
/// This record is typically used to return paged query results from repositories or application services,
/// complementing the <see cref="Pagination"/> request model.
/// </remarks>
public record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount);
