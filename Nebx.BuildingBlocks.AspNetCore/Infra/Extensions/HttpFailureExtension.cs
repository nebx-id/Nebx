using Microsoft.AspNetCore.Http;
using Nebx.BuildingBlocks.AspNetCore.Core.Failures;
using Nebx.BuildingBlocks.AspNetCore.Core.Models.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Extensions;

/// <summary>
/// Provides extension methods for converting <see cref="HttpFailure"/> instances 
/// into standardized error responses or Minimal API results.
/// </summary>
public static class HttpFailureExtension
{
    /// <summary>
    /// Converts an <see cref="HttpFailure"/> instance into an <see cref="ErrorDetail"/> object.
    /// </summary>
    /// <param name="failure">The <see cref="HttpFailure"/> to convert.</param>
    /// <returns>
    /// An <see cref="ErrorDetail"/> representing the specified failure, 
    /// including the HTTP status code, message, and any validation errors if available.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the specified <paramref name="failure"/> type is not supported or 
    /// cannot be mapped to an <see cref="ErrorDetail"/>.
    /// </exception>
    /// <remarks>
    /// This method supports the following <see cref="HttpFailure"/> types:
    /// <list type="bullet">
    /// <item><description><see cref="BadRequestFailure"/></description></item>
    /// <item><description><see cref="NotFoundFailure"/></description></item>
    /// <item><description><see cref="ConflictFailure"/></description></item>
    /// <item><description><see cref="UnauthorizedFailure"/></description></item>
    /// <item><description><see cref="ForbiddenFailure"/></description></item>
    /// <item><description><see cref="UnprocessableEntityFailure"/></description></item>
    /// <item><description><see cref="InternalServerErrorFailure"/></description></item>
    /// </list>
    /// If the failure is a <see cref="BadRequestFailure"/> or 
    /// <see cref="UnprocessableEntityFailure"/> containing validation errors, 
    /// those will be added to the <see cref="ErrorDetail"/> instance.
    /// </remarks>
    private static ErrorDetail ToErrorDetail(this HttpFailure failure)
    {
        var errorDetail = failure switch
        {
            BadRequestFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            NotFoundFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            ConflictFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            UnauthorizedFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            ForbiddenFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            UnprocessableEntityFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            InternalServerErrorFailure f => ErrorDetail.Create(f.StatusCode, f.Message),
            _ => throw new NotSupportedException(
                $"Failure type '{failure.GetType().Name}' is not mapped to an {nameof(HttpFailure)} type."),
        };

        if (failure is BadRequestFailure badRequest && badRequest.Errors is not null)
            errorDetail.AddErrors(badRequest.Errors);

        return errorDetail;
    }

    /// <summary>
    /// Converts an <see cref="HttpFailure"/> into a Minimal API <see cref="IResult"/>,
    /// producing a JSON error response with the appropriate HTTP status code.
    /// </summary>
    /// <param name="failure">The <see cref="HttpFailure"/> instance to convert.</param>
    /// <returns>
    /// An <see cref="IResult"/> suitable for returning from a Minimal API endpoint.
    /// The result includes the failure's status code, message, and any associated validation errors.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the given <paramref name="failure"/> type is not supported.
    /// </exception>
    public static IResult ToMinimalApiResult(this HttpFailure failure)
        => Results.Json(failure.ToErrorDetail(), statusCode: failure.StatusCode);
}