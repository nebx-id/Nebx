using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Nebx.Labs.EntityFrameworkCore.Extensions;

/// <summary>
/// Provides extension methods for registering <see cref="DbContext"/> instances
/// with dependency injection.
/// </summary>
public static class DbContextConfiguration
{
    /// <summary>
    /// Registers a scoped <see cref="DbContext"/> of type <typeparamref name="TDbContext"/>
    /// and applies the specified configuration options.
    /// </summary>
    /// <typeparam name="TDbContext">
    /// The concrete <see cref="DbContext"/> type to register.
    /// </typeparam>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> into which the <typeparamref name="TDbContext"/> is registered.
    /// </param>
    /// <param name="optionBuilder">
    /// A delegate used to configure the <see cref="DbContextOptionsBuilder"/>
    /// for the <typeparamref name="TDbContext"/>, typically applying the database provider and settings.
    /// </param>
    /// <returns>
    /// The same <see cref="IServiceCollection"/> instance to allow method chaining.
    /// </returns>
    /// <remarks>
    /// <para>
    /// This method registers the <typeparamref name="TDbContext"/> using
    /// <see cref="EntityFrameworkServiceCollectionExtensions.AddDbContext{TContext}(IServiceCollection, Action{IServiceProvider, DbContextOptionsBuilder}, ServiceLifetime, ServiceLifetime)"/>.
    /// </para>
    ///
    /// <para>Additional behavior applied during configuration:</para>
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///         Resolves and attaches all registered <see cref="ISaveChangesInterceptor"/> instances.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///         Enables detailed error messages through <see cref="DbContextOptionsBuilder.EnableDetailedErrors"/>.
    ///         </description>
    ///     </item>
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
        Action<IServiceProvider, DbContextOptionsBuilder> optionBuilder)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((sp, options) =>
        {
            optionBuilder.Invoke(sp, options);

            // add interceptors
            var interceptors = sp.GetServices<ISaveChangesInterceptor>().ToList();
            if (interceptors.Count != 0)
                options.AddInterceptors(interceptors);

            // add logger
            options.EnableDetailedErrors();
        });

        return services;
    }
}
