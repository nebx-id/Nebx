using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

public abstract record Success(int StatusCode);

public record NoContent() : Success(StatusCodes.Status204NoContent);

public record Ok() : Success(StatusCodes.Status200OK);

public record Ok<T>(T Value) : Success(StatusCodes.Status200OK);

public record Created(string Location) : Success(StatusCodes.Status201Created);

public record Created<T>(string Location, T Value) : Success(StatusCodes.Status201Created);