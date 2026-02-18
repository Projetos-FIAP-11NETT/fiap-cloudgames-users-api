using FiapCloudGames.Users.Auth.Adapter;
using FiapCloudGames.Users.Contract.Dto.Response;
using FiapCloudGames.Users.Domain.Exceptions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace FiapCloudGames.Users.Auth;

public sealed class AuthService
    (
        IFirebaseService client
    )
    : IAuthService
{
    public async Task<string> CreateUserAsync(string email, string password, string name, IEnumerable<string> roles)
    {
        var firebaseUserId = await client.CreateUserAsync(email, password, name);
        if (string.IsNullOrEmpty(firebaseUserId))
            return string.Empty;

        await client.SetUserRoleAsync(firebaseUserId, roles);

        return firebaseUserId;
    }

    public async Task SetUserRoleAsync(string firebaseUserId, IEnumerable<string> roles)
    {
        await client.SetUserRoleAsync(firebaseUserId, roles);
    }

    public async Task<LoginResponse> LoginUserAsync(string email, string password)
    {
        try
        {
            return await client.LoginUserAsync(email, password);
        }
        catch (FirebaseAuthException ex)
        {
            throw FirebaseExceptionMapper.Map(ex);
        }
        catch (FirebaseException ex)
        {
            throw new ExternalException("Firebase", ex.Message);
        }
    }
}