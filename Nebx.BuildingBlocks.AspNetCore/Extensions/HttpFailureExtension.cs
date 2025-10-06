using Microsoft.AspNetCore.Http;
using Nebx.BuildingBlocks.AspNetCore.Models;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions;

/// <summary>
/// Provides extension methods for converting <see cref="HttpFailure"/> instances 
/// into standardized error responses or Minimal API results.
/// </summary>
public static class HttpFailureExtension
{
    /// <summary>
    /// Converts an <see cref="HttpFailure"/> instance into an <see cref="ErrorResponse"/>.
    /// </summary>
    /// <param name="failure">The HTTP failure to convert.</param>
    /// <returns>
    /// An <see cref="ErrorResponse"/> representing the failure, 
    /// including status code, message, and any validation errors if applicable.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the <paramref name="failure"/> type is not mapped to an <see cref="ErrorResponse"/>.
    /// </exception>
    /// <remarks>
    /// This method supports the following <see cref="HttpFailure"/> types:
    /// <list type="bullet">
    /// <item><description><see cref="BadRequest"/></description></item>
    /// <item><description><see cref="NotFound"/></description></item>
    /// <item><description><see cref="Conflict"/></description></item>
    /// <item><description><see cref="Unauthorized"/></description></item>
    /// <item><description><see cref="Forbidden"/></description></item>
    /// <item><description><see cref="UnprocessableEntity"/></description></item>
    /// <item><description><see cref="InternalServerError"/></description></item>
    /// </list>
    /// 
    /// Additionally, if the failure is a <see cref="BadRequest"/> that contains 
    /// validation errors, those will be included in the <see cref="ErrorResponse"/>.
    /// </remarks>
    private static ErrorResponse ToErrorResponse(this Failure failure)
    {
        var errorResponse = failure switch
        {
            BadRequest f => ErrorResponse.Create(f.Message, f.StatusCode),
            NotFound f => ErrorResponse.Create(f.Message, f.StatusCode),
            Conflict f => ErrorResponse.Create(f.Message, f.StatusCode),
            Unauthorized f => ErrorResponse.Create(f.Message, f.StatusCode),
            Forbidden f => ErrorResponse.Create(f.Message, f.StatusCode),
            UnprocessableEntity f => ErrorResponse.Create(f.Message, f.StatusCode),
            InternalServerError f => ErrorResponse.Create(f.Message, f.StatusCode),
            _ => throw new NotSupportedException(
                $"Failure type '{failure.GetType().Name}' is not mapped to an {nameof(HttpFailure)} type."),
        };

        if (failure is BadRequest badRequest)
            errorResponse.AddErrors(badRequest.Errors);

        return errorResponse;
    }

    /// <summary>
    /// Converts an <see cref="HttpFailure"/> into an <see cref="IResult"/> 
    /// suitable for returning from a Minimal API endpoint.
    /// </summary>
    /// <param name="failure">The HTTP failure to convert.</param>
    /// <returns>
    /// An <see cref="IResult"/> that can be returned from a Minimal API endpoint,
    /// representing the error as a properly formatted response.
    /// </returns>
    public static IResult ToMinimalApiResult(this HttpFailure failure)
        => failure.ToErrorResponse().ToMinimalApiResult();
}