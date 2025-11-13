namespace Nebx.Labs.Core.Domain.Abstractions;

/// <summary>
/// Represents an entity with audit timestamps.
/// </summary>
public interface IEntity : ITimeAuditable;

/// <summary>
/// Represents an entity with a strongly typed identifier.
/// </summary>
/// <typeparam name="TId">The type of the entity identifier.</typeparam>
public interface IEntity<TId> : IEntity
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    public TId Id { get; set; }
}
