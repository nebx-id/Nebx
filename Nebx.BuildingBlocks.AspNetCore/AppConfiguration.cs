using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nebx.BuildingBlocks.AspNetCore.Data.Interceptors;
using Nebx.BuildingBlocks.AspNetCore.Exceptions;
using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Implementations;
using Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;
using Nebx.BuildingBlocks.AspNetCore.Models;
using Nebx.BuildingBlocks.AspNetCore.Services;
using Quartz;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Nebx.BuildingBlocks.AspNetCore;

public static class AppConfiguration
{
    public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.AddHostConfiguration();
        builder.WebHost.AddIWebHostConfiguration();
        builder.Services.AddServicesConfiguration();
        
        return builder;
    }

    private static IHostBuilder AddHostConfiguration(this IHostBuilder host)
    {
        host.UseDefaultServiceProvider((_, options) =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });

        return host;
    }

    private static void AddIWebHostConfiguration(this IWebHostBuilder webHost)
    {
        webHost.ConfigureKestrel(kestrel => { kestrel.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30); });
    }

    private static IServiceCollection AddServicesConfiguration(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddAntiforgery();

        services.AddEndpointsApiExplorer();
        services.AddOpenApi();

        services.AddRateLimiter(options =>
        {
            const int statusCode = StatusCodes.Status429TooManyRequests;
            options.RejectionStatusCode = statusCode;

            options.OnRejected = (context, token) =>
            {
                const string message = "You have exceeded the allowed request limit, please try again later.";
                var errorResponse = ErrorResponse.Create(message, statusCode);

                context.HttpContext.Response.StatusCode = statusCode;
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token).GetAwaiter().GetResult();
                return ValueTask.CompletedTask;
            };
        });

        // Minimal API json formatter
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
        });

        // MVC / Swagger json formatter
        services.Configure<MvcJsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
        });

        // Background Jobs
        services.AddQuartz();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddSingleton<IClock, Clock>();
        services.AddScoped<IMediator, MediatorImpl>();

        // EF Core interceptors
        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static WebApplication UseConfiguration(this WebApplication app)
    {
        app.UseExceptionHandler(_ => { });
        app.UseAntiforgery();

        if (!app.Environment.IsProduction())
        {
            app.MapOpenApi();
        }

        return app;
    }
}