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
        private readonly ITypesenseClient _typesenseClient;

        private readonly FirestoreDb _firestoreDb;

        private readonly SchemaBuilder _schemaBuilder;

        public SearchIndexService(ILogger logger,
                                  ITypesenseClient typesenseClient,
                                  FirestoreDb firestoreDb,
                                  SchemaBuilder schemaBuilder)
        {
            _logger = logger;
            _typesenseClient = typesenseClient;
            _firestoreDb = firestoreDb;
            _schemaBuilder = schemaBuilder;
        }

        public async Task UpdateSearchIndexes()
        {
            var schemaDefinitions = _schemaBuilder.SchemaDefinitions
                .ToList();

            schemaDefinitions.ForEach(schemaDefinition => {

            });

            // _firestoreDb.
        }

        private async Task UpdateSearchIndex(SchemaDefinition schemaDefinition)
        {
            var schemaName = schemaDefinition.Name;

            var snapshots = await _firestoreDb.Collection(schemaName)
                .GetSnapshotAsync();
            //     .StreamAsync();

            foreach (var snapshot in snapshots)
            {
                // UpsertDocument

                // var type = getDocumentType(schemaName);
                // snapshot.ConvertTo<Type>();
            }
        }

        private async Task processDocument(string collection, DocumentSnapshot snapshot)
        {
            // var foo = 
            // collection switch
            // {
            //     "car-brands" => await upsertDocument<CarBrand, CarBrandView>(collection, snapshot),
            //     _ => throw new Exception("Document not recognized")
            // };
        }

        private async Task upsertDocument<T, R>(string collection, DocumentSnapshot snapshot) where R : class
        {
            var model = snapshot.ConvertTo<T>();

            var view = (R) toView(collection, model);

            await _typesenseClient.UpsertDocument<R>(collection, view);
        }

        private object toView(string collection, object model)
        {
            return collection switch
            {
                "car-brands" => CarBrandTransformer.ToView((CarBrand) model),
                _ => throw new Exception("Document not recognized")
            };
        }

    }
}