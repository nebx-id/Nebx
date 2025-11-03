namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

/// <summary>
/// Defines a base contract for all entities that include auditing information.
/// </summary>
/// <remarks>
/// The <see cref="IEntity"/> interface serves as a marker for domain or persistence entities 
/// that track creation and modification timestamps via <see cref="ITimeAuditable"/>.  
/// It provides a consistent abstraction for all database or domain entities that require 
/// time-based auditing.
/// </remarks>
public interface IEntity : ITimeAuditable;

/// <summary>
/// Defines a generic base contract for entities that have a unique identifier.
/// </summary>
/// <typeparam name="TKey">The type of the entity's unique identifier (e.g., <see cref="Guid"/>, <see cref="int"/>).</typeparam>
/// <remarks>
/// The <see cref="IEntity{TKey}"/> interface extends <see cref="IEntity"/> by introducing a strongly typed <c>Id</c> property.  
/// This allows generic repository patterns, domain models, and persistence mechanisms 
/// to operate consistently across entities with various key types.
/// </remarks>
public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public TKey Id { get; set; }
}