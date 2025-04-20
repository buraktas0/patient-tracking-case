using System.Security.Claims;
using PatientTrackingApi.Data.Repositories;
using PatientTrackingApi.Domain.Entities;

namespace PatientTrackingApi.Middlewares;

public class AccessLogMiddleware
{
    private readonly RequestDelegate next;

    public AccessLogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, LogRepository logRepo)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var endpoint = context.Request.Path;

        if (!string.IsNullOrEmpty(userId))
        {
            await logRepo.LogAsync(new Log
            {
                UserId = int.Parse(userId),
                Endpoint = endpoint,
                Action = context.Request.Method,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow,
            });
        }

        await next(context);
    }
}