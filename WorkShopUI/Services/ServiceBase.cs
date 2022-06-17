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

        protected Either<string, IEnumerable<T>> Search<T>(string collection, SearchParameters searchParameters)
        {
            var search = TypesenseClient.Search<T>(collection, searchParameters)
                .Result;

            return search.Hits.Select(hit => hit.Document)
                .ToList();            
        }

        protected void AddToFireStore<T>(string collection, string id, T model)
        {
            _logger.LogInformation("Store into firestore collection: {0} with id: {1}", collection, id);
            var docRef = FirestoreDb.Collection(collection)
                .Document(id);
            
            docRef.SetAsync(model)
                .Wait();
        }

        protected void UpdateSearchIndex<T>(string collection, T indexObject) where T : class
        {
            _logger.LogInformation("Update search index for collection: {0}", collection);
            TypesenseClient.UpsertDocument<T>(collection, indexObject);
        }


        protected DocumentSnapshot FindById(string collection, string id)
        {
            var docRef = FirestoreDb.Collection(collection)
                .Document(id);

            return docRef.GetSnapshotAsync()
                .Result;
        }
    }
}