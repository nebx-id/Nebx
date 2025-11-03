namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

/// <summary>
/// Serves as the abstract base class for aggregate roots with domain event support.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the aggregate root.</typeparam>
/// <remarks>
/// The <see cref="Aggregate{TId}"/> class implements the core functionality for 
/// managing domain events, including adding, clearing, and dequeuing them.  
/// This class provides a foundation for all aggregate root entities 
/// within a domain model that follow DDD principles.
/// </remarks>
public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <inheritdoc />
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public List<IDomainEvent> DequeueEvents()
    {
        var events = _domainEvents.ToList();
        ClearDomainEvents();
        return events;
    }

    /// <inheritdoc />
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Adds a new domain event to the aggregate's event collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    /// <remarks>
    /// This method should be called from within aggregate methods 
    /// whenever a meaningful business change occurs that other parts of the system 
    /// may need to react to.
    /// </remarks>
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}