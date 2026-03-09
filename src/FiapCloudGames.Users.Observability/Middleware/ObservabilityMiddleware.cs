using FiapCloudGames.Users.Observability.Abstractions;
using Microsoft.AspNetCore.Http;

namespace FiapCloudGames.Users.Observability.Middleware;

public class ObservabilityMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IObservabilityService obs)
    {
        obs.AddCustomAttribute("TraceId", context.TraceIdentifier);

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            obs.NoticeError(ex);
            throw;
        }
    }
}