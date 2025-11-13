namespace Nebx.Labs.Core.Domain.Abstractions;

/// <inheritdoc />
public abstract class Entity : IEntity
{
    /// <inheritdoc />
    public DateTime CreatedOn { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedOn { get; set; }
}

/// <inheritdoc cref="Id" />
public abstract class Entity<TId> : Entity, IEntity<TId>
{
    /// <inheritdoc />
    public TId Id { get; set; } = default!;
}
