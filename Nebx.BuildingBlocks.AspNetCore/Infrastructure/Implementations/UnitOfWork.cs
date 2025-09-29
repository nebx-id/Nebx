using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Implementations;

public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
{
    public UnitOfWork(TDbContext dbContext)
    {
        Context = dbContext;
    }

    public TDbContext Context { get; }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }
}
