using FiapCloudGames.Users.API.Configurations;
using FiapCloudGames.Users.API.Middlewares;
using FiapCloudGames.Users.Application.Abstractions;
using FiapCloudGames.Users.Application.Configurations;
using FiapCloudGames.Users.Infrastructure.Configurations;
using FiapCloudGames.Users.Infrastructure.Correlation;
using Microsoft.EntityFrameworkCore;
using FiapCloudGames.Users.Auth.Configurations;

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