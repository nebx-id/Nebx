using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Exceptions;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations.Pipeline;

/// <summary>
/// A global exception handler that processes unhandled exceptions and writes a standardized API response.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">
    /// The <see cref="ILogger{TCategoryName}"/> instance used to log unhandled exceptions
    /// and diagnostic information during request processing.
    /// </param>
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
        var (statusCode, title, detail) = exception switch
        {
            DomainException => (StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", exception.Message),
            ValidationException => (StatusCodes.Status400BadRequest, "Bad Request", "One or more validation errors occurred."),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred. Please try again later.")
        };

        // Build error details
        var errorDetail = ErrorResponse.Create(
            status: statusCode,
            title: title,
            detail: detail
        );

        errorDetail.AddInstance(httpContext.Request.Path);
        errorDetail.AddTraceId(httpContext.TraceIdentifier);

        // Handle validation-specific errors
        if (exception is ValidationException validationException)
        {
            var validationErrors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            errorDetail.AddErrors(validationErrors);
        }

        // Log the exception with context
        _logger.LogError(exception,
            "Request failed with status code {StatusCode}. Exception: {ExceptionType} - {ExceptionMessage}. " +
            "RequestId: {RequestId}, Path: {RequestPath}, Method: {RequestMethod}.",
            statusCode,
            exception.GetType().Name,
            errorDetail.Detail,
            errorDetail.TraceId,
            errorDetail.Instance,
            httpContext.Request.Method);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(errorDetail, cancellationToken);
        return true;
    }
}
