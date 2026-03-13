using FiapCloudGames.Users.Contract.Dto.Response;

namespace FiapCloudGames.Users.Auth;

public interface IAuthService
{
    Task<string> CreateUserAsync(string email, string password, string name, IEnumerable<string> roles, Guid userId);
    
    Task SetUserRoleAsync(string firebaseUserId, IEnumerable<string> roles);

    Task<LoginResponse> LoginUserAsync(string email, string password);
}