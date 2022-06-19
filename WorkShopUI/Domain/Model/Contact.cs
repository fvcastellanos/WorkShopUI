
using Google.Cloud.Firestore;

namespace WorkShopUI.Domain.Model
{
    [FirestoreData]
    public class Contact : BaseModel
    {
        // [FirestoreDocumentId]
        // public string Id { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("type")]
        public string Type { get; set; }

        [FirestoreProperty("code")]
        public string Code { get; set; }

        [FirestoreProperty("contact")]
        public string ContactName { get; set; }

        [FirestoreProperty("taxId")]
        public string TaxId { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("active")]
        public string Active { get; set; }

        // [FirestoreProperty("tenant")]
        // public string Tenant { get; set; }
    }
}