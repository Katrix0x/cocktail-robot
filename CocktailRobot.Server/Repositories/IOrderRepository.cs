using CocktailRobot.Shared.Models;

namespace CocktailRobot.Server.Repositories;

public interface IOrderRepository
{
    Task<List<OrderDto>> GetQueueAsync(); // Все заказы, кроме Done
    Task<OrderDto?> GetByIdAsync(string id);
    Task<string> CreateAsync(OrderDto order);
    Task UpdateStatusAsync(string id, OrderStatus status);
}
