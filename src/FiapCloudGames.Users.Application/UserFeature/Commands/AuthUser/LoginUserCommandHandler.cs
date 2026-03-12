using FiapCloudGames.Queue.Publisher;
using FiapCloudGames.Users.Auth;
using FiapCloudGames.Users.Contract.Dto.Response;
using FiapCloudGames.Users.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using static MassTransit.ValidationResultExtensions;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.AuthUser;

public class LoginUserCommandHandler
    (
        IAuthService firebaseAuthService,
        ILogger<LoginUserCommandHandler> logger
    )
    : IRequestHandler<LoginUserCommand,LoginResponse>
{

    public async Task<LoginResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var responseToken = await AuthUserInFirebase(command);

        return new LoginResponse
        {
            IdToken = responseToken.IdToken,
            RefreshToken = responseToken.RefreshToken,
            ExpiresIn = responseToken.ExpiresIn,
            Email = responseToken.Email
        };
    }
    
    private async Task<LoginResponse> AuthUserInFirebase(LoginUserCommand command)
    {
        var responseToken = await firebaseAuthService.LoginUserAsync(
            command.Email,
            command.Password);
        return responseToken;
    }
}