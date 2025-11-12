using Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;

namespace Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

/// <summary>
/// Defines a contract for domain events that represent significant business occurrences within the domain model.
/// </summary>
/// <remarks>
/// A domain event captures an important change or action that has occurred within an aggregate or entity.
/// Events are used to decouple domain logic from side effects, allowing other components
/// (such as handlers, subscribers, or integration systems) to react to business changes asynchronously.
///
/// Implementations of <see cref="IDomainEvent"/> should be immutable and convey clear domain intent.
/// Each event carries metadata such as an event identifier, timestamp, and type name for reliable tracking and dispatching.
/// </remarks>
public interface IDomainEvent : INotification
{
    /// <summary>
    /// Gets the unique identifier of the domain event instance.
    /// </summary>
    /// <remarks>
    /// This identifier is typically used for deduplication, tracing, and outbox persistence.
    /// It ensures each event can be uniquely referenced and correlated across systems.
    /// </remarks>
    Guid EventId { get; }

    /// <summary>
    /// Gets the UTC timestamp indicating when the domain event occurred.
    /// </summary>
    /// <remarks>
    /// This timestamp provides temporal context for the event and is useful for event ordering, logging, and auditing.
    /// </remarks>
    DateTime OccurredOn { get; }

    /// <summary>
    /// Gets the fully qualified type name of the event.
    /// </summary>
    /// <remarks>
    /// The <see cref="EventType"/> property provides the assembly-qualified name of the event type,
    /// enabling dynamic deserialization, reflection-based handling, and type-safe event routing.
    /// </remarks>
    string EventType { get; }
}
