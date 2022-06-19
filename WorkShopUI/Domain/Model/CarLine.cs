using Google.Cloud.Firestore;

namespace WorkShopUI.Domain.Model
{
    [FirestoreData]
    public class CarLine
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

        [FirestoreProperty("tenant")]
        public string Tenant { get; set; }
    }

}