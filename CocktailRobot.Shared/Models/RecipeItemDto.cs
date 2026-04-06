namespace CocktailRobot.Shared.Models;

public class RecipeItemDto
{
    public string DrinkId { get; set; } = default!; // Id ингредиента (DrinkDto)
    public int DoseCount { get; set; }              // 1 = 100 мл, 2 = 200 мл
}
