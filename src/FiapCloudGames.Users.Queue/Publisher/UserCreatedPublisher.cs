using FiapCloudGames.Queue.Configurations.Sqs;
using FiapCloudGames.Queue.Contracts;
using FiapCloudGames.Users.Shared.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Queue.Publisher
{
    public class UserCreatedPublisher(
            ISqsPublish bus, 
            ILogger<UserCreatedPublisher> logger, ICorrelationIdAccessor correlation
        ) 
        : IUserCreatedPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint = bus;
        private readonly ILogger<UserCreatedPublisher> _logger = logger;
        public Task PublishAsync(Guid idUser, string email, string name)
        {
            _logger.LogDebug(
                "Publishing ICreatedPublisher to RabbitMQ: Id={idUser}, Email={email}, Name={name}",
                idUser, email, name);

            return _publishEndpoint.Publish<IUserCreated>(new
            {
                Id = idUser,
                Email = email,
                Name = name
            }, context => {
                context.CorrelationId = correlation.CorrelationId;
            });      
        }
    }
}
