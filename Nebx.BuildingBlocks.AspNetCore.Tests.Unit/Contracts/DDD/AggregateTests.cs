using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.DDD;

public class AggregateTests
{
    private sealed class TestEvent : IDomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
        public string EventType { get; } = typeof(TestEvent).FullName!;
    }

    private sealed class TestAggregate : Aggregate<Guid>
    {
        public void RaiseEvent(IDomainEvent domainEvent) => AddDomainEvent(domainEvent);
    }

    [Fact]
    public void DomainEvents_Should_Be_Empty_By_Default()
    {
        // Arrange
        var aggregate = new TestAggregate();

        // Act & Assert
        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void AddDomainEvent_Should_Add_Event_To_DomainEvents()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var evt = new TestEvent();

        // Act
        aggregate.RaiseEvent(evt);

        // Assert
        Assert.Single(aggregate.DomainEvents);
        Assert.Contains(evt, aggregate.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_Should_Remove_All_Events()
    {
        // Arrange
        var aggregate = new TestAggregate();
        aggregate.RaiseEvent(new TestEvent());
        aggregate.RaiseEvent(new TestEvent());

        // Act
        aggregate.ClearDomainEvents();

        // Assert
        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void DequeueEvents_Should_Return_All_Events_And_Clear_Collection()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var evt1 = new TestEvent();
        var evt2 = new TestEvent();

        aggregate.RaiseEvent(evt1);
        aggregate.RaiseEvent(evt2);

        // Act
        var dequeued = aggregate.DequeueEvents();

        // Assert
        Assert.Equal(2, dequeued.Count);
        Assert.Contains(evt1, dequeued);
        Assert.Contains(evt2, dequeued);
        Assert.Empty(aggregate.DomainEvents);
    }

    [Fact]
    public void DomainEvents_Should_Be_ReadOnly()
    {
        // Arrange
        var aggregate = new TestAggregate();
        aggregate.RaiseEvent(new TestEvent());

        // Act
        var events = aggregate.DomainEvents;

        // Assert
        Assert.IsType<IReadOnlyList<IDomainEvent>>(events, exactMatch: false);

        // Verify consumer cannot modify underlying collection
        Assert.ThrowsAny<Exception>(() =>
        {
            // Attempt to cast back and mutate — must fail
            var list = (List<IDomainEvent>)events;
            list.Clear();
        });
    }
}
