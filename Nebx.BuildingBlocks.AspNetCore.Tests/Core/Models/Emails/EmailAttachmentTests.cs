using Nebx.BuildingBlocks.AspNetCore.Core.Models.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Models.Emails;

public class EmailAttachmentTests
{
    [Fact]
    public void Constructor_Should_AssignPropertiesCorrectly()
    {
        // Arrange
        const string expectedFileName = "report.pdf";
        var expectedContent = new byte[] { 1, 2, 3, 4 };
        const string expectedContentType = "application/pdf";

        // Act
        var attachment = new EmailAttachment(expectedFileName, expectedContent, expectedContentType);

        // Assert
        Assert.Equal(expectedFileName, attachment.FileName);
        Assert.Equal(expectedContent, attachment.Content);
        Assert.Equal(expectedContentType, attachment.ContentType);
    }

    [Fact]
    public void Records_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var content = new byte[] { 1, 2, 3 };
        var a1 = new EmailAttachment("file.txt", content, "text/plain");
        var a2 = new EmailAttachment("file.txt", content, "text/plain");

        // Act & Assert
        Assert.Equal(a1, a2);
        Assert.True(a1 == a2);
        Assert.False(a1 != a2);
    }

    [Fact]
    public void Records_WithDifferentValues_ShouldNotBeEqual()
    {
        // Arrange
        var a1 = new EmailAttachment("file1.txt", [1], "text/plain");
        var a2 = new EmailAttachment("file2.txt", [2], "text/plain");

        // Act & Assert
        Assert.NotEqual(a1, a2);
        Assert.True(a1 != a2);
    }

    [Fact]
    public void ToString_ShouldContainAllPropertyValues()
    {
        // Arrange
        var attachment = new EmailAttachment("document.pdf", new byte[] { 1, 2 }, "application/pdf");

        // Act
        var result = attachment.ToString();

        // Assert
        Assert.Contains("document.pdf", result);
        Assert.Contains("application/pdf", result);
    }
}