using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Endpoint;

public static class EndpointExplorerSetup
{
    /// <summary>
    ///     Registers the services needed for API versioning and endpoint metadata exploration.
    /// </summary>
    /// <param name="services">The service collection for dependency injection.</param>
    internal static void AddEndpointExplorerSetup(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApi();

        services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;

                // Use ONLY the "x-api-version" header to read the version
                options.ApiVersionReader = new HeaderApiVersionReader(SwaggerSetup.VersioningHeaderName);
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = false; // not using URL segment
            });
    }

    /// <summary>
    ///     Configures OpenAPI middleware for the application.
    ///     Intended for development or non-production environments only.
    /// </summary>
    /// <param name="app">The <c>WebApplication</c> instance.</param>
    internal static void UseEndpointExplorerSetup(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.Logger.LogDebug("Skip adding Open API in production");
            return;
        }

        app.MapOpenApi();
    }
}