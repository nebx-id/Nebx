using Microsoft.EntityFrameworkCore.Storage;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interfaces;

/// <summary>
/// Defines a unit of work abstraction for coordinating database operations within a transactional boundary.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of the database context being managed.
/// </typeparam>
public interface IUnitOfWork<out TDbContext>
{
    /// <summary>
    /// Gets the underlying database context.
    /// </summary>
    public TDbContext Context { get; }

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the started <see cref="IDbContextTransaction"/>.
    /// </returns>
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists all changes made in the current unit of work to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}