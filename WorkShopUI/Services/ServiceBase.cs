using Google.Cloud.Firestore;
using Typesense;

namespace WorkShopUI.Services
{
    public abstract class ServiceBase
    {
        protected readonly FirestoreDb FirestoreDb;

        private const string DefaultTenant = "default";

        public ServiceBase(FirestoreDb firestoreDb)
        {
            FirestoreDb = firestoreDb;
        }
        
        protected string GetTenant()
        {
            return DefaultTenant;
        }

        protected string BuildGuid() {

            return Guid.NewGuid()
                .ToString();
        }

        protected void AddToFireStore<T>(string collection, string id, T model)
        {
            var docRef = FirestoreDb.Collection(collection)
                .Document(id);
            
            var task = docRef.SetAsync(model)
                .Result;            
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