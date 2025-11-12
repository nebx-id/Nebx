using System.Reflection;
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Events.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Mediator.Abstractions;
using Nebx.BuildingBlocks.AspNetCore.Mediator.Data;
using Nebx.BuildingBlocks.AspNetCore.Mediator.Implementation;

namespace Nebx.BuildingBlocks.AspNetCore.Mediator;

/// <summary>
/// Registers mediator modules and supporting infrastructure.
/// </summary>
public static class MediatorSetup
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
