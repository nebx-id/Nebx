using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data.Extensions;

public static class EntityExtension
{
    /// <summary>
    ///     Determines whether any owned entities associated with the given <see cref="EntityEntry" /> have been added or
    ///     modified.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry" /> representing the entity being tracked.</param>
    /// <returns><c>true</c> if any owned entities have been added or modified; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method examines the references of the provided entity entry to identify any owned entities
    ///     that are in the <see cref="EntityState.Added" /> or <see cref="EntityState.Modified" /> state.
    /// </remarks>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }

    /// <summary>
    ///     Retrieves entities that are tracked for auditing.
    /// </summary>
    /// <param name="context">The current <see cref="DbContext" /> instance.</param>
    /// <returns>A list of <see cref="EntityEntry" /> entries that require audit updates.</returns>
    public static List<EntityEntry<ITimeAuditable>> GetAuditEntityEntries(this DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ITimeAuditable>()
            .Where(x =>
                x.State == EntityState.Added ||
                x.State == EntityState.Modified ||
                x.HasChangedOwnedEntities())
            .ToList();

        return entities;
    }
}