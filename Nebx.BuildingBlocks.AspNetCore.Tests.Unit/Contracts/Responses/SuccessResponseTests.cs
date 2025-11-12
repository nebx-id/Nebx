using Nebx.BuildingBlocks.AspNetCore.Contracts.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.Responses
{
    public class SuccessResponseTests
    {
        [Fact]
        public void Create_ShouldReturnValidInstance_WhenDataProvided()
        {
            // Arrange
            var data = new { Id = 1, Name = "John" };

            // Act
            var result = SuccessResponse<object>.Create(data);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(data, result.Data);
            Assert.Null(result.Meta);
        }

        [Fact]
        public void Create_ShouldIncludeMeta_WhenProvided()
        {
            // Arrange
            var data = new[] { 1, 2, 3 };
            var meta = new Meta();
            meta.AddPagination(1, 10, 30);

            // Act
            var result = SuccessResponse<int[]>.Create(data, meta);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(data, result.Data);
            Assert.NotNull(result.Meta);
            Assert.Equal(1, result.Meta!.Page);
            Assert.Equal(3, result.Meta!.TotalPages);
            Assert.Equal(30, result.Meta!.TotalCount);
        }

        [Fact]
        public void StaticHelper_Create_ShouldReturnValidInstance()
        {
            // Arrange
            const string data = "hello";

            // Act
            var result = SuccessResponse.Create(data);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(data, result.Data);
        }

        [Fact]
        public void JsonConstructor_ShouldDeserializeProperly()
        {
            // Arrange
            var data = new { Value = 123 };
            var meta = new Meta();
            meta.AddPagination(2, 10, 40);

            // Act
            var response = new SuccessResponse<object>(data, meta);

            // Assert
            Assert.Equal(data, response.Data);
            Assert.Equal(2, response.Meta!.Page);
            Assert.Equal(4, response.Meta!.TotalPages);
        }
    }
}
