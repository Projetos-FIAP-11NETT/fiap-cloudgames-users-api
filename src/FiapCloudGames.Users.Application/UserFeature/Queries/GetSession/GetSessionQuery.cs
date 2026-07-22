using FiapCloudGames.Users.Contract.Dto.Response;
using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Queries.GetSession;

public sealed record class GetSessionQuery(Guid SessionId) : IRequest<SessionResponse?>;
