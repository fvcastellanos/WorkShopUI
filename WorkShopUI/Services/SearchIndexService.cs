using Google.Cloud.Firestore;
using Typesense;
using WorkShopUI.Clients.Domain;
using WorkShopUI.Domain;
using WorkShopUI.SearchSchema;
using WorkShopUI.SearchSchema.Domain;
using WorkShopUI.Transformers;

namespace WorkShopUI.Services
{
    public class SearchIndexService
    {
        private readonly ILogger _logger;

        private readonly FirestoreDb _firestoreDb;

        private readonly SchemaBuilder _schemaBuilder;

        private readonly IndexUpdateProvider _indexUpdateProvider;

        public SearchIndexService(ILogger<SearchIndexService> logger,
                                  FirestoreDb firestoreDb,
                                  SchemaBuilder schemaBuilder,
                                  IndexUpdateProvider indexUpdateProvider)
        {
            _logger = logger;
            _firestoreDb = firestoreDb;
            _schemaBuilder = schemaBuilder;
            _indexUpdateProvider = indexUpdateProvider;
        }

        public async Task UpdateSearchIndexes()
        {
            _logger.LogInformation("Updating searc indexes");
            var schemaDefinitions = _schemaBuilder.SchemaDefinitions
                .ToList();

            foreach(var schemaDefinition in schemaDefinitions)
            {
                await updateSearchIndex(schemaDefinition);
            }
        }

        private async Task updateSearchIndex(SchemaDefinition schemaDefinition)
        {
            var schemaName = schemaDefinition.Name;

            var snapshots = await _firestoreDb.Collection(schemaName)
                .GetSnapshotAsync();

            foreach (var snapshot in snapshots)
            {
                try 
                {
                    var indexUpdate = _indexUpdateProvider.GetSearchIndexUpdate(schemaName);

                    if (indexUpdate != null)
                    {
                        _logger.LogInformation($"Updating search index for schema: {schemaName}");
                        await indexUpdate.UpdateIndexAsync(schemaName, snapshot);

                        return;
                    }

                    _logger.LogWarning($"Search Index Updater not found for schema: {schemaName}");
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, $"Unable to update search index for collection: {schemaName}");
                }
            }
        }
    }
}