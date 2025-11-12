using Nebx.BuildingBlocks.AspNetCore.Infra.Time;

namespace Nebx.BuildingBlocks.AspNetCore.Tests;

/// <summary>
/// Test double for <see cref="IClock"/> providing a controllable, deterministic time source.
/// </summary>
public sealed class FakeClock : IClock
{
    private DateTime _currentUtc;

    /// <summary>
    /// Initializes the clock with a fixed starting UTC time.
    /// </summary>
    /// <param name="initialUtc">The initial UTC time value.</param>
    public FakeClock(DateTime initialUtc)
    {
        _currentUtc = DateTime.SpecifyKind(initialUtc, DateTimeKind.Utc);
    }

    /// <summary>
    /// Advances the internal clock by the specified duration.
    /// </summary>
    public void Advance(TimeSpan duration)
    {
        _currentUtc = _currentUtc.Add(duration);
    }

    /// <inheritdoc />
    public DateTime UtcNow => _currentUtc;

    /// <inheritdoc />
    public DateTime Now => _currentUtc.ToLocalTime();

    /// <inheritdoc />
    public DateTimeOffset UtcNowOffset => new(_currentUtc);

    /// <inheritdoc />
    public DateTimeOffset NowOffset => new(_currentUtc.ToLocalTime());

    /// <inheritdoc />
    public DateOnly Today => DateOnly.FromDateTime(Now);

    /// <inheritdoc />
    public DateOnly UtcToday => DateOnly.FromDateTime(_currentUtc);
}
