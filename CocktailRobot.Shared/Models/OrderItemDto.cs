namespace CocktailRobot.Shared.Models;

public class OrderItemDto
{
    public OrderItemType Type { get; set; }
    public string ReferenceId { get; set; } = default!; // Id напитка или рецепта
}
