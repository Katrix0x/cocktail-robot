using CocktailRobot.Server.Repositories;
using CocktailRobot.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace CocktailRobot.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IDrinkRepository _drinkRepository;
    private readonly IRecipeRepository _recipeRepository;

    public MenuController(IDrinkRepository drinkRepository, IRecipeRepository recipeRepository)
    {
        _drinkRepository = drinkRepository;
        _recipeRepository = recipeRepository;
    }

    // GET api/menu/drinks
    [HttpGet("drinks")]
    public async Task<ActionResult<List<DrinkDto>>> GetDrinks()
    {
        var drinks = await _drinkRepository.GetAllAsync();
        return Ok(drinks);
    }

    // GET api/menu/recipes
    [HttpGet("recipes")]
    public async Task<ActionResult<List<RecipeDto>>> GetRecipes()
    {
        var recipes = await _recipeRepository.GetAllAsync();
        return Ok(recipes);
    }

    // POST api/menu/drinks (только организатор, но проверку сделаем на клиенте)
    [HttpPost("drinks")]
    public async Task<ActionResult<string>> CreateDrink([FromBody] DrinkDto drink)
    {
        var id = await _drinkRepository.CreateAsync(drink);
        return Ok(id);
    }

    // PUT api/menu/drinks/{id}
    [HttpPut("drinks/{id}")]
    public async Task<IActionResult> UpdateDrink(string id, [FromBody] DrinkDto drink)
    {
        drink.Id = id;
        await _drinkRepository.UpdateAsync(drink);
        return NoContent();
    }

    // DELETE api/menu/drinks/{id}
    [HttpDelete("drinks/{id}")]
    public async Task<IActionResult> DeleteDrink(string id)
    {
        await _drinkRepository.DeleteAsync(id);
        return NoContent();
    }

    // POST api/menu/recipes
    [HttpPost("recipes")]
    public async Task<ActionResult<string>> CreateRecipe([FromBody] RecipeDto recipe)
    {
        var id = await _recipeRepository.CreateAsync(recipe);
        return Ok(id);
    }

    // PUT api/menu/recipes/{id}
    [HttpPut("recipes/{id}")]
    public async Task<IActionResult> UpdateRecipe(string id, [FromBody] RecipeDto recipe)
    {
        recipe.Id = id;
        await _recipeRepository.UpdateAsync(recipe);
        return NoContent();
    }

    // DELETE api/menu/recipes/{id}
    [HttpDelete("recipes/{id}")]
    public async Task<IActionResult> DeleteRecipe(string id)
    {
        await _recipeRepository.DeleteAsync(id);
        return NoContent();
    }
}
