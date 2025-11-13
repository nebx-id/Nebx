using Microsoft.Extensions.Hosting;

namespace Nebx.Labs.AspNetCore.Pipelines;

/// <summary>
/// Provides extension methods for configuring the application host.
/// </summary>
public static class HostPipeline
{
    /// <summary>
    /// Enables validation for the default service provider to ensure correct dependency injection scopes and service resolution.
    /// </summary>
    /// <param name="host">The <see cref="IHostBuilder"/> to configure.</param>
    /// <returns>The configured <see cref="IHostBuilder"/> instance.</returns>
    public static IHostBuilder UseServiceProviderValidator(this IHostBuilder host)
    {
        host.UseDefaultServiceProvider((_, options) =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });

        return host;
    }
}
