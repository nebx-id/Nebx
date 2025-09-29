using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

public abstract record Failure(int StatusCode, string Message);

public record NotFound(string? Message = null)
    : Failure(StatusCodes.Status404NotFound, Message ?? "Resource not found");

public record Conflict(string? Message = null)
    : Failure(StatusCodes.Status409Conflict, Message ?? "Resource already exists");

public record BadRequest(string? Message = null, Dictionary<string, string>? Errors = null)
    : Failure(StatusCodes.Status400BadRequest, Message ?? "Invalid request");

public record Unauthorized(string? Message = null)
    : Failure(StatusCodes.Status401Unauthorized, Message ?? "Unauthorized");

public record Forbidden(string? Message = null)
    : Failure(StatusCodes.Status403Forbidden, Message ?? "Forbidden");

public record UnprocessableEntity(string Message)
    : Failure(StatusCodes.Status422UnprocessableEntity, Message);

public record InternalServerError()
    : Failure(StatusCodes.Status500InternalServerError, "Internal server error"); 