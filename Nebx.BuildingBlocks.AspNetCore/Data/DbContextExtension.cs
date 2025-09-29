using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Implementations;
using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

namespace Nebx.BuildingBlocks.AspNetCore.Data;

public static class DbContextExtension
{
    public static IServiceCollection AddScopedDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionBuilder) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            optionBuilder.Invoke(sp, options);

            var environment = sp.GetRequiredService<IWebHostEnvironment>();
            var interceptor = sp.GetServices<ISaveChangesInterceptor>();

            options.AddInterceptors(interceptor);
            options.LogTo(Console.WriteLine, (_, level) => level == LogLevel.Debug);
            options.EnableDetailedErrors();

            if (!environment.IsProduction())
            {
                options.EnableSensitiveDataLogging();
            }
        });

        services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();
        return services;
    }
}
