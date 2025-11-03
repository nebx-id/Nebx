using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Infra;

namespace Nebx.BuildingBlocks.AspNetCore;

internal class ExampleSetup : IFakeModule
{
    public void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddBuildingBlocks(configuration);
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public void UseModule(WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
    }
}