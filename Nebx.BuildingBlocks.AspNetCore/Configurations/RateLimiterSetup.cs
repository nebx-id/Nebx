using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nebx.Shared.Dtos;

namespace Nebx.BuildingBlocks.AspNetCore.Configurations;

public static class RateLimiterSetup
{
    /// <summary>
    ///     Adds rate limiting services and configures a global rejection handler that returns a standardized JSON error
    ///     response
    ///     when the request limit is exceeded.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to register the rate limiter with.</param>
    /// <remarks>
    ///     This method:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Sets the HTTP status code to 429 (Too Many Requests) on rejection.</description>
    ///         </item>
    ///         <item>
    ///             <description>Formats the error response as a JSON object of type <c>ErrorDto</c>.</description>
    ///         </item>
    ///         <item>
    ///             <description>Includes request path and trace identifier in the error payload.</description>
    ///         </item>
    ///     </list>
    ///     To apply specific rate-limiting policies, define them separately using middleware or per-endpoint configuration.
    /// </remarks>
    internal static void AddRateLimiterSetup(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            const int statusCode = StatusCodes.Status429TooManyRequests;
            options.RejectionStatusCode = statusCode;

            options.OnRejected = (context, token) =>
            {
                const string message = "You have exceeded the allowed request limit, please try again later.";
                var errorResponse = ErrorDto.Create(message, statusCode);
                errorResponse.SetPath(context.HttpContext.Request.Path);
                errorResponse.SetRequestId(context.HttpContext.TraceIdentifier);

                context.HttpContext.Response.StatusCode = statusCode;
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.WriteAsJsonAsync(errorResponse, token).GetAwaiter().GetResult();
                return ValueTask.CompletedTask;
            };
        });
    }
}