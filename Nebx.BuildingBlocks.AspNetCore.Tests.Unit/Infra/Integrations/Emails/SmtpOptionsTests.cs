using System.ComponentModel.DataAnnotations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Infra.Integrations.Emails;

public class SmtpOptionsTests
{
    private static IList<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        // Validate the model itself
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);

        // Recursively validate nested properties
        foreach (var property in model.GetType().GetProperties())
        {
            var value = property.GetValue(model);

            if (value is null)
                continue;

            // Validate single nested object
            Validator.TryValidateObject(value, new ValidationContext(value), results, validateAllProperties: true);
        }

        return results;
    }

    [Fact]
    public void Should_Pass_Validation_When_Host_Is_Provided_And_No_Credentials()
    {
        // Arrange
        var options = new SmtpOptions
        {
            Host = "smtp.mailserver.com",
            Port = 587,
            Credentials = null
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Should_Fail_Validation_When_Host_Is_Missing()
    {
        // Arrange
        var options = new SmtpOptions
        {
            Host = "",
            Port = 587
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, r => r.ErrorMessage == "The SMTP Host is required.");
    }

    [Fact]
    public void Should_Pass_When_Credentials_Are_Provided_And_Valid()
    {
        // Arrange
        var options = new SmtpOptions
        {
            Host = "smtp.mailserver.com",
            Credentials = new SmtpCredentials
            {
                Username = "noreply@myapp.com",
                Password = "secure-password",
                UseSsl = true
            }
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void Should_Fail_When_Credentials_Have_Missing_Username()
    {
        // Arrange
        var options = new SmtpOptions
        {
            Host = "smtp.mailserver.com",
            Credentials = new SmtpCredentials
            {
                Username = "",
                Password = "secure-password"
            }
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, r => r.ErrorMessage == "The SMTP Username is required when authentication is used.");
    }

    [Fact]
    public void Should_Fail_When_Credentials_Have_Missing_Password()
    {
        // Arrange
        var options = new SmtpOptions
        {
            Host = "smtp.mailserver.com",
            Credentials = new SmtpCredentials
            {
                Username = "noreply@myapp.com",
                Password = ""
            }
        };

        // Act
        var results = Validate(options);

        // Assert
        Assert.Single(results);
        Assert.Contains(results, r => r.ErrorMessage == "The SMTP Password is required when authentication is used.");
    }
}
