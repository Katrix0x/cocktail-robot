namespace CocktailRobot.Shared.Models;

public class DrinkDto
{
    public string Id { get; set; } = default!;          // Firestore Id
    public string Name { get; set; } = default!;        // Название (Кола, Виски и т.п.)
    public int Position { get; set; }                   // Номер позиции на рельсе (1,2,3...)
    public bool IsStandalone { get; set; }              // true - самостоятельный напиток, false - только ингредиент
    public string? Description { get; set; }            // Описание (может быть null/пустым)
}
