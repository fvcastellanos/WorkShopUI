
using Google.Cloud.Firestore;

namespace WorkShopUI.Domain.Model
{
    [FirestoreData]
    public class Product : BaseModel
    {
        [FirestoreProperty("type")]
        public string Type { get; set; }

        [FirestoreProperty("code")]
        public string Code { get; set; }

        [FirestoreProperty("name")]
        public string Name { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("active")]
        public string Active { get; set; }
    }
}