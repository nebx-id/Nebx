using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;

namespace Nebx.BuildingBlocks.AspNetCore;

internal class ExampleSetup : IFakeModule
{
    private readonly Assembly _assembly = typeof(ExampleSetup).Assembly;

    public void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleDependencies(_assembly);
        services.AddEndpoints(_assembly);
    }

    public void UseModule(WebApplication app)
    {
        app.MapEndpoints(_assembly);
    }
}