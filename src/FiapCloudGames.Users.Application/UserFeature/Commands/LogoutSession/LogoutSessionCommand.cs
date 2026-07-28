using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.LogoutSession;

public sealed record class LogoutSessionCommand(Guid SessionId) : IRequest<bool>;
