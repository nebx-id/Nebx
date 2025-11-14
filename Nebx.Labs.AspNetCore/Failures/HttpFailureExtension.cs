using Microsoft.AspNetCore.Http;
using Nebx.Labs.AspNetCore.Responses;

namespace Nebx.Labs.AspNetCore.Failures;

/// <summary>
/// Provides extension methods for converting <see cref="HttpFailure"/> instances
/// into standardized error responses or Minimal API results.
/// </summary>
public static class HttpFailureExtension
{
    /// <summary>
    /// Converts an <see cref="HttpFailure"/> into an <see cref="ErrorResponse"/>.
    /// </summary>
    /// <param name="failure">The HTTP failure to convert.</param>
    /// <returns>
    /// An <see cref="ErrorResponse"/> containing the failure’s status, title, detail,
    /// and validation errors if available.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the <paramref name="failure"/> type cannot be mapped to an <see cref="ErrorResponse"/>.
    /// </exception>
    /// <remarks>
    /// Supports these failure types:
    /// <list type="bullet">
    /// <item><description><see cref="BadRequestFailure"/></description></item>
    /// <item><description><see cref="UnauthorizedFailure"/></description></item>
    /// <item><description><see cref="ForbiddenFailure"/></description></item>
    /// <item><description><see cref="NotFoundFailure"/></description></item>
    /// <item><description><see cref="ConflictFailure"/></description></item>
    /// <item><description><see cref="UnprocessableEntityFailure"/></description></item>
    /// <item><description><see cref="InternalServerErrorFailure"/></description></item>
    /// </list>
    /// Validation errors from <see cref="BadRequestFailure"/> or <see cref="UnprocessableEntityFailure"/>
    /// are added automatically to the response.
    /// </remarks>
    private static ErrorResponse ToErrorDetail(this HttpFailure failure)
    {
        var errorDetail = failure switch
        {
            BadRequestFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            NotFoundFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            ConflictFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            UnauthorizedFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            ForbiddenFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            UnprocessableEntityFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            InternalServerErrorFailure f => ErrorResponse.Create(f.StatusCode, f.Title, f.Detail),
            _ => throw new NotSupportedException(
                $"Failure type '{failure.GetType().Name}' is not mapped to an {nameof(HttpFailure)} type."),
        };

        if (failure is BadRequestFailure badRequest && badRequest.Errors is not null)
            errorDetail.AddErrors(badRequest.Errors);

        return errorDetail;
    }

    /// <summary>
    /// Converts an <see cref="HttpFailure"/> into a Minimal API <see cref="IResult"/>.
    /// </summary>
    /// <param name="failure">The HTTP failure to convert.</param>
    /// <param name="context">Optional <see cref="HttpContext"/> for instance and trace data.</param>
    /// <returns>
    /// An <see cref="IResult"/> producing a JSON error response with the appropriate
    /// status code and error details.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Thrown when the <paramref name="failure"/> type cannot be converted.
    /// </exception>
    public static IResult ToMinimalApiResult(this HttpFailure failure, HttpContext? context = null)
    {
        var response = failure.ToErrorDetail();

        if (context is not null)
        {
            response.AddInstance(context.Request.Path);
            response.AddTraceId(context.TraceIdentifier);
        }

        return Results.Json(response, statusCode: failure.StatusCode);
    }
}
