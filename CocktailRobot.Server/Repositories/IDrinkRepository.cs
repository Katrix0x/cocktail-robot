using CocktailRobot.Shared.Models;

namespace CocktailRobot.Server.Repositories;

public interface IDrinkRepository
{
    Task<List<DrinkDto>> GetAllAsync();
    Task<DrinkDto?> GetByIdAsync(string id);
    Task<string> CreateAsync(DrinkDto drink);
    Task UpdateAsync(DrinkDto drink);
    Task DeleteAsync(string id);
}
