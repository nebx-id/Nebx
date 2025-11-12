using Nebx.BuildingBlocks.AspNetCore.Contracts.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.Responses
{
    public class ErrorResponseTests
    {
        [Fact]
        public void Create_ShouldReturnValidInstance_WhenParametersAreValid()
        {
            // Arrange
            var status = 400;
            var title = "Bad Request";
            var detail = "Invalid input data.";

            // Act
            var result = ErrorResponse.Create(status, title, detail);

            // Assert
            Assert.Equal(status, result.Status);
            Assert.Equal(title, result.Title);
            Assert.Equal(detail, result.Detail);
            Assert.False(string.IsNullOrWhiteSpace(result.TraceId));
            Assert.Null(result.Errors);
            Assert.Null(result.ErrorCode);
            Assert.Equal(string.Empty, result.Instance);
        }

        [Theory]
        [InlineData(99)]
        [InlineData(600)]
        public void Create_ShouldThrow_WhenStatusCodeIsOutOfRange(int status)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                ErrorResponse.Create(status, "Title", "Detail"));

            Assert.Contains("Invalid HTTP status code", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_ShouldThrow_WhenTitleIsInvalid(string? title)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ErrorResponse.Create(400, title!, "Detail"));

            Assert.Contains("Title cannot be null, empty, or whitespace", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Create_ShouldThrow_WhenDetailIsInvalid(string? detail)
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                ErrorResponse.Create(400, "Bad Request", detail!));

            Assert.Contains("Detail cannot be null, empty, or whitespace", ex.Message);
        }

        [Fact]
        public void AddTraceId_ShouldSetTraceId()
        {
            // Arrange
            var error = ErrorResponse.Create(400, "Bad Request", "Invalid input");
            var traceId = Guid.NewGuid().ToString();

            // Act
            error.AddTraceId(traceId);

            // Assert
            Assert.Equal(traceId, error.TraceId);
        }

        [Fact]
        public void AddInstance_ShouldSetInstance()
        {
            // Arrange
            var error = ErrorResponse.Create(404, "Not Found", "Resource missing");
            var instance = "/api/users/123";

            // Act
            error.AddInstance(instance);

            // Assert
            Assert.Equal(instance, error.Instance);
        }

        [Fact]
        public void AddErrorCode_ShouldSetErrorCode()
        {
            // Arrange
            var error = ErrorResponse.Create(400, "Bad Request", "Invalid request");

            // Act
            error.AddErrorCode("VALIDATION_ERROR");

            // Assert
            Assert.Equal("VALIDATION_ERROR", error.ErrorCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddErrorCode_ShouldThrow_WhenInvalid(string? code)
        {
            // Arrange
            var error = ErrorResponse.Create(400, "Bad Request", "Invalid request");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => error.AddErrorCode(code!));

            Assert.Contains("Error code cannot be null, empty, or whitespace", ex.Message);
        }

        [Fact]
        public void AddErrors_ShouldSetValidationErrors()
        {
            // Arrange
            var error = ErrorResponse.Create(422, "Unprocessable Entity", "Validation failed");
            var errors = new Dictionary<string, string[]>
            {
                ["email"] = new[] { "Email is required." },
                ["password"] = new[] { "Password must be at least 8 characters long." }
            };

            // Act
            error.AddErrors(errors);

            // Assert
            Assert.NotNull(error.Errors);
            Assert.True(error.Errors!.ContainsKey("email"));
            Assert.Equal("Email is required.", error.Errors["email"][0]);
        }

        [Fact]
        public void AddErrors_ShouldThrow_WhenCollectionIsNullOrEmpty()
        {
            // Arrange
            var error = ErrorResponse.Create(422, "Unprocessable Entity", "Validation failed");

            // Act & Assert (null)
            var ex1 = Assert.Throws<ArgumentException>(() => error.AddErrors(null!));
            Assert.Contains("cannot be null or empty", ex1.Message);

            // Act & Assert (empty)
            var ex2 = Assert.Throws<ArgumentException>(() => error.AddErrors(new Dictionary<string, string[]>()));
            Assert.Contains("cannot be null or empty", ex2.Message);
        }
    }
}
