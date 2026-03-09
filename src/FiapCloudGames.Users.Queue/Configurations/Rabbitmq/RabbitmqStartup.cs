using FiapCloudGames.Queue.Configurations.MassTransit;
using FiapCloudGames.Users.Observability.Providers.NewRelic;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FiapCloudGames.Queue.Configurations.Rabbitmq;

public static class RabbitmqStartup
{
    public static void RegisterRabbitmqStartup(this IServiceCollection services)
    {
        services.AddMassTransit<IRabbitmqPublish>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                var massTransitSettings = context.GetRequiredService<IOptions<MassTransitSettings>>().Value;
                
                cfg.UseMessageRetry(r => r.Interval(massTransitSettings.RetryCount, massTransitSettings.Interval));

                var rabbitmqSettings = context.GetRequiredService<IOptions<RabbitmqSettings>>().Value;

                cfg.Host(new Uri($"rabbitmq://{rabbitmqSettings.Address}:{rabbitmqSettings.Port}{rabbitmqSettings.VirtualHost}"), h =>
                {
                    h.Username(rabbitmqSettings.Username);
                    h.Password(rabbitmqSettings.Password);
                });

                cfg.UseConsumeFilter(typeof(NewRelicConsumeFilter<>), context);

                cfg.ConfigureEndpoints(context);

            });
        });
    }
}