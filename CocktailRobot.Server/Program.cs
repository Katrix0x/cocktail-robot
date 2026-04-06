using CocktailRobot.Server.Data;
using CocktailRobot.Server.Repositories;
using CocktailRobot.Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем FirestoreContext
builder.Services.AddSingleton<FirestoreContext>();

// Репозитории
builder.Services.AddScoped<IDrinkRepository, FirestoreDrinkRepository>();
builder.Services.AddScoped<IRecipeRepository, FirestoreRecipeRepository>();
builder.Services.AddScoped<IOrderRepository, FirestoreOrderRepository>();

// Контроллеры
builder.Services.AddControllers();

// CORS — чтобы клиент с другого домена мог обращаться
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();

app.MapControllers();

app.Run();
