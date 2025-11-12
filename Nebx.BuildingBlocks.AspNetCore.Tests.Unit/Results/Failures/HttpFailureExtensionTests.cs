using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Responses;
using Nebx.BuildingBlocks.AspNetCore.Results.Failures;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Results.Failures
{
    public class HttpFailureExtensionTests
    {
        private static HttpContext CreateHttpContext(string path = "/test")
        {
            var context = new DefaultHttpContext();
            context.Request.Path = path;
            context.TraceIdentifier = Guid.NewGuid().ToString();
            return context;
        }

        [Fact]
        public void ToMinimalApiResult_ShouldReturnJsonResult_ForBadRequestFailure()
        {
            // Arrange
            var failure = new BadRequestFailure();
            var context = CreateHttpContext();

            // Act
            var result = failure.ToMinimalApiResult(context);

            // Assert
            var jsonResult = Assert.IsType<JsonHttpResult<ErrorResponse>>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
            Assert.NotNull(jsonResult.Value);
            Assert.Equal("Bad Request", jsonResult.Value!.Title);
            Assert.Equal("Invalid request.", jsonResult.Value.Detail);
            Assert.Equal(context.TraceIdentifier, jsonResult.Value.TraceId);
            Assert.Equal(context.Request.Path, jsonResult.Value.Instance);
        }

        [Fact]
        public void ToMinimalApiResult_ShouldIncludeValidationErrors_ForBadRequestFailure()
        {
            // Arrange
            var errors = new Dictionary<string, string[]> { ["name"] = new[] { "Name is required." } };
            var failure = new BadRequestFailure(errors);
            var context = CreateHttpContext();

            // Act
            var result = failure.ToMinimalApiResult(context);

            // Assert
            var jsonResult = Assert.IsType<JsonHttpResult<ErrorResponse>>(result);
            var errorResponse = jsonResult.Value!;
            Assert.NotNull(errorResponse.Errors);
            Assert.True(errorResponse.Errors!.ContainsKey("name"));
            Assert.Equal("Name is required.", errorResponse.Errors["name"][0]);
        }

        [Theory]
        [InlineData(typeof(UnauthorizedFailure), StatusCodes.Status401Unauthorized, "Unauthorized")]
        [InlineData(typeof(ForbiddenFailure), StatusCodes.Status403Forbidden, "Forbidden")]
        [InlineData(typeof(NotFoundFailure), StatusCodes.Status404NotFound, "Not Found")]
        [InlineData(typeof(ConflictFailure), StatusCodes.Status409Conflict, "Conflict")]
        [InlineData(typeof(UnprocessableEntityFailure), StatusCodes.Status422UnprocessableEntity,
            "Unprocessable Entity")]
        [InlineData(typeof(InternalServerErrorFailure), StatusCodes.Status500InternalServerError,
            "Internal Server Error")]
        public void ToMinimalApiResult_ShouldMapStandardFailuresCorrectly(Type failureType, int expectedStatus,
            string expectedTitle)
        {
            // Arrange
            var failure = (HttpFailure)Activator.CreateInstance(failureType)!;
            var context = CreateHttpContext();

            // Act
            var result = failure.ToMinimalApiResult(context);

            // Assert
            var jsonResult = Assert.IsType<JsonHttpResult<ErrorResponse>>(result);
            var errorResponse = jsonResult.Value!;

            Assert.Equal(expectedStatus, jsonResult.StatusCode);
            Assert.Equal(expectedTitle, errorResponse.Title);
            Assert.Equal(context.TraceIdentifier, errorResponse.TraceId);
            Assert.Equal(context.Request.Path, errorResponse.Instance);
        }

        [Fact]
        public void ToMinimalApiResult_ShouldThrow_WhenFailureTypeNotSupported()
        {
            // Arrange
            var customFailure = new CustomHttpFailure();
            var context = CreateHttpContext();

            // Act & Assert
            var ex = Assert.Throws<NotSupportedException>(() => customFailure.ToMinimalApiResult(context));
            Assert.Contains("is not mapped to an HttpFailure type", ex.Message);
        }

        private sealed record CustomHttpFailure() : HttpFailure(499, "Custom Failure", "Something weird happened");
    }
}
