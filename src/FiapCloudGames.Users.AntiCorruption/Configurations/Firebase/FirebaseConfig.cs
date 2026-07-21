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
        var credentialJson = configuration["Firebase:CredentialJson"];

        if (string.IsNullOrWhiteSpace(credentialJson))
            throw new InvalidOperationException("Firebase credential json não configurado.");

        var credential = CredentialFactory.FromJson<ServiceAccountCredential>(credentialJson).ToGoogleCredential();

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