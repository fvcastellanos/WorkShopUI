using System.Text.Json.Serialization;
using Google.Cloud.Firestore;

namespace WorkShopUI.Clients.Domain
{
    [FirestoreData]
    public class CarBrand : ResourceObject
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("active")]
        public string Active { get; set; }

        [FirestoreProperty("tenant")]
        public string Tenant { get; set; }
    }
}