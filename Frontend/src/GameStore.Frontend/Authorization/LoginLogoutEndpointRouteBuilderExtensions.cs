using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Frontend.Authorization;

internal static class LoginLogoutEndpointRouteBuilderExtensions
{
    internal static IEndpointConventionBuilder MapLoginAndLogout(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("authentication");

        group.MapGet("/login", (string? returnUrl, IConfiguration configuration) =>
        {
            var idp = configuration["IdentityProvider"] ?? throw new InvalidOperationException("Identity Provider not found");
            return TypedResults.Challenge(GetAuthProperties(returnUrl), [idp]);
        })
        .AllowAnonymous();

        // Sign out of the Cookie and OIDC handlers. If you do not sign out with the OIDC handler,
        // the user will automatically be signed back in the next time they visit a page that requires authentication
        // without being able to choose another account.
        group.MapPost("/logout", ([FromForm] string? returnUrl, IConfiguration configuration) =>
        {
            var idp = configuration["IdentityProvider"] ?? throw new InvalidOperationException("Identity Provider not found");
            return TypedResults.SignOut(GetAuthProperties(returnUrl), [CookieAuthenticationDefaults.AuthenticationScheme, idp]);
        });

        group.MapGet("/signout-oidc", () =>
        {
            return TypedResults.LocalRedirect("~/");
        });

        return group;
    }

    private static AuthenticationProperties GetAuthProperties(string? returnUrl)
    {
        // TODO: Use HttpContext.Request.PathBase instead.
        const string pathBase = "/";

        // Prevent open redirects.
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = pathBase;
        }
        else if (!Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
        {
            returnUrl = new Uri(returnUrl, UriKind.Absolute).PathAndQuery;
        }
        else if (returnUrl[0] != '/')
        {
            returnUrl = $"{pathBase}{returnUrl}";
        }

        return new AuthenticationProperties { RedirectUri = returnUrl };
    }
}
