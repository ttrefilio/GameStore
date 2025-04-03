using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Baskets.GetBasket;

public static class GetBasketEndoint
{
    public static void MapGetBasket(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{userId}", async (
            Guid userId,
            GameStoreContext dbContext,
            IAuthorizationService authorizationService,
            ClaimsPrincipal user
        ) =>
        {
            if (userId == Guid.Empty)
            {
                return Results.BadRequest("Invalid user id");
            }

            var basket = await dbContext.Baskets
                .Include(basket => basket.Items)
                .ThenInclude(item => item.Game)
                .FirstOrDefaultAsync(basket => basket.Id == userId)
                                     ?? new() { Id = userId };

            var authResult = await authorizationService.AuthorizeAsync(
                user,
                basket,
                new OwnerOrAdminRequirement()
            );

            if (!authResult.Succeeded)
            {
                return Results.Forbid();
            }

            var dto = new BasketDto(
                basket.Id,
                basket.Items.Select(item => new BasketItemDto(
                    item.GameId,
                    item.Game!.Name,
                    item.Game!.Price,
                    item.Quantity,
                    item.Game!.ImageUri
                ))
                .OrderBy(item => item.Name));

            return Results.Ok(dto);
        });
    }
}
