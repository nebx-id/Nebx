using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;

/// <summary>
/// Provides extension methods for configuring JSON serialization settings.
/// </summary>
public static class JsonConfiguration
{
    /// <summary>
    /// Configures JSON serialization options for Minimal APIs, MVC, and Swagger.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddJsonConfiguration(this IServiceCollection services)
    {
        // Minimal API JSON formatter
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
        });

        // MVC / Swagger JSON formatter
        services.Configure<MvcJsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });

        return services;
    }
}