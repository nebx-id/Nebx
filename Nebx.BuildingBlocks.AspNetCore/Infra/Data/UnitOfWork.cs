using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interfaces;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data;

/// <summary>
/// Provides a unit of work implementation for managing database transactions and saving changes.
/// </summary>
/// <typeparam name="TDbContext">The type of the Entity Framework database context.</typeparam>
public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TDbContext}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context to be managed by this unit of work.</param>
    public UnitOfWork(TDbContext dbContext)
    {
        Context = dbContext;
    }

    /// <inheritdoc />
    public TDbContext Context { get; }

    /// <inheritdoc />
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}