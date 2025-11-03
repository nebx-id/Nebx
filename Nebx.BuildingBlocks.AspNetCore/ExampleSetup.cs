using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Core.Interfaces.Services;
using Nebx.BuildingBlocks.AspNetCore.Infra;
using Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interceptors;
using Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;
using Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Mediator;

namespace Nebx.BuildingBlocks.AspNetCore;

internal class ExampleSetup : IFakeModule
{
    public void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddJsonConfiguration();
        services.SetRateLimiterResponse();

        services.AddSingleton<IClock, Clock>();
        services.AddScoped<IMediator, MediatorImplementation>();

        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
    }

    public void UseModule(WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
    }
}