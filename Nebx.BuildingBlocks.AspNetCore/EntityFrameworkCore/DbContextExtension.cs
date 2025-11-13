using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

namespace Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for registering Entity Framework Core <see cref="DbContext"/>
/// instances with dependency injection.
/// </summary>
public static class DbContextExtension
{
    /// <summary>
    /// Registers a scoped <see cref="DbContext"/> of type <typeparamref name="TDbContext"/>
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
    /// <item><description>Enables sensitive data logging when not in production (useful for development and testing).</description></item>
    /// </list>
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

            // add interceptors
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            // add logger
            options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());
            options.EnableDetailedErrors();
        });
        return services;
    }

    /// <summary>
    ///     Determines whether any owned entities associated with the given <see cref="EntityEntry" /> have been added or
    ///     modified.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry" /> representing the entity being tracked.</param>
    /// <returns><c>true</c> if any owned entities have been added or modified; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     This method examines the references of the provided entity entry to identify any owned entities
    ///     that are in the <see cref="EntityState.Added" /> or <see cref="EntityState.Modified" /> state.
    /// </remarks>
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
    }

    /// <summary>
    ///     Retrieves entities that are tracked for auditing.
    /// </summary>
    /// <param name="context">The current <see cref="DbContext" /> instance.</param>
    /// <returns>A list of <see cref="EntityEntry" /> entries that require audit updates.</returns>
    public static List<EntityEntry<ITimeAuditable>> GetAuditEntityEntries(this DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<ITimeAuditable>()
            .Where(x =>
                x.State == EntityState.Added ||
                x.State == EntityState.Modified ||
                x.HasChangedOwnedEntities())
            .ToList();

        return entities;
    }
}
