using System.Net.Http.Json;
using CocktailRobot.Shared.Models;

namespace CocktailRobot.Client.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public Task<List<DrinkDto>?> GetDrinksAsync()
        => _http.GetFromJsonAsync<List<DrinkDto>>("api/menu/drinks");

    public Task<List<RecipeDto>?> GetRecipesAsync()
        => _http.GetFromJsonAsync<List<RecipeDto>>("api/menu/recipes");

    public Task<List<OrderDto>?> GetQueueAsync()
        => _http.GetFromJsonAsync<List<OrderDto>>("api/orders/queue");

    public async Task<string?> CreateOrderAsync(OrderDto order)
    {
        var response = await _http.PostAsJsonAsync("api/orders", order);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status)
    {
        var response = await _http.PostAsJsonAsync($"api/orders/{id}/status", status);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> CreateDrinkAsync(DrinkDto drink)
    {
        var response = await _http.PostAsJsonAsync("api/menu/drinks", drink);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> UpdateDrinkAsync(DrinkDto drink)
    {
        var response = await _http.PutAsJsonAsync($"api/menu/drinks/{drink.Id}", drink);
        return response.IsSuccessStatusCode;
    }
}
