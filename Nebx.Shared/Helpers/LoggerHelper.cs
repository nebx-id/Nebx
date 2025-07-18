using Serilog;
using Serilog.Events;

namespace Nebx.Shared.Helpers;

public static class LoggerHelper
{
    public static ILogger Create(Action<LoggerConfiguration>? configuration = null)
    {
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration.Enrich
            .FromLogContext();

        loggerConfiguration.WriteTo
            .Console();

        loggerConfiguration.MinimumLevel
            .Override("Microsoft", LogEventLevel.Warning);

        configuration?.Invoke(loggerConfiguration);
        return Log.Logger = loggerConfiguration.CreateLogger();
    }
}