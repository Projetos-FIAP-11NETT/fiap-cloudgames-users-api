using FiapCloudGames.Users.Api.Configurations.OpenApi;
using FiapCloudGames.Users.Api.Middlewares;
using FiapCloudGames.Users.Observability.Middleware;

namespace FiapCloudGames.Users.Api.Configurations;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddOpenApiConfiguration();

        return services;
    }

    public static void UseApiConfig(this WebApplication app)
    {
        app.UseMiddleware<ObservabilityMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<RequestResponseLoggingMiddleware>();
    }
}