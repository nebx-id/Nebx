namespace Nebx.Shared.Providers.TimeProvider;

public sealed class FakeTimeProvider : ITimeProvider
{
    public DateTime UtcNow { get; set; }
    public DateTime Now => UtcNow.ToLocalTime();
    public DateTimeOffset UtcNowOffset => new DateTimeOffset(UtcNow, TimeSpan.Zero);
    public DateTimeOffset NowOffset => UtcNowOffset.ToLocalTime();
    public DateOnly Today => DateOnly.FromDateTime(Now);
    public DateOnly UtcToday => DateOnly.FromDateTime(UtcNow);

    public FakeTimeProvider(DateTime utcNow)
    {
        UtcNow = utcNow;
    }
}