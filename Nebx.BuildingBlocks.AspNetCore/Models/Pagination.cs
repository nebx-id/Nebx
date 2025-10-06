namespace Nebx.BuildingBlocks.AspNetCore.Models;

using System;

/// <summary>
/// Represents pagination parameters for paged data retrieval.
/// </summary>
/// <remarks>
/// The <see cref="Pagination"/> record defines the page number and page size used to request
/// a specific subset of data from a larger collection. It also provides an <see cref="Offset"/>
/// property to assist with database or collection queries that support skip/take semantics.
/// </remarks>
public record Pagination
{
    /// <summary>
    /// Gets the current page number (1-based index).
    /// </summary>
    /// <remarks>
    /// This value must be greater than or equal to 1. 
    /// A value of 1 indicates the first page of results.
    /// </remarks>
    public int Page { get; }

    /// <summary>
    /// Gets the number of items to include per page.
    /// </summary>
    /// <remarks>
    /// This value must be greater than or equal to 1.
    /// The default page size is 10 if not specified.
    /// </remarks>
    public int PageSize { get; }

    /// <summary>
    /// Gets the number of items to skip when retrieving paged results.
    /// </summary>
    /// <remarks>
    /// The <see cref="Offset"/> is calculated as <c>(Page - 1) * PageSize</c>.
    /// It can be used in database queries (e.g., with LINQ’s <c>Skip()</c> and <c>Take()</c> methods).
    /// </remarks>
    public int Offset => (Page - 1) * PageSize;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pagination"/> record with the specified page and page size.
    /// </summary>
    /// <param name="page">The current page number. Must be greater than or equal to 1.</param>
    /// <param name="pageSize">The number of items per page. Must be greater than or equal to 1. Defaults to 10.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="page"/> or <paramref name="pageSize"/> is less than 1.
    /// </exception>
    public Pagination(int page, int pageSize = 10)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        Page = page;
        PageSize = pageSize;
    }
}