using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebx.BuildingBlocks.AspNetCore.Common;
using Nebx.BuildingBlocks.AspNetCore.Configurations;
using Nebx.BuildingBlocks.AspNetCore.Configurations.Endpoint;
using Nebx.BuildingBlocks.AspNetCore.Configurations.EntityFramework;
using Nebx.BuildingBlocks.AspNetCore.CQRS;
using Nebx.Shared.Providers.TimeProvider;

namespace Nebx.BuildingBlocks.AspNetCore;

public static class BuildingBlockSetup
{
    /// <summary>
    /// Provides a standard setup for hosting a web application, including default service provider validation
    /// and Kestrel server configuration.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="WebApplicationBuilder"/> used to configure the application host.
    /// </param>
    /// <param name="kestrelServerConfiguration">
    /// An optional delegate to further configure the <see cref="KestrelServerOptions"/>.
    /// </param>
    /// <remarks>
    /// This method:
    /// <list type="bullet">
    /// <item>
    /// Validates DI scopes and service registrations at build time to catch configuration issues early.
    /// </item>
    /// <item>
    /// Configures Kestrel server limits such as request headers timeout and maximum request body size.
    /// </item>
    /// </list>
    /// </remarks>
    public static void AddBuildingBlockHostSetup(
        this WebApplicationBuilder builder,
        Action<KestrelServerOptions>? kestrelServerConfiguration = null)
    {
        // ensure DI is valid when building the project
        builder.Host.UseDefaultServiceProvider((_, options) =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30); // based on browser timeout
            options.Limits.MaxRequestBodySize = 1 * 1024 * 1024; // Limit file upload to 1 MB (based on NGINX default)
            kestrelServerConfiguration?.Invoke(options);
        });
    }

    public static void AddBuildingBlockSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddAntiforgery();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddEntityFrameworkSetup();

        services.AddJsonSerializerSetup();
        services.AddRateLimiterSetup();
        services.AddEndpointExplorerSetup();

        // services.AddSwaggerSetup();
        // services.AddSwaggerVersioning();

        services.AddScoped<IMediator, Mediator>();
        services.AddSingleton<ITimeProvider, TimeProviderImpl>();
    }

    /// <summary>
    /// Configures and applies the standard API building blocks middleware for the application,
    /// including rate limiting, anti-forgery, user-defined middleware, endpoint mapping, and
    /// Swagger endpoint exploration.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance to configure.
    /// </param>
    /// <param name="middleware">
    /// An optional delegate for registering additional custom middleware components. This delegate
    /// is invoked after the default middleware (rate limiting and anti-forgery) and before
    /// endpoint mapping. If <c>null</c>, no additional middleware will be added.
    /// </param>
    /// <remarks>
    /// The order of execution is:
    /// <list type="number">
    /// <item>Apply rate limiter middleware.</item>
    /// <item>Apply anti-forgery middleware.</item>
    /// <item>Invoke any user-provided custom middleware setup.</item>
    /// <item>Map endpoints using Reflections (Assembly scan of <see cref="IEndpoint"/>).</item>
    /// <item>Enable the Open API endpoint explorer.</item>
    /// </list>
    /// </remarks>
    public static void UseBuildingBlockSetup(this WebApplication app, Action<WebApplication>? middleware = null)
    {
        app.UseExceptionHandler(_ => { });
        app.UseRateLimiter();
        app.UseAntiforgery();

        // dynamically setup middleware before endpoints as the user need
        middleware?.Invoke(app);

        // map endpoint should run before the UseEndpointExplorerSetup to properly load the endpoint documentation
        app.MapEndpoints();
        app.UseEndpointExplorerSetup();
        // app.UseSwaggerSetup();
    }

    public static void AddModuleSetup(
        this IServiceCollection services,
        Assembly assembly,
        IConfiguration configuration)
    {
        services.AddEndpoints(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    }
}