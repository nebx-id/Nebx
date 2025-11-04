using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces;

/// <summary>
/// Defines a contract for modular components that can register services
/// and configure application endpoints.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Registers the module's dependencies and services with the application's
    /// dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register services.</param>
    /// <param name="configuration">The application's <see cref="IConfiguration"/> instance.</param>
    void RegisterModule(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures the module's endpoints, middleware, or other runtime components
    /// after the application has been built.
    /// </summary>
    /// <param name="app">The current <see cref="WebApplication"/> instance.</param>
    void UseModule(WebApplication app);
}