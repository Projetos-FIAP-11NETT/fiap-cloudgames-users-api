using Amazon.SimpleNotificationService;
using Amazon.SQS;
using FiapCloudGames.Queue.Configurations.MassTransit;
using FiapCloudGames.Users.Observability.Providers.NewRelic;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FiapCloudGames.Queue.Configurations.Sqs;

public static class SqsStartup
{
    public static void RegisterSqsStartup(this IServiceCollection services)
    {
        services.AddMassTransit<ISqsPublish>(x =>
        {
            x.SetEndpointNameFormatter(
                new KebabCaseEndpointNameFormatter("users", false));

            x.UsingAmazonSqs((context, cfg) =>
            {
                var sqsSettings = context.GetRequiredService<IOptions<SqsSettings>>().Value;
                var massTransitSettings = context.GetRequiredService<IOptions<MassTransitSettings>>().Value;

                cfg.Host(sqsSettings.Region, h =>
                {
                    h.AccessKey(sqsSettings.AccessKey);
                    h.SecretKey(sqsSettings.SecretKey);

                    if (!string.IsNullOrWhiteSpace(sqsSettings.ServiceUrl))
                    {
                        h.Config(new AmazonSQSConfig
                        {
                            ServiceURL = sqsSettings.ServiceUrl,
                            AuthenticationRegion = sqsSettings.Region
                        });

                        h.Config(new AmazonSimpleNotificationServiceConfig
                        {
                            ServiceURL = sqsSettings.ServiceUrl,
                            AuthenticationRegion = sqsSettings.Region
                        });
                    }
                });

                cfg.UseMessageRetry(r => r.Interval(massTransitSettings.RetryCount, massTransitSettings.Interval));

                cfg.UseConsumeFilter(typeof(NewRelicConsumeFilter<>), context);

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}