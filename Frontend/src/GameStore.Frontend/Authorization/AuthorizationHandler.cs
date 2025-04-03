using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace GameStore.Frontend.Authorization;

public class AuthorizationHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext ??
            throw new InvalidOperationException("No HttpContext available from the IHttpContextAccessor!");

        var idp = configuration["IdentityProvider"] ?? throw new InvalidOperationException("Identity Provider not found");
        string? accessToken = await httpContext!.GetTokenAsync(idp, "access_token");

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}