using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Users.Application.Configurations;

public static class ApplicationConfig
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatorConfig();
        services.AddValidatorConfig();
        
        return services;
    }
}