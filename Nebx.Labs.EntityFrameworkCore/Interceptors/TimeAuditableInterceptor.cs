using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Nebx.Labs.Core.Domain.Abstractions;
using Nebx.Labs.EntityFrameworkCore.Extensions;

namespace Nebx.Labs.EntityFrameworkCore.Interceptors;

/// <summary>
/// An EF Core save-changes interceptor that automatically updates timestamp
/// fields on entities implementing <see cref="ITimeAuditable"/>.
/// </summary>
/// <remarks>
/// This interceptor applies audit values during <c>SaveChanges</c> and
/// <c>SaveChangesAsync</c> by setting:
/// <list type="bullet">
///   <item><description><c>CreatedOn</c> when an entity is added.</description></item>
///   <item><description><c>ModifiedOn</c> when an entity is modified or when owned entities have changed.</description></item>
/// </list>
/// </remarks>
internal sealed class TimeAuditInterceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Invoked before <see cref="DbContext.SaveChanges()"/> to apply audit timestamp values.
    /// </summary>
    /// <param name="eventData">
    /// Contextual information for the <see cref="DbContext"/> initiating the save operation.
    /// </param>
    /// <param name="result">
    /// The current interception result. This can be returned unchanged or modified.
    /// </param>
    /// <returns>
    /// An <see cref="InterceptionResult{TResult}"/> controlling continuation of the save operation.
    /// </returns>
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
    /// Invoked before <see cref="DbContext.SaveChangesAsync(CancellationToken)"/> to apply audit timestamp values.
    /// </summary>
    /// <param name="eventData">
    /// Contextual information for the <see cref="DbContext"/> initiating the async save operation.
    /// </param>
    /// <param name="result">
    /// The current interception result. This can be returned unchanged or modified.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that may be used to cancel the operation.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> producing an <see cref="InterceptionResult{TResult}"/> for the save operation.
    /// </returns>
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
    /// Applies <c>CreatedOn</c> and <c>ModifiedOn</c> timestamps to entities being saved.
    /// </summary>
    /// <param name="entities">
    /// A list of tracked entries whose entities implement <see cref="ITimeAuditable"/>.
    /// </param>
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
