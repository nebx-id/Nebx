using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces.Services;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Services;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

/// <inheritdoc />
public sealed class Clock : IClock
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;

    /// <inheritdoc />
    public DateTime Now => DateTime.Now;

    /// <inheritdoc />
    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateTimeOffset NowOffset => DateTimeOffset.Now;

    /// <inheritdoc />
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    /// <inheritdoc />
    public DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);
}

/// <summary>
/// A fake clock for testing, where the current time can be controlled.
/// </summary>
public sealed class FakeClock : IClock
{
    private DateTime _utcNow;

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeClock"/> class.
    /// </summary>
    /// <param name="utcNow">The initial UTC time.</param>
    public FakeClock(DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            throw new ArgumentException("utcNow must be UTC", nameof(utcNow));

        _utcNow = utcNow;
    }

    /// <inheritdoc />
    public DateTime UtcNow => _utcNow;

    /// <inheritdoc />
    public DateTime Now => _utcNow.ToLocalTime();

    /// <inheritdoc />
    public DateTimeOffset UtcNowOffset => new DateTimeOffset(_utcNow, TimeSpan.Zero);

    /// <inheritdoc />
    public DateTimeOffset NowOffset => UtcNowOffset.ToLocalTime();

    /// <inheritdoc />
    public DateOnly Today => DateOnly.FromDateTime(Now);

    /// <inheritdoc />
    public DateOnly UtcToday => DateOnly.FromDateTime(UtcNow);

    /// <summary>
    /// Sets the current time to a new UTC value.
    /// </summary>
    public void SetUtcNow(DateTime utcNow)
    {
        if (utcNow.Kind != DateTimeKind.Utc)
            throw new ArgumentException("utcNow must be UTC", nameof(utcNow));

        _utcNow = utcNow;
    }

    /// <summary>
    /// Advances the clock forward by the given time span.
    /// </summary>
    public void Advance(TimeSpan timeSpan)
    {
        _utcNow = _utcNow.Add(timeSpan);
    }
}