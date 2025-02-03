using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
var builder = WebApplication.CreateBuilder(args);

#region REGISTER SERVICES HERE

builder.Services.AddTransient<GameDataLogger>();
builder.Services.AddSingleton<GameStoreData>();

#endregion

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.Run();