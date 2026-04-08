using CocktailRobot.Server.Data;
using CocktailRobot.Server.Repositories;
using CocktailRobot.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Firestore
builder.Services.AddSingleton<FirestoreContext>();

// Repositories
builder.Services.AddScoped<IDrinkRepository, FirestoreDrinkRepository>();
builder.Services.AddScoped<IRecipeRepository, FirestoreRecipeRepository>();
builder.Services.AddScoped<IOrderRepository, FirestoreOrderRepository>();

// Controllers
builder.Services.AddControllers();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("https://cocktail-robot-client.onrender.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowClient");

app.MapGet("/", () => "CocktailRobot API is running 🚀");

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.Run();