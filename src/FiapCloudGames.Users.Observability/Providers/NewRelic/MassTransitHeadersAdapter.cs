using System.Diagnostics;
using MassTransit;

namespace FiapCloudGames.Users.Observability.Providers.NewRelic;

public static class MassTransitHeadersAdapter
{
    public static void SetTraceHeaders(SendHeaders headers)
    {
        var activity = Activity.Current;

        if (activity is null)
        {
            return;
        }

        headers.Set("traceparent", activity.Id);

        if (!string.IsNullOrWhiteSpace(activity.TraceStateString))
        {
            headers.Set("tracestate", activity.TraceStateString);
        }

        headers.Set("correlation_id", activity.TraceId.ToString());
    }
}
