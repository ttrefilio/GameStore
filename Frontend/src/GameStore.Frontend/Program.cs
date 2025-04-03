using GameStore.Frontend.Authorization;
using GameStore.Frontend.Clients;
using GameStore.Frontend.Components;
using GameStore.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents();

builder.AddGameStoreOpenIdConnect();

var backendApiUrl = builder.Configuration["BackendApiUrl"] ??
    throw new Exception("BackendApiUrl is not set");

builder.Services.AddHttpClient<GamesClient>(
    client => client.BaseAddress = new Uri(backendApiUrl))
    .AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddHttpClient<GenresClient>(
    client => client.BaseAddress = new Uri(backendApiUrl));

builder.Services.AddHttpClient<IBasketClient, ServerBasketClient>(
    client => client.BaseAddress = new Uri(backendApiUrl))
    .AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddScoped<BasketState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.MapLoginAndLogout();
// app.MapDefaultEndpoints();

app.Run();
