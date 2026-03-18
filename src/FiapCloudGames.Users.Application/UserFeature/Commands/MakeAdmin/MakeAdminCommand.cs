using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.MakeAdmin;

public sealed record class MakeAdminCommand : IRequest<bool>
{
    public string Email { get; init; }

    public MakeAdminCommand(string email)
    {
        Email = email;
    }
}