using CocktailRobot.Server.Data;
using CocktailRobot.Shared.Models;
using Google.Cloud.Firestore;

namespace CocktailRobot.Server.Repositories;

public class FirestoreOrderRepository : IOrderRepository
{
    private readonly FirestoreDb _db;
    private const string CollectionName = "orders";

    public FirestoreOrderRepository(FirestoreContext context)
    {
        _db = context.Db;
    }

    public async Task<List<OrderDto>> GetQueueAsync()
    {
        var collection = _db.Collection(CollectionName);

        var query = collection
            .WhereIn("Status", new List<int>
            {
                (int)OrderStatus.InQueue,
                (int)OrderStatus.InProgress
            })
            .OrderBy("CreatedAtUtc");

        var snapshot = await query.GetSnapshotAsync();
        var result = new List<OrderDto>();

        foreach (var doc in snapshot.Documents)
        {
            result.Add(MapOrder(doc));
        }

        return result;
    }

    public async Task<OrderDto?> GetByIdAsync(string id)
    {
        var doc = await _db.Collection(CollectionName).Document(id).GetSnapshotAsync();
        if (!doc.Exists) return null;

        return MapOrder(doc);
    }

    public async Task<string> CreateAsync(OrderDto order)
    {
        var items = order.Items.Select(i => new Dictionary<string, object?>
        {
            ["Type"] = (int)i.Type,
            ["ReferenceId"] = i.ReferenceId
        }).ToList();

        var docRef = await _db.Collection(CollectionName).AddAsync(new Dictionary<string, object?>
        {
            ["DisplayName"] = order.DisplayName,
            ["IsOrganizer"] = order.IsOrganizer,
            ["Status"] = (int)order.Status,
            ["CreatedAtUtc"] = order.CreatedAtUtc,
            ["Items"] = items
        });

        return docRef.Id;
    }

    public async Task UpdateStatusAsync(string id, OrderStatus status)
    {
        var docRef = _db.Collection(CollectionName).Document(id);
        await docRef.UpdateAsync("Status", (int)status);
    }

    private static OrderDto MapOrder(DocumentSnapshot doc)
    {
        var dict = doc.ToDictionary();

        var items = new List<OrderItemDto>();
        if (dict.TryGetValue("Items", out var itemsObj) && itemsObj is IEnumerable<object> list)
        {
            foreach (var item in list)
            {
                if (item is Dictionary<string, object> itemDict)
                {
                    items.Add(new OrderItemDto
                    {
                        Type = (OrderItemType)Convert.ToInt32(itemDict["Type"]),
                        ReferenceId = itemDict["ReferenceId"] as string ?? ""
                    });
                }
            }
        }

        return new OrderDto
        {
            Id = doc.Id,
            DisplayName = dict["DisplayName"] as string ?? "",
            IsOrganizer = Convert.ToBoolean(dict["IsOrganizer"]),
            Status = (OrderStatus)Convert.ToInt32(dict["Status"]),
            CreatedAtUtc = ((Timestamp)dict["CreatedAtUtc"]).ToDateTime(),
            Items = items
        };
    }
}
