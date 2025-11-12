using Nebx.BuildingBlocks.AspNetCore.Contracts.Models;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Contracts.Responses;

public class MetaTests
{
    [Fact]
    public void AddPagination_Should_Set_Basic_Values()
    {
        var meta = new Meta();

        meta.AddPagination(page: 2, pageSize: 10, totalCount: 45);

        Assert.Equal(2, meta.Page);
        Assert.Equal(10, meta.PageSize);
        Assert.Equal(45, meta.TotalCount);
        Assert.Equal(5, meta.TotalPages); // 45/10 = 4.5 -> ceil = 5
    }

    [Fact]
    public void AddPagination_Should_Reset_Page_To_1_When_Page_Exceeds_TotalPages()
    {
        var meta = new Meta();

        meta.AddPagination(page: 10, pageSize: 10, totalCount: 30);

        Assert.Equal(1, meta.Page); // Page reset due to being out of range
        Assert.Equal(3, meta.TotalPages);
    }

    [Fact]
    public void HasNextPage_Should_Be_True_When_Items_Remain()
    {
        var meta = new Meta();
        meta.AddPagination(page: 2, pageSize: 10, totalCount: 35);

        Assert.True(meta.HasNextPage); // Page 2 * 10 = 20 -> still < 35
    }

    [Fact]
    public void HasNextPage_Should_Be_False_When_On_Last_Page()
    {
        var meta = new Meta();
        meta.AddPagination(page: 4, pageSize: 10, totalCount: 40);

        Assert.False(meta.HasNextPage);
    }

    [Fact]
    public void HasPreviousPage_Should_Be_True_When_Page_Greater_Than_1()
    {
        var meta = new Meta();
        meta.AddPagination(page: 3, pageSize: 10, totalCount: 100);

        Assert.True(meta.HasPreviousPage);
    }

    [Fact]
    public void HasPreviousPage_Should_Be_False_On_First_Page()
    {
        var meta = new Meta();
        meta.AddPagination(page: 1, pageSize: 10, totalCount: 100);

        Assert.False(meta.HasPreviousPage);
    }

    [Fact]
    public void HasNextPage_And_HasPreviousPage_Should_Be_Null_When_Not_Initialized()
    {
        var meta = new Meta();

        Assert.Null(meta.HasNextPage);
        Assert.Null(meta.HasPreviousPage);
    }

    [Fact]
    public void AddPagination_With_Pagination_Object_Should_Delegate_Correctly()
    {
        var pagination = new Pagination(page: 3, pageSize: 20);
        var meta = new Meta();

        meta.AddPagination(pagination, totalCount: 95);

        Assert.Equal(3, meta.Page);
        Assert.Equal(20, meta.PageSize);
        Assert.Equal(95, meta.TotalCount);
        Assert.Equal(5, meta.TotalPages); // 95/20 -> 4.75 -> ceil = 5
    }
}
