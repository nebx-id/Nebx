using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Configurations;

/// <summary>
/// Provides extension methods for configuring application modules.
/// </summary>
public static class ModuleConfiguration
{
    /// <summary>
    /// Registers services, endpoints, validators, and MediatR handlers from the specified assembly.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="assembly">The assembly that contains the endpoints, validators, and MediatR handlers.</param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.
    /// </returns>
    /// <remarks>
    /// This method:
    /// <list type="bullet">
    /// <item><description>Registers minimal API endpoints from the given assembly via <c>AddEndpoints</c>.</description></item>
    /// <item><description>Registers FluentValidation validators from the given assembly.</description></item>
    /// <item><description>Registers MediatR handlers from the given assembly.</description></item>
    /// </list>
    /// </remarks>
    public static IServiceCollection AddModuleConfiguration(this IServiceCollection services, Assembly assembly)
    {
        services.AddEndpoints(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

        return services;
    }
}