using Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

namespace Nebx.BuildingBlocks.AspNetCore.Tests.Infra.Implementations;

public sealed class ClockTests
{
    private readonly Clock _sut = new();

    [Fact]
    public void UtcNow_ShouldBeCloseToSystemUtcNow()
    {
        var expected = DateTime.UtcNow;
        Assert.InRange(_sut.UtcNow, expected.AddSeconds(-1), expected.AddSeconds(1));
    }

    [Fact]
    public void Now_ShouldBeCloseToSystemNow()
    {
        var expected = DateTime.Now;
        Assert.InRange(_sut.Now, expected.AddSeconds(-1), expected.AddSeconds(1));
    }

    [Fact]
    public void UtcNowOffset_ShouldBeCloseToSystemUtcNowOffset()
    {
        var expected = DateTimeOffset.UtcNow;
        Assert.InRange(_sut.UtcNowOffset, expected.AddSeconds(-1), expected.AddSeconds(1));
    }

    [Fact]
    public void NowOffset_ShouldBeCloseToSystemNowOffset()
    {
        var expected = DateTimeOffset.Now;
        Assert.InRange(_sut.NowOffset, expected.AddSeconds(-1), expected.AddSeconds(1));
    }

    [Fact]
    public void Today_ShouldMatchSystemToday()
    {
        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), _sut.Today);
    }

    [Fact]
    public void UtcToday_ShouldMatchSystemUtcToday()
    {
        Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), _sut.UtcToday);
    }
}