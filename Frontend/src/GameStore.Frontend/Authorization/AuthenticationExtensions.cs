using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace GameStore.Frontend.Authorization;

public static class AuthenticationExtensions
{
    public const string OpenIdConnectBackchannel = "OpenIdConnectBackchannel";

    public static void AddGameStoreOpenIdConnect(this IHostApplicationBuilder builder)
    {
        var idp = builder.Configuration["IdentityProvider"] ?? throw new InvalidOperationException("Identity Provider not found");

        var authBuilder = builder.Services.AddAuthentication(idp);

        if (builder.Environment.IsDevelopment())
        {
            authBuilder.AddOpenIdConnect(
                authenticationScheme: Schemes.Keycloak,
                options =>
                {
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.SaveTokens = true;
                    options.MapInboundClaims = false;
                    options.Scope.Add("gamestore_api.all");
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                    options.TokenValidationParameters.RoleClaimType = "role";
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.SignOutScheme = Schemes.Keycloak;
                    options.RequireHttpsMetadata = false;
                });
        }

        authBuilder.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        // ConfigureCookieOidcRefresh attaches a cookie OnValidatePrincipal callback to get
        // a new access token when the current one expires, and reissue a cookie with the
        // new access token saved inside. If the refresh fails, the user will be signed
        // out. OIDC connect options are set for saving tokens and the offline access
        // scope.
        builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, idp);

        builder.Services.AddAuthorizationBuilder();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<AuthorizationHandler>();
    }
}
