using Nebx.BuildingBlocks.AspNetCore.Contracts.Models;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Contracts.Models;

public class PaginationTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        const int page = 2;
        const int pageSize = 25;

        // Act
        var pagination = new Pagination(page, pageSize);

        // Assert
        Assert.Equal(page, pagination.Page);
        Assert.Equal(pageSize, pagination.PageSize);
        Assert.Equal((page - 1) * pageSize, pagination.Offset);
    }

    [Fact]
    public void Constructor_ShouldDefaultPageSizeToTen_WhenNotProvided()
    {
        // Arrange
        const int page = 3;

        // Act
        var pagination = new Pagination(page);

        // Assert
        Assert.Equal(10, pagination.PageSize);
        Assert.Equal((page - 1) * pagination.PageSize, pagination.Offset);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    public void Constructor_ShouldThrowArgumentOutOfRange_WhenInvalidValuesProvided(int page, int pageSize)
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Pagination(page, pageSize));
    }

    [Fact]
    public void Offset_ShouldReturnZero_WhenPageIsOne()
    {
        // Act
        var pagination = new Pagination(1, 50);

        // Assert
        Assert.Equal(0, pagination.Offset);
    }

    [Fact]
    public void ShouldBeImmutable()
    {
        // Arrange
        var original = new Pagination(1, 20);

        // Act
        var modified = original with { };

        // Assert
        Assert.Equal(original.Page, modified.Page);
        Assert.Equal(original.PageSize, modified.PageSize);
        Assert.Equal(original.Offset, modified.Offset);
        Assert.NotSame(original, modified);
    }
}