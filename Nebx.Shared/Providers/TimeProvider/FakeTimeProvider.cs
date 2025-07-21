namespace Nebx.Shared.Providers.TimeProvider;

/// <summary>
/// Provides a fake implementation of <see cref="ITimeProvider"/> for testing purposes.
/// </summary>
public sealed class FakeTimeProvider : ITimeProvider
{
    /// <summary>
    /// Gets or sets the current UTC date and time used by this fake provider.
    /// </summary>
    public DateTime UtcNow { get; set; }

    /// <summary>
    /// Gets the current local date and time, based on <see cref="UtcNow"/>.
    /// </summary>
    public DateTime Now => UtcNow.ToLocalTime();

    /// <summary>
    /// Gets the current UTC date and time as a <see cref="DateTimeOffset"/>, based on <see cref="UtcNow"/>.
    /// </summary>
    public DateTimeOffset UtcNowOffset => new DateTimeOffset(UtcNow, TimeSpan.Zero);

    /// <summary>
    /// Gets the current local date and time as a <see cref="DateTimeOffset"/>, based on <see cref="UtcNow"/>.
    /// </summary>
    public DateTimeOffset NowOffset => UtcNowOffset.ToLocalTime();

    /// <summary>
    /// Gets today's date in the local time zone, based on <see cref="Now"/>.
    /// </summary>
    public DateOnly Today => DateOnly.FromDateTime(Now);

    /// <summary>
    /// Gets today's date in UTC, based on <see cref="UtcNow"/>.
    /// </summary>
    public DateOnly UtcToday => DateOnly.FromDateTime(UtcNow);

    /// <summary>
    /// Initializes a new instance of the <see cref="FakeTimeProvider"/> class with the specified UTC time.
    /// </summary>
    /// <param name="utcNow">The fixed UTC date and time to use.</param>
    public FakeTimeProvider(DateTime utcNow)
    {
        UtcNow = utcNow;
    }
}