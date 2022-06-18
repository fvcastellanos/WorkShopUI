using Google.Cloud.Firestore;

namespace WorkShopUI.SearchSchema.IndexUpdate
{
    public interface ISearchIndexUpdate
    {
        Task UpdateIndexAsync(string collectionName, DocumentSnapshot documentSnapshot);
    }
}