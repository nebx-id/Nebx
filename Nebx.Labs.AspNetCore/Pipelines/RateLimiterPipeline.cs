using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nebx.Labs.AspNetCore.Responses;

namespace Nebx.Labs.AspNetCore.Pipelines;

/// <summary>
/// Provides extension methods for configuring rate limiter behavior and response handling.
/// </summary>
public static class RateLimiterConfiguration
{
    /// <summary>
    /// Configures a global rate limiter rejection response for the application.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The configured <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection SetRateLimiterResponse(this IServiceCollection services)
    {
        const int statusCode = StatusCodes.Status429TooManyRequests;
        const string detail = "You have exceeded the allowed request limit, please try again later.";

        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = statusCode;

            options.OnRejected = async (context, token) =>
            {
                var errorDetail = ErrorResponse.Create(
                    status: statusCode,
                    title: "Too many requests",
                    detail: detail
                );

                errorDetail.AddInstance(context.HttpContext.Request.Path);
                errorDetail.AddTraceId(context.HttpContext.TraceIdentifier);

                var httpResponse = context.HttpContext.Response;
                httpResponse.StatusCode = statusCode;
                httpResponse.ContentType = "application/json";
                await httpResponse.WriteAsJsonAsync(errorDetail, token).ConfigureAwait(false);
            };
        });
    }
}
