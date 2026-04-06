using CocktailRobot.Shared.Models;

public class RobotCommandBuilder
{
    public List<int> BuildCommandsForOrder(
        OrderDto order,
        List<DrinkDto> drinks,
        List<RecipeDto> recipes)
    {
        var result = new List<int>();

        foreach (var item in order.Items)
        {
            if (item.Type == OrderItemType.Drink)
            {
                var drink = drinks.First(d => d.Id == item.ReferenceId);
                // Для самостоятельного напитка можно считать, что 1 доза = 100 мл
                result.Add(drink.Position);
            }
            else if (item.Type == OrderItemType.Recipe)
            {
                var recipe = recipes.First(r => r.Id == item.ReferenceId);
                foreach (var ri in recipe.Items)
                {
                    var drink = drinks.First(d => d.Id == ri.DrinkId);
                    for (int i = 0; i < ri.DoseCount; i++)
                    {
                        result.Add(drink.Position);
                    }
                }
            }
        }

        return result;
    }
}
