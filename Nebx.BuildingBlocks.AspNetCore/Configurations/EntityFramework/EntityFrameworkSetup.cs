using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Data;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.EntityFramework;

public static class EntityFrameworkSetup
{
    /// <summary>
    /// Registers Entity Framework Core services and related infrastructure components 
    /// required by the application. This method configures global save changes interceptors,
    /// such as time auditing, soft delete behaviors, and domain event dispatching.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Entity Framework services and interceptors will be added.
    /// </param>
    /// <remarks>
    /// This method adds the following interceptors as singletons:
    /// <list type="bullet">
    /// <item><see cref="TimeAuditInterceptor"/> — automatically sets or updates created and modified timestamps.</item>
    /// <item><see cref="DispatchDomainEventInterceptor"/> — automatically dispatches domain events raised by aggregate roots during save operations.</item>
    /// </list>
    /// These interceptors are applied globally to all <see cref="DbContext"/> instances registered in the application.
    /// </remarks>
    public static void AddEntityFrameworkSetup(this IServiceCollection services)
    {
        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
    }

    /// <summary>
    ///     Applies any pending EF Core migrations for the specified <typeparamref name="TDbContext" /> at application startup.
    ///     Skips execution in production environments by default.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext" /> to migrate.</typeparam>
    /// <param name="app">The <see cref="WebApplication" /> instance.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken" /> instance.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ApplyMigrationsAsync<TDbContext>(
        this WebApplication app,
        CancellationToken cancellationToken = default) where TDbContext : DbContext
    {
        if (app.Environment.IsProduction())
        {
            app.Logger.LogInformation("Skipping database migration in production.");
            return;
        }

        try
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
            app.Logger.LogInformation("Database migration applied for {DbContext}.", typeof(TDbContext).Name);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations for {DbContext}.",
                typeof(TDbContext).Name);
            throw;
        }
    }


    /// <summary>
    /// Registers a <see cref="DbContext"/> of type <typeparamref name="TDbContext"/> in the service collection,
    /// with optional context pooling, automatic registration of <see cref="ISaveChangesInterceptor"/> instances,
    /// logging configuration, and a scoped <see cref="IUnitOfWork{TDbContext}"/> implementation.
    /// </summary>
    /// <typeparam name="TDbContext">The type of the <see cref="DbContext"/> to register.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the context to.</param>
    /// <param name="dbContextOptions">
    /// A delegate used to configure the <see cref="DbContextOptionsBuilder"/>. This allows access to the
    /// <see cref="IServiceProvider"/> for service-based configuration such as connection strings, provider settings, or additional options.
    /// </param>
    /// <remarks>
    /// This method performs the following actions:
    /// <list type="bullet">
    /// <item>
    /// Resolves and adds all registered <see cref="ISaveChangesInterceptor"/> instances to the context options.
    /// </item>
    /// <item>
    /// Configures logging for EF Core by logging SQL and other debug information to the console
    /// when the log level is <see cref="LogLevel.Debug"/>.
    /// </item>
    /// <item>
    /// Registers a scoped <see cref="IUnitOfWork{TDbContext}"/> to provide a unit of work abstraction for the DbContext.
    /// </item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <code>
    /// services.AddDbContextSetup&lt;MyDbContext&gt;((sp, options) =&gt;
    /// {
    ///     var config = sp.GetRequiredService&lt;IConfiguration&gt;();
    ///     options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
    /// });
    /// </code>
    /// </example>
    public static void AddDbContextSetup<TDbContext>(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            dbContextOptions.Invoke(sp, options);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.LogTo(Console.WriteLine, (_, level) => level == LogLevel.Debug);
        });

        services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();
    }
}