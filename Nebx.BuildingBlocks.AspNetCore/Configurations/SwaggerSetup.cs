using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations;

public static class SwaggerSetup
{
    public const string VersioningHeaderName = "X-Api-Version";

    /// <summary>
    /// Registers and configures the Swagger (OpenAPI) services for the application,
    /// including versioning support and optional security definitions.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Swagger services will be added.
    /// </param>
    /// <param name="schemes">
    /// An optional array of <see cref="OpenApiSecurityScheme"/> definitions
    /// to be included in the Swagger documentation.
    /// </param>
    /// <remarks>
    /// This method:
    /// <list type="bullet">
    /// <item>Registers the Swagger generator using <c>AddSwaggerGen</c>.</item>
    /// <item>Adds the provided security schemes and requirements to the Swagger configuration.</item>
    /// </list>
    ///
    /// <para>
    /// <b>Example usage:</b>
    /// <code>
    /// var authScheme = new OpenApiSecurityScheme
    /// {
    ///     Name = "Authorization",
    ///     Type = SecuritySchemeType.Http,
    ///     Scheme = "bearer",
    ///     BearerFormat = "JWT",
    ///     In = ParameterLocation.Header,
    ///     Description = "JWT Authorization header Scheme",
    ///     Reference = new OpenApiReference
    ///     {
    ///         Type = ReferenceType.SecurityScheme,
    ///         Id = "unique name of the scheme"
    ///     }
    /// };
    ///
    /// services.AddSwaggerSetup(authScheme);
    /// </code>
    /// </para>
    /// </remarks>
    internal static void AddSwaggerSetup(this IServiceCollection services, params OpenApiSecurityScheme[] schemes)
    {
        services.AddSwaggerGen(options =>
        {
            var requirement = new OpenApiSecurityRequirement();

            foreach (var scheme in schemes)
            {
                options.AddSecurityDefinition(scheme.Reference.Id, scheme);
                requirement.Add(new OpenApiSecurityScheme { Reference = scheme.Reference }, Array.Empty<string>());
            }

            options.AddSecurityRequirement(requirement);
        });
    }

    /// <summary>
    /// Registers API versioning support for Swagger (OpenAPI) generation.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the versioning configuration will be added.
    /// </param>
    /// <remarks>
    /// This method registers the <see cref="VersioningConfigureOptions"/> to apply API versioning
    /// to the Swagger documentation.<br/>
    /// <para>
    /// <b>⚠️ Warning:</b> This method should be called <b>only once</b> during application startup.<br/>
    /// Registering it multiple times may lead to duplicate or conflicting Swagger version configurations.
    /// </para>
    /// </remarks>
    public static void AddSwaggerVersioning(this IServiceCollection services)
    {
        services.ConfigureOptions<VersioningConfigureOptions>();
    }

    /// <summary>
    /// Configures and enables the Swagger middleware and Swagger UI for the application,
    /// including support for multiple API versions.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance to configure with Swagger middleware and UI.
    /// </param>
    /// <remarks>
    /// <para>
    /// This method:
    /// </para>
    /// <list type="bullet">
    /// <item>Registers the Swagger middleware to serve the generated OpenAPI JSON documents.</item>
    /// <item>Configures the Swagger UI to expose a separate Swagger endpoint for each API version
    /// discovered via the application's versioning provider.</item>
    /// <item>Logs the list of configured API version groups for debugging purposes.</item>
    /// </list>
    /// <para>
    /// <b>⚠️ WARNING:</b> This method must be called <b>after all endpoints are mapped or registered.</b>
    /// Failing to do so may result in incomplete or missing
    /// endpoint documentation in the generated Swagger UI.
    /// </para>
    /// </remarks>
    /// <example>
    /// Example usage in <c>Program.cs</c>:
    /// <code>
    /// app.UseApiBuildingBlocksSetup();
    /// app.UseSwaggerSetup();
    /// </code>
    /// </example>
    internal static void UseSwaggerSetup(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.Logger.LogDebug("Skip adding Swagger in production");
            return;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var descriptions = app.DescribeApiVersions();

            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                options.SwaggerEndpoint(url, description.GroupName);
            }

            app.Logger.LogDebug("API version {VersionGroups} added to swagger endpoint",
                string.Join(", ", descriptions.Select(x => x.GroupName)));
        });
    }

    internal class VersioningConfigureOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly ILogger<VersioningConfigureOptions> _logger;
        private readonly IApiVersionDescriptionProvider _provider;

        private readonly string? _customDescription;

        public VersioningConfigureOptions(
            IApiVersionDescriptionProvider provider,
            ILogger<VersioningConfigureOptions> logger,
            IConfiguration configuration)
        {
            _provider = provider;
            _logger = logger;

            _customDescription = configuration["Swagger:Description"];
        }

        public void Configure(SwaggerGenOptions options)
        {
            var descriptions = _provider.ApiVersionDescriptions;

            foreach (var description in descriptions)
            {
                var openApiInfo = CreateOpenApiInfo(description);
                options.SwaggerDoc(description.GroupName, openApiInfo);
            }

            _logger.LogDebug("API version {VersionGroups} added to swagger doc",
                string.Join(", ", descriptions.Select(x => x.GroupName)));

            options.OperationFilter<SwaggerAutoApiVersionHeader>();
        }

        private OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"API {description.GroupName.ToUpperInvariant()} Documentation",
                Description = string.IsNullOrWhiteSpace(_customDescription)
                    ? "This API provides access to various services and operations. Use the available endpoints to integrate with system functionality."
                    : _customDescription,
                Version = description.ApiVersion.ToString()
            };

            return openApiInfo;
        }
    }

    private class SwaggerAutoApiVersionHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersion = context.ApiDescription.GroupName?.ToUpperInvariant().Replace("V", "");
            var headerParams = operation.Parameters
                .Where(p => p.In == ParameterLocation.Header && p.Name == VersioningHeaderName)
                .ToList();

            if (string.IsNullOrWhiteSpace(apiVersion) || headerParams.Count < 1) return;

            foreach (var param in headerParams)
                param.Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(apiVersion)
                };
        }
    }
}