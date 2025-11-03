using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces.Services;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Extensions;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interceptors;

/// <summary>
/// Intercepts SaveChanges operations to automatically update time audit fields
/// (CreatedOn and ModifiedOn) for entities implementing <see cref="ITimeAuditable" />.
/// </summary>
public sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    private readonly IClock _clock;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeAuditInterceptor"/> class.
    /// </summary>
    /// <param name="clock">The clock service used to obtain the current time.</param>
    public TimeAuditInterceptor(IClock clock)
    {
        _clock = clock;
    }

    /// <summary>
    /// Called automatically during <see cref="DbContext.SaveChanges()"/> to update audit fields.
    /// </summary>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
            return base.SavingChanges(eventData, result);

        var entries = eventData.Context.GetAuditEntityEntries();
        UpdateTimeAuditProperties(entries);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Called automatically during <see cref="DbContext.SaveChangesAsync(CancellationToken)"/> to update audit fields.
    /// </summary>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.GetAuditEntityEntries();
        UpdateTimeAuditProperties(entries);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the audit timestamps for the tracked entities.
    /// </summary>
    private void UpdateTimeAuditProperties(List<EntityEntry<ITimeAuditable>> entities)
    {
        var actionTime = _clock.UtcNow;

        foreach (var entry in entities)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedOn = actionTime;

            if (entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                entry.Entity.ModifiedOn = actionTime;
        }
    }
}