namespace CocktailRobot.Shared.Models;

public class OrderDto
{
    public string Id { get; set; } = default!;
    public string DisplayName { get; set; } = default!; // Забавное имя или "Организатор"
    public bool IsOrganizer { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();

    public OrderStatus Status { get; set; } = OrderStatus.InQueue;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
