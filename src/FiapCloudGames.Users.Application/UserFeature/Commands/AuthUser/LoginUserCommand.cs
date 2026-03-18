using FiapCloudGames.Users.Contract.Dto.Response;
using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.AuthUser;

public sealed record class LoginUserCommand
(
    string Email,
    string Password
)
    : IRequest<LoginResponse>;