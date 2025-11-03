using Nebx.BuildingBlocks.AspNetCore.Core.Contracts.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Contracts.Emails;

public class EmailAddressTests
{
    [Fact]
    public void Constructor_Should_AssignPropertiesCorrectly()
    {
        // Arrange
        const string expectedName = "John Doe";
        const string expectedAddress = "john.doe@example.com";

        // Act
        var email = new EmailAddress(expectedName, expectedAddress);

        // Assert
        Assert.Equal(expectedName, email.Name);
        Assert.Equal(expectedAddress, email.Address);
    }

    [Fact]
    public void Records_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var email1 = new EmailAddress("Alice", "alice@example.com");
        var email2 = new EmailAddress("Alice", "alice@example.com");

        // Act & Assert
        Assert.Equal(email1, email2);
        Assert.True(email1 == email2);
        Assert.False(email1 != email2);
    }

    [Fact]
    public void Records_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var email1 = new EmailAddress("Alice", "alice@example.com");
        var email2 = new EmailAddress("Bob", "bob@example.com");

        // Act & Assert
        Assert.NotEqual(email1, email2);
        Assert.True(email1 != email2);
    }

    [Fact]
    public void ToString_ShouldReturnReadableFormat()
    {
        // Arrange
        var email = new EmailAddress("John Doe", "john.doe@example.com");

        // Act
        var result = email.ToString();

        // Assert
        Assert.Contains("John Doe", result);
        Assert.Contains("john.doe@example.com", result);
    }
}