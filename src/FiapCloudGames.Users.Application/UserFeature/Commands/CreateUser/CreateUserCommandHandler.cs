using FiapCloudGames.Users.Auth;
using FiapCloudGames.Users.Domain.Contracts.Repositories;
using FiapCloudGames.Users.Domain.Entities;
using FiapCloudGames.Users.Domain.Exceptions;
using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.CreateUser;

public class CreateUserCommandHandler
    (
        IAuthService authService,
        IUserRepository userRepository,
        IRoleRepository roleRepository
    )
    : IRequestHandler<CreateUserCommand, bool>
{

    public async Task<bool> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var emailAlreadyExists = await userRepository.ExistsEmailAsync(command.Email);
        if (emailAlreadyExists)
            throw new BusinessException("Esse e-mail já está cadastrado");

        var role = await roleRepository.FindUserRoleAsync();
        if (role is null)
            throw new NotFoundException("Perfil de usuário não encontrado");

        var user = new User(command.Name, command.Email, command.Password, role);

        var firebaseUserId = await CreateUserInFirebase(user);
        if (string.IsNullOrEmpty(firebaseUserId))
            throw new ExternalException("Firebase","Não foi possível criar a conta no Firebase");

        user.SetFirebaseUserId(firebaseUserId);

        await userRepository.AddAsync(user);
        return await userRepository.SaveChangesAsync(cancellationToken);
    }

    
    private async Task<string> CreateUserInFirebase(User user)
    {
        var firebaseUserId = await authService.CreateUserAsync(
            user.Email,
            user.Password,
            user.Name,
            user.Roles.Select(x => x.Name));

        return firebaseUserId;
    }
}