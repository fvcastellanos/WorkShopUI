using Google.Cloud.Firestore;
using LanguageExt;
using Typesense;

namespace WorkShopUI.Services
{
    public abstract class ServiceBase
    {
        protected readonly FirestoreDb FirestoreDb;

        protected readonly ITypesenseClient TypesenseClient;

        private readonly ILogger _logger;

        private const string DefaultTenant = "default";

        public ServiceBase(ILogger logger,
                           FirestoreDb firestoreDb, 
                           ITypesenseClient typesenseClient)
        {
            FirestoreDb = firestoreDb;
            TypesenseClient = typesenseClient;
            _logger = logger;
        }
        
        protected string GetTenant()
        {
            return DefaultTenant;
        }

        protected string BuildGuid() {

            return Guid.NewGuid()
                .ToString();
        }

        protected async Task<Either<string, IEnumerable<T>>> SearchAsync<T>(string collection, SearchParameters searchParameters)
        {
            var search = await TypesenseClient.Search<T>(collection, searchParameters);

            return search.Hits.Select(hit => hit.Document)
                .ToList();            
        }

        protected async Task AddToFireStoreAsync<T>(string collection, string id, T model)
        {
            _logger.LogInformation("Store into firestore collection: {0} with id: {1}", collection, id);
            var docRef = FirestoreDb.Collection(collection)
                .Document(id);
            
            await docRef.SetAsync(model);
        }

        protected async Task UpdateSearchIndexAsync<T>(string collection, T indexObject) where T : class
        {
            _logger.LogInformation("Update search index for collection: {0}", collection);
            await TypesenseClient.UpsertDocument<T>(collection, indexObject);
        }


        protected async Task<DocumentSnapshot> FindByIdAsync(string collection, string id)
        {
            var docRef = FirestoreDb.Collection(collection)
                .Document(id);

            return await docRef.GetSnapshotAsync();
        }
    }
}