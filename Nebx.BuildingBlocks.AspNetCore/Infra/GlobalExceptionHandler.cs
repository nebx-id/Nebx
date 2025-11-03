using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Models.Responses;
using Nebx.BuildingBlocks.AspNetCore.Core.Exceptions;

namespace Nebx.BuildingBlocks.AspNetCore.Infra;

/// <summary>
/// A global exception handler that processes unhandled exceptions and writes a standardized API response.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Map exception type to message and status code
        var (message, statusCode) = exception switch
        {
            DomainException => (exception.Message, StatusCodes.Status422UnprocessableEntity),
            ValidationException => ("One or more validation errors occurred.", StatusCodes.Status400BadRequest),
            BadHttpRequestException => (exception.Message, StatusCodes.Status400BadRequest),
            _ => ("An unexpected error occurred. Please try again later.", StatusCodes.Status500InternalServerError)
        };

        // Build error details
        var errorDetail = ErrorDetail.Create(
            statusCode: statusCode,
            message: message,
            traceId: httpContext.TraceIdentifier
        );

        // Handle validation-specific errors
        if (exception is ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                .ToDictionary(e => e.PropertyName, e => e.ErrorMessage);

            errorDetail.AddErrors(validationErrors);
        }

        // Log the exception with context
        _logger.LogError(exception,
            "Request failed with status code {StatusCode}. Exception: {ExceptionType} - {ExceptionMessage}. " +
            "RequestId: {RequestId}, Path: {RequestPath}, Method: {RequestMethod}.",
            statusCode,
            exception.GetType().Name,
            exception.Message,
            httpContext.TraceIdentifier,
            httpContext.Request.Path,
            httpContext.Request.Method);

        // Create a standardized API error response
        var apiResponse = ApiResponse<object, ErrorDetail>.Fail(errorDetail);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(apiResponse, cancellationToken);
        return true;
    }
}