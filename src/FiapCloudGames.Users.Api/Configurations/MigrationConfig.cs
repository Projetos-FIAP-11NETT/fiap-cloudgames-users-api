using FiapCloudGames.Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Users.Api.Configurations;

public static class MigrationConfig
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dataContext.Database.Migrate();
    }
}
