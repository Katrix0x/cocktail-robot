using Google.Cloud.Firestore;

namespace CocktailRobot.Server.Data;

public class FirestoreContext
{
    public FirestoreDb Db { get; }

    public FirestoreContext(IConfiguration configuration)
    {
        // Читаем настройки из appsettings.json
        var projectId = configuration["Firestore:ProjectId"];
        var credentialPath = configuration["Firestore:CredentialPath"];

        if (string.IsNullOrWhiteSpace(projectId) || string.IsNullOrWhiteSpace(credentialPath))
        {
            throw new InvalidOperationException("Firestore configuration is missing.");
        }

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
        Db = FirestoreDb.Create(projectId);
    }
}
