using Nebx.BuildingBlocks.AspNetCore.Results.Failures;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Results.Failures;

public class FailureTests
{
    [Fact]
    public void ShouldStoreMessage_WhenConstructed()
    {
        // Arrange
        const string message = "Operation failed due to an unexpected error.";

        // Act
        var failure = new Failure(message);

        // Assert
        Assert.Equal(message, failure.Detail);
    }

    [Fact]
    public void ShouldSupportValueEquality_WhenMessagesAreIdentical()
    {
        // Arrange
        var failure1 = new Failure("Network failure");
        var failure2 = new Failure("Network failure");

        // Act & Assert
        Assert.Equal(failure1, failure2);
        Assert.True(failure1 == failure2);
        Assert.False(failure1 != failure2);
    }

    [Fact]
    public void ShouldNotBeEqual_WhenMessagesDiffer()
    {
        // Arrange
        var failure1 = new Failure("Timeout");
        var failure2 = new Failure("Connection lost");

        // Act & Assert
        Assert.NotEqual(failure1, failure2);
        Assert.True(failure1 != failure2);
        Assert.False(failure1 == failure2);
    }

    [Fact]
    public void ShouldHaveConsistentHashCode_ForEqualRecords()
    {
        // Arrange
        var failure1 = new Failure("Validation error");
        var failure2 = new Failure("Validation error");

        // Act
        var hash1 = failure1.GetHashCode();
        var hash2 = failure2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ToString_ShouldIncludeTypeAndMessage()
    {
        // Arrange
        var failure = new Failure("File not found");

        // Act
        var result = failure.ToString();

        // Assert
        Assert.Contains(nameof(Failure), result);
        Assert.Contains("File not found", result);
    }
}