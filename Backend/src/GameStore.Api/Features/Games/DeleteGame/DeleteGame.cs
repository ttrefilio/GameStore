using System;
using GameStore.Api.Data;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGame
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        // DELETE /games/1
        app.MapDelete("/{id}", (Guid id, GameStoreData data) =>
        {
            data.RemoveGame(id);

            return Results.NoContent();
        });
    }
}
