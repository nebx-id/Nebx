using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.DDD;

public class DomainEventTests
{
    private sealed record TestEvent(string Value) : DomainEvent;

    [Fact]
    public void EventId_Should_Be_Unique_For_Each_Instance()
    {
        // Arrange
        var evt1 = new TestEvent("A");
        var evt2 = new TestEvent("B");

        // Act & Assert
        Assert.NotEqual(evt1.EventId, evt2.EventId);
        Assert.NotEqual(Guid.Empty, evt1.EventId);
        Assert.NotEqual(Guid.Empty, evt2.EventId);
    }

    [Fact]
    public void OccurredOn_Should_Be_Set_To_Utc_Time_Close_To_Now()
    {
        // Arrange
        var before = DateTime.UtcNow.AddSeconds(-1);
        var evt = new TestEvent("test");
        var after = DateTime.UtcNow.AddSeconds(1);

        // Act
        var occurred = evt.OccurredOn;

        // Assert
        Assert.InRange(occurred, before, after);
        Assert.Equal(DateTimeKind.Utc, occurred.Kind);
    }

    [Fact]
    public void EventType_Should_Return_AssemblyQualifiedName()
    {
        // Arrange
        var evt = new TestEvent("test");

        // Act
        var type = evt.EventType;

        // Assert
        Assert.NotNull(type);
        Assert.Equal(typeof(TestEvent).AssemblyQualifiedName, type);
    }

    [Fact]
    public void DomainEvent_Should_Implement_IDomainEvent()
    {
        // Arrange
        var evt = new TestEvent("X");

        // Assert
        Assert.IsAssignableFrom<IDomainEvent>(evt);
    }
}
