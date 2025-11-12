using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Composition;

/// <summary>
/// Provides extension methods for discovering and mapping endpoint implementations
/// defined within a specific assembly.
/// </summary>
public static class EndpointsConfiguration
{
    /// <summary>
    /// Registers all <see cref="IEndpoint"/> implementations found in the specified assembly
    /// with the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the endpoints to.</param>
    /// <param name="assembly">The assembly to scan for <see cref="IEndpoint"/> implementations.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection RegisterEndpoints(this IServiceCollection services, Assembly assembly)
    {
        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(classes => classes.AssignableTo<IEndpoint>())
            .As<IEndpoint>()
            .WithTransientLifetime());

        return services;
    }

    /// <summary>
    /// Maps all registered <see cref="IEndpoint"/> implementations from the specified assembly
    /// to the application's routing system.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance used for endpoint mapping.</param>
    /// <param name="assembly">The assembly containing the endpoints to map.</param>
    /// <param name="groupBuilder">
    /// An optional <see cref="RouteGroupBuilder"/> for grouping endpoints under a specific route.
    /// If not provided, endpoints are mapped directly to the root application.
    /// </param>
    public static void MapEndpoints(
        this WebApplication app,
        Assembly assembly,
        RouteGroupBuilder? groupBuilder = null)
    {
        IEndpointRouteBuilder routeBuilder = groupBuilder is null ? app : groupBuilder;

        var endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>()
            .Where(x => x.GetType().Assembly == assembly)
            .ToList();

        endpoints.ForEach(x => x.AddRoutes(routeBuilder));
    }
}
