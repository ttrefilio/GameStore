using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.Api.Shared.Authorization;

public static class AuthorizationExtensions
{
    private const string ApiAccessScope = "gamestore_api.all";

    public static IHostApplicationBuilder AddGameStoreAuthentication(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<KeycloakClaimsTransformer>();

        builder.Services.AddAuthentication(Schemes.Keycloak)
                        .AddJwtBearer(options =>
                        {
                            options.MapInboundClaims = false;
                            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                        })
                        .AddJwtBearer(Schemes.Keycloak, options =>
                        {
                            options.MapInboundClaims = false;
                            options.TokenValidationParameters.RoleClaimType = ClaimTypes.Role;
                            options.RequireHttpsMetadata = false;
                            options.Events = new JwtBearerEvents
                            {
                                OnTokenValidated = context =>
                                {
                                    var transformer = context.HttpContext.RequestServices.GetRequiredService<KeycloakClaimsTransformer>();
                                    transformer.Transform(context);

                                    return Task.CompletedTask;
                                }
                            };
                        });
        return builder;
    }

    public static IHostApplicationBuilder AddGameStoreAuthorization(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
                .AddFallbackPolicy(Policies.UserAccess, authBuilder =>
                {
                    authBuilder.RequireClaim(ClaimTypes.Scope, ApiAccessScope);
                })
                .AddPolicy(Policies.AdminAccess, authBuilder =>
                {
                    authBuilder.RequireClaim(ClaimTypes.Scope, ApiAccessScope);
                    authBuilder.RequireRole(Roles.Admin);
                });
        return builder;
    }
}
