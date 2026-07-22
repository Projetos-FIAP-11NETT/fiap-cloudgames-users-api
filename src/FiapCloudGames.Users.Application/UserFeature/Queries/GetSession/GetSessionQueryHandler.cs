using FiapCloudGames.Users.Application.Sessions;
using FiapCloudGames.Users.Contract.Dto.Response;
using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Queries.GetSession;

public sealed class GetSessionQueryHandler(ISessionCacheService sessionCacheService)
    : IRequestHandler<GetSessionQuery, SessionResponse?>
{
    public async Task<SessionResponse?> Handle(GetSessionQuery query, CancellationToken cancellationToken)
    {
        var session = await sessionCacheService.GetAsync(query.SessionId, cancellationToken);

        if (session is null)
            return null;

        return new SessionResponse
        {
            SessionId = session.SessionId,
            Email = session.Email,
            CreatedAt = session.CreatedAt,
            ExpiresAt = session.ExpiresAt
        };
    }
}
