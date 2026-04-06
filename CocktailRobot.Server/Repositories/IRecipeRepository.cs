using CocktailRobot.Shared.Models;

namespace CocktailRobot.Server.Repositories;

public interface IRecipeRepository
{
    Task<List<RecipeDto>> GetAllAsync();
    Task<RecipeDto?> GetByIdAsync(string id);
    Task<string> CreateAsync(RecipeDto recipe);
    Task UpdateAsync(RecipeDto recipe);
    Task DeleteAsync(string id);
}
