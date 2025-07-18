using Microsoft.AspNetCore.Routing;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Endpoint;

public interface IEndpoint
{
    void AddRoutes(IEndpointRouteBuilder app);
}