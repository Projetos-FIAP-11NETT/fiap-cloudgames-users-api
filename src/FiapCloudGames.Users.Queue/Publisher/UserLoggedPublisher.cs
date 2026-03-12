using FiapCloudGames.Queue.Configurations.Rabbitmq;
using FiapCloudGames.Queue.Contracts;
using FiapCloudGames.Users.Shared.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Queue.Publisher
{
    public class UserLoggedPublisher(IRabbitmqPublish bus, ILogger<UserLoggedPublisher> logger, ICorrelationIdAccessor correlation) : IUserLoggedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint = bus;
        private readonly ILogger<UserLoggedPublisher> _logger = logger;

        public  Task PublishAsync(string idToken, string refreshToken, int expiresIn, string email)
        {
            _logger.LogDebug(
                "Publishing ILoggedPublisher to RabbitMQ: IdToken={idToken}, RefreshToken={refreshToken}, ExpiresIn={expiresIn}, Email={email}",
                idToken, refreshToken, expiresIn, email);

            return _publishEndpoint.Publish<IUserLogged>(new 
            {
                IdToken = idToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn,
                Email = email
            }, context => {
                context.CorrelationId = correlation.CorrelationId;
            });
        }
    }
}
