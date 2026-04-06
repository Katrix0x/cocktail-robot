namespace CocktailRobot.Shared.Models;

public class RecipeDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public List<RecipeItemDto> Items { get; set; } = new();
}
