using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations;

/// <summary>
///     Provides an extension method to configure consistent JSON serialization options
///     across both minimal APIs and MVC controllers.
/// </summary>
public static class JsonSerializerSetup
{
    /// <summary>
    ///     Configures JSON serialization settings for both minimal APIs (<see cref="JsonOptions" />)
    ///     and MVC controllers (<see cref="MvcJsonOptions" />).
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the configuration with.</param>
    /// <remarks>
    ///     This method sets the following options:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Enum values are serialized as strings using <see cref="JsonStringEnumConverter" />.</description>
    ///         </item>
    ///         <item>
    ///             <description>Property names are case-insensitive when deserializing.</description>
    ///         </item>
    ///         <item>
    ///             <description>Property naming uses camelCase (<see cref="JsonNamingPolicy.CamelCase" />).</description>
    ///         </item>
    ///         <item>
    ///             <description>Indented (pretty-printed) JSON output.</description>
    ///         </item>
    ///         <item>
    ///             <description>Null properties are ignored during serialization.</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal static void AddJsonSerializerSetup(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

        services.Configure<MvcJsonOptions>(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
    }
}