namespace Nebx.Labs.Core.Infrastructure.Time;

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
