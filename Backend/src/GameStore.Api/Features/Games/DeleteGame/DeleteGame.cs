using System;
using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGame
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app, GameStoreData data)
    {
        // DELETE /games/1
        app.MapDelete("/games/{id}", (Guid id) =>
        {
            data.RemoveGame(id);

            return Results.NoContent();
        });
    }
}
