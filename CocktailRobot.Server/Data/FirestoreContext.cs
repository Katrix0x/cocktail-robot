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

        var credentialPath = configuration["Firestore:CredentialPath"];
        if (string.IsNullOrWhiteSpace(credentialPath))
            throw new Exception("Firestore credential path is missing.");

        var fullPath = Path.Combine(AppContext.BaseDirectory, credentialPath);
        if (!File.Exists(fullPath))
            throw new Exception($"Firebase key file not found: {fullPath}");

        var client = new FirestoreClientBuilder
        {
            CredentialsPath = fullPath
        }.Build();

        Db = FirestoreDb.Create(projectId, client);
    }
}
