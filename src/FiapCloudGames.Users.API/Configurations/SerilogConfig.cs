using Serilog;

namespace FiapCloudGames.Users.Api.Configurations;

public static class SerilogConfig
{
    public static void AddSerilogConfig(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });
    }
}