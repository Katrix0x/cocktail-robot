using CocktailRobot.Client;
using CocktailRobot.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Базовый адрес API — пока для разработки localhost:5000
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:5001";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
});

builder.Services.AddScoped<ApiClient>();

await builder.Build().RunAsync();
