using GameStore.Api.Data;
using GameStore.Api.Features.Baskets;
using GameStore.Api.Features.Games;
using GameStore.Api.Features.Genres;
using GameStore.Api.Shared.Authorization;
using GameStore.Api.Shared.ErrorHandling;
using GameStore.Api.Shared.FileUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

#region REGISTER SERVICES HERE
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connString); // Does the same as the commented code bellow
// builder.Services.AddDbContext<GameStoreContext>(
//     options => options.UseSqlite(connString)
// );

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor().AddSingleton<FileUploader>();

builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters.RoleClaimType = "role";
                });

builder.AddGameStoreAuthorization(); // Extended
builder.Services.AddSingleton<IAuthorizationHandler, BasketAuthorizationHandler>();

#endregion

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthorization();

app.MapGames();
app.MapGenres();
app.MapBaskets();

app.UseHttpLogging();
// app.UseMiddleware<RequestTimingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.InitializeDbAsync();

app.Run();