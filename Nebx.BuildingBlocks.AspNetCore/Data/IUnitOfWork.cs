using Microsoft.EntityFrameworkCore.Storage;

namespace Nebx.BuildingBlocks.AspNetCore.Data;

public interface IUnitOfWork<out TDbContext>
{
    public TDbContext Context { get; }
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    public Task SaveChangesAsync(CancellationToken cancellationToken = default);
}