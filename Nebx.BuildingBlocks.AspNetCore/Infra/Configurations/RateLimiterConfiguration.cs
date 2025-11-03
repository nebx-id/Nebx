using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Models.Responses;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Configurations;

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
        const string message = "You have exceeded the allowed request limit, please try again later.";

        return services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = statusCode;

            options.OnRejected = async (context, token) =>
            {
                var errorDetail = ErrorDetail.Create(
                    statusCode: statusCode,
                    message: message,
                    traceId: context.HttpContext.TraceIdentifier
                );

                var response = ApiResponse<object, ErrorDetail>.Fail(errorDetail);

                var httpResponse = context.HttpContext.Response;
                httpResponse.StatusCode = statusCode;
                httpResponse.ContentType = "application/json";
                await httpResponse.WriteAsJsonAsync(response, token).ConfigureAwait(false);
            };
        });
    }
}