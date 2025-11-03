namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.DDD;

/// <summary>
/// Provides a base implementation of <see cref="IDomainEvent"/> with automatic metadata initialization.
/// </summary>
/// <remarks>
/// The <see cref="DomainEvent"/> record serves as a convenient base type for implementing 
/// specific domain events. It automatically generates a unique <see cref="EventId"/>, 
/// records the UTC timestamp of creation (<see cref="OccurredOn"/>), and resolves the 
/// assembly-qualified type name (<see cref="EventType"/>).  
///
/// In most cases, domain events should inherit from this base type to ensure consistent metadata 
/// and reliable event tracking across the system.
/// </remarks>
public abstract record DomainEvent : IDomainEvent
{
    /// <inheritdoc />
    public Guid EventId { get; } = Guid.NewGuid();

    /// <inheritdoc />
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    /// <inheritdoc />
    public string EventType => GetType().AssemblyQualifiedName!;
}