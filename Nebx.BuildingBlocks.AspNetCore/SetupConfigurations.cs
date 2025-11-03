using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces.Services;
using Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interceptors;
using Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

namespace Nebx.BuildingBlocks.AspNetCore;

/// <summary>
/// Provides extension methods for registering shared infrastructure and building block services
/// used throughout the application.
/// </summary>
/// <remarks>
/// The methods in this class are intended to be used during application startup 
/// to configure dependency injection and essential cross-cutting services.
/// </remarks>
public static class SetupConfigurations
{
    /// <summary>
    /// Registers foundational infrastructure services, configurations, interceptors, and abstractions 
    /// required by the application.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> used to register dependencies.
    /// </param>
    /// <param name="configuration">
    /// The application's <see cref="IConfiguration"/> instance used to resolve configuration settings.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance to allow fluent chaining of additional service registrations.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method should be invoked **once** in the application's <c>Program.cs</c> during startup 
    /// to ensure consistent registration of core building block services.  
    /// Repeated calls are not necessary and may cause duplicate service registrations.
    /// </para>
    /// 
    /// The following components are registered:
    /// <list type="bullet">
    /// <item><description>JSON serialization configuration via <c>AddJsonConfiguration()</c>.</description></item>
    /// <item><description>Rate limiter response handling via <c>SetRateLimiterResponse()</c>.</description></item>
    /// <item><description>EF Core <see cref="ISaveChangesInterceptor"/> implementations for time auditing and domain event dispatching.</description></item>
    /// <item><description>System abstraction <see cref="IClock"/> for testable and reliable time management.</description></item>
    /// <item><description>Mediator configuration with scoped <see cref="IMediator"/> for in-process event handling.</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddBuildingBlocks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddJsonConfiguration();
        services.SetRateLimiterResponse();

        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        services.AddSingleton<IClock, Clock>();

        // need assembly to be supplied, or else it'll throw exception
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SetupConfigurations).Assembly));
        services.AddScoped<IMediator, MediatorImplementation>();

        return services;
    }

    /// <summary>
    /// Registers module-level dependencies such as FluentValidation validators and MediatR handlers
    /// from the specified assembly.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> used to register dependencies.
    /// </param>
    /// <param name="assembly">
    /// The assembly containing validators, MediatR handlers, and other module-related components.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance, allowing fluent chaining of registrations.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method automatically scans the provided <paramref name="assembly"/> to:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Register all <see cref="FluentValidation.IValidator{T}"/> implementations for dependency injection.</description></item>
    /// <item><description>Register all mediator request handlers, notification handlers, and pipeline behaviors found in the assembly.</description></item>
    /// </list>
    /// <para>
    /// Typically, this method should be called once per application module — usually during startup
    /// (e.g., in <c>Program.cs</c> or within a module registration method).
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to register validators and handlers from a module:
    /// <code>
    /// builder.Services.AddModuleDependencies(typeof(MyModule).Assembly);
    /// </code>
    /// </example>
    public static IServiceCollection AddModuleDependencies(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        return services;
    }
}