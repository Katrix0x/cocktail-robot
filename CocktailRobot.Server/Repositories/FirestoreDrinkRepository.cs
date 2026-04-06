using CocktailRobot.Server.Data;
using CocktailRobot.Shared.Models;
using Google.Cloud.Firestore;

namespace CocktailRobot.Server.Repositories;

public class FirestoreDrinkRepository : IDrinkRepository
{
    private readonly FirestoreDb _db;
    private const string CollectionName = "drinks";

    public FirestoreDrinkRepository(FirestoreContext context)
    {
        _db = context.Db;
    }

    public async Task<List<DrinkDto>> GetAllAsync()
    {
        var snapshot = await _db.Collection(CollectionName).GetSnapshotAsync();
        var result = new List<DrinkDto>();

        foreach (var doc in snapshot.Documents)
        {
            var dict = doc.ToDictionary();
            result.Add(new DrinkDto
            {
                Id = doc.Id,
                Name = dict["Name"] as string ?? "",
                Position = Convert.ToInt32(dict["Position"]),
                IsStandalone = Convert.ToBoolean(dict["IsStandalone"]),
                Description = dict.ContainsKey("Description") ? dict["Description"] as string : null
            });
        }

        return result.OrderBy(d => d.Position).ToList();
    }

    public async Task<DrinkDto?> GetByIdAsync(string id)
    {
        var doc = await _db.Collection(CollectionName).Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        var dict = doc.ToDictionary();
        return new DrinkDto
        {
            Id = doc.Id,
            Name = dict["Name"] as string ?? "",
            Position = Convert.ToInt32(dict["Position"]),
            IsStandalone = Convert.ToBoolean(dict["IsStandalone"]),
            Description = dict.ContainsKey("Description") ? dict["Description"] as string : null
        };
    }

    public async Task<string> CreateAsync(DrinkDto drink)
    {
        var docRef = await _db.Collection(CollectionName).AddAsync(new Dictionary<string, object?>
        {
            ["Name"] = drink.Name,
            ["Position"] = drink.Position,
            ["IsStandalone"] = drink.IsStandalone,
            ["Description"] = drink.Description
        });

        return docRef.Id;
    }

    public async Task UpdateAsync(DrinkDto drink)
    {
        var docRef = _db.Collection(CollectionName).Document(drink.Id);
        await docRef.SetAsync(new Dictionary<string, object?>
        {
            ["Name"] = drink.Name,
            ["Position"] = drink.Position,
            ["IsStandalone"] = drink.IsStandalone,
            ["Description"] = drink.Description
        }, SetOptions.Overwrite);
    }

    public async Task DeleteAsync(string id)
    {
        await _db.Collection(CollectionName).Document(id).DeleteAsync();
    }
}
