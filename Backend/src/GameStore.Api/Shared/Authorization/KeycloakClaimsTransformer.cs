using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.Api.Shared.Authorization;

public class KeycloakClaimsTransformer(ILogger<KeycloakClaimsTransformer> logger)
{
    public void Transform(TokenValidatedContext context)
    {
        var identity = context.Principal?.Identity as ClaimsIdentity;

        var scopeClaim = identity?.FindFirst(ClaimTypes.Scope);

        if (scopeClaim is null) return;

        //profile gamestore_api.all email
        var scopes = scopeClaim.Value.Split(' ');

        identity?.RemoveClaim(scopeClaim);

        identity?.AddClaims(
                scopes.Select(
                scope => new Claim(ClaimTypes.Scope, scope)));

        var claims = context.Principal?.Claims;

        if (claims is null) return;

        foreach (var claim in claims)
        {
            logger.LogTrace("Claim: {ClaimType}, Value: {ClaimValue}",
                                   claim.Type,
                                   claim.Value);
        }
    }
}
