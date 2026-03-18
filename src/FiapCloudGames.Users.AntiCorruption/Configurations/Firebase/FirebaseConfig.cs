using FiapCloudGames.Users.Auth.Adapter;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Users.Auth.Configurations.Firebase;

public static class FirebaseConfig
{
    public static void AddFirebase(this IServiceCollection services, IConfiguration configuration)
    {
        var credentialPath = configuration["Firebase:CredentialPath"];

        if (string.IsNullOrWhiteSpace(credentialPath))
            throw new InvalidOperationException("Firebase credential path não configurado.");

        var credential = CredentialFactory
            .FromFile<ServiceAccountCredential>(credentialPath)
            .ToGoogleCredential();

        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });
        }

        services.AddScoped<IFirebaseService, FirebaseService>();
    }
}