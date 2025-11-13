using System.Reflection;
using LiteBus.Commands;
using LiteBus.Events;
using LiteBus.Extensions.Microsoft.DependencyInjection;
using LiteBus.Queries;
using Microsoft.Extensions.DependencyInjection;
using Nebx.Labs.Mediator.Abstractions;

namespace Nebx.Labs.Mediator;

/// <summary>
/// Registers mediator modules and supporting infrastructure.
/// </summary>
public static class MediatorComposition
{
    /// <summary>
    /// Registers LiteBus command, query, and event handlers from the specified assembly,
    /// and configures <see cref="IMediator"/> and domain event dispatching.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="assembly">The assembly to scan for handlers.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
    {
        services.AddLiteBus(liteBus =>
        {
            liteBus.AddCommandModule(m => m.RegisterFromAssembly(assembly));
            liteBus.AddQueryModule(m => m.RegisterFromAssembly(assembly));
            liteBus.AddEventModule(m => m.RegisterFromAssembly(assembly));
        });

        services.AddScoped<IMediator, LiteBusMediator>();
        return services;
    }
}
