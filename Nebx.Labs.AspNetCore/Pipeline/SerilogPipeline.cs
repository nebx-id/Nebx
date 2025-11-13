using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nebx.Labs.AspNetCore.Pipeline;

/// <summary>
/// Provides extension methods for configuring Serilog as the application's
/// logging provider.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Adds Serilog to the application's host builder using configuration,
    /// dependency-injected enrichers, and optional Seq integration.
    /// </summary>
    /// <remarks>
    /// Configuration sources are resolved in the following order:
    /// <para>
    /// <b>1. Docker/host environment variables (highest precedence)</b><br/>
    /// • <c>SEQ_SERVER_URL</c><br/>
    /// • <c>SEQ_API_KEY</c><br/>
    /// If these are set, they override all other Seq settings.
    /// </para>
    /// <para>
    /// <b>2. appsettings.json / appsettings.*.json</b><br/>
    /// Uses <c>Seq:ServerUrl</c> and <c>Seq:ApiKey</c> if environment variables
    /// are not provided.
    /// </para>
    /// <para>
    /// <b>3. Default Serilog configuration</b><br/>
    /// The remaining Serilog settings (minimum level, enrichers, sinks, etc.)
    /// are loaded from <c>appsettings.json</c> via <c>ReadFrom.Configuration</c>.
    /// </para>
    ///
    /// Additional behavior:
    /// <list type="bullet">
    /// <item><description>
    /// Enrichers and sinks registered in dependency injection are automatically
    /// added via <c>ReadFrom.Services</c>.
    /// </description></item>
    /// <item><description>
    /// Seq is only configured if a non-empty <c>ServerUrl</c> is found.
    /// </description></item>
    /// </list>
    /// </remarks>
    /// <param name="host">The <see cref="IHostBuilder"/> to configure.</param>
    /// <returns>The same <see cref="IHostBuilder"/> instance for chaining.</returns>
    public static IHostBuilder AddSerilogConfiguration(this IHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            var seqUrl = Environment.GetEnvironmentVariable("SEQ_SERVER_URL") ?? context.Configuration["Seq:ServerUrl"];
            var seqApiKey = Environment.GetEnvironmentVariable("SEQ_API_KEY") ?? context.Configuration["Seq:ApiKey"];

            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);

            if (!string.IsNullOrWhiteSpace(seqUrl))
            {
                configuration.WriteTo.Seq(seqUrl, apiKey: seqApiKey);
            }
        });

        return host;
    }
}
