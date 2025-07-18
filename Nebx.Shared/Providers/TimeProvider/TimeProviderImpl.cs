namespace Nebx.Shared.Providers.TimeProvider;

/// <summary>
/// Provides the system time using the machine clock.
/// </summary>
public sealed class TimeProviderImpl : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;

    public DateTimeOffset UtcNowOffset => DateTimeOffset.UtcNow;

    public DateTimeOffset NowOffset => DateTimeOffset.Now;

    public DateOnly Today => DateOnly.FromDateTime(DateTime.Now);

    public DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);
}