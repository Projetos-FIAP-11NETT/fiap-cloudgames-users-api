using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FiapCloudGames.Users.API.Configurations;

public static class AuthenticationConfig
{
    public static void AddAuthenticationConfig(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/fiapcloudgames-eaced";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/fiapcloudgames-eaced",
                    ValidateAudience = true,
                    ValidAudience = "fiapcloudgames-eaced",
                    ValidateLifetime = true
                };
            });
    }
}
