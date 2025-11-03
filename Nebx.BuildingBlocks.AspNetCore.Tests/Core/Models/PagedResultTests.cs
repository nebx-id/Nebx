using Nebx.BuildingBlocks.AspNetCore.Core.Models;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Models;

public class PagedResultTests
{
    [Fact]
    public void ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var items = new List<string> { "A", "B", "C" };
        var totalCount = 10;

        // Act
        var result = new PagedResult<string>(items, totalCount);

        // Assert
        Assert.Equal(items, result.Items);
        Assert.Equal(totalCount, result.TotalCount);
    }

    [Fact]
    public void ShouldSupportEqualityComparison()
    {
        // Arrange
        var items = new List<int> { 1, 2, 3 };
        var result1 = new PagedResult<int>(items, 3);
        var result2 = new PagedResult<int>(items, 3);

        // Act & Assert
        Assert.Equal(result1, result2);
        Assert.True(result1 == result2);
        Assert.False(result1 != result2);
    }

    [Fact]
    public void ShouldReturnDifferentInstancesAsNotEqual_WhenItemsOrTotalCountDiffer()
    {
        // Arrange
        var result1 = new PagedResult<int>(new List<int> { 1, 2, 3 }, 3);
        var result2 = new PagedResult<int>(new List<int> { 4, 5, 6 }, 3);
        var result3 = new PagedResult<int>(new List<int> { 1, 2, 3 }, 5);

        // Act & Assert
        Assert.NotEqual(result1, result2);
        Assert.NotEqual(result1, result3);
    }

    [Fact]
    public void ShouldBeImmutable()
    {
        // Arrange
        var result = new PagedResult<int>(new List<int> { 1, 2 }, 2);

        // Act
        var newResult = result with { TotalCount = 5 };

        // Assert
        Assert.Equal(2, result.TotalCount); // original unchanged
        Assert.Equal(5, newResult.TotalCount); // new record with updated count
        Assert.NotSame(result, newResult);
    }

    [Fact]
    public void ShouldHandleEmptyItemsList()
    {
        // Act
        var result = new PagedResult<string>(new List<string>(), 0);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }
}