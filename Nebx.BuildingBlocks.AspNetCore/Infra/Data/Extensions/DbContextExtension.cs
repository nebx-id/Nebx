using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Infra.Data.Interfaces;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Data.Extensions;

/// <summary>
/// Provides extension methods for registering Entity Framework Core <see cref="DbContext"/> 
/// instances with dependency injection.
/// </summary>
public static class DbContextExtension
{
    /// <summary>
    /// Registers a scoped <see cref="DbContext"/> of type <typeparamref name="TDbContext"/> 
    /// with the dependency injection container, along with its associated <see cref="IUnitOfWork{TDbContext}"/>.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the registration to.</param>
    /// <param name="optionBuilder">
    /// A delegate used to configure the <see cref="DbContextOptionsBuilder"/> for the <typeparamref name="TDbContext"/>.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance so that additional calls can be chained.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method registers the <typeparamref name="TDbContext"/> using 
    /// <see cref="EntityFrameworkServiceCollectionExtensions.AddDbContext{TContext}(IServiceCollection, Action{IServiceProvider, DbContextOptionsBuilder}, ServiceLifetime, ServiceLifetime)"/>.
    /// </para>
    /// <para>
    /// Additional configuration applied:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Registers all <see cref="ISaveChangesInterceptor"/> implementations found in DI.</description></item>
    /// <item><description>Logs EF Core debug-level messages to the console.</description></item>
    /// <item><description>Enables detailed errors for better debugging.</description></item>
    /// </list>
    /// <para>
    /// Also registers a scoped <see cref="IUnitOfWork{TDbContext}"/> implementation, 
    /// allowing consumers to use the Unit of Work pattern with the registered DbContext.
    /// </para>
    /// </remarks>
    /// <example>
    /// Example registration in <c>Program.cs</c>:
    /// <code><![CDATA[
    /// builder.Services.AddScopedDbContext<AppDbContext>((sp, options) =>
    /// {
    ///     var configuration = sp.GetRequiredService<IConfiguration>();
    ///     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    /// });
    /// ]]></code>
    /// </example>
    public static IServiceCollection AddScopedDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> optionBuilder) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            optionBuilder.Invoke(sp, options);

            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
            options.EnableDetailedErrors();
        });

        services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();
        return services;
    }
}