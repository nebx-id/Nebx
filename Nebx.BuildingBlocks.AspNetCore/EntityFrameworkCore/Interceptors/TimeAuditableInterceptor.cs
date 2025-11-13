using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Interceptors;

/// <summary>
/// Intercepts SaveChanges operations to automatically update
/// <see cref="ITimeAuditable"/> timestamp fields.
/// </summary>
internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Updates audit timestamps before <see cref="DbContext.SaveChanges"/>.
    /// </summary>
    /// <param name="eventData">Information about the current <see cref="DbContext"/> instance.</param>
    /// <param name="result">The original result of the operation.</param>
    /// <returns>An <see cref="InterceptionResult{Int32}"/> allowing modification of the save result.</returns>
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
            return base.SavingChanges(eventData, result);

        var entries = eventData.Context.GetAuditEntityEntries();
        UpdateTimeAuditProperties(entries);

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Updates audit timestamps before <see cref="DbContext.SaveChangesAsync"/>.
    /// </summary>
    /// <param name="eventData">Information about the current <see cref="DbContext"/> instance.</param>
    /// <param name="result">The original result of the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="ValueTask{InterceptionResult{Int32}}"/> allowing modification of the save result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.GetAuditEntityEntries();
        UpdateTimeAuditProperties(entries);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Sets <c>CreatedOn</c> and <c>ModifiedOn</c> timestamps on tracked entities.
    /// </summary>
    /// <param name="entities">Tracked entities implementing <see cref="ITimeAuditable"/>.</param>
    private static void UpdateTimeAuditProperties(List<EntityEntry<ITimeAuditable>> entities)
    {
        var actionTime = DateTime.UtcNow;

        entities.ForEach(entry =>
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedOn = actionTime;

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                entry.Entity.ModifiedOn = actionTime;
        });
    }
}
