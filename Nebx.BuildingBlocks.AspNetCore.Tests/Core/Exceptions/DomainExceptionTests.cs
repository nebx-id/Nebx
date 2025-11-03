using Nebx.BuildingBlocks.AspNetCore.Core.Exceptions;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Exceptions;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_Should_SetMessageProperly()
    {
        // Arrange
        const string expectedMessage = "Invalid domain operation.";

        // Act
        var exception = new DomainException(expectedMessage);

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<DomainException>(exception);
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void DomainException_Should_BeAssignableTo_Exception()
    {
        // Act
        var exception = new DomainException("Rule violated.");

        // Assert
        Assert.IsAssignableFrom<Exception>(exception);
    }
}