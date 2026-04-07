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
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();

app.Run();

