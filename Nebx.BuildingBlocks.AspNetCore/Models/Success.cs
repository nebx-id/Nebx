namespace Nebx.BuildingBlocks.AspNetCore.Models;

public record Success();

public record Success<T>(T Value) : Success;