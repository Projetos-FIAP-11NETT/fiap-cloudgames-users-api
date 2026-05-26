using System.Diagnostics;
using FiapCloudGames.Users.Observability.Abstractions;
using MassTransit;

namespace FiapCloudGames.Users.Observability.Providers.NewRelic;

public class NewRelicConsumeFilter<T>(IObservabilityService obs) : IFilter<ConsumeContext<T>> where T : class
{
    private static readonly ActivitySource ActivitySource =
        new("FiapCloudGames.MassTransit");

    public async Task Send(
        ConsumeContext<T> context,
        IPipe<ConsumeContext<T>> next)
    {
        var traceParent = context.Headers.Get<string>("traceparent");
        var traceState = context.Headers.Get<string>("tracestate");

        ActivityContext activityContext = default;

        if (!string.IsNullOrWhiteSpace(traceParent))
        {
            ActivityContext.TryParse(
                traceParent,
                traceState,
                out activityContext);
        }

        using var activity = ActivitySource.StartActivity(
            $"Consume {typeof(T).Name}",
            ActivityKind.Consumer,
            activityContext);

        var queue = context.ReceiveContext.InputAddress?.AbsolutePath ?? "unknown";
        var message = typeof(T).Name;

        obs.SetTransactionName("MassTransit", $"/{queue}/{message}");

        obs.AddCustomAttribute("MessageType", message);

        obs.AddCustomAttribute(
            "CorrelationId",
            context.CorrelationId?.ToString() ?? string.Empty);

        try
        {
            await next.Send(context);
        }
        catch (Exception ex)
        {
            obs.NoticeError(ex);
            throw;
        }
    }

    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("newRelicConsumeFilter");
    }
}