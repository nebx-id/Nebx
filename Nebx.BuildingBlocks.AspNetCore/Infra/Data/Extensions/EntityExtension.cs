using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data.Extensions;

/// <summary>
/// Provides extension methods for working with tracked entities in Entity Framework Core,
/// particularly for auditing and detecting changes in owned entities.
/// </summary>
/// <remarks>
/// This static class adds utility methods that simplify working with EF Core’s <see cref="EntityEntry"/> 
/// and <see cref="DbContext"/> APIs, such as identifying changed owned entities and retrieving
/// entities that require audit updates.
/// </remarks>
public static class EntityExtension
{
    /// <summary>
    /// Determines whether any owned entities associated with the specified <see cref="EntityEntry"/>
    /// have been added or modified.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the tracked entity.</param>
    /// <returns>
    /// <c>true</c> if any owned entities are in the <see cref="EntityState.Added"/> or
    /// <see cref="EntityState.Modified"/> state; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method examines all reference navigations of the entity to detect owned entities that
    /// have been created or changed. It can be used to ensure audit timestamps or metadata are
    /// updated when owned entities change.
    /// </remarks>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }

    /// <summary>
    /// Retrieves all entities tracked by the current <see cref="DbContext"/> that should be audited.
    /// </summary>
    /// <param name="context">The <see cref="DbContext"/> instance being tracked.</param>
    /// <returns>
    /// A list of <see cref="EntityEntry{TEntity}"/> objects representing entities that
    /// implement <see cref="ITimeAuditable"/> and are in the
    /// <see cref="EntityState.Added"/>, <see cref="EntityState.Modified"/>, or have
    /// changed owned entities.
    /// </returns>
    /// <remarks>
    /// Use this method in your audit interceptor or save pipeline to automatically detect
    /// which entities require updates to audit fields such as <c>CreatedAt</c> or <c>ModifiedAt</c>.
    /// </remarks>
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