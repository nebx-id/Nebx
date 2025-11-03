using Microsoft.AspNetCore.Http;
using Nebx.BuildingBlocks.AspNetCore.Core.Contracts.Failures;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Core.Contracts.Failures;

public class HttpFailureTests
{
    [Fact]
    public void HttpFailure_ShouldStoreStatusCodeAndMessage()
    {
        // Arrange
        const int statusCode = StatusCodes.Status500InternalServerError;
        const string message = "An error occurred.";

        // Act
        var failure = new TestHttpFailure(statusCode, message);

        // Assert
        Assert.Equal(statusCode, failure.StatusCode);
        Assert.Equal(message, failure.Message);
        Assert.Equal(message, ((Failure)failure).Message); // inherited property
    }

    [Fact]
    public void BadRequestFailure_ShouldSetDefaultStatusAndMessage()
    {
        // Act
        var failure = new BadRequestFailure();

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, failure.StatusCode);
        Assert.Equal("The request could not be understood or was missing required parameters.", failure.Message);
        Assert.Null(failure.Errors);
    }

    [Fact]
    public void BadRequestFailure_ShouldStoreValidationErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string>
        {
            ["Email"] = "Invalid format",
            ["Password"] = "Too short"
        };

        // Act
        var failure = new BadRequestFailure(errors);

        // Assert
        Assert.Equal(errors, failure.Errors);
        Assert.Equal(StatusCodes.Status400BadRequest, failure.StatusCode);
    }

    [Theory]
    [InlineData(StatusCodes.Status401Unauthorized, "Authentication is required or has failed.",
        typeof(UnauthorizedFailure))]
    [InlineData(StatusCodes.Status403Forbidden, "You do not have permission to perform this action.",
        typeof(ForbiddenFailure))]
    [InlineData(StatusCodes.Status404NotFound, "The requested resource could not be found.", typeof(NotFoundFailure))]
    [InlineData(StatusCodes.Status409Conflict, "A conflict occurred with the current state of the resource.",
        typeof(ConflictFailure))]
    [InlineData(StatusCodes.Status415UnsupportedMediaType, "The media type of the request is not supported.",
        typeof(UnsupportedMediaTypeFailure))]
    [InlineData(StatusCodes.Status422UnprocessableEntity,
        "The request could not be processed due to validation errors.", typeof(UnprocessableEntityFailure))]
    [InlineData(StatusCodes.Status500InternalServerError, "An unexpected server error occurred.",
        typeof(InternalServerErrorFailure))]
    [InlineData(StatusCodes.Status501NotImplemented, "The requested functionality is not implemented.",
        typeof(NotImplementedFailure))]
    public void HttpFailures_ShouldHaveExpectedDefaults(int expectedCode, string expectedMessage, Type failureType)
    {
        // Act
        var failure = (HttpFailure)Activator.CreateInstance(failureType)!;

        // Assert
        Assert.Equal(expectedCode, failure.StatusCode);
        Assert.Equal(expectedMessage, failure.Message);
    }

    [Fact]
    public void ShouldSupportValueEquality_ForSameTypeAndValues()
    {
        // Arrange
        var failure1 = new NotFoundFailure();
        var failure2 = new NotFoundFailure();

        // Act & Assert
        Assert.Equal(failure1, failure2);
        Assert.True(failure1 == failure2);
        Assert.False(failure1 != failure2);
    }

    [Fact]
    public void ShouldNotBeEqual_WhenDifferentFailureTypes()
    {
        // Arrange
        HttpFailure failure1 = new ForbiddenFailure();
        HttpFailure failure2 = new NotFoundFailure();

        // Act & Assert
        Assert.NotEqual(failure1, failure2);
        Assert.True(failure1 != failure2);
    }

    [Fact]
    public void ToString_ShouldIncludeTypeAndStatus()
    {
        // Arrange
        var failure = new InternalServerErrorFailure();

        // Act
        var result = failure.ToString();

        // Assert
        Assert.Contains(nameof(InternalServerErrorFailure), result);
        Assert.Contains(failure.StatusCode.ToString(), result);
    }

    // Internal helper record for base testing
    private sealed record TestHttpFailure(int StatusCode, string Message) : HttpFailure(StatusCode, Message);
}