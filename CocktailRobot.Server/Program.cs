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

// CORS — единственная правильная политика
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

app.UseHttpsRedirection();

// Включаем CORS ДО контроллеров
app.UseCors("AllowClient");

app.MapControllers();

app.Run();
