using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions.MinimalApi;

/// <summary>
/// Provides extension methods for working with API versioning.
/// </summary>
public static class ApiVersionExtension
{
    /// <summary>
    /// Creates an <see cref="ApiVersionSet"/> with the specified API versions,
    /// ensuring they are distinct and ordered by major and minor version.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    /// <param name="apiVersions">One or more API versions to include in the version set.</param>
    /// <returns>
    /// A configured <see cref="ApiVersionSet"/> that reports supported versions.
    /// </returns>
    public static ApiVersionSet CreateVersionSet(
        this IEndpointRouteBuilder app,
        params ApiVersion[] apiVersions)
    {
        var orderedVersions = apiVersions
            .Distinct()
            .OrderBy(v => v.MajorVersion)
            .ThenBy(v => v.MinorVersion ?? 0);

        var versionSetBuilder = app.NewApiVersionSet();

        foreach (var apiVersion in orderedVersions)
        {
            versionSetBuilder.HasApiVersion(apiVersion);
        }

        return versionSetBuilder
            .ReportApiVersions()
            .Build();
    }
}