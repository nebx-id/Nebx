using System.ComponentModel.DataAnnotations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Infra.Integrations.Emails;

public class SenderIdentityOptionsTests
{
    private static IList<ValidationResult> Validate(SenderIdentityOptions options)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(options);
        Validator.TryValidateObject(options, context, results, true);
        return results;
    }

    [Fact]
    public void Should_Pass_Validation_When_All_Fields_Are_Provided()
    {
        // Arrange
        var options = new SenderIdentityOptions
        {
            Address = "noreply@myapp.com",
            Name = "MyApp Notifications"
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Should_Fail_Validation_When_Address_Is_Missing()
    {
        // Arrange
        var options = new SenderIdentityOptions
        {
            Address = "",
            Name = "MyApp Notifications"
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, r => r.ErrorMessage == "The sender email Address is required.");
    }

    [Fact]
    public void Should_Fail_Validation_When_Name_Is_Missing()
    {
        // Arrange
        var options = new SenderIdentityOptions
        {
            Address = "noreply@myapp.com",
            Name = ""
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, r => r.ErrorMessage == "The sender Name is required.");
    }

    [Fact]
    public void Should_Fail_Validation_When_All_Fields_Are_Empty()
    {
        // Arrange
        var options = new SenderIdentityOptions();

        // Act
        var results = Validate(options);

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(results, r => r.ErrorMessage == "The sender email Address is required.");
        Assert.Contains(results, r => r.ErrorMessage == "The sender Name is required.");
    }
}
