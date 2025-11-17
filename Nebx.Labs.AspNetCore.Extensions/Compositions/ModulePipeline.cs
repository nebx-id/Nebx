using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Nebx.Labs.Mediator;

namespace Nebx.Labs.AspNetCore.Extensions.Compositions;

/// <summary>
/// Provides extension methods for registering module-level dependencies.
/// </summary>
public static class ModulePipeline
{
    /// <summary>
    /// Adds all module dependencies discovered in the specified assembly,
    /// including endpoints, Mediator handlers, and validators.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="assembly">The assembly to scan for module components.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddModuleDependencies(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediator(assembly);
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
