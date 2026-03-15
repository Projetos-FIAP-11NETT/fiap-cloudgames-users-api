using FiapCloudGames.Users.Auth;
using FiapCloudGames.Users.Domain.Contracts.Repositories;
using FiapCloudGames.Users.Domain.Exceptions;
using MediatR;

namespace FiapCloudGames.Users.Application.UserFeature.Commands.MakeAdmin;

public class MakeAdminCommandHandler
    (
        IAuthService authService,
        IUserRepository userRepository,
        IRoleRepository roleRepository
    )
    : IRequestHandler<MakeAdminCommand, bool>
{
    public async Task<bool> Handle(MakeAdminCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email)
            ?? throw new NotFoundException("Operação não realizada. O usuário não encontrado.");
        
        if (user is null)
            return false;
        
        var role = await roleRepository.FindAdminRoleAsync();
        user.MakeAdmin(role);

        try
        {
            await authService.SetUserRoleAsync(user.FirebaseUserId, user.Roles.Select(x => x.Name), user.Id);
        }
        catch (Exception)
        {
            throw new ExternalException("Firebase","Operação não realizada. Perfil não atualizado no serviço de autenticação.");
        }
        
        try
        {
            userRepository.Update(user);
            return await userRepository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw new DomainException("Operação não realizada. Perfil não atualizado no banco de dados."); // 5xx
        }
        
    }
}