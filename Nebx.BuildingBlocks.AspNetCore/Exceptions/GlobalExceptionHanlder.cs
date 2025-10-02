using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Models;

namespace Nebx.BuildingBlocks.AspNetCore.Exceptions;

/// <summary>
/// A global exception handler that processes unhandled exceptions and writes a standardized error response.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var message = exception switch
        {
            DomainException => exception.Message,
            BadHttpRequestException => exception.Message,
            ValidationException => "One or more validation errors occurred.",
            _ => "An unexpected error occurred. Please try again later."
        };

        var statusCode = exception switch
        {
            DomainException => StatusCodes.Status422UnprocessableEntity,
            ValidationException or BadHttpRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorResponse = ErrorResponse.Create(message, statusCode);

        switch (exception)
        {
            case ValidationException ex:
            {
                var errors = ex.Errors
                    .ToDictionary(c => c.PropertyName, c => c.ErrorMessage);

                errorResponse.AddErrors(errors);
                break;
            }
        }

        _logger.LogError(exception,
            "Request failed with status code {StatusCode}. Exception: {ExceptionType} - {ExceptionMessage}. RequestId: {RequestId}, Path: {RequestPath}, Method: {RequestMethod};",
            statusCode, exception.GetType().Name, exception.Message, httpContext.TraceIdentifier,
            httpContext.Request.Method, httpContext.Request.Path);

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }
}
