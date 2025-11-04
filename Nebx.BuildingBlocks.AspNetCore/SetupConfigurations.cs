using System.Reflection;
using FluentValidation;
using LiteBus.Commands.Extensions.MicrosoftDependencyInjection;
using LiteBus.Events.Extensions.MicrosoftDependencyInjection;
using LiteBus.Messaging.Extensions.MicrosoftDependencyInjection;
using LiteBus.Queries.Extensions.MicrosoftDependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interceptors;
using Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Services;

namespace Nebx.BuildingBlocks.AspNetCore;

/// <summary>
/// Provides extension methods for configuring shared infrastructure, mediator modules,
/// and cross-cutting building block services.
/// </summary>
/// <remarks>
/// These methods are typically invoked during application startup to register 
/// foundational infrastructure, validators, and messaging abstractions.
/// </remarks>
public static class SetupConfigurations
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
    public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
    {
        services.AddJsonConfiguration();
        services.SetRateLimiterResponse();

        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddSingleton<IClock, Clock>();

        return services;
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
    /// </remarks>
    public static IServiceCollection AddModuleDependencies(this IServiceCollection services, Assembly assembly)
    {
        services.AddLiteBus(liteBus =>
        {
            liteBus.AddCommandModule(m => m.RegisterFromAssembly(assembly));
            liteBus.AddQueryModule(m => m.RegisterFromAssembly(assembly));
            liteBus.AddEventModule(m => m.RegisterFromAssembly(assembly));
        });

        services.AddScoped<IMediator, LiteBusMediator>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}