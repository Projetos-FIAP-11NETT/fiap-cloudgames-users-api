using FiapCloudGames.Users.Auth.Configurations.Firebase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Users.Auth.Configurations;

public static class AuthServiceConfig
{
    public static IServiceCollection AddAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFirebase(configuration);

        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}