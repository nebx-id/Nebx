namespace Nebx.BuildingBlocks.AspNetCore.Services;

/// <summary>
/// Provides the current system time, abstracted for testability.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current UTC date and time as an offset.
    /// </summary>
    DateTimeOffset UtcNowOffset { get; }

    /// <summary>
    /// Gets the current local date and time as an offset.
    /// </summary>
    DateTimeOffset NowOffset { get; }

    /// <summary>
    /// Gets the current date (local).
    /// </summary>
    DateOnly Today { get; }

    /// <summary>
    /// Gets the current UTC date.
    /// </summary>
    DateOnly UtcToday { get; }
}

public sealed class Clock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    public DateTimeOffset NowOffset => DateTimeOffset.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    public DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);
}