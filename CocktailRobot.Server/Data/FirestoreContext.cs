using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace CocktailRobot.Server.Data;

public class FirestoreContext
{
    public FirestoreDb Db { get; }

    public FirestoreContext(IConfiguration configuration)
    {
        var projectId = configuration["Firestore:ProjectId"];

        if (string.IsNullOrWhiteSpace(projectId))
            throw new InvalidOperationException("Firestore ProjectId is missing.");

        // Читаем JSON-ключ из переменной окружения (Render / локально)
        var json = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");
        if (string.IsNullOrWhiteSpace(json))
            throw new Exception("Firebase credentials not found in environment variables.");

        // Создаём FirestoreClient напрямую из JSON-ключа
        var client = new FirestoreClientBuilder
        {
            JsonCredentials = json
        }.Build();

        Db = FirestoreDb.Create(projectId, client);
    }
}
