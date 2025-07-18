using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions.MinimalApi;

public static class ApiVersionExtension
{
    public static ApiVersionSet CreateVersionSet(this IEndpointRouteBuilder app, params ApiVersion[] apiVersions)
    {
        var orderedVersions = apiVersions
            .Distinct()
            .OrderBy(v => v.MajorVersion)
            .ThenBy(v => v.MinorVersion ?? 0);

        var versionSet = app.NewApiVersionSet();

        foreach (var apiVersion in orderedVersions) versionSet.HasApiVersion(apiVersion);
        return versionSet.ReportApiVersions().Build();
    }
}