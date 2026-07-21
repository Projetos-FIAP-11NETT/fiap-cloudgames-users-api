using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FiapCloudGames.Users.Api.Configurations;

public class DevHeaderAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public DevHeaderAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (TryBuildPrincipalFromHeaders(out var headerPrincipal))
        {
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(headerPrincipal, Scheme.Name)));
        }

        return Task.FromResult(AuthenticateResult.NoResult());
    }

    private bool TryBuildPrincipalFromHeaders(out ClaimsPrincipal principal)
    {
        principal = null!;

        if (!Request.Headers.TryGetValue("x-user-id", out var uid) || string.IsNullOrWhiteSpace(uid))
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, uid.ToString()),
            new("sub", uid.ToString())
        };

        if (Request.Headers.TryGetValue("x-roles", out var roles) && !string.IsNullOrWhiteSpace(roles))
        {
            foreach (var role in roles.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
            }
        }

        principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
        return true;
    }
}
