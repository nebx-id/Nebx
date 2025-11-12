using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebx.BuildingBlocks.AspNetCore.Configurations.Pipeline;
using Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Configurations.Interceptors;
using Nebx.BuildingBlocks.AspNetCore.Infra.Time;
using Nebx.BuildingBlocks.AspNetCore.Mediator;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Composition;

/// <summary>
/// Provides extension methods for configuring shared infrastructure, mediator modules,
/// and cross-cutting building block services.
/// </summary>
/// <remarks>
/// These methods are typically invoked during application startup to register
/// foundational infrastructure, validators, and messaging abstractions.
/// </remarks>
public static class HostDefaults
{
    /// <summary>
    /// Registers core infrastructure services, system abstractions, and EF Core interceptors.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance for fluent chaining.
    /// </returns>
    /// <remarks>
    /// Configures the following:
    /// <list type="bullet">
    /// <item><description>JSON serialization options.</description></item>
    /// <item><description>Rate limiter response behavior.</description></item>
    /// <item><description>EF Core save-change interceptors for auditing and domain event dispatching.</description></item>
    /// <item><description>System abstractions such as <see cref="IClock"/> for testable time management.</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddJsonConfiguration();
        services.SetRateLimiterResponse();

        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddSingleton<IClock, Clock>();

        return services;
    }

    /// <summary>
    /// Applies default host-level configuration settings.
    /// </summary>
    /// <param name="host">The host builder to configure.</param>
    /// <returns>The configured host builder.</returns>
    public static IHostBuilder ConfigureHost(this IHostBuilder host)
    {
        host.UseServiceProviderValidator();
        return host;
    }

    /// <summary>
    /// Registers validators and LiteBus handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="assembly">The assembly to scan for validators and handlers.</param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance for fluent chaining.
    /// </returns>
    /// <remarks>
    /// Scans the given assembly and automatically registers:
    /// <list type="bullet">
    /// <item><description>All <see cref="IValidator{T}"/> implementations.</description></item>
    /// <item><description>All mediator command, query, and event handlers.</description></item>
    /// </list>
    /// The API project must reference the module project; otherwise, its types will not be discovered by reflection.
    /// </remarks>
    public static IServiceCollection AddModuleDependencies(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediator(assembly);
        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
