using System;
using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Features.Games.DeleteGame;

public static class DeleteGame
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        // DELETE /games/1
        app.MapDelete("/{id}", async (Guid id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                     .Where(game => game.Id == id)
                     .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}
