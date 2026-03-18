using Scalar.AspNetCore;

namespace FiapCloudGames.Users.Api.Configurations.OpenApi;

public static class OpenApiPipeline
{
    public static IEndpointRouteBuilder MapOpenApiConfiguration(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapOpenApi();
        endpoints.MapScalarApiReference();
        return endpoints;
    }
}