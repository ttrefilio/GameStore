using GameStore.Api.Features.Baskets.GetBasket;
using GameStore.Api.Features.Baskets.UpsertBasket;

namespace GameStore.Api.Features.Baskets;

public static class BasketsEndpoints
{
    public static void MapBaskets(this WebApplication app)
    {
        var group = app.MapGroup("/baskets");

        group.MapUpsertBasket();
        group.MapGetBasket();
    }
}
