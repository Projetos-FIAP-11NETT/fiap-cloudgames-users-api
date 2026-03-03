using FiapCloudGames.Users.Api.Configurations;
using FiapCloudGames.Users.Api.Middlewares;
using FiapCloudGames.Users.Application.Abstractions;
using FiapCloudGames.Users.Application.Configurations;
using FiapCloudGames.Users.Auth.Configurations;
using FiapCloudGames.Users.Infrastructure.Configurations;
using FiapCloudGames.Users.Infrastructure.Correlation;
using FiapCloudGames.Queue.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

builder.Services.AddSwaggerConfig();

builder.Services.AddAuthenticationConfig();

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();

builder.Host.AddSerilogConfig();

builder.Services.AddAuthService(builder.Configuration);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddQueueConfig(builder.Configuration);


var app = builder.Build();

app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseRequestResponseLogging();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();