namespace Nebx.BuildingBlocks.AspNetCore.Tests.Unit.Tests;

public class FakeClockTests
{
    [Fact]
    public void Constructor_SetsInitialUtcTime()
    {
        // Arrange
        var initial = new DateTime(2024, 01, 01, 12, 00, 00, DateTimeKind.Unspecified);

        // Act
        var clock = new FakeClock(initial);

        // Assert
        Assert.Equal(DateTimeKind.Utc, clock.UtcNow.Kind);
        Assert.Equal(new DateTime(2024, 01, 01, 12, 00, 00, DateTimeKind.Utc), clock.UtcNow);
    }

    [Fact]
    public void Now_Returns_LocalTimeVariant()
    {
        // Arrange
        var utc = new DateTime(2024, 01, 01, 12, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(utc);

        // Act
        var local = clock.Now;

        // Assert
        Assert.Equal(utc.ToLocalTime(), local);
    }

    [Fact]
    public void Advance_MovesTimeForward()
    {
        // Arrange
        var start = new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(start);

        // Act
        clock.Advance(TimeSpan.FromHours(5));

        // Assert
        Assert.Equal(start.AddHours(5), clock.UtcNow);
    }

    [Fact]
    public void Today_Returns_LocalDate()
    {
        // Arrange
        var utc = new DateTime(2024, 01, 01, 23, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(utc);

        // Act
        var today = clock.Today;

        // Assert
        Assert.Equal(DateOnly.FromDateTime(utc.ToLocalTime()), today);
    }

    [Fact]
    public void UtcToday_Returns_UtcDateOnly()
    {
        // Arrange
        var utc = new DateTime(2024, 03, 15, 01, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(utc);

        // Act
        var today = clock.UtcToday;

        // Assert
        Assert.Equal(new DateOnly(2024, 03, 15), today);
    }

    [Fact]
    public void UtcNowOffset_UsesUtcTime()
    {
        // Arrange
        var utc = new DateTime(2024, 02, 10, 10, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(utc);

        // Act
        var dto = clock.UtcNowOffset;

        // Assert
        Assert.Equal(new DateTimeOffset(utc), dto);
    }

    [Fact]
    public void NowOffset_UsesLocalTime()
    {
        // Arrange
        var utc = new DateTime(2024, 02, 10, 10, 00, 00, DateTimeKind.Utc);
        var clock = new FakeClock(utc);

        // Act
        var dto = clock.NowOffset;

        // Assert
        Assert.Equal(new DateTimeOffset(utc.ToLocalTime()), dto);
    }
}
