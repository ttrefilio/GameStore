namespace GameStore.Api.Shared.Authorization;

public static class AuthorizationExtensions
{
    private const string ApiAccessScope = "gamestore_api.all";

    public static IHostApplicationBuilder AddGameStoreAuthorization(this IHostApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
                .AddFallbackPolicy(Policies.UserAccess, authBuilder =>
                {
                    authBuilder.RequireClaim("scope", ApiAccessScope);
                })
                .AddPolicy(Policies.AdminAccess, authBuilder =>
                {
                    authBuilder.RequireClaim("scope", ApiAccessScope);
                    authBuilder.RequireRole(Roles.Admin);
                });
        return builder;
    }
}
