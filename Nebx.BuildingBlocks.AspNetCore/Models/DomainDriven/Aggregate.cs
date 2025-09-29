namespace Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

public interface IAggregate : IEntity
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public List<IDomainEvent> DequeueEvents();
    public void ClearDomainEvents();
}

public interface IAggregate<TId> : IAggregate, IEntity<TId>;

public abstract class Aggregate<TId> : Entity<TId>, IAggregate<TId>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public List<IDomainEvent> DequeueEvents()
    {
        var events = _domainEvents.ToList();
        ClearDomainEvents();
        return events;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}