using CocktailRobot.Server.Data;
using CocktailRobot.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Firestore
builder.Services.AddSingleton<FirestoreContext>();

// Repositories
builder.Services.AddScoped<IDrinkRepository, FirestoreDrinkRepository>();
builder.Services.AddScoped<IRecipeRepository, FirestoreRecipeRepository>();
builder.Services.AddScoped<IOrderRepository, FirestoreOrderRepository>();

// Controllers
builder.Services.AddControllers();

// ✅ CORS (открытый)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

app.MapGet("/", () => "CocktailRobot API is running 🚀");

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

app.Run();