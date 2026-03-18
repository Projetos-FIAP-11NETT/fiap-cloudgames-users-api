using FiapCloudGames.Users.Contract.Dto.Response;

namespace FiapCloudGames.Users.Auth.Adapter;

public interface IFirebaseService
{
    Task<string> CreateUserAsync(string email, string password, string name);

    Task SetUserRoleAsync(string firebaseUserId, IEnumerable<string> roles,Guid? userId = null);
    Task SetUserIdAsync(string firebaseUserId, Guid userId);
    Task<LoginResponse> LoginUserAsync(string email, string password);
}