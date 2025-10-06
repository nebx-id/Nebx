namespace Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

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

/// <summary>
/// Provides a base implementation for entities that include time-based audit properties.
/// </summary>
/// <remarks>
/// The <see cref="Entity"/> class implements <see cref="IEntity"/> and provides concrete 
/// <see cref="CreatedOn"/> and <see cref="ModifiedOn"/> properties to track when the entity was created 
/// and last modified.  
/// This base class can be extended by entities that do not require a typed identifier.
/// </remarks>
public abstract class Entity : IEntity
{
    /// <summary>
    /// Gets or sets the timestamp indicating when the entity was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }
}

/// <summary>
/// Provides a base implementation for entities with a unique identifier and audit tracking.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the entity (e.g., <see cref="Guid"/>, <see cref="int"/>).</typeparam>
/// <remarks>
/// The <see cref="Entity{TId}"/> class extends <see cref="Entity"/> to include a typed <see cref="Id"/> property.  
/// It is commonly used as a base class for entities in domain-driven or persistence-oriented architectures, 
/// providing both identity and audit tracking in one consistent structure.
/// </remarks>
public abstract class Entity<TId> : Entity, IEntity<TId>
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    /// <remarks>
    /// This property is initialized to the default value for the given type (for example, 
    /// <see cref="Guid.Empty"/> for <see cref="Guid"/>, or <c>0</c> for <see cref="int"/>).
    /// </remarks>
    public TId Id { get; set; } = default!;
}