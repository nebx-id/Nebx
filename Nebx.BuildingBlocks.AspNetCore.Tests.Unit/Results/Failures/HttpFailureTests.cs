using Microsoft.AspNetCore.Http;
using Nebx.BuildingBlocks.AspNetCore.Results.Failures;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Results.Failures
{
    public class HttpFailureTests
    {
        [Fact]
        public void BadRequestFailure_ShouldUseDefaultMessage_WhenNoErrorsProvided()
        {
            // Act
            var failure = new BadRequestFailure();

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, failure.StatusCode);
            Assert.Equal("Bad Request", failure.Title);
            Assert.Equal("Invalid request.", failure.Detail);
            Assert.Null(failure.Errors);
        }

        [Fact]
        public void BadRequestFailure_ShouldUseValidationMessage_WhenErrorsProvided()
        {
            // Arrange
            var errors = new Dictionary<string, string[]>
            {
                { "email", new[] { "Email is required." } }
            };

            // Act
            var failure = new BadRequestFailure(errors);

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, failure.StatusCode);
            Assert.Equal("Bad Request", failure.Title);
            Assert.Equal("One or more request parameters are invalid.", failure.Detail);
            Assert.NotNull(failure.Errors);
            Assert.True(failure.Errors!.ContainsKey("email"));
            Assert.Equal("Email is required.", failure.Errors["email"][0]);
        }

        [Fact]
        public void UnauthorizedFailure_ShouldHaveExpectedValues()
        {
            // Act
            var failure = new UnauthorizedFailure();

            // Assert
            Assert.Equal(StatusCodes.Status401Unauthorized, failure.StatusCode);
            Assert.Equal("Unauthorized", failure.Title);
            Assert.Equal("Authentication is required to access this resource or the provided credentials are invalid.", failure.Detail);
        }

        [Fact]
        public void ForbiddenFailure_ShouldHaveExpectedValues()
        {
            // Act
            var failure = new ForbiddenFailure();

            // Assert
            Assert.Equal(StatusCodes.Status403Forbidden, failure.StatusCode);
            Assert.Equal("Forbidden", failure.Title);
            Assert.Equal("You do not have permission to perform this action.", failure.Detail);
        }

        [Fact]
        public void NotFoundFailure_ShouldUseDefaultDetail_WhenNoneProvided()
        {
            // Act
            var failure = new NotFoundFailure();

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, failure.StatusCode);
            Assert.Equal("Not Found", failure.Title);
            Assert.Equal("The requested resource could not be found.", failure.Detail);
        }

        [Fact]
        public void NotFoundFailure_ShouldUseCustomDetail_WhenProvided()
        {
            // Act
            var failure = new NotFoundFailure("Custom detail");

            // Assert
            Assert.Equal("Custom detail", failure.Detail);
        }

        [Fact]
        public void ConflictFailure_ShouldUseDefaultDetail_WhenNoneProvided()
        {
            // Act
            var failure = new ConflictFailure();

            // Assert
            Assert.Equal(StatusCodes.Status409Conflict, failure.StatusCode);
            Assert.Equal("Conflict", failure.Title);
            Assert.Equal("A conflict occurred with the current state of the resource.", failure.Detail);
        }

        [Fact]
        public void ConflictFailure_ShouldUseCustomDetail_WhenProvided()
        {
            // Act
            var failure = new ConflictFailure("Custom conflict message");

            // Assert
            Assert.Equal("Custom conflict message", failure.Detail);
        }

        [Fact]
        public void UnsupportedMediaTypeFailure_ShouldHaveExpectedValues()
        {
            // Act
            var failure = new UnsupportedMediaTypeFailure();

            // Assert
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, failure.StatusCode);
            Assert.Equal("Unsupported Media Type", failure.Title);
            Assert.Equal("The media type of the request is not supported.", failure.Detail);
        }

        [Fact]
        public void UnprocessableEntityFailure_ShouldUseDefaultDetail_WhenNoneProvided()
        {
            // Act
            var failure = new UnprocessableEntityFailure();

            // Assert
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, failure.StatusCode);
            Assert.Equal("Unprocessable Entity", failure.Title);
            Assert.Equal("The request could not be processed due to validation errors.", failure.Detail);
        }

        [Fact]
        public void UnprocessableEntityFailure_ShouldUseCustomDetail_WhenProvided()
        {
            // Act
            var failure = new UnprocessableEntityFailure("Validation failed for field: username");

            // Assert
            Assert.Equal("Validation failed for field: username", failure.Detail);
        }

        [Fact]
        public void InternalServerErrorFailure_ShouldHaveExpectedValues()
        {
            // Act
            var failure = new InternalServerErrorFailure();

            // Assert
            Assert.Equal(StatusCodes.Status500InternalServerError, failure.StatusCode);
            Assert.Equal("Internal Server Error", failure.Title);
            Assert.Equal("An unexpected server error occurred.", failure.Detail);
        }

        [Fact]
        public void NotImplementedFailure_ShouldHaveExpectedValues()
        {
            // Act
            var failure = new NotImplementedFailure();

            // Assert
            Assert.Equal(StatusCodes.Status501NotImplemented, failure.StatusCode);
            Assert.Equal("Not Implemented", failure.Title);
            Assert.Equal("The requested functionality is not implemented.", failure.Detail);
        }
    }
}
