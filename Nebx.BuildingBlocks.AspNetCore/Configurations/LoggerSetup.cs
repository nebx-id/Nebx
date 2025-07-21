using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebx.Shared.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations;

public static class LoggerSetup
{
    public static void AddLoggerSetup(
        this IServiceCollection services,
        Action<LoggerConfiguration>? configuration = null)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

        var logger = LoggerHelper.Create(c =>
        {
            var middlewareSource = Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware");

            c
                .MinimumLevel.Is(environment.IsProduction() ? LogEventLevel.Warning : LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .Filter.ByExcluding(middlewareSource);

            c
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithCorrelationId();

            configuration?.Invoke(c);
        });

        services.AddSerilog(logger);
    }
}