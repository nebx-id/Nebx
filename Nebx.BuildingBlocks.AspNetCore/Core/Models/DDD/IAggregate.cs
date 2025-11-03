namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

/// <summary>
/// Defines the base contract for an aggregate root within a domain-driven design (DDD) context.
/// </summary>
/// <remarks>
/// An <see cref="IAggregate"/> represents the root entity that controls the lifecycle 
/// and consistency of a group of related entities (the aggregate).  
/// It is responsible for managing and exposing <see cref="IDomainEvent"/> instances
/// that describe significant state changes within the aggregate.
/// </remarks>
public interface IAggregate : IEntity
{
    /// <summary>
    /// Gets the collection of domain events that have occurred within the aggregate.
    /// </summary>
    /// <remarks>
    /// Domain events are used to communicate significant business changes 
    /// or intentions to other parts of the system in a decoupled manner.
    /// </remarks>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Retrieves and clears all domain events currently stored in the aggregate.
    /// </summary>
    /// <returns>
    /// A list containing all domain events that were raised prior to the call.
    /// </returns>
    /// <remarks>
    /// This method is typically used by application layers or infrastructure components 
    /// (such as event dispatchers or outbox processors) to collect domain events for publishing.
    /// </remarks>
    List<IDomainEvent> DequeueEvents();

    /// <summary>
    /// Clears all domain events from the aggregate without returning them.
    /// </summary>
    /// <remarks>
    /// This is primarily used internally to reset the event queue 
    /// after the events have been processed or dispatched.
    /// </remarks>
    void ClearDomainEvents();
}

/// <summary>
/// Defines a generic contract for an aggregate root that includes a strongly typed identifier.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the aggregate root.</typeparam>
/// <remarks>
/// The <see cref="IAggregate{TId}"/> interface combines the functionality of 
/// <see cref="IAggregate"/> and <see cref="IEntity{TKey}"/>, enabling 
/// type-safe aggregate identifiers while preserving domain event capabilities.
/// </remarks>
public interface IAggregate<TId> : IAggregate, IEntity<TId>;