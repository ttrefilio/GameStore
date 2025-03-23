using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GameStore.Api.Data;
using GameStore.Api.Features.Games.Constants;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        // PUT /games/1
        app.MapPut("/{id}", async (Guid id, 
                                   [FromForm] UpdateGameDto gameDto, 
                                   GameStoreContext dbContext,
                                   FileUploader fileUploader,
                                   ClaimsPrincipal user) =>
        {
            var currentUserId = user?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Results.Unauthorized();
            }
            
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            if (gameDto.ImageFile is not null)
            {
                var fileUploadResult = await fileUploader.UploadFileAsync(gameDto.ImageFile, StorageNames.GameImagesFolder);

                if (!fileUploadResult.IsSuccess)
                {
                    return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                }

                existingGame.ImageUri = fileUploadResult.FileUrl!;
            }

            existingGame.Name = gameDto.Name;
            existingGame.GenreId = gameDto.GenreId;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;
            existingGame.LastUpdatedBy = currentUserId;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).WithParameterValidation()
          .DisableAntiforgery();
    }
}
