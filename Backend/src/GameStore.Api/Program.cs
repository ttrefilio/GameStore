using GameStore.Api.Data;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);



#region REGISTER SERVICES HERE

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString); // Does the same as the commented code bellow
// builder.Services.AddDbContext<GameStoreContext>(
//     options => options.UseSqlite(connString)
// );

#endregion

var app = builder.Build();

app.MapGames();
app.MapGenres();

app.InitializeDb();

app.Run();