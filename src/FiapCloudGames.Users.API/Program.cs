using FiapCloudGames.Queue.Configurations;
using FiapCloudGames.Users.Api.Configuration;
using FiapCloudGames.Users.Api.Configurations;
using FiapCloudGames.Users.Api.Configurations.OpenApi;
using FiapCloudGames.Users.Application.Abstractions;
using FiapCloudGames.Users.Application.Configurations;
using FiapCloudGames.Users.Auth.Configurations;
using FiapCloudGames.Users.Infrastructure.Configurations;
using FiapCloudGames.Users.Infrastructure.Correlation;
using FiapCloudGames.Users.Observability.Configurations;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddAuthenticationConfig();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();

builder.Host.AddSerilogConfig();

builder.Services.AddAuthService(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddQueueConfig(builder.Configuration);

builder.Services.AddObservabilityConfig();

builder.Services.AddApiConfig(builder.Configuration);

var app = builder.Build();

app.UseApiConfig();

app.MapControllers();

app.MapOpenApiConfiguration();

app.MapHealthCheckEndpoints();

app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.Run();