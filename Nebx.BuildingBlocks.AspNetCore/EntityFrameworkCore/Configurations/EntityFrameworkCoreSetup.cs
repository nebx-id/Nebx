using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Configurations.Converters;

namespace Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Configurations;

/// <summary>
/// Provides helper extensions for configuring Entity Framework Core services.
/// </summary>
public static class EntityFrameworkCoreSetup
{
    /// <summary>
    /// Registers <typeparamref name="TDbContext"/> in DI.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type.</typeparam>
    /// <param name="services">The DI service collection.</param>
    /// <param name="optionBuilder">Callback to configure DbContext options.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddScopedDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionBuilder)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            optionBuilder(sp, options);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
            options.EnableDetailedErrors();
        });

        return services;
    }

    /// <summary>
    /// Configures all <see cref="DateTime"/> properties to use UTC value converters.
    /// </summary>
    /// <param name="builder">The model configuration builder.</param>
    /// <returns>The updated builder.</returns>
    public static ModelConfigurationBuilder AddUtcDateTimeConverters(this ModelConfigurationBuilder builder)
    {
        builder.Properties<DateTime>().HaveConversion<UtcDateTimeValueConverter>();
        builder.Properties<DateTime?>().HaveConversion<UtcNullableDateTimeValueConverter>();
        return builder;
    }
}
