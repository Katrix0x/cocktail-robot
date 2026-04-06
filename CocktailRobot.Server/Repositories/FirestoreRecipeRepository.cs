using CocktailRobot.Server.Data;
using CocktailRobot.Shared.Models;
using Google.Cloud.Firestore;

namespace CocktailRobot.Server.Repositories;

public class FirestoreRecipeRepository : IRecipeRepository
{
    private readonly FirestoreDb _db;
    private const string CollectionName = "recipes";

    public FirestoreRecipeRepository(FirestoreContext context)
    {
        _db = context.Db;
    }

    public async Task<List<RecipeDto>> GetAllAsync()
    {
        var snapshot = await _db.Collection(CollectionName).GetSnapshotAsync();
        var result = new List<RecipeDto>();

        foreach (var doc in snapshot.Documents)
        {
            var dict = doc.ToDictionary();

            var items = new List<RecipeItemDto>();
            if (dict.TryGetValue("Items", out var itemsObj) && itemsObj is IEnumerable<object> list)
            {
                foreach (var item in list)
                {
                    if (item is Dictionary<string, object> itemDict)
                    {
                        items.Add(new RecipeItemDto
                        {
                            DrinkId = itemDict["DrinkId"] as string ?? "",
                            DoseCount = Convert.ToInt32(itemDict["DoseCount"])
                        });
                    }
                }
            }

            result.Add(new RecipeDto
            {
                Id = doc.Id,
                Name = dict["Name"] as string ?? "",
                Description = dict.ContainsKey("Description") ? dict["Description"] as string : null,
                Items = items
            });
        }

        return result.OrderBy(r => r.Name).ToList();
    }

    public async Task<RecipeDto?> GetByIdAsync(string id)
    {
        var doc = await _db.Collection(CollectionName).Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var dict = doc.ToDictionary();

        var items = new List<RecipeItemDto>();
        if (dict.TryGetValue("Items", out var itemsObj) && itemsObj is IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                if (item is Dictionary<string, object> itemDict)
                {
                    items.Add(new RecipeItemDto
                    {
                        DrinkId = itemDict["DrinkId"] as string ?? "",
                        DoseCount = Convert.ToInt32(itemDict["DoseCount"])
                    });
                }
            }
        }

        return new RecipeDto
        {
            Id = doc.Id,
            Name = dict["Name"] as string ?? "",
            Description = dict.ContainsKey("Description") ? dict["Description"] as string : null,
            Items = items
        };
    }

    public async Task<string> CreateAsync(RecipeDto recipe)
    {
        var items = recipe.Items.Select(i => new Dictionary<string, object?>
        {
            ["DrinkId"] = i.DrinkId,
            ["DoseCount"] = i.DoseCount
        }).ToList();

        var docRef = await _db.Collection(CollectionName).AddAsync(new Dictionary<string, object?>
        {
            ["Name"] = recipe.Name,
            ["Description"] = recipe.Description,
            ["Items"] = items
        });

        return docRef.Id;
    }

    public async Task UpdateAsync(RecipeDto recipe)
    {
        var items = recipe.Items.Select(i => new Dictionary<string, object?>
        {
            ["DrinkId"] = i.DrinkId,
            ["DoseCount"] = i.DoseCount
        }).ToList();

        var docRef = _db.Collection(CollectionName).Document(recipe.Id);
        await docRef.SetAsync(new Dictionary<string, object?>
        {
            ["Name"] = recipe.Name,
            ["Description"] = recipe.Description,
            ["Items"] = items
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id)
    {
        await _db.Collection(CollectionName).Document(id).DeleteAsync();
    }
}
