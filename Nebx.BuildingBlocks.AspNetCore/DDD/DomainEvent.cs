using Nebx.BuildingBlocks.AspNetCore.CQRS;

namespace Nebx.BuildingBlocks.AspNetCore.DDD;

public interface IDomainEvent : INotification
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string EventType { get; }
}

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.CreateVersion7();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName!;
}