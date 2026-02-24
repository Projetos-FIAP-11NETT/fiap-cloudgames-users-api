using FiapCloudGames.Users.Api.Constants;
using Serilog.Context;
using System.Diagnostics;

namespace FiapCloudGames.Users.Api.Middlewares;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public sealed class RequestResponseLoggingMiddleware(
RequestDelegate next,
ILogger<RequestResponseLoggingMiddleware> logger)
{
    private const string MessageRequest = "CorrelationId: {CorrelationId} | Inicio da Requisicao {Method} {Path}";
    private const string MessageResponse = "CorrelationId: {CorrelationId} | Final da Requisicao {Method} {Path} | StatusCode: {StatusCode} {Elapsed}ms";

    private readonly RequestDelegate _next = next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        // CorrelationId
        var correlationId = GetOrCreateCorrelationId(context);

        context.Items[ContextItems.CorrelationId] = correlationId;
        context.Items["X-Correlation-Id"] = correlationId;

        context.Response.Headers[HeaderNames.CorrelationId] = correlationId;

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["RequestPath"] = context.Request.Path
        }))
        {
            var stopwatch = Stopwatch.StartNew();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                // New Relic
                var agent = NewRelic.Api.Agent.NewRelic.GetAgent();
                var transaction = agent.CurrentTransaction;

                transaction.AddCustomAttribute("CorrelationId", correlationId);
                _logger.LogInformation(MessageRequest, correlationId, context.Request.Method, context.Request.Path);

                await _next(context);
            }
            stopwatch.Stop();

            _logger.LogInformation(MessageResponse, correlationId, context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderNames.CorrelationId, out var correlationId)
           && !string.IsNullOrWhiteSpace(correlationId))
        {
            return correlationId!;
        }

        var newCorrelationId = Guid.NewGuid().ToString();
        context.Request.Headers[HeaderNames.CorrelationId] = newCorrelationId;

        return newCorrelationId;
    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}
