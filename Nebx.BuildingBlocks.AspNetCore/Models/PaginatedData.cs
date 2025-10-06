namespace Nebx.BuildingBlocks.AspNetCore.Models;

/// <summary>
/// Provides factory methods for creating instances of <see cref="PaginatedData{T}"/>.
/// </summary>
/// <remarks>
/// The <see cref="PaginatedData"/> static class serves as a non-generic entry point 
/// for constructing paginated data results in a type-safe manner.  
/// It enables simplified creation without having to explicitly specify the generic type parameter 
/// when type inference is available.
/// </remarks>
public static class PaginatedData
{
    /// <summary>
    /// Creates a new <see cref="PaginatedData{T}"/> instance containing the specified items and total record count.
    /// </summary>
    /// <typeparam name="T">The type of items contained within the paginated result.</typeparam>
    /// <param name="items">The collection of items returned for the current page.</param>
    /// <param name="totalCount">The total number of items available across all pages.</param>
    /// <returns>
    /// A new instance of <see cref="PaginatedData{T}"/> encapsulating the provided items and total count.
    /// </returns>
    /// <example>
    /// <code>
    /// var pagedUsers = PaginatedData.Create(users, totalUsers);
    /// </code>
    /// </example>
    public static PaginatedData<T> Create<T>(IEnumerable<T> items, int totalCount)
        => PaginatedData<T>.Create(items, totalCount);
}

/// <summary>
/// Represents a paginated collection of data and the total number of available records.
/// </summary>
/// <typeparam name="T">The type of the items in the paginated result set.</typeparam>
/// <remarks>
/// The <see cref="PaginatedData{T}"/> record is commonly used in API responses or 
/// data access layers to represent a subset of results (the current page) 
/// along with the overall record count for pagination purposes.  
///
/// This model pairs well with the <see cref="Meta"/> and <see cref="Pagination"/> types 
/// to deliver structured, self-describing paginated responses.
/// </remarks>
public record PaginatedData<T>
{
    /// <summary>
    /// Gets the collection of items contained in the current page of data.
    /// </summary>
    public IEnumerable<T> Items { get; init; }

    /// <summary>
    /// Gets the total number of items available across all pages.
    /// </summary>
    /// <remarks>
    /// This property represents the total count of records matching the query or data source, 
    /// regardless of the current page size or offset.
    /// </remarks>
    public int TotalCount { get; init; }

    private PaginatedData(IEnumerable<T> items, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Creates a new <see cref="PaginatedData{T}"/> instance.
    /// </summary>
    /// <param name="items">The items to include in the paginated response.</param>
    /// <param name="totalCount">The total number of available records.</param>
    /// <returns>A new <see cref="PaginatedData{T}"/> object containing the provided data and count.</returns>
    /// <example>
    /// <code>
    /// var result = PaginatedData&lt;Product&gt;.Create(products, totalProducts);
    /// </code>
    /// </example>
    public static PaginatedData<T> Create(IEnumerable<T> items, int totalCount) => new(items, totalCount);
}
