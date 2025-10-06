using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Models;

using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

/// <summary>
/// Represents a general failure result with a descriptive message.
/// </summary>
/// <remarks>
/// This is the base record for representing failures across different layers (e.g., domain, application, or HTTP).
/// It provides a simple message indicating the cause or description of the failure.
/// </remarks>
/// <param name="Message">A descriptive message explaining the cause of the failure.</param>
public record Failure(string Message);

/// <summary>
/// Represents a failure that corresponds to a specific HTTP status code.
/// </summary>
/// <remarks>
/// Extends <see cref="Failure"/> by including an HTTP status code,
/// enabling a unified structure for reporting API or web service errors.
/// </remarks>
/// <param name="StatusCode">The HTTP status code associated with the failure.</param>
/// <param name="Message">A descriptive message providing context for the error.</param>
public record HttpFailure(int StatusCode, string Message) : Failure(Message);

/// <summary>
/// Represents a failure indicating that the requested resource was not found (HTTP 404).
/// </summary>
/// <remarks>
/// Commonly used when a resource (e.g., user, file, or entity) does not exist in the system.
/// </remarks>
/// <param name="Message">
/// An optional custom message. Defaults to "Resource not found" if not provided.
/// </param>
public record NotFound(string? Message = null)
    : HttpFailure(StatusCodes.Status404NotFound, Message ?? "Resource not found");

/// <summary>
/// Represents a failure indicating a conflict in the request (HTTP 409).
/// </summary>
/// <remarks>
/// Typically used when a resource already exists or a duplicate operation is attempted.
/// </remarks>
/// <param name="Message">
/// An optional custom message. Defaults to "Resource already exists" if not provided.
/// </param>
public record Conflict(string? Message = null)
    : HttpFailure(StatusCodes.Status409Conflict, Message ?? "Resource already exists");

/// <summary>
/// Represents a failure caused by a bad request (HTTP 400).
/// </summary>
/// <remarks>
/// This type is often used for validation failures or malformed client requests.
/// It optionally includes a collection of specific validation errors.
/// </remarks>
/// <param name="Message">
/// An optional custom message. Defaults to "Bad request" if not provided.
/// </param>
/// <param name="Errors">
/// An optional dictionary containing validation errors where each key is a field name
/// and each value is the corresponding error message.
/// </param>
public record BadRequest(string? Message = null, Dictionary<string, string>? Errors = null)
    : HttpFailure(StatusCodes.Status400BadRequest, Message ?? "Bad request");

/// <summary>
/// Represents a failure due to unauthorized access (HTTP 401).
/// </summary>
/// <remarks>
/// This typically occurs when authentication credentials are missing or invalid.
/// </remarks>
/// <param name="Message">
/// An optional custom message. Defaults to "Unauthorized" if not provided.
/// </param>
public record Unauthorized(string? Message = null)
    : HttpFailure(StatusCodes.Status401Unauthorized, Message ?? "Unauthorized");

/// <summary>
/// Represents a failure due to forbidden access (HTTP 403).
/// </summary>
/// <remarks>
/// This is used when a user is authenticated but lacks the necessary permissions for the requested operation.
/// </remarks>
/// <param name="Message">
/// An optional custom message. Defaults to "Forbidden" if not provided.
/// </param>
public record Forbidden(string? Message = null)
    : HttpFailure(StatusCodes.Status403Forbidden, Message ?? "Forbidden");

/// <summary>
/// Represents a failure due to a request that could not be processed (HTTP 422).
/// </summary>
/// <remarks>
/// This is commonly used for semantic validation errors — the request is syntactically correct
/// but cannot be processed in its current state.
/// </remarks>
/// <param name="Message">
/// A descriptive message explaining why the request could not be processed.
/// </param>
public record UnprocessableEntity(string Message)
    : HttpFailure(StatusCodes.Status422UnprocessableEntity, Message);

/// <summary>
/// Represents an internal server error (HTTP 500).
/// </summary>
/// <remarks>
/// This is used when an unexpected or unhandled error occurs on the server side.
/// </remarks>
public record InternalServerError()
    : HttpFailure(StatusCodes.Status500InternalServerError, "Internal server error");
