using FiapCloudGames.Users.Observability.Abstractions;
using FiapCloudGames.Users.Observability.Providers.NewRelic;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Users.Observability.Configurations;

public static class ObservabilityConfig
{
    public static IServiceCollection AddObservabilityConfig(this IServiceCollection services)
    {
        services.AddScoped<IObservabilityService, NewRelicObservabilityService>();
        services.AddScoped(typeof(NewRelicConsumeFilter<>));
        services.AddScoped(typeof(NewRelicPublishFilter<>));

        return services;
    }
}
