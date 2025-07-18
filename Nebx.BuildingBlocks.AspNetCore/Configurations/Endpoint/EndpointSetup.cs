using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Nebx.Shared.Helpers;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Endpoint;

/// <summary>
///     Provides extension methods for registering and mapping endpoint classes that implement <see cref="IEndpoint" />.
/// </summary>
public static class EndpointSetup
{
    /// <summary>
    ///     Registers all types implementing <see cref="IEndpoint" /> from the specified assembly
    ///     as transient services in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register services with.</param>
    /// <param name="assembly">The assembly to scan for implementations of <see cref="IEndpoint" />.</param>
    internal static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var types = AssembliesHelper.GetTypes<IEndpoint>(assembly);
        var descriptors = types.Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
        services.TryAddEnumerable(descriptors);
    }

    /// <summary>
    ///     Maps all registered endpoints (that implement <see cref="IEndpoint" />) to the application's routing system.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication" /> to map routes for.</param>
    /// <param name="routeGroupBuilder">An optional <see cref="RouteGroupBuilder" /> to group endpoints under a common route.</param>
    internal static void MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEndpointRouteBuilder routeBuilder = routeGroupBuilder is null ? app : routeGroupBuilder;

        var endpoints = app.Services
            .GetRequiredService<IEnumerable<IEndpoint>>()
            .ToList();

        endpoints.ForEach(x => x.AddRoutes(routeBuilder));
    }
}