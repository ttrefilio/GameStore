using System;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGame
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        // DELETE /games/1
        app.MapDelete("/{id}", (Guid id, GameStoreContext dbContext) =>
        {
            dbContext.Games
                     .Where(game => game.Id == id)
                     .ExecuteDelete();

            return Results.NoContent();
        });
    }
}
