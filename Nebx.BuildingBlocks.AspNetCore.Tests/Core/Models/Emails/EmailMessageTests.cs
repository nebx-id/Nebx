using System.Collections.Immutable;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Models.Emails;

public class EmailMessageTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeCollections_AndSetDefaults()
    {
        // Act
        var message = new EmailMessage();

        // Assert
        Assert.NotNull(message.To);
        Assert.NotNull(message.Cc);
        Assert.NotNull(message.Bcc);
        Assert.NotNull(message.Attachments);

        Assert.Empty(message.To);
        Assert.Empty(message.Cc);
        Assert.Empty(message.Bcc);
        Assert.Empty(message.Attachments);

        Assert.Equal(string.Empty, message.Subject);
        Assert.Equal(string.Empty, message.Body);
        Assert.True(message.IsHtml);
        Assert.Null(message.From);
    }

    [Fact]
    public void ShouldAllowSettingPropertiesViaInit()
    {
        // Arrange
        var sender = new EmailAddress("Admin", "admin@example.com");
        var recipient = new EmailAddress("User", "user@example.com");
        var attachment = new EmailAttachment("file.txt", [1], "text/plain");

        // Act
        var message = new EmailMessage
        {
            From = sender,
            To = ImmutableHashSet.Create(recipient),
            Subject = "Test Email",
            Body = "Hello World",
            IsHtml = false,
            Attachments = ImmutableHashSet.Create(attachment)
        };

        // Assert
        Assert.Equal(sender, message.From);
        Assert.Contains(recipient, message.To);
        Assert.Equal("Test Email", message.Subject);
        Assert.Equal("Hello World", message.Body);
        Assert.False(message.IsHtml);
        Assert.Contains(attachment, message.Attachments);
    }


    [Fact]
    public void ShouldSupportEqualityComparison()
    {
        // Arrange
        var sender = new EmailAddress("Sender", "sender@example.com");
        var message1 = new EmailMessage { From = sender, Subject = "Subject" };
        var message2 = new EmailMessage { From = sender, Subject = "Subject" };

        // Act & Assert
        Assert.Equal(message1, message2);
        Assert.True(message1 == message2);
        Assert.False(message1 != message2);
    }

    [Fact]
    public void ShouldNotBeEqual_WhenPropertiesDiffer()
    {
        // Arrange
        var message1 = new EmailMessage { Subject = "Subject A" };
        var message2 = new EmailMessage { Subject = "Subject B" };

        // Act & Assert
        Assert.NotEqual(message1, message2);
    }

    [Fact]
    public void ToString_ShouldContainKeyPropertyValues()
    {
        // Arrange
        var message = new EmailMessage
        {
            Subject = "Report",
            Body = "Monthly report attached."
        };

        // Act
        var result = message.ToString();

        // Assert
        Assert.Contains("Report", result);
        Assert.Contains("Monthly report", result);
    }
}