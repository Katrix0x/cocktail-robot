using CocktailRobot.Server.Repositories;
using CocktailRobot.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CocktailRobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrdersController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    // GET api/orders/queue
    [HttpGet("queue")]
    public async Task<ActionResult<List<OrderDto>>> GetQueue()
    {
        var queue = await _orderRepository.GetQueueAsync();
        return Ok(queue);
    }

    // POST api/orders
    [HttpPost]
    public async Task<ActionResult<string>> CreateOrder([FromBody] OrderDto order)
    {
        order.CreatedAtUtc = DateTime.UtcNow;
        order.Status = OrderStatus.InQueue;
        var id = await _orderRepository.CreateAsync(order);
        return Ok(id);
    }

    // POST api/orders/{id}/status
    [HttpPost("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] OrderStatus status)
    {
        await _orderRepository.UpdateStatusAsync(id, status);
        return NoContent();
    }
}
