using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.DDD;

public class EntityTests
{
    private sealed class TestEntity : Entity
    {
    }

    private sealed class TestEntityWithId : Entity<Guid>
    {
    }

    [Fact]
    public void Entity_Should_Set_And_Get_CreatedOn()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var entity = new TestEntity();

        // Act
        entity.CreatedOn = now;

        // Assert
        Assert.Equal(now, entity.CreatedOn);
    }

    [Fact]
    public void Entity_Should_Set_And_Get_ModifiedOn()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var entity = new TestEntity();

        // Act
        entity.ModifiedOn = now;

        // Assert
        Assert.Equal(now, entity.ModifiedOn);
    }

    [Fact]
    public void EntityOfT_Should_Have_Default_Id()
    {
        // Arrange
        var entity = new TestEntityWithId();

        // Act
        var id = entity.Id;

        // Assert
        Assert.Equal(default(Guid), id);
    }

    [Fact]
    public void EntityOfT_Should_Set_And_Get_Id()
    {
        // Arrange
        var expected = Guid.NewGuid();
        var entity = new TestEntityWithId();

        // Act
        entity.Id = expected;

        // Assert
        Assert.Equal(expected, entity.Id);
    }
}
