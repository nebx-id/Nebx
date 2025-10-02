using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

public record Failure(string Message);

public record HttpFailure(int StatusCode, string Message) : Failure(Message);

public record NotFound(string? Message = null)
    : HttpFailure(StatusCodes.Status404NotFound, Message ?? "Resource not found");

public record Conflict(string? Message = null)
    : HttpFailure(StatusCodes.Status409Conflict, Message ?? "Resource already exists");

public record BadRequest(string? Message = null, Dictionary<string, string>? Errors = null)
    : HttpFailure(StatusCodes.Status400BadRequest, Message ?? "Bad request");

public record Unauthorized(string? Message = null)
    : HttpFailure(StatusCodes.Status401Unauthorized, Message ?? "Unauthorized");

public record Forbidden(string? Message = null)
    : HttpFailure(StatusCodes.Status403Forbidden, Message ?? "Forbidden");

public record UnprocessableEntity(string Message)
    : HttpFailure(StatusCodes.Status422UnprocessableEntity, Message);

public record InternalServerError()
    : HttpFailure(StatusCodes.Status500InternalServerError, "Internal server error");