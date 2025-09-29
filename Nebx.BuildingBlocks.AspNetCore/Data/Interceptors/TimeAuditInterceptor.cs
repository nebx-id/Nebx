using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

namespace Nebx.BuildingBlocks.AspNetCore.Data.Interceptors;

/// <summary>
///     Intercepts SaveChanges operations to automatically update time audit fields
///     (CreatedOn and ModifiedOn) for entities implementing <see cref="ITimeAuditable" />.
/// </summary>
internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    ///     Called at the start of <see cref="DbContext.SaveChanges" /> to update audit timestamps.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext" />.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <returns>An <see cref="InterceptionResult{Int32}" /> that may modify the result of the operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null) return base.SavingChanges(eventData, result);
        var entries = eventData.Context.GetAuditEntityEntries();

        UpdateTimeAuditProperties(entries);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    ///     Called at the start of <see cref="DbContext.SaveChangesAsync" /> to update audit timestamps asynchronously.
    /// </summary>
    /// <param name="eventData">Contextual information about the <see cref="DbContext" />.</param>
    /// <param name="result">The result of the save operation.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="ValueTask{InterceptionResult{Int32}}" /> that may modify the result of the operation.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = eventData.Context.GetAuditEntityEntries();

        UpdateTimeAuditProperties(entries);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    ///     Updates the audit timestamps for the tracked entities.
    /// </summary>
    /// <param name="entities">A list of entities implementing <see cref="ITimeAuditable" />.</param>
    private static void UpdateTimeAuditProperties(List<EntityEntry<ITimeAuditable>> entities)
    {
        var actionTime = DateTime.UtcNow;

        entities.ForEach(x =>
        {
            if (x.State == EntityState.Added)
                x.Entity.CreatedOn = actionTime;

            if (x.State == EntityState.Modified || x.HasChangedOwnedEntities())
                x.Entity.ModifiedOn = actionTime;
        });
    }
}
