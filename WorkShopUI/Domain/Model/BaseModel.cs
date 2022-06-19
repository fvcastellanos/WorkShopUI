using Google.Cloud.Firestore;

namespace WorkShopUI.Domain.Model
{
    public abstract class BaseModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Tenant { get; set; }
    }
}