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

        services.AddScoped<IMediator, MediatorImplementation>();
        services.AddMediatR(_ => { });

        return services;
    }
}