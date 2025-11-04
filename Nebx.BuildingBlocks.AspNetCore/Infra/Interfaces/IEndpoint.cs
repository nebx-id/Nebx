using Microsoft.AspNetCore.Routing;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces;

/// <summary>
/// Defines a contract for automatically registering HTTP endpoints.
/// </summary>
/// <remarks>
/// This interface is intended to be used with assembly scanning to discover and register endpoints at application startup.
/// </remarks>
public interface IEndpoint
{
    /// <summary>
    /// Adds routes to the application's endpoint route builder.
    /// </summary>
    /// <param name="app">The application's endpoint route builder.</param>
    void AddRoutes(IEndpointRouteBuilder app);
}