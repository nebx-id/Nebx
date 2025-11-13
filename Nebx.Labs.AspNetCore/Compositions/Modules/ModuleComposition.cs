using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nebx.Labs.AspNetCore.Compositions.Modules;

/// <summary>
/// Provides discovery, registration, and initialization of application modules that implement <see cref="IModule"/>.
/// Enables modular composition through automatic scanning and registration.
/// </summary>
public static class ModuleComposition
{
    private const string LoggerName = nameof(ModuleComposition);

    /// <summary>
    /// Scans all loaded assemblies for types implementing <see cref="IModule"/>,
    /// registers them in the dependency injection container, and invokes their <see cref="IModule.AddServices"/> method.
    /// </summary>
    /// <param name="services">The current <see cref="IServiceCollection"/>.</param>
    /// <param name="configuration">The current <see cref="IConfiguration"/> instance.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
    /// <remarks>
    /// <para>
    /// This method automatically discovers all modules that implement <see cref="IModule"/> across loaded assemblies.
    /// Each discovered module’s <see cref="IModule.AddServices"/> method is invoked while the service collection
    /// is still mutable, allowing modules to register their own dependencies and configurations.
    /// </para>
    /// <para>
    /// A temporary <see cref="ServiceProvider"/> is created only for resolving module instances during discovery.
    /// This occurs before the main provider is built and does not affect runtime performance.
    /// </para>
    /// </remarks>
    public static IServiceCollection RegisterModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Discover all modules
        services.Scan(scan => scan
            .FromApplicationDependencies(a => !a.IsDynamic)
            .AddClasses(c => c.AssignableTo<IModule>())
            .As<IModule>()
            .WithSingletonLifetime());

        // Build a temporary provider to resolve modules for registration
        using var provider = services.BuildServiceProvider();
        var modules = provider.GetServices<IModule>().ToList();

        // Allow each module to register its own services
        foreach (var module in modules)
        {
            module.AddServices(services, configuration);
        }

        // Store the discovered modules for later runtime use
        services.AddSingleton<IReadOnlyCollection<IModule>>(modules);
        return services;
    }

    /// <summary>
    /// Initializes all previously discovered modules by invoking their <see cref="IModule.UseMiddlewares"/> method.
    /// Should be called after the application is built.
    /// </summary>
    /// <param name="app">The current <see cref="WebApplication"/> instance.</param>
    /// <returns>The modified <see cref="WebApplication"/> for chaining.</returns>
    /// <remarks>
    /// <para>
    /// Typically called in <c>Program.cs</c> after the application has been built.
    /// This phase allows each module to configure its routes, middleware, or endpoints.
    /// </para>
    /// <para>
    /// Any logging related to module initialization is handled through the application’s configured <see cref="ILoggerFactory"/>.
    /// </para>
    /// </remarks>
    public static WebApplication UseModules(this WebApplication app)
    {
        var modules = app.Services.GetRequiredService<IReadOnlyCollection<IModule>>();
        var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger(LoggerName);

        foreach (var module in modules)
        {
            module.UseMiddlewares(app);
        }

        logger.LogDebug("All modules initialized successfully.");
        return app;
    }
}
