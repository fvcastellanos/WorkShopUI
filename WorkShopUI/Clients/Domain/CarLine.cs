using Google.Cloud.Firestore;

namespace WorkShopUI.Clients.Domain
{
    [FirestoreData]
    public class CarLine: ResourceObject
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty("carBrandId")]
        public string CarBrandId { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("active")]
        public string Active { get; set; }
    }

}