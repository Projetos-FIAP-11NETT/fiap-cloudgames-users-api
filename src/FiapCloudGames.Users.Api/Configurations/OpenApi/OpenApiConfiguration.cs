using Microsoft.OpenApi;

namespace FiapCloudGames.Users.Api.Configurations.OpenApi;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "FIAP Cloud Games - Users API",
                    Version = "v1",
                    Description = "API de Criação e login de usuários para a plataforma FIAP Cloud Games",
                    Contact = new OpenApiContact
                    {
                        Name = "FIAP Cloud Games Team",
                        Email = "contato@fiapcloudgames.com"
                    }
                };

                return Task.CompletedTask;
            });
        });

        return services;
    }
}